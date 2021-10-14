using System;

namespace Models.Token
{
    public class JsonWebToken
    {
        public string Token { get; }
        public long Expiration { get; }

        protected JsonWebToken(string token, long expiration)
        {
            if(string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Invalid token.");

            if(expiration <= 0)
                throw new ArgumentException("Invalid expiration.");

            Token = token;
            Expiration = expiration;
        }

        public bool IsExpired() => DateTime.UtcNow.Ticks > Expiration;
    }
}