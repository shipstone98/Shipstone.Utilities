using System;
using System.Net;
using Shipstone.Utilities.IO;

namespace Shipstone.UtilitiesTest.Mocks;

internal sealed class MockNcsaCommonLog : INcsaCommonLog
{
    internal Func<String?> _authenticatedUserFunc;
    internal Func<Nullable<long>> _contentLengthFunc;
    internal Func<IPAddress?> _hostFunc;
    internal Func<String?> _identityFunc;
    internal Func<Nullable<DateTime>> _receivedFunc;
    internal Func<String?> _requestLineFunc;
    internal Func<Nullable<HttpStatusCode>> _statusCodeFunc;

    String? INcsaCommonLog.AuthenticatedUser => this._authenticatedUserFunc();

    Nullable<long> INcsaCommonLog.ContentLength => this._contentLengthFunc();
    IPAddress? INcsaCommonLog.Host => this._hostFunc();
    String? INcsaCommonLog.Identity => this._identityFunc();
    Nullable<DateTime> INcsaCommonLog.Received => this._receivedFunc();
    String? INcsaCommonLog.RequestLine => this._requestLineFunc();

    Nullable<HttpStatusCode> INcsaCommonLog.StatusCode =>
        this._statusCodeFunc();

    internal MockNcsaCommonLog()
    {
        this._authenticatedUserFunc = () =>
            throw new NotImplementedException();

        this._contentLengthFunc = () => throw new NotImplementedException();
        this._hostFunc = () => throw new NotImplementedException();
        this._identityFunc = () => throw new NotImplementedException();
        this._receivedFunc = () => throw new NotImplementedException();
        this._requestLineFunc = () => throw new NotImplementedException();
        this._statusCodeFunc = () => throw new NotImplementedException();
    }
}
