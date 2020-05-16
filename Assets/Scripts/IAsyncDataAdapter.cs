using System;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;

namespace CAFU.Data
{
    public interface IAsyncDataAdapter
    {
        UniTask<IEnumerable<byte>> LoadAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask SaveAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default);
        UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default);
    }
}