using System;
using System.Security.Cryptography;

namespace NXTBackend.API.Domain.Extensions;

public static class GuidExtensions
{
    public unsafe static Guid NewUuidV7()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Create random data for the remaining bits
        Span<byte> randomBytes = stackalloc byte[10];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Fill the timestamp (48 bits)
        Span<byte> uuidBytes = stackalloc byte[16];
        uuidBytes[0] = (byte)((timestamp >> 40) & 0xFF);
        uuidBytes[1] = (byte)((timestamp >> 32) & 0xFF);
        uuidBytes[2] = (byte)((timestamp >> 24) & 0xFF);
        uuidBytes[3] = (byte)((timestamp >> 16) & 0xFF);
        uuidBytes[4] = (byte)((timestamp >> 8) & 0xFF);
        uuidBytes[5] = (byte)(timestamp & 0xFF);

        // Set the version to 7 and fill the first 12 bits of random data
        uuidBytes[6] = (byte)((randomBytes[0] & 0x0F) | 0x70);
        uuidBytes[7] = randomBytes[1];

        // Fill the remaining 60 bits with random data
        randomBytes.Slice(2, 8).CopyTo(uuidBytes[8..]);

        // Create the GUID from the byte array
        return new Guid(uuidBytes);
    }
}
