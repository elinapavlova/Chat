using System;

namespace Models.Token
{
    public class AccessToken : JsonWebToken
    {
        public RefreshToken RefreshToken { get;  }

        public AccessToken(string token, long expiration, RefreshToken refreshToken) : base(token, expiration)
        {
            RefreshToken = refreshToken ?? throw new ArgumentException("Specify a valid refresh token.");
        }
    }
}