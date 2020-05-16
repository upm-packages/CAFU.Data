using System;
using System.Threading;
using CAFU.Data.Repository;
using UniRx;
using UniRx.Async;

namespace CAFU.Data.DataSource
{
    internal sealed class WebRequest : IAsyncCRUDHandler
    {
        public async UniTask CreateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data).ToUniTask(cancellationToken);
        }

        public async UniTask<byte[]> ReadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await ObservableUnityWebRequest.GetBytesAsObservable(uri.ToString()).ToUniTask(cancellationToken);
        }

        public async UniTask UpdateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data).ToUniTask(cancellationToken);
        }

        public async UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.DeleteAsObservable(uri.ToString()).ToUniTask(cancellationToken);
        }

        public async UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await ObservableUnityWebRequest
                .HeadAsObservable(uri.ToString())
                .Select(_ => true)
                .Catch((UnityWebRequestErrorException e) => Observable.Return(false))
                .ToUniTask(cancellationToken);
        }
    }
}