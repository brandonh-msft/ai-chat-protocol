// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

public record AIChatCompletion
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "EA0000:Use source generated logging methods for improved performance", Justification = "Can't do this in a linked file")]
    public AIChatCompletion(AIChatMessage message, ILogger? log = null)
    {
        if (message.Role is not AIChatRole.Assistant)
        {
            log?.LogWarning("Completion created for non-assistant role: {CompletionRole}", message.Role);
        }

        this.Message = message;
    }

    [JsonInclude, JsonPropertyName("message"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AIChatMessage Message { get; }

    [JsonInclude, JsonPropertyName("sessionState"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context;
}
