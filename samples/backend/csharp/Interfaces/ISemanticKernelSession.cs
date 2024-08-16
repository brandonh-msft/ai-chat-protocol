// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Interfaces;
using Backend.Model;

public interface ISemanticKernelSession
{
    Guid Id { get; }
    Task<AIChatCompletion> ProcessRequestAsync(AIChatRequest request);
    IAsyncEnumerable<AIChatCompletionDelta> ProcessStreamingRequestAsync(AIChatRequest request);
}
