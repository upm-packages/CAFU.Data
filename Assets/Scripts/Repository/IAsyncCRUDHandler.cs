using System;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;

namespace CAFU.Data.Repository
{
    public interface IAsyncCRUDHandler
    {
        UniTask CreateAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default);
        UniTask<IEnumerable<byte>> ReadAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask UpdateAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default);
        UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default);
    }
}