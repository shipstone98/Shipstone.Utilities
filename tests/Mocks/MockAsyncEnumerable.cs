using System;
using System.Collections.Generic;
using System.Threading;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockAsyncEnumerable<T> : IAsyncEnumerable<T>
{
    internal Func<IAsyncEnumerator<T>> _getAsyncEnumeratorFunc;

    internal MockAsyncEnumerable() =>
        this._getAsyncEnumeratorFunc = () =>
            throw new NotImplementedException();

    IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken) =>
        this._getAsyncEnumeratorFunc();
}
