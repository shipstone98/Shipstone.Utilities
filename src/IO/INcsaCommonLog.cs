using System;
using System.Net;

namespace Shipstone.Utilities.IO;

/// <summary>
/// Represents a log in the NCSA common log format.
/// </summary>
public interface INcsaCommonLog
{
    /// <summary>
    /// Gets the ID of the authenticated user.
    /// </summary>
    /// <value>The ID of the authenticated user, or <c>null</c>.</value>
    String? AuthenticatedUser { get; }

    /// <summary>
    /// Gets the number of bytes returned.
    /// </summary>
    /// <value>The number of bytes returned, or <c>null</c>.</value>
    Nullable<long> ContentLength { get; }

    /// <summary>
    /// Gets the IP address of the remote host.
    /// </summary>
    /// <value>The <see cref="IPAddress" /> of the remote host, or <c>null</c>.</value>
    IPAddress? Host { get; }

    /// <summary>
    /// Gets the RFC 1413 identity of the client.
    /// </summary>
    /// <value>The RFC 1413 identity of the client, or <c>null</c>.</value>
    String? Identity { get; }

    /// <summary>
    /// Gets the date and time the request was received at.
    /// </summary>
    /// <value>The date and time the request was received at, or <c>null</c>.</value>
    Nullable<DateTime> Received { get; }

    /// <summary>
    /// Gets the request line.
    /// </summary>
    /// <value>The request line, or <c>null</c>.</value>
    String? RequestLine { get; }

    /// <summary>
    /// Gets the HTTP status code returned.
    /// </summary>
    /// <value>The <see cref="HttpStatusCode" /> returned, or <c>null</c>.</value>
    Nullable<HttpStatusCode> StatusCode { get; }
}
