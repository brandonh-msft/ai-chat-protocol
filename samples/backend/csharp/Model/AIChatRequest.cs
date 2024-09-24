// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
public record AIChatRequest(IReadOnlyList<AIChatMessage> Messages)
{
    public Guid? SessionState { get; init; }

    public BinaryData? Context { get; init; }
}
