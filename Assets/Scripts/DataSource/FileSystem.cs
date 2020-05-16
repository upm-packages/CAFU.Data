using System;
using System.IO;
using System.Threading;
using CAFU.Data.Repository;
using UniRx.Async;

namespace CAFU.Data.DataSource
{
    internal sealed class FileSystem : IAsyncCRUDHandler
    {
        public async UniTask CreateAsync(Uri uri, byte[] data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await ExistsAsync(uri, cancellationToken))
            {
                throw new InvalidOperationException($"File `{GetUnescapedAbsolutePath(uri)}' has already exists.");
            }

            CreateDirectoryIfNeeded(uri);

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            using (var stream = new FileStream(GetUnescapedAbsolutePath(uri), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                throw new FileNotFoundException($"File `{GetUnescapedAbsolutePath(uri)}' does not found.");
            }

            await UniTask.Run(() => File.Delete(GetUnescapedAbsolutePath(uri)));
        }

        public async UniTask<bool> ExistsAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await UniTask.FromResult(File.Exists(GetUnescapedAbsolutePath(uri)));
        }

        private static void CreateDirectoryIfNeeded(Uri uri)
        {
            if (!Directory.Exists(Path.GetDirectoryName(GetUnescapedAbsolutePath(uri))))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(GetUnescapedAbsolutePath(uri)));
            }
        }

        private static string GetUnescapedAbsolutePath(Uri uri)
        {
            return Uri.UnescapeDataString(uri.AbsolutePath);
        }
    }
}