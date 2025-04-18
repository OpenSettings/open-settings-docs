using Microsoft.Extensions.ObjectPool;
using System;
using System.Text;
using System.Threading;

namespace OpenSettings.Docs
{
    internal sealed class StringBuilderPool : IDisposable
    {
        private static readonly Lazy<ObjectPool<StringBuilder>> LazyPool =
            new Lazy<ObjectPool<StringBuilder>>(() => new DefaultObjectPoolProvider().CreateStringBuilderPool(), LazyThreadSafetyMode.ExecutionAndPublication);

        private bool _disposed;

        private readonly ObjectPool<StringBuilder> _stringBuilderPool;

        public StringBuilderPool()
        {
            _stringBuilderPool = LazyPool.Value;
            Builder = _stringBuilderPool.Get();
        }

        public StringBuilder Builder { get; }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _stringBuilderPool.Return(Builder);
            _disposed = true;
        }
    }
}