// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
using System.Text.Json.Serialization;

public record AIChatRequest([property: JsonPropertyName("messages")] IList<AIChatMessage> Messages)
{
    [JsonInclude, JsonPropertyName("sessionState")]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context")]
    public BinaryData? Context;
}
