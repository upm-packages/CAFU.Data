using System;

namespace CAFU.Data.Repository
{
    internal interface ICRUDHandler
    {
        void Create(Uri uri, byte[] data);
        byte[] Read(Uri uri);
        void Update(Uri uri, byte[] data);
        void Delete(Uri uri);
        bool Exists(Uri uri);
    }
}