using Godot;
using System;
using Godot.Collections;
using System.Data.Common;
using System.Collections.Generic;

public partial class SceneManager : Node
{
	Godot.Collections.Array<Node2D> SpawnPoints;
	private PackedScene playerLoader = GD.Load<PackedScene>("res://Assets/Player/player.tscn");
	// Called when the node enters the scene tree for the first time.
	public List<Player> playersToSpawn = new List<Player>();
	public override void _Ready()
	{
		Node SpawnPointsNode = GetNode("SpawnPoints");
		Array<Node>SpawnPoints = SpawnPointsNode.GetChildren();
		// GD.Print($"Spawn Points: {SpawnPoints}");
		for(int i = 0; i < SpawnPoints.Count; i++)
		{
			// GD.Print(i);
			PlayerInformation player_info = GameManager.GamePlayerInfo[i];
			Player new_player = (Player)playerLoader.Instantiate();
			
			Node2D spawnpoint = (Node2D)SpawnPoints[i];
			
			new_player.name = player_info.Name;
			new_player.GlobalPosition = spawnpoint.GlobalPosition;
			new_player.Id = player_info.Id;
			AddChild(new_player);
		}
	}

	void spawnPlayer(Player player)
	{
		Node SpawnPointsNode = GetNode("SpawnPoints");
		Array<Node> SpawnPoints = SpawnPointsNode.GetChildren();
		//Spawn the player at the spawnpoint with the furthest distance from them dieing
		float greatest_distance = 0;
		GD.Print($"Spawn Points: {SpawnPoints}");
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

	}

	void spawnPlayers()
	{
		if (playersToSpawn.Count > 0)
		{
			foreach(Player player in playersToSpawn)
			{
				spawnPlayer(player);
				playersToSpawn.Remove(player);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		spawnPlayers();
	}
}
