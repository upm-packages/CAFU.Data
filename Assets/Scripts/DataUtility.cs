using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace CAFU.Data
{
    [PublicAPI]
    public static class DataUtility
    {
        public static Uri GeneratePersistentFileUriByType<T>(string directory = "", string extension = ".bytes", string prefix = "", string suffix = "")
        {
            return GenerateFileUriByType<T>(Application.persistentDataPath, directory, extension, prefix, suffix);
        }

        public static Uri GenerateTemporaryFileUriByType<T>(string directory = "", string extension = ".bytes", string prefix = "", string suffix = "")
        {
            return GenerateFileUriByType<T>(Application.temporaryCachePath, directory, extension, prefix, suffix);
        }

        private static Uri GenerateFileUriByType<T>(string rootPath, string directory, string extension, string prefix, string suffix)
        {
            return new UriBuilder
            {
                Scheme = "file",
                Host = string.Empty,
                Path = Path.Combine(rootPath, directory, $"{prefix}{typeof(T).Namespace}.{typeof(T).Name}{suffix}{extension}"),
            }.Uri;
        }
    }
}