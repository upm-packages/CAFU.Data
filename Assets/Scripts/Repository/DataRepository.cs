using System;
using System.Threading;
using Cysharp.Threading.Tasks;

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
                data = dataSerializer.Deserialize(await asyncCRUDHandler.ReadAsync(uri, cancellationToken));
            }

            return data != null ? data : new T();
        }

        public async UniTask SaveAsync(Uri uri, T data, CancellationToken cancellationToken = default)
        {
            if (await asyncCRUDHandler.ExistsAsync(uri, cancellationToken))
            {
                await asyncCRUDHandler.UpdateAsync(uri, dataSerializer.Serialize(data), cancellationToken);
            }
            else
            {
                await asyncCRUDHandler.CreateAsync(uri, dataSerializer.Serialize(data), cancellationToken);
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

    internal sealed class DataRepository<T> : IDataAdapter<T>
        where T : new()
    {
        private readonly ICRUDHandler crudHandler;
        private readonly IDataSerializer<T> dataSerializer;

        public DataRepository(ICRUDHandler crudHandler, IDataSerializer<T> dataSerializer)
        {
            this.crudHandler = crudHandler;
            this.dataSerializer = dataSerializer;
        }

        public T Load(Uri uri)
        {
            T data = default;
            if (crudHandler.Exists(uri))
            {
                data = dataSerializer.Deserialize(crudHandler.Read(uri));
            }

            return data != null ? data : new T();
        }

        public void Save(Uri uri, T data)
        {
            if (crudHandler.Exists(uri))
            {
                crudHandler.Update(uri, dataSerializer.Serialize(data));
            }
            else
            {
                crudHandler.Create(uri, dataSerializer.Serialize(data));
            }
        }

        public void Delete(Uri uri)
        {
            if (crudHandler.Exists(uri))
            {
                crudHandler.Delete(uri);
            }
        }

        public bool Exists(Uri uri)
        {
            return crudHandler.Exists(uri);
        }
    }
}