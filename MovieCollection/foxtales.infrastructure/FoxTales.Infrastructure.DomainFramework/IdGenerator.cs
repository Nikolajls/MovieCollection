using System;
using System.Security.Cryptography;

namespace FoxTales.Infrastructure.DomainFramework
{
    public static class IdGenerator
    {
        private static RNGCryptoServiceProvider _rng;
        private static RNGCryptoServiceProvider RNG
        {
            get { return _rng ?? (_rng = new RNGCryptoServiceProvider()); }
        }

        public static IdGeneratorMethod Method { get; set; }

        public static Guid NewId()
        {
            switch (Method)
            {
                case IdGeneratorMethod.SequentialGuid:
                    var randomBytes = new byte[10];
                    RNG.GetBytes(randomBytes);

                    var timestamp = DateTime.UtcNow.Ticks / 10000L;
                    var timestampBytes = BitConverter.GetBytes(timestamp);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(timestampBytes);
                    }

                    var guidBytes = new byte[16];
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    return new Guid(guidBytes);
                case IdGeneratorMethod.RandomGuid:
                    return Guid.NewGuid();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum IdGeneratorMethod
    {
        SequentialGuid,
        RandomGuid
    }
}
