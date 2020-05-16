using System;
using System.Threading;
using UniRx.Async;

namespace CAFU.Data.Repository
{
    internal interface IAsyncCRUDHandler
    {
        UniTask CreateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default);
        UniTask<byte[]> ReadAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask UpdateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default);
        UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default);
    }
}