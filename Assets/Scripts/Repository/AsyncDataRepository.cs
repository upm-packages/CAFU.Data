using System;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;

namespace CAFU.Data.Repository
{
    public class AsyncDataRepository : IAsyncDataAdapter
    {
        private readonly IAsyncCRUDHandler asyncCRUDHandler;

        public AsyncDataRepository(IAsyncCRUDHandler asyncCRUDHandler)
        {
            this.asyncCRUDHandler = asyncCRUDHandler;
        }

        public async UniTask<IEnumerable<byte>> LoadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                return await asyncCRUDHandler.ReadAsync(uri, cancellationToken);
            }

            return new byte[0];
        }

        public async UniTask SaveAsync(Uri uri, IEnumerable<byte> data, CancellationToken cancellationToken = default)
        {
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                await asyncCRUDHandler.UpdateAsync(uri, data, cancellationToken);
            }
            else
            {
                await asyncCRUDHandler.CreateAsync(uri, data, cancellationToken);
            }
        }

        public async UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                await asyncCRUDHandler.DeleteAsync(uri, cancellationToken);
            }
        }

        public async UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await asyncCRUDHandler.ExistsAsync(uri, cancellationToken);
        }
    }
}