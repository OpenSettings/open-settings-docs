using Microsoft.Extensions.Primitives;
using System;

namespace OpenSettings.Docs
{
    /// <summary>
    /// Represents HTTP cache control headers that define caching behavior for responses.
    /// Allows setting the cache expiration time and whether the cache is private or public.
    /// </summary>
    public class CacheControl
    {
        private readonly string _cacheControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControl"/> class with a specified expiration time.
        /// The cache is set to public by default.
        /// </summary>
        /// <param name="expiresInSeconds">The number of seconds before the cache expires. A value less than 0 is treated as 0.</param>
        public CacheControl(int expiresInSeconds) : this(expiresInSeconds, isPrivate: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControl"/> class.
        /// Constructs a cache control header based on the provided expiration time and privacy settings.
        /// </summary>
        /// <param name="expiresInSeconds">The number of seconds before the cache expires. A value less than 0 is treated as 0.</param>
        /// <param name="isPrivate">A boolean value indicating whether the cache is private. If <c>true</c>, the cache is private to the user's browser; otherwise, it's public and can be cached by shared caches such as CDNs, proxy servers, or ISP caches.</param>
        public CacheControl(int expiresInSeconds, bool isPrivate)
        {
            ExpiresInSeconds = expiresInSeconds < 0 ? 0 : expiresInSeconds;
            IsPrivate = isPrivate;

            var specifier = IsPrivate ? "private" : "public";

            _cacheControl = $"{specifier}, max-age={ExpiresInSeconds}";
        }

        /// <summary>
        /// Gets the expiration time in seconds for the cache.
        /// </summary>
        public int ExpiresInSeconds { get; }

        /// <summary>
        /// Indicates whether the cache is private.
        /// If set to <c>true</c>, the response is cached only by the user's browser
        /// and is not stored by shared caches such as CDNs, proxy servers, or ISP caches.
        /// </summary>
        public bool IsPrivate { get; }

        /// <summary>
        /// Generates the HTTP "Expires" header value based on the provided reference date.
        /// </summary>
        /// <param name="referenceDate">
        /// An optional reference date to calculate the expiration time.
        /// If not provided, the current UTC time is used.
        /// </param>
        /// <returns>
        /// The HTTP "Expires" header value as a string in RFC1123 format (e.g., "Sat, 15 Mar 2025 12:00:00 GMT").
        /// </returns>
        public StringValues GetHttpExpiresHeader(DateTimeOffset? referenceDate = null)
        {
            referenceDate ??= DateTimeOffset.UtcNow;

            return referenceDate.Value.UtcDateTime.AddSeconds(ExpiresInSeconds).ToString("R");
        }

        /// <summary>
        /// Generates the Cache-Control HTTP header value.
        /// </summary>
        /// <returns>
        /// A string representing the Cache-Control header value, e.g., "public, max-age=300".
        /// </returns>
        public override string ToString()
        {
            return _cacheControl;
        }
    }
}