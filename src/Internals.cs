using System.Text.RegularExpressions;

namespace Shipstone.Utilities;

internal static partial class Internals
{
    internal static readonly Regex _whiteSpacePattern;

    static Internals() =>
        Internals._whiteSpacePattern = Internals.GenerateWhiteSpacePattern();

    [GeneratedRegex(@"[\s]+")]
    private static partial Regex GenerateWhiteSpacePattern();
}
