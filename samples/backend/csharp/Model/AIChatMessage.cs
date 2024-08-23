// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;

using System.Diagnostics;
using System.Text.Json.Serialization;

[DebuggerDisplay("{Role}: {Content}")]
public record struct AIChatMessage(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("role")] AIChatRole Role,
    [property: JsonPropertyName("context")] BinaryData? Context = null,
    [property: JsonPropertyName("files")] IList<AIChatFile>? Files = null);