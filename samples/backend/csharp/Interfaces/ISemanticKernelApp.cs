// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Interfaces;

using Microsoft.SemanticKernel;

public interface ISemanticKernelApp
{
    Kernel Kernel { get; }
    ISemanticKernelSession CreateSession(Guid sessionId);
    ISemanticKernelSession GetSession(Guid sessionId);
}
