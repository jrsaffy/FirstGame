using Godot;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

public partial class MultiplayerController : Control
{
	PackedScene level_loader = GD.Load<PackedScene>("res://testScene.tscn");
	PackedScene portForwardingLoader = GD.Load<PackedScene>("res://Assets/port_fowarder.tscn");
	private ENetMultiplayerPeer peer;
	private int port = 15973;
	// private string host_ip = "192.168.0.207";
	private string host_ip;

	private bool is_host = false;
	
	private string join_ip;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.PeerDisconnected += OnPeerDisconnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.ConnectionFailed += OnConnectionFailed;
	}

	//Runs on client when connection fails
	public void OnConnectionFailed()
	{
		GD.Print("Connection Failed");
	}

	//Runs on client when connection is successful
	public void OnConnectedToServer()
	{
		string team = GetNode<TextEdit>("TeamEdit").Text;
		string name = GetNode<TextEdit>("NameEdit").Text;
		GD.Print("Connected To Server");
		RpcId(1, "sendPlayerInformation", name, Multiplayer.GetUniqueId(), team);
		GameManager.GamePlayerInfo.ForEach(playerinfo => GD.Print($"Player: {Multiplayer.GetUniqueId() }, Name: {playerinfo.Name}, Id: {playerinfo.Id}"));
	}

	//Runs on all peers when player disconnects
	public void OnPeerDisconnected(long id)
	{
		GD.Print($"{id} Disconnected");
	}

	//Runs on all peers when player connects
	public void OnPeerConnected(long id)
	{
	   GD.Print($"{id} Connected");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void startGame()
	{
		foreach(PlayerInformation player in GameManager.GamePlayerInfo)
		{
			GD.Print($"{player.Name} started game");
		}

		Node2D level = (Node2D)level_loader.Instantiate();
		GetTree().Root.AddChild(level);
		this.Hide();

	}
	public void OnStartGame()
	{
		if(is_host)
		{
			Rpc("startGame");
		}
		else
		{
			GD.Print("Not Host Cannot Start Game");
		}
		
	}
	
	public void OnHost()
	{
		Node2D port_fowarder = (Node2D)portForwardingLoader.Instantiate();
		GetTree().Root.AddChild(port_fowarder);
		is_host = true;
		GD.Print("######");
		join_ip = GetNode<TextEdit>("TextEdit").Text;
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateServer(port);
		string team = GetNode<TextEdit>("TeamEdit").Text;
		string name = GetNode<TextEdit>("NameEdit").Text;
		
		if (error == Error.Ok)
		{
			
			ENetConnection.CompressionMode compression = ENetConnection.CompressionMode.RangeCoder;
			peer.Host.Compress(compression);
			Multiplayer.MultiplayerPeer = peer;
			GD.Print("Creating Server");
			sendPlayerInformation(name, 1, team);

		}
			
		else
		{
			GD.Print($"Failed to Host Game: {error}");
			return;
		}
		
	}
	
	public void OnJoin()
	{
		GD.Print("###########");
		peer = new ENetMultiplayerPeer();

		join_ip = GetNode<TextEdit>("TextEdit").Text;
		string team = GetNode<TextEdit>("TeamEdit").Text;
		string name = GetNode<TextEdit>("NameEdit").Text;

		Error error = peer.CreateClient(join_ip, port);

		if (error == Error.Ok)
		{
			ENetConnection.CompressionMode compression = ENetConnection.CompressionMode.RangeCoder;
			peer.Host.Compress(compression);
			Multiplayer.MultiplayerPeer = peer;
		}
		else
		{
			GD.Print($"Failed to Join Game: {error}");
			return;
		}

	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void sendPlayerInformation(string name, int id, string team)
	{
		GD.Print($"Sending Player Information: {Multiplayer.GetUniqueId()}");
		PlayerInformation player_info = new PlayerInformation(){
			Name = name,
			Id = id,
			Team = team
		};
		// GD.Print($"Attempting to add {player_info.Name}, {player_info.Id}, {player_info.Team} to GamePlayerInfo");
		// GD.Print("Players in GamePlayerInfo: ");
		// GameManager.GamePlayerInfo.ForEach(info => GD.Print($"Name: {info.Name} Id: {info.Id}, Team: {info.Team}"));
		List<int> ids = GameManager.GamePlayerInfo.Select(info => info.Id).ToList();
		
		if (!ids.Contains(player_info.Id))
		{
			GameManager.GamePlayerInfo.Add(player_info);
			// GD.Print($"Adding {player_info.Name}");
		}

		// GD.Print("Players in GamePlayerInfo After Adding: ");
		// GameManager.GamePlayerInfo.ForEach(info => GD.Print($"Name: {info.Name} Id: {info.Id}, Team: {info.Team}"));
		// GD.Print("");

		if(Multiplayer.IsServer())
		{
			foreach(PlayerInformation player in GameManager.GamePlayerInfo)
			{
				Rpc("sendPlayerInformation", player.Name, player.Id, player.Team);
			}
		}
		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}
}
