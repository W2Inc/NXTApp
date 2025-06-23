using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface.Network;

public class NetworkValidationService : INetworkValidationService
{

}

// Helper class for IP network operations
public class IPNetwork
{
    private readonly IPAddress _networkAddress;
    private readonly IPAddress _subnetMask;

    public static IPNetwork Parse(string cidr)
    {
        var parts = cidr.Split('/');
        var networkAddress = IPAddress.Parse(parts[0]);
        var prefixLength = int.Parse(parts[1]);

        // Calculate subnet mask from prefix length
        byte[] mask = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            if (prefixLength >= 8)
            {
                mask[i] = 255;
                prefixLength -= 8;
            }
            else if (prefixLength > 0)
            {
                mask[i] = (byte)(256 - Math.Pow(2, 8 - prefixLength));
                prefixLength = 0;
            }
            else
            {
                mask[i] = 0;
            }
        }

        var subnetMask = new IPAddress(mask);
        return new IPNetwork(networkAddress, subnetMask);
    }

    public IPNetwork(IPAddress networkAddress, IPAddress subnetMask)
    {
        _networkAddress = networkAddress;
        _subnetMask = subnetMask;
    }

    public bool Contains(IPAddress ip)
    {
        if (ip.AddressFamily != AddressFamily.InterNetwork)
            return false;

        byte[] ipBytes = ip.GetAddressBytes();
        byte[] networkBytes = _networkAddress.GetAddressBytes();
        byte[] maskBytes = _subnetMask.GetAddressBytes();

        for (int i = 0; i < ipBytes.Length; i++)
        {
            if ((ipBytes[i] & maskBytes[i]) != (networkBytes[i] & maskBytes[i]))
                return false;
        }

        return true;
    }
}
