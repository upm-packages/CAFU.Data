using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Data
{
    public interface IAsyncDataAdapter<T>
    {
        UniTask<T> LoadAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask SaveAsync(Uri uri, T data, CancellationToken cancellationToken = default);
        UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default);
        UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default);
    }
}