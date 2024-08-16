// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Interfaces;

public interface ISemanticKernelApp
{
    ISemanticKernelSession CreateSession(Guid sessionId);
    ISemanticKernelSession GetSession(Guid sessionId);
}
