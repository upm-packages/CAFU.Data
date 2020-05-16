using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CAFU.Data.Repository;
using UniRx;
using UniRx.Async;

namespace CAFU.Data.DataSource
{
    public class WebRequest : IAsyncCRUDHandler
    {
        public async UniTask CreateAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data.ToArray()).ToUniTask(cancellationToken);
        }

        public async UniTask<IEnumerable<byte>> ReadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await ObservableUnityWebRequest.GetBytesAsObservable(uri.ToString()).ToUniTask(cancellationToken);
        }

        public async UniTask UpdateAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default)
        {
            await ObservableUnityWebRequest.PutAsObservable(uri.ToString(), data.ToArray()).ToUniTask(cancellationToken);
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