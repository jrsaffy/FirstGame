using Godot;
using System;

public partial class GameMenu : Control
{
	VBoxContainer vbox_1;
	VBoxContainer vbox_2;

	public bool is_host;

	int num_players = 0;

	PackedScene level_loader = GD.Load<PackedScene>("res://testScene.tscn");

	public bool checkForNewPlayers()
	{
		bool new_players = false;
		if (num_players != GameManager.GamePlayerInfo.Count)
		{
			new_players = true;
		}
		return new_players;
	}

	public void displayPlayers()
	{
		foreach(var child in vbox_1.GetChildren())
		{
			vbox_1.RemoveChild(child);
			child.QueueFree();
		}
		foreach (PlayerInformation player in GameManager.GamePlayerInfo)
		{
			// GD.Print(player.Name);
			if(player.Team == "1")
			{
				Label playerLabel = new Label();
				playerLabel.Text = player.Name;

				vbox_1.AddChild(playerLabel);
			}

		}

		foreach(var child in vbox_2.GetChildren())
		{
			vbox_2.RemoveChild(child);
			child.QueueFree();
		}
		foreach (PlayerInformation player in GameManager.GamePlayerInfo)
		{
			if(player.Team == "2")
			{
				Label playerLabel = new Label();
				playerLabel.Text = player.Name;

				vbox_2.AddChild(playerLabel);
			}

		}

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void joinTeam(string team, int id)
	{
		for(int i = 0; i < GameManager.GamePlayerInfo.Count; i++)
		{
			PlayerInformation player = GameManager.GamePlayerInfo[i];
			if(player.Id == id)
			{
				GD.Print($"id: {id}, player id: {player.Id}, player: {player.Name}");
				player.Team = team;
			}
		}
	}

	private void onStartGame()
	{
		if(is_host)
		{
			Rpc("startGame");
		}
	}
	
	private void joinTeam1()
	{
		// GameManager.GamePlayerInfo.ForEach(info => GD.Print($"Name: {info.Name} Id: {info.Id}, Team: {info.Team}"));
		int id = Multiplayer.GetUniqueId();
		Rpc("joinTeam", "1", id);
	}
	
	private void joinTeam2()
	{
		// GameManager.GamePlayerInfo.ForEach(info => GD.Print($"Name: {info.Name} Id: {info.Id}, Team: {info.Team}"));
		int id = Multiplayer.GetUniqueId();
		Rpc("joinTeam", "2", id);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void startGame()
	{
		foreach(PlayerInformation player in GameManager.GamePlayerInfo)
		{
			GD.Print($"{player.Name} started game");
		}

		Node2D level = (Node2D)level_loader.Instantiate();
		GetTree().Root.AddChild(level);
		this.Hide();

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print(GetTree().Root);
		vbox_1 = GetTree().Root.FindChild("Team1List", true, false).GetNode<VBoxContainer>("VBoxContainer");
		vbox_2 = GetTree().Root.FindChild("Team2List", true, false).GetNode<VBoxContainer>("VBoxContainer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		displayPlayers();
	}


}


