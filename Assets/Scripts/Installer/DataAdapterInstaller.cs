using System;
using CAFU.Data.DataSource;
using CAFU.Data.Repository;
using Zenject;

namespace CAFU.Data.Installer
{
    public class DataAdapterInstaller : Installer<DataSourceType, DataAdapterInstaller>
    {
        private readonly DataSourceType dataSourceType;

        public DataAdapterInstaller(DataSourceType dataSourceType)
        {
            this.dataSourceType = dataSourceType;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AsyncDataRepository>().AsSingle();
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