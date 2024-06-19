using Godot;
using System;
using Godot.Collections;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

public partial class SceneManager : Node
{
	Godot.Collections.Array<Node2D> SpawnPoints;
	private PackedScene playerLoader = GD.Load<PackedScene>("res://Assets/Player/player.tscn");
	private PackedScene ScorebaordLoader = GD.Load<PackedScene>("res://Assets/scoreboard.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public List<Player> playersToSpawn = new List<Player>();
	public override void _Ready()
	{
		// GD.Print($"Scene Manager Loaded: {Multiplayer.GetUniqueId()}");
		// GD.Print($"You'd have seen an error in the first statement but if not here's the multiplayer: {Multiplayer}");
		//Need to add team spawn points
		Node SpawnPointsNode = GetNode("SpawnPoints");
		Array<Node>SpawnPoints = SpawnPointsNode.GetChildren();

		GD.Print("Loading and Spawining Players:");
		GameManager.GamePlayerInfo.ForEach(playerinfo => GD.Print($"Player: {Multiplayer.GetUniqueId() }, Name: {playerinfo.Name}, Id: {playerinfo.Id}"));
		
		int local_id = Multiplayer.GetUniqueId();
		PlayerInformation local_player_info = GameManager.GamePlayerInfo.FirstOrDefault(p => (p.Id == local_id));

		for(int i = 0; i < GameManager.GamePlayerInfo.Count; i++)
		{
			
			PlayerInformation player_info = GameManager.GamePlayerInfo[i];
			
			Player new_player = (Player)playerLoader.Instantiate();
			
			Node2D spawnpoint = (Node2D)SpawnPoints[i];
			
			new_player.name = player_info.Name;
			new_player.GlobalPosition = spawnpoint.GlobalPosition;
			new_player.Id = player_info.Id;
			new_player.team = player_info.Team;
			new_player.Name = $"{player_info.Name}_{player_info.Id}";
			setPlayerTeamColor(new_player, local_player_info);
			AddChild(new_player);
		}
		Scoreboard scoreboard = (Scoreboard)ScorebaordLoader.Instantiate();
		AddChild(scoreboard);
	}

	void setPlayerTeamColor(Player player, PlayerInformation local_player_info)
	{
		int multiplayer_id = Multiplayer.GetUniqueId();
		Sprite2D team_indicator = player.GetNode<Sprite2D>("TeamIndicator");
		if (player.Id == multiplayer_id)
		{
			team_indicator.Modulate = new Color(0, 0, 255, .5f);
		}
		else if(player.team == local_player_info.Team)
		{
			team_indicator.Modulate = new Color(0, 255, 0, .5f);
		}
		else if(player.team != local_player_info.Team)
		{
			team_indicator.Modulate = new Color(255, 0, 0, .5f);
		}

	}
	void spawnPlayer(Player player)
	{
		
		Node SpawnPointsNode = GetNode("SpawnPoints");
		Array<Node> SpawnPoints = SpawnPointsNode.GetChildren();
		//Spawn the player at the spawnpoint with the furthest distance from them dieing
		float greatest_distance = 0;
		
		Node2D new_spawnpoint = (Node2D)SpawnPoints[0];
		foreach(Node2D spawnpoint in SpawnPoints)
		{
			float new_distance = (spawnpoint.GlobalPosition - player.GlobalPosition).Length();

			if (new_distance > greatest_distance)
			{
				greatest_distance = new_distance;
				new_spawnpoint = spawnpoint;
			}
		
		}

		player.Position = new_spawnpoint.Position;

		GD.Print($"Spawning: {player.name} Team: {player.team}");

	}

	void spawnPlayers()
	{
		if (playersToSpawn.Count > 0)
		{
			GD.Print("Player Added to playersToSpawn: ");
			playersToSpawn.ForEach(player => GD.Print($"Name: {player.Name}, Id: {player.Id}"));

			foreach(Player player in playersToSpawn)
			{
				spawnPlayer(player);
			}
			playersToSpawn.Clear();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		spawnPlayers();
	}
}
