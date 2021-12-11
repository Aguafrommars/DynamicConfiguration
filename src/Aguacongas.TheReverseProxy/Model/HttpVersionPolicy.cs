namespace Aguacongas.TheReverseProxy.Model
{
    /// <summary>
    /// Specifies behaviors for selecting and negotiating the HTTP version for a request.
    /// </summary>
    public enum HttpVersionPolicy
    {
        /// <summary>
        /// Use the requested version or downgrade to a lower one. This is the default behavior.
        /// If the server supports the requested version, either negotiated via ALPN (H2) or advertised via Alt-Svc (H3), and a secure connection is being requested, the result is the Version. Otherwise, the version downgrades to HTTP/1.1. This option does not allow use of a prenegotiated clear text connection, for example, H2C.
        /// </summary>
        RequestVersionOrLower,
        /// <summary>
        /// Use the highest available version, downgrading only to the requested version but not below.
        /// If the server supports a higher version than the requested version (either negotiated via ALPN (H2) or advertised via Alt-Svc (H3)) and a secure connection is requested, the result is the highest available version. Otherwise, the version downgrades to Version. This option allows use of a prenegotiated clear text connection for the requested version but not for a higher version.
        /// </summary>
        RequestVersionOrHigher,
        /// <summary>
        /// Only use the requested version.
        /// This option allows for use of a prenegotiated clear text connection for the requested version.
        /// </summary>
        RequestVersionExact
    }
}
