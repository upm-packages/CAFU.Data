using System;
using System.Threading;
using UniRx.Async;

namespace CAFU.Data.Repository
{
    internal sealed class AsyncDataRepository<T> : IAsyncDataAdapter<T>
        where T : new()
    {
        private readonly IAsyncCRUDHandler asyncCRUDHandler;
        private readonly IDataSerializer<T> dataSerializer;

        public AsyncDataRepository(IAsyncCRUDHandler asyncCRUDHandler, IDataSerializer<T> dataSerializer)
        {
            this.asyncCRUDHandler = asyncCRUDHandler;
            this.dataSerializer = dataSerializer;
        }

        public async UniTask<T> LoadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            T data = default;
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                data = dataSerializer.Serialize(await asyncCRUDHandler.ReadAsync(uri, cancellationToken));
            }

            return data != null ? data : new T();
        }

        public async UniTask SaveAsync(Uri uri, T data, CancellationToken cancellationToken = default)
        {
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                await asyncCRUDHandler.UpdateAsync(uri, dataSerializer.Deserialize(data), cancellationToken);
            }
            else
            {
                await asyncCRUDHandler.CreateAsync(uri, dataSerializer.Deserialize(data), cancellationToken);
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