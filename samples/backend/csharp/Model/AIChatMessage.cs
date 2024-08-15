// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
using System.Text.Json.Serialization;

public struct AIChatMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("role")]
    public AIChatRole Role { get; set; }

    [JsonPropertyName("context")]
    public BinaryData? Context { get; set; }

    [JsonPropertyName("files")]
    public IList<AIChatFile>? Files { get; set; }
}

