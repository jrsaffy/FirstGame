using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Scoreboard : Node2D
{
	
	Array<Node> Players;
	int Team1Score = 0;
	int Team2Score = 0;


	private void connectPlayers()
	{
		foreach(Node nodePlayer in Players)
		{
			Player playerPlayer = (Player)nodePlayer;
			playerPlayer.DeadSignal += OnPlayerDeath;
		}
	}

	void OnPlayerDeath(string team)
	{
		GD.Print("Signal Recieved");
		if(team == "1")
		{
			Team2Score += 1;
		}
		if(team == "2")
		{
			Team1Score += 1;
		}

		GD.Print($"Team 1: {Team1Score}");
		GD.Print($"Team 2: {Team2Score}");
	}



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Players = GetTree().GetNodesInGroup("Player");
		connectPlayers();
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}
}
