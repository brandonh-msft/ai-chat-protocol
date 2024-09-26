// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Interfaces;

using System.Threading;

public interface IStateStore<T>
{
    Task<T?> GetStateAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task SetStateAsync(Guid sessionId, T state, CancellationToken cancellationToken = default);
    Task RemoveStateAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
