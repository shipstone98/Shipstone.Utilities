using System;
using System.Globalization;
using System.Net;
using Xunit;

using Shipstone.Utilities.IO;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.IO;

public sealed class NcsaCommonLogExtensionsTest
{
#region Format method
    [Fact]
    public void TestFormat_Invalid()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                NcsaCommonLogExtensions.Format(null!));

        // Assert
        Assert.Equal("log", ex.ParamName);
    }

#region Valid arguments
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        null,
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "- ident johndoe2025 [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        null,
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 - johndoe2025 [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        null,
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident - [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        null,
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 +00:00 13:17:19] - 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        null,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" - 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        null,
        "127.0.0.1 ident johndoe2025 [23/07/1993 +00:00 13:17:19] \"GET /index.html HTTP/1.1\" 500 -"
    )]
    [Theory]
    public void TestFormat_Valid_ReceivedNotNull_ProviderNotNull(
        String? hostString,
        String? identity,
        String? authenticatedUser,
        String? requestLine,
        Nullable<HttpStatusCode> statusCode,
        Nullable<long> contentLength,
        String resultExpected
    )
    {
        // Arrange
        MockNcsaCommonLog log = new();
        MockFormatProvider provider = new();

        log._receivedFunc = () =>
            new(1993, 7, 23, 13, 17, 19, DateTimeKind.Utc);

        provider._getFormatFunc = t =>
            new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy zzz",
                ShortTimePattern = "HH:mm:ss",
            };

        log._requestLineFunc = () => requestLine;
        log._statusCodeFunc = () => statusCode;

        log._hostFunc = () =>
            hostString is null ? null : IPAddress.Parse(hostString);

        log._identityFunc = () => identity;
        log._authenticatedUserFunc = () => authenticatedUser;
        log._contentLengthFunc = () => contentLength;

        // Act
        String resultActual = NcsaCommonLogExtensions.Format(log, provider);

        // Assert
        Assert.Equal(resultExpected, resultActual);
    }

    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        null,
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "- ident johndoe2025 [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        null,
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 - johndoe2025 [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        null,
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident - [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        null,
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 13:17:19 +00:00] - 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        null,
        12345L,
        "127.0.0.1 ident johndoe2025 [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" - 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        null,
        "127.0.0.1 ident johndoe2025 [23/07/1993 13:17:19 +00:00] \"GET /index.html HTTP/1.1\" 500 -"
    )]
    [Theory]
    public void TestFormat_Valid_ReceivedNotNull_ProviderNull(
        String? hostString,
        String? identity,
        String? authenticatedUser,
        String? requestLine,
        Nullable<HttpStatusCode> statusCode,
        Nullable<long> contentLength,
        String resultExpected
    )
    {
        // Arrange
        MockNcsaCommonLog log = new();

        log._receivedFunc = () =>
            new(1993, 7, 23, 13, 17, 19, DateTimeKind.Utc);

        log._requestLineFunc = () => requestLine;
        log._statusCodeFunc = () => statusCode;

        log._hostFunc = () =>
            hostString is null ? null : IPAddress.Parse(hostString);

        log._identityFunc = () => identity;
        log._authenticatedUserFunc = () => authenticatedUser;
        log._contentLengthFunc = () => contentLength;

        // Act
        String resultActual = NcsaCommonLogExtensions.Format(log);

        // Assert
        Assert.Equal(resultExpected, resultActual);
    }

    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 - \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        null,
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "- ident johndoe2025 - \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        null,
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 - johndoe2025 - \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        null,
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident - - \"GET /index.html HTTP/1.1\" 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        null,
        HttpStatusCode.InternalServerError,
        12345L,
        "127.0.0.1 ident johndoe2025 - - 500 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        null,
        12345L,
        "127.0.0.1 ident johndoe2025 - \"GET /index.html HTTP/1.1\" - 12345"
    )]
    [InlineData(
        "127.0.0.1",
        "ident",
        "johndoe2025",
        " GET /index.html HTTP/1.1\r\n ",
        HttpStatusCode.InternalServerError,
        null,
        "127.0.0.1 ident johndoe2025 - \"GET /index.html HTTP/1.1\" 500 -"
    )]
    [Theory]
    public void TestFormat_Valid_ReceivedNull(
        String? hostString,
        String? identity,
        String? authenticatedUser,
        String? requestLine,
        Nullable<HttpStatusCode> statusCode,
        Nullable<long> contentLength,
        String resultExpected
    )
    {
        // Arrange
        MockNcsaCommonLog log = new();
        log._receivedFunc = () => null;
        log._requestLineFunc = () => requestLine;
        log._statusCodeFunc = () => statusCode;

        log._hostFunc = () =>
            hostString is null ? null : IPAddress.Parse(hostString);

        log._identityFunc = () => identity;
        log._authenticatedUserFunc = () => authenticatedUser;
        log._contentLengthFunc = () => contentLength;

        // Act
        String resultActual = NcsaCommonLogExtensions.Format(log);

        // Assert
        Assert.Equal(resultExpected, resultActual);
    }
#endregion
#endregion
}
