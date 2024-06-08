using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MultiMenu : Control
{
	
	PackedScene game_screen_loader = (PackedScene)GD.Load("res://Assets/Menus/GameMenu.tscn");
	private ENetMultiplayerPeer peer;
	private int port = 15973;
	private string ip;
	private string name;
	string team = "1";
	public string fowarded_port;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.PeerDisconnected += OnPeerDisconnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.ConnectionFailed += OnConnectionFailed;
	}


	public void OnHost()
	{
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateServer(port);
		name = GetNode<Node>("NameEdit").GetNode<TextEdit>("TextEdit").Text;
		
		if (error == Error.Ok)
		{
			
			ENetConnection.CompressionMode compression = ENetConnection.CompressionMode.RangeCoder;
			peer.Host.Compress(compression);
			Multiplayer.MultiplayerPeer = peer;
			GD.Print("Creating Server");
			sendPlayerInformation(name, 1, team);
			loadGameScreen(true);
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
		name = GetNode<Node>("NameEdit").GetNode<TextEdit>("TextEdit").Text;
		string join_ip = GetNode<Node>("IPEdit").GetNode<TextEdit>("TextEdit").Text;

		Error error = peer.CreateClient(join_ip, port);

		if (error == Error.Ok)
		{
			ENetConnection.CompressionMode compression = ENetConnection.CompressionMode.RangeCoder;
			peer.Host.Compress(compression);
			Multiplayer.MultiplayerPeer = peer;
			loadGameScreen(false);
		}
		else
		{
			GD.Print($"Failed to Join Game: {error}");
			return;
		}

	}
	private void OnConnectionFailed()
	{
		GD.Print("Connection Failed");
	}


	private void OnConnectedToServer()
	{
		GD.Print("Connected To Server");
		RpcId(1, "sendPlayerInformation", name, Multiplayer.GetUniqueId(), team);
	}


	private void OnPeerDisconnected(long id)
	{
		GD.Print($"{id} Disconnected");
	}


	private void OnPeerConnected(long id)
	{
		GD.Print($"{id} Connected");
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

	private void loadGameScreen(bool is_host)
	{
		GameMenu game_screen = (GameMenu)game_screen_loader.Instantiate();
		game_screen.is_host = is_host;
		GetTree().Root.AddChild(game_screen);
		this.Hide();	

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}

