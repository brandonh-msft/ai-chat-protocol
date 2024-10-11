// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;

using System.Text.Json;

public record struct AIChatFile(string Filename, string ContentType, BinaryData Data)
{
    readonly public override string ToString() => JsonSerializer.Serialize(this);
}
