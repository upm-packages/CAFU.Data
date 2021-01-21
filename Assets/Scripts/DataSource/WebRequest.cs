using System;
using System.Threading;
using CAFU.Data.Repository;
using Cysharp.Threading.Tasks;
using UniRx;

namespace CAFU.Data.DataSource
{
    // WebRequest の同期版は提供しない

    internal sealed class AsyncWebRequest : IAsyncCRUDHandler
    {
        public async UniTask CreateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<byte[]> ReadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await ObservableUnityWebRequest.GetBytesAsObservable(uri.ToString()).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask UpdateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.DeleteAsObservable(uri.ToString()).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await ObservableUnityWebRequest
                .HeadAsObservable(uri.ToString())
                .Select(_ => true)
                .Catch((UnityWebRequestErrorException e) => Observable.Return(false))
                .ToUniTask(cancellationToken: cancellationToken);
        }
    }
}