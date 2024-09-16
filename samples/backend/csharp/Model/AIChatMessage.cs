// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;

using System.Diagnostics;
using System.Text.Json.Serialization;

[DebuggerDisplay("{Role}: {Content}")]
public record AIChatMessage(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("role")] AIChatRole Role)
{
    [JsonPropertyName("context")]
    public BinaryData? Context { get; set; }

    [JsonPropertyName("files")]
    public IList<AIChatFile>? Files { get; set; }
}