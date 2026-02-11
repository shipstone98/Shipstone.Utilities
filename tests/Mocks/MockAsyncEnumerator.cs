using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    internal Func<T> _currentFunc;
    internal Action _disposeAction;
    internal Func<bool> _moveNextFunc;

    T IAsyncEnumerator<T>.Current => this._currentFunc();

    internal MockAsyncEnumerator()
    {
        this._currentFunc = () => throw new NotImplementedException();
        this._disposeAction = () => throw new NotImplementedException();
        this._moveNextFunc = () => throw new NotImplementedException();
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        this._disposeAction();
        return ValueTask.CompletedTask;
    }

    ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
    {
        bool result = this._moveNextFunc();
        return ValueTask.FromResult(result);
    }
}
