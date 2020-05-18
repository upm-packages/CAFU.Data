using System;
using System.Collections.Generic;
using CAFU.Data.DataSerializer;
using CAFU.Data.DataSource;
using CAFU.Data.Repository;
using JetBrains.Annotations;
using Zenject;

namespace CAFU.Data.Installer
{
    [PublicAPI]
    public sealed class DataAdapterInstaller<T> : Installer<DataSourceType, SynchronizeMode, DataAdapterInstaller<T>>
        where T : new()
    {
        private readonly DataSourceType dataSourceType;
        private readonly SynchronizeMode synchronizeMode;

        public DataAdapterInstaller(DataSourceType dataSourceType, SynchronizeMode synchronizeMode)
        {
            this.dataSourceType = dataSourceType;
            this.synchronizeMode = synchronizeMode;
        }

        public override void InstallBindings()
        {
            DataAdapterInstaller<T, JsonUtility<T>>.Install(Container, JsonUtility<T>.Default, dataSourceType, synchronizeMode);
        }

        public static void Install(DiContainer container)
        {
            Install(container, DataSourceType.FileSystem, SynchronizeMode.Async);
        }

        public static void Install(DiContainer container, DataSourceType dataSourceType)
        {
            Install(container, dataSourceType, SynchronizeMode.Async);
        }
    }

    [PublicAPI]
    public sealed class DataAdapterInstaller<T, TDataSerializer> : Installer<TDataSerializer, DataSourceType, SynchronizeMode, DataAdapterInstaller<T, TDataSerializer>>
        where T : new()
        where TDataSerializer : IDataSerializer<T>
    {
        private readonly DataSourceType dataSourceType;
        private readonly SynchronizeMode synchronizeMode;
        private readonly TDataSerializer dataSerializer;

        public DataAdapterInstaller(DataSourceType dataSourceType, SynchronizeMode synchronizeMode, TDataSerializer dataSerializer)
        {
            this.dataSourceType = dataSourceType;
            this.synchronizeMode = synchronizeMode;
            this.dataSerializer = dataSerializer;
        }

        public override void InstallBindings()
        {
            if (!TypeMap.Interface.ContainsKey(synchronizeMode))
            {
                throw new ArgumentException($"Interface for '{synchronizeMode}' does not defined.");
            }
            if (!TypeMap.Repository.ContainsKey(synchronizeMode))
            {
                throw new ArgumentException($"Repository for '{synchronizeMode}' does not defined.");
            }
            if (!TypeMap.DataSource.ContainsKey((dataSourceType, synchronizeMode)))
            {
                throw new ArgumentException($"DataSource type '{dataSourceType}' for '{synchronizeMode}' does not defined.");
            }

            Container
                .Bind(TypeMap.Interface[synchronizeMode].MakeGenericType(typeof(T)))
                .FromSubContainerResolve()
                .ByMethod(
                    subContainer =>
                    {
                        subContainer.Bind<IDataSerializer<T>>().FromInstance(dataSerializer).AsSingle();
                        subContainer.BindInterfacesTo(TypeMap.Repository[synchronizeMode].MakeGenericType(typeof(T))).AsSingle();
                        subContainer.BindInterfacesTo(TypeMap.DataSource[(dataSourceType, synchronizeMode)]).AsSingle();
                    }
                ).AsSingle();
        }

        public static void Install(DiContainer container, TDataSerializer dataSerializer)
        {
            Install(container, dataSerializer, DataSourceType.FileSystem, SynchronizeMode.Async);
        }

        public static void Install(DiContainer container, TDataSerializer dataSerializer, DataSourceType dataSourceType)
        {
            Install(container, dataSerializer, dataSourceType, SynchronizeMode.Async);
        }
    }

    internal static class TypeMap
    {
        internal static readonly IDictionary<SynchronizeMode, Type> Interface = new Dictionary<SynchronizeMode, Type>
        {
            {SynchronizeMode.Sync, typeof(IDataAdapter<>)},
            {SynchronizeMode.Async, typeof(IAsyncDataAdapter<>)},
        };
        internal static readonly IDictionary<SynchronizeMode, Type> Repository = new Dictionary<SynchronizeMode, Type>
        {
            {SynchronizeMode.Sync, typeof(DataRepository<>)},
            {SynchronizeMode.Async, typeof(AsyncDataRepository<>)},
        };
        internal static readonly IDictionary<(DataSourceType, SynchronizeMode), Type> DataSource = new Dictionary<(DataSourceType, SynchronizeMode), Type>
        {
            {(DataSourceType.FileSystem, SynchronizeMode.Sync), typeof(FileSystem)},
            {(DataSourceType.FileSystem, SynchronizeMode.Async), typeof(AsyncFileSystem)},
            {(DataSourceType.WebRequest, SynchronizeMode.Async), typeof(AsyncWebRequest)},
        };
    }
}