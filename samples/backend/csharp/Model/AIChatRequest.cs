// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

public record AIChatRequest([MaybeNull] IReadOnlyList<AIChatMessage> Messages)
{
    public Guid? SessionState { get; init; }

    public BinaryData? Context { get; init; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
