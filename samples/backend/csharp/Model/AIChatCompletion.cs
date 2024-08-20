// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public record AIChatCompletion
{
    public AIChatCompletion(AIChatMessage message, ILogger? log = null)
    {
        if (message.Role is not AIChatRole.Assistant)
        {
            log?.LogWarning("Completion created for non-assistant role: {CompletionRole}", message.Role);
        }

        this.Message = message;
    }

    [JsonInclude, JsonPropertyName("message"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AIChatMessage Message { get; }

    [JsonInclude, JsonPropertyName("sessionState"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context;

    public static implicit operator AIChatCompletion(OpenAIChatMessageContent openAiCompletion)
    {
        return new(
            new AIChatMessage
            {
                Content = openAiCompletion.ToString(),
                Context = new BinaryData(openAiCompletion.InnerContent),
                Role = AIChatRole.Assistant
            });
    }
}
