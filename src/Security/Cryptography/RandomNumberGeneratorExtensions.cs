using System;
using System.Security.Cryptography;
using System.Text;

namespace Shipstone.Utilities.Security.Cryptography;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that extend <see cref="RandomNumberGenerator" />.
/// </summary>
public static class RandomNumberGeneratorExtensions
{
    /// <summary>
    /// Returns a string containing a cryptographically strong random sequence of non-zero values.
    /// </summary>
    /// <param name="rng">The <see cref="RandomNumberGenerator" /> to use when generating values.</param>
    /// <param name="length">The length of the string to return.</param>
    /// <returns>A string containing a cryptographically strong random sequence of non-zero values.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="rng" /></c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="length" /></c> is less than 0 (zero).</exception>
    public static String GetNonZeroString(
        this RandomNumberGenerator rng,
        int length
    )
    {
        ArgumentNullException.ThrowIfNull(rng);
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);

        Span<byte> bytes =
            length < 1024 ? stackalloc byte[length] : new byte[length];

        rng.GetNonZeroBytes(bytes);

        for (int i = 0; i < length; i ++)
        {
            bytes[i] = (byte) ((bytes[i] % 9) + '1');
        }

        return Encoding.ASCII.GetString(bytes);
    }
}
