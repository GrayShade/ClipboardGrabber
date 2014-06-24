using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using NATUPNPLib;

namespace ClipboardGrabber
{
	static class PortMapper
	{
		public static string[] AddMapping(int externalPort, int internalPort, string protocol, string name)
		{
            var upnpNat = new UPnPNAT();
			var staticPortMappingCollection = upnpNat.StaticPortMappingCollection;
			var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			if (staticPortMappingCollection == null)
			{
				var addresses = new List<string>();
				foreach (var ipAddress in ipAddresses)
					if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
					{
						var bytes = ipAddress.GetAddressBytes();
						if (bytes[0] == 10 ||
							bytes[0] == 172 && bytes[1] >= 16 && bytes[1] < 32 ||
							bytes[0] == 192 && bytes[1] >= 168 && bytes[1] < 169 ||
							bytes[0] == 169 && bytes[1] == 254)
							continue;

						addresses.Add(ipAddress.ToString());
					}
				return addresses.ToArray();
			}

			string resultAddress = String.Empty;
			foreach (var ipAddress in ipAddresses)
				if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					var bytes = ipAddress.GetAddressBytes();
					if (bytes[0] == 169 && bytes[1] == 254)
						continue;

					var address = ipAddress.ToString();
					try
					{
						resultAddress = staticPortMappingCollection.Add(externalPort, protocol, internalPort, address, true, name).ExternalIPAddress;
					}
					catch (COMException)
					{
					}
				}

			return new string[] { resultAddress };
		}

		public static void RemoveMapping(int externalPort, string protocol)
		{
			var staticPortMappingCollection = new UPnPNAT().StaticPortMappingCollection;
			if (staticPortMappingCollection != null)
				try
				{
					staticPortMappingCollection.Remove(externalPort, protocol);
				}
				catch
				{
				}
		}
	}
}
