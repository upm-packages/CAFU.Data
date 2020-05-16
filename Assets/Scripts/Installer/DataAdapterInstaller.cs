using System;
using CAFU.Data.DataSerializer;
using CAFU.Data.DataSource;
using CAFU.Data.Repository;
using Zenject;

namespace CAFU.Data.Installer
{
    public class DataAdapterInstaller<T> : Installer<DataSourceType, DataAdapterInstaller<T>>
        where T : new()
    {
        private readonly DataSourceType dataSourceType;

        public DataAdapterInstaller(DataSourceType dataSourceType)
        {
            this.dataSourceType = dataSourceType;
        }

        public override void InstallBindings()
        {
            DataAdapterInstaller<T, JsonUtility<T>>.Install(Container, dataSourceType, JsonUtility<T>.Default);
        }
    }

    public class DataAdapterInstaller<T, TDataSerializer> : Installer<DataSourceType, TDataSerializer, DataAdapterInstaller<T, TDataSerializer>>
        where T : new()
        where TDataSerializer : IDataSerializer<T>
    {
        private readonly DataSourceType dataSourceType;
        private readonly TDataSerializer dataSerializer;

        public DataAdapterInstaller(DataSourceType dataSourceType, TDataSerializer dataSerializer)
        {
            this.dataSourceType = dataSourceType;
            this.dataSerializer = dataSerializer;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AsyncDataRepository<T>>().AsSingle();
            Container.Bind<IDataSerializer<T>>().FromInstance(dataSerializer).AsSingle();
            switch (dataSourceType)
            {
                case DataSourceType.FileSystem:
                    Container.BindInterfacesTo<FileSystem>().AsSingle();
                    break;
                case DataSourceType.WebRequest:
                    Container.BindInterfacesTo<WebRequest>().AsSingle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}