using Godot;
using System;

public partial class PortFowarder : Node2D
{
	private string externalIp;
	
	public override void _Ready()
	{
		Upnp upnp = new Upnp();
		int discoverResult = upnp.Discover();

		
		if (discoverResult == (int)Upnp.UpnpResult.Success)
		{
			if (upnp.GetGateway() != null && upnp.GetGateway().IsValidGateway())
			{
				int udpMapResult = upnp.AddPortMapping(15973, 0, "Godot_udp", "UDP", 0);
				int tcpMapResult = upnp.AddPortMapping(15973, 0, "Godot_tcp", "TCP", 0);

				if (udpMapResult != (int)Upnp.UpnpResult.Success)
				{
					upnp.AddPortMapping(15973, 0, "", "UDP", 0);
				}
				if (tcpMapResult != (int)Upnp.UpnpResult.Success)
				{
					upnp.AddPortMapping(15973, 0, "Godot_tcp", "TCP", 0);
				}
			}
		}


		externalIp = upnp.QueryExternalAddress();
		GD.Print($"Give this IP to your players: {externalIp}");
		
		// Optionally delete port mappings
		// upnp.DeletePortMapping(15973, "UDP");
		// upnp.DeletePortMapping(15973, "TCP");
	}

	public string getExternalIP()
	{
		return externalIp;
	}
}
