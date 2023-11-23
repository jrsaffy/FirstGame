using Godot;
using System;
using Godot.Collections;
using System.Data.Common;

public partial class SceneManager : Node
{
	Godot.Collections.Array<Node2D> SpawnPoints;
	private PackedScene playerLoader = GD.Load<PackedScene>("res://Assets/Player/player.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node SpawnPointsNode = GetNode("SpawnPoints");
		Array<Node>SpawnPoints = SpawnPointsNode.GetChildren();

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
