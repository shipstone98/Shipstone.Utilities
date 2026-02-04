using System;
using System.Security.Cryptography;
using System.Threading;

namespace Shipstone.Utilities.Security.Cryptography;

/// <summary>
/// Provides thread-safe functionality for generating random values.
/// </summary>
public sealed class ConcurrentRandomNumberGenerator : RandomNumberGenerator
{
    private readonly Lock _locker;
    private readonly RandomNumberGenerator _rng;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentRandomNumberGenerator" /> that is a thread-safe wrapper around the specified <see cref="RandomNumberGenerator" />.
    /// </summary>
    /// <param name="rng">The <see cref="RandomNumberGenerator" /> to wrap.</param>
    /// <exception cref="ArgumentNullException"><c><paramref name="rng" /></c> is <c>null</c>.</exception>
    public ConcurrentRandomNumberGenerator(RandomNumberGenerator rng)
    {
        ArgumentNullException.ThrowIfNull(rng);
        this._locker = new();
        this._rng = rng;
    }

    /// <inheritdoc />
    protected sealed override void Dispose(bool disposing)
    {
        lock (this._locker)
        {
            this._rng.Dispose();
        }
    }

    /// <inheritdoc />
    public sealed override bool Equals(Object? obj)
    {
        lock (this._locker)
        {
            return obj is ConcurrentRandomNumberGenerator crng
                ? this._rng.Equals(crng._rng)
                : obj is RandomNumberGenerator rng && this._rng.Equals(rng);
        }
    }

    /// <inheritdoc />
    public sealed override void GetBytes(byte[] data)
    {
        lock (this._locker)
        {
            this._rng.GetBytes(data);
        }
    }

    /// <inheritdoc />
    public sealed override void GetBytes(byte[] data, int offset, int count)
    {
        lock (this._locker)
        {
            this._rng.GetBytes(data, offset, count);
        }
    }

    /// <inheritdoc />
    public sealed override void GetBytes(Span<byte> data)
    {
        lock (this._locker)
        {
            this._rng.GetBytes(data);
        }
    }

    /// <inheritdoc />
    public sealed override int GetHashCode()
    {
        lock (this._locker)
        {
            return this._rng.GetHashCode();
        }
    }

    /// <inheritdoc />
    public sealed override void GetNonZeroBytes(byte[] data)
    {
        lock (this._locker)
        {
            this._rng.GetNonZeroBytes(data);
        }
    }

    /// <inheritdoc />
    public sealed override void GetNonZeroBytes(Span<byte> data)
    {
        lock (this._locker)
        {
            this._rng.GetNonZeroBytes(data);
        }
    }

    /// <inheritdoc />
    public sealed override String? ToString()
    {
        lock (this._locker)
        {
            return this._rng.ToString();
        }
    }
}
