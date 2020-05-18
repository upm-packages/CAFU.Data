using System;
using System.IO;
using System.Threading;
using CAFU.Data.Repository;
using UniRx.Async;

namespace CAFU.Data.DataSource
{
    internal sealed class FileSystem : ICRUDHandler
    {
        public void Create(Uri uri, byte[] data)
        {
            if (Exists(uri))
            {
                throw new InvalidOperationException($"File `{GetUnescapedAbsolutePath(uri)}' has already exists.");
            }

            CreateDirectoryIfNeeded(uri);

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                data = data ?? new byte[0];
                stream.Write(data, 0, data.Length);
            }
        }

        public byte[] Read(Uri uri)
        {
            if (!Exists(uri))
            {
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, (int) stream.Length);
                return data;
            }
        }

        public void Update(Uri uri, byte[] data)
        {
            if (!Exists(uri))
            {
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                data = data ?? new byte[0];
                stream.Write(data, 0, data.Length);
            }
        }

        public void Delete(Uri uri)
        {
            if (!Exists(uri))
            {
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            File.Delete(GetUnescapedAbsolutePath(uri));
        }

        public bool Exists(Uri uri)
        {
            return File.Exists(GetUnescapedAbsolutePath(uri));
        }

        internal static void CreateDirectoryIfNeeded(Uri uri)
        {
            if (!Directory.Exists(Path.GetDirectoryName(GetUnescapedAbsolutePath(uri))))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(GetUnescapedAbsolutePath(uri)));
            }
        }

        internal static string GetUnescapedAbsolutePath(Uri uri)
        {
            return Uri.UnescapeDataString(uri.AbsolutePath);
        }
    }

    internal sealed class AsyncFileSystem : IAsyncCRUDHandler
    {
        public async UniTask CreateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await ExistsAsync(uri, cancellationToken))
            {
                throw new InvalidOperationException($"File `{FileSystem.GetUnescapedAbsolutePath(uri)}' has already exists.");
            }

            FileSystem.CreateDirectoryIfNeeded(uri);

            using (var stream = new FileStream(FileSystem.GetUnescapedAbsolutePath(uri), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                data = data ?? new byte[0];
                await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            }
        }

        public async UniTask<byte[]> ReadAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await ExistsAsync(uri, cancellationToken))
            {
                throw new FileNotFoundException($"File `{FileSystem.GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(FileSystem.GetUnescapedAbsolutePath(uri), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var data = new byte[stream.Length];
                await stream.ReadAsync(data, 0, (int) stream.Length, cancellationToken);
                return data;
            }
        }

        public async UniTask UpdateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await ExistsAsync(uri, cancellationToken))
            {
                throw new FileNotFoundException($"File `{FileSystem.GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(FileSystem.GetUnescapedAbsolutePath(uri), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                data = data ?? new byte[0];
                await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            }
        }

        public async UniTask DeleteAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await ExistsAsync(uri, cancellationToken))
            {
                throw new FileNotFoundException($"File `{FileSystem.GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            await UniTask.Run(() => File.Delete(FileSystem.GetUnescapedAbsolutePath(uri)));
        }

        public async UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await UniTask.FromResult(File.Exists(FileSystem.GetUnescapedAbsolutePath(uri)));
        }
    }
}