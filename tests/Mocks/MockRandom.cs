using System;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockRandom : Random
{
    internal Func<int, int, int> _nextFunc;

    internal MockRandom() =>
        this._nextFunc = (_, _) => throw new NotImplementedException();

    public sealed override bool Equals(Object? obj) =>
        throw new NotImplementedException();

    public sealed override int GetHashCode() =>
        throw new NotImplementedException();

    public sealed override int Next() =>
        throw new NotImplementedException();

    public sealed override int Next(int maxValue) =>
        throw new NotImplementedException();

    public sealed override int Next(int minValue, int maxValue) =>
        this._nextFunc(minValue, maxValue);

    public sealed override void NextBytes(byte[] buffer) =>
        throw new NotImplementedException();

    public sealed override void NextBytes(Span<byte> buffer) =>
        throw new NotImplementedException();

    public sealed override double NextDouble() =>
        throw new NotImplementedException();

    public sealed override long NextInt64() =>
        throw new NotImplementedException();

    public sealed override long NextInt64(long maxValue) =>
        throw new NotImplementedException();

    public sealed override long NextInt64(long minValue, long maxValue) =>
        throw new NotImplementedException();

    public sealed override float NextSingle() =>
        throw new NotImplementedException();

    protected sealed override double Sample() =>
        throw new NotImplementedException();

    public sealed override String? ToString() =>
        throw new NotImplementedException();
}
