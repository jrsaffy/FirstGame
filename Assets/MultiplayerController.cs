using Godot;
using System;
using System.IO.Compression;

public partial class MultiplayerController : Control
{
	PackedScene level_loader = GD.Load<PackedScene>("res://testScene.tscn");
	private ENetMultiplayerPeer peer;
	private int port = 8910;
	// private string host_ip = "192.168.0.207";
	private string host_ip;
	
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
		
		Rpc("startGame");
		
	}
	
	public void OnHost()
	{
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
		PlayerInformation player_info = new PlayerInformation(){
			Name = name,
			Id = id,
			Team = team
		};
		if (!GameManager.GamePlayerInfo.Contains(player_info))
		{
			GameManager.GamePlayerInfo.Add(player_info);
		}
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
