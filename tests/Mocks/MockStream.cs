using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockStream : Stream
{
    internal Func<bool> _canWriteFunc;
    internal Action _closeAction;
    internal Action _flushAction;
    internal Action<ReadOnlySpan<byte>> _writeAction;

    public sealed override bool CanRead => throw new NotImplementedException();
    public sealed override bool CanSeek => throw new NotImplementedException();

    public sealed override bool CanTimeout =>
        throw new NotImplementedException();

    public sealed override bool CanWrite => this._canWriteFunc();
    public sealed override long Length => throw new NotImplementedException();

    public sealed override long Position
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public sealed override int ReadTimeout
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public sealed override int WriteTimeout
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    internal MockStream()
    {
        this._canWriteFunc = () => throw new NotImplementedException();
        this._closeAction = () => throw new NotImplementedException();
        this._flushAction = () => throw new NotImplementedException();
        this._writeAction = _ => throw new NotImplementedException();
    }

    public sealed override IAsyncResult BeginRead(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback? callback,
        Object? state
    ) =>
        throw new NotImplementedException();

    public sealed override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback? callback,
        Object? state
    ) =>
        throw new NotImplementedException();

    public sealed override void Close() => this._closeAction();

    public sealed override void CopyTo(Stream destination, int bufferSize) =>
        throw new NotImplementedException();

    public sealed override Task CopyToAsync(
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken
    ) =>
        throw new NotImplementedException();

    [Obsolete]
    protected sealed override WaitHandle CreateWaitHandle() =>
        throw new NotImplementedException();

    protected sealed override void Dispose(bool disposing) =>
        throw new NotImplementedException();

    public sealed override ValueTask DisposeAsync() =>
        throw new NotImplementedException();

    public sealed override int EndRead(IAsyncResult asyncResult) =>
        throw new NotImplementedException();

    public sealed override void EndWrite(IAsyncResult asyncResult) =>
        throw new NotImplementedException();

    public sealed override bool Equals(Object? obj) =>
        throw new NotImplementedException();

    public sealed override void Flush() => this._flushAction();

    public sealed override Task FlushAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public sealed override int GetHashCode() =>
        throw new NotImplementedException();

    [Obsolete]
    public sealed override object InitializeLifetimeService() =>
        throw new NotImplementedException();

    [Obsolete]
    protected sealed override void ObjectInvariant() =>
        throw new NotImplementedException();

    public sealed override int Read(byte[] buffer, int offset, int count) =>
        throw new NotImplementedException();

    public sealed override int Read(Span<byte> buffer) =>
        throw new NotImplementedException();

    public sealed override Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken
    ) =>
        throw new NotImplementedException();

    public sealed override ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default
    ) =>
        throw new NotImplementedException();

    public sealed override int ReadByte() =>
        throw new NotImplementedException();

    public sealed override long Seek(long offset, SeekOrigin origin) =>
        throw new NotImplementedException();

    public sealed override void SetLength(long value) =>
        throw new NotImplementedException();

    public sealed override String ToString() =>
        throw new NotImplementedException();

    public sealed override void Write(byte[] buffer, int offset, int count) =>
        throw new NotImplementedException();

    public sealed override void Write(ReadOnlySpan<byte> buffer) =>
        this._writeAction(buffer);

    public sealed override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken
    ) =>
        throw new NotImplementedException();

    public sealed override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default
    ) =>
        throw new NotImplementedException();

    public sealed override void WriteByte(byte value) =>
        throw new NotImplementedException();
}
