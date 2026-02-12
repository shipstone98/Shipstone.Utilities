using System;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockFormatProvider : IFormatProvider
{
    internal Func<Type?, Object?> _getFormatFunc;

    internal MockFormatProvider() =>
        this._getFormatFunc = _ => throw new NotImplementedException();

    Object? IFormatProvider.GetFormat(Type? formatType) =>
        this._getFormatFunc(formatType);
}
