// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Interfaces;

using System.Threading;

public interface IStateStore<T>
{
    Task<T?> GetAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<T> GetOrCreateAsync(Guid sessionId, T valueIfNotFound, CancellationToken cancellationToken = default);
    Task SetAsync(Guid sessionId, T state, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
