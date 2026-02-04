using System;
using System.Security.Cryptography;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockRandomNumberGenerator : RandomNumberGenerator
{
    internal Action<bool> _disposeAction;
    internal Func<Object?, bool> _equalsFunc;
    internal Action<byte[]> _getBytesArrayAction;
    internal Action<byte[], int, int> _getBytesArrayInt32Int32Action;
    internal Action<Span<byte>> _getBytesSpanAction;
    internal Func<int> _getHashCodeFunc;
    internal Action<byte[]> _getNonZeroBytesArrayAction;
    internal Action<Span<byte>> _getNonZeroBytesSpanAction;
    internal Func<String?> _toStringFunc;

    internal MockRandomNumberGenerator()
    {
        this._disposeAction = _ => throw new NotImplementedException();
        this._equalsFunc = _ => throw new NotImplementedException();
        this._getBytesArrayAction = _ => throw new NotImplementedException();

        this._getBytesArrayInt32Int32Action = (_, _, _) =>
            throw new NotImplementedException();

        this._getBytesSpanAction = _ => throw new NotImplementedException();
        this._getHashCodeFunc = () => throw new NotImplementedException();

        this._getNonZeroBytesArrayAction = _ =>
            throw new NotImplementedException();

        this._getNonZeroBytesSpanAction = _ =>
            throw new NotImplementedException();

        this._toStringFunc = () => throw new NotImplementedException();
    }

    protected sealed override void Dispose(bool disposing) =>
        this._disposeAction(disposing);

    public sealed override bool Equals(Object? obj) => this._equalsFunc(obj);

    public sealed override void GetBytes(byte[] data) =>
        this._getBytesArrayAction(data);

    public sealed override void GetBytes(byte[] data, int offset, int count) =>
        this._getBytesArrayInt32Int32Action(data, offset, count);

    public sealed override void GetBytes(Span<byte> data) =>
        this._getBytesSpanAction(data);

    public sealed override int GetHashCode() => this._getHashCodeFunc();

    public sealed override void GetNonZeroBytes(byte[] data) =>
        this._getNonZeroBytesArrayAction(data);

    public sealed override void GetNonZeroBytes(Span<byte> data) =>
        this._getNonZeroBytesSpanAction(data);

    public sealed override String? ToString() => this._toStringFunc();
}
