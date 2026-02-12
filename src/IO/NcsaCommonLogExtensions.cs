using System;
using System.Net;
using System.Text;

using Shipstone.Utilities.Text;

namespace Shipstone.Utilities.IO;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that implement <see cref="INcsaCommonLog" />.
/// </summary>
public static class NcsaCommonLogExtensions
{
    /// <summary>
    /// Converts the value of the specified NCSA common log to its equivalent string representation using the specified culture-specific format information.
    /// </summary>
    /// <param name="log">The <see cref="INcsaCommonLog" /> to format.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information, or <c>null</c>.</param>
    /// <returns>A string representation of <c><paramref name="log" /></c> as specified by <c><paramref name="provider" /></c>.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="log" /></c> is <c>null</c>.</exception>
    public static String Format(
        this INcsaCommonLog log,
        IFormatProvider? provider = null
    )
    {
        ArgumentNullException.ThrowIfNull(log);
        Nullable<DateTime> received = log.Received;
        String? receivedString;

        if (received.HasValue)
        {
            if (provider is null)
            {
                receivedString =
                    received.Value.ToString("dd/MM/yyyy HH:mm:ss zzz");
            }

            else
            {
                receivedString = received.Value.ToString(provider);
            }

            receivedString = $"[{receivedString}]";
        }

        else
        {
            receivedString = null;
        }

        String? requestLine = log.RequestLine;

        if (requestLine is not null)
        {
            requestLine = requestLine.Trim();

            requestLine =
                Internals._whiteSpacePattern.Replace(requestLine, " ");

            requestLine = $"\"{requestLine}\"";
        }

        Nullable<HttpStatusCode> statusCode = log.StatusCode;

        Nullable<int> statusCodeInt32 =
            statusCode.HasValue ? (int) statusCode.Value : null;

        StringBuilder sb = new();

        return sb
            .AppendNcsaCommonLog(log.Host)
            .AppendNcsaCommonLog(log.Identity)
            .AppendNcsaCommonLog(log.AuthenticatedUser)
            .AppendNcsaCommonLog(receivedString)
            .AppendNcsaCommonLog(requestLine)
            .AppendNcsaCommonLog(statusCodeInt32)
            .AppendNcsaCommonLog(log.ContentLength)
            .ToString();
    }
}
