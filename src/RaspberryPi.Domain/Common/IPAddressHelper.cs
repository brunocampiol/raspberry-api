﻿using System.Net;

namespace RaspberryPi.Domain.Common
{
    public static class IPAddressHelper
    {
        public static IPAddress GenerateRandomIPAddress()
        {
            var random = new Random();
            byte[] ipAddressBytes = new byte[4];
            random.NextBytes(ipAddressBytes);
            ipAddressBytes[0] = (byte)random.Next(1, 256);
            return new IPAddress(ipAddressBytes);
        }
    }
}