// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;

using Azure.Identity;

using Backend.Interfaces;
using Backend.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace Backend.Services;

internal record LLMConfig;
internal record OpenAIConfig(string Model, string Key) : LLMConfig;
internal record AzureOpenAIConfig(string Deployment, string Endpoint) : LLMConfig;

internal struct SemanticKernelConfig
{
    internal LLMConfig LLMConfig { get; private init; }

    internal static SemanticKernelConfig Create(IConfiguration config)
    {
        if (bool.TryParse(config["UseAzureOpenAI"], out var b) && b)
        {
            var azureDeployment = config["AzureDeployment"];
            ArgumentException.ThrowIfNullOrWhiteSpace(azureDeployment, "AzureDeployment");

            var azureEndpoint = config["AzureEndpoint"];
            ArgumentException.ThrowIfNullOrWhiteSpace(azureEndpoint, "AzureEndpoint");

            return new SemanticKernelConfig
            {
                LLMConfig = new AzureOpenAIConfig(azureDeployment, azureEndpoint),
            };
        }
        else
        {
            var apiKey = config["APIKey"];
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey, "APIKey");

            var model = config["Model"];
            ArgumentException.ThrowIfNullOrWhiteSpace(model, "Model");

            return new SemanticKernelConfig
            {
                LLMConfig = new OpenAIConfig(model, apiKey),
            };
        }
    }
}

internal class SemanticKernelSession : ISemanticKernelSession
{
    private readonly Kernel _kernel;
    private readonly IStateStore<string> _stateStore;
    private readonly KernelFunction _chatFunction;

    public Guid Id { get; private set; }

    internal SemanticKernelSession(IConfiguration config, Kernel kernel, IStateStore<string> stateStore, Guid sessionId)
    {
        _kernel = kernel;
        _stateStore = stateStore;
        _chatFunction = _kernel.CreateFunctionFromPrompt(config["SystemPrompt"]!);
        this.Id = sessionId;
    }

    public async Task<AIChatCompletion> ProcessRequestAsync(AIChatRequest message)
    {
        AIChatMessage userInput = message.Messages.Last();
        string history = await _stateStore.GetStateAsync(this.Id) ?? "";
        /* TODO: Add support for text+image content */
        var arguments = new KernelArguments()
        {
            ["history"] = history,
            ["userInput"] = userInput.Content,
        };
        FunctionResult botResponse = await _chatFunction.InvokeAsync(_kernel, arguments);
        var updatedHistory = $"{history}\nUser: {userInput.Content}\nChatBot: {botResponse}";
        await _stateStore.SetStateAsync(this.Id, updatedHistory);
        return new AIChatCompletion(Message: new AIChatMessage
        {
            Role = AIChatRole.Assistant,
            Content = $"{botResponse}",
        })
        {
            SessionState = this.Id,
        };
    }

    public async IAsyncEnumerable<AIChatCompletionDelta> ProcessStreamingRequestAsync(AIChatRequest message)
    {
        AIChatMessage userInput = message.Messages.Last();
        string history = await _stateStore.GetStateAsync(this.Id) ?? "";
        var arguments = new KernelArguments()
        {
            ["history"] = history,
            ["userInput"] = userInput.Content,
        };
        IAsyncEnumerable<StreamingKernelContent> streamedBotResponse = _chatFunction.InvokeStreamingAsync(_kernel, arguments);
        StringBuilder response = new();
        await foreach (StreamingKernelContent botResponse in streamedBotResponse)
        {
            response.Append(botResponse);
            yield return new AIChatCompletionDelta(Delta: new AIChatMessageDelta
            {
                Role = AIChatRole.Assistant,
                Content = $"{botResponse}",
            })
            {
                SessionState = this.Id,
            };
        }
        var updatedHistory = $"{history}\nUser: {userInput.Content}\nChatBot: {response}";
        await _stateStore.SetStateAsync(this.Id, updatedHistory);
    }
}

public class SemanticKernelApp : ISemanticKernelApp
{
    private readonly IConfiguration _config;
    private readonly IStateStore<string> _stateStore;
    private readonly Lazy<Kernel> _kernel;

    private Kernel InitKernel()
    {
        var config = SemanticKernelConfig.Create(_config);
        IKernelBuilder builder = Kernel.CreateBuilder();
        if (config.LLMConfig is AzureOpenAIConfig azureOpenAIConfig)
        {
            if (azureOpenAIConfig.Deployment is null || azureOpenAIConfig.Endpoint is null)
            {
                throw new InvalidOperationException("AzureOpenAI is enabled but AzureDeployment and AzureEndpoint are not set.");
            }

            builder.AddAzureOpenAIChatCompletion(azureOpenAIConfig.Deployment, azureOpenAIConfig.Endpoint, new DefaultAzureCredential());
        }
        else if (config.LLMConfig is OpenAIConfig openAIConfig)
        {
            if (openAIConfig.Model is null || openAIConfig.Key is null)
            {
                throw new InvalidOperationException("AzureOpenAI is disabled but Model and APIKey are not set.");
            }

            builder.AddOpenAIChatCompletion(openAIConfig.Model, openAIConfig.Key);
        }
        else
        {
            throw new InvalidOperationException("Unsupported LLMConfig type.");
        }

        return builder.Build();
    }

    public SemanticKernelApp(IConfiguration config, IStateStore<string> stateStore)
    {
        _config = config;
        _stateStore = stateStore;
        _kernel = new(InitKernel);
    }

    public ISemanticKernelSession CreateSession(Guid sessionId)
    {
        Kernel kernel = _kernel.Value;
        return new SemanticKernelSession(_config, kernel, _stateStore, sessionId);
    }

    public ISemanticKernelSession GetSession(Guid sessionId)
    {
        Kernel kernel = _kernel.Value;

        return new SemanticKernelSession(_config, kernel, _stateStore, sessionId);
    }
}
