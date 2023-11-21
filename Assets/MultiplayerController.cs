using Godot;
using System;
using System.IO.Compression;

public partial class MultiplayerController : Control
{
	PackedScene level_loader = GD.Load<PackedScene>("res://testScene.tscn");
	private ENetMultiplayerPeer peer;
	private int port = 8910;
	private string ip = "127.0.0.1";
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
		GD.Print("Connected To Server");
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
		Node2D level = (Node2D)level_loader.Instantiate();
		GetTree().Root.AddChild(level);
		this.Hide();
	}
	public void OnStartGame()
	{
		
		Rpc("startGame");
		
	}
	
	public void OnHost()
	{
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateServer(port);
		if (error == Error.Ok)
		{
			
			ENetConnection.CompressionMode compression = ENetConnection.CompressionMode.RangeCoder;
			peer.Host.Compress(compression);
			Multiplayer.MultiplayerPeer = peer;
			GD.Print("Creating Server");

		}
			
		else
		{
			GD.Print($"Failed to Host Game: {error}");
			return;
		}
		
	}
	
	public void OnJoin()
	{
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateClient(ip, port);

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
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
