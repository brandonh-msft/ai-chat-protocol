// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
using System.Text.Json.Serialization;

public record AIChatCompletion([property: JsonPropertyName("message")] AIChatMessage Message)
{
    [JsonInclude, JsonPropertyName("sessionState"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context;
}
