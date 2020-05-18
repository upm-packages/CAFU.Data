using System;

namespace CAFU.Data
{
    public interface IDataAdapter<T>
    {
        T Load(Uri uri);
        void Save(Uri uri, T data);
        void Delete(Uri uri);
        bool Exists(Uri uri);
    }
}