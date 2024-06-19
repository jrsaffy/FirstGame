using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Scoreboard : CanvasLayer
{
	
	Array<Node> Players;
	int Team1Score = 0;
	int Team2Score = 0;

	VBoxContainer vbox_1;
	VBoxContainer vbox_2;

	Control team1list;
	Control team2list;




	private void connectPlayers()
	{
		foreach(Node nodePlayer in Players)
		{
			Player playerPlayer = (Player)nodePlayer;
			playerPlayer.DeadSignal += OnPlayerDeath;
		}
	}


	void OnPlayerDeath(Player victim, Player killer)
	{
		GD.Print("Signal Recieved");
		if(killer.team == "1")
		{
			Team1Score += 1;
		}
		if(killer.team == "2")
		{
			Team2Score += 1;
		}

		killer.kills += 1;
		victim.deaths += 1;

		GD.Print($"Team 1: {Team1Score}");
		GD.Print($"Team 2: {Team2Score}");
	}


	void showScoreboard()
	{
		if (Input.IsActionPressed("tab"))
		{
			team1list.Visible = true;
			team2list.Visible = true;
			foreach (var child in vbox_1.GetChildren())
			{
				vbox_1.RemoveChild(child);
				child.QueueFree();
			}

			foreach(var child in vbox_2.GetChildren())
			{
			vbox_2.RemoveChild(child);
			child.QueueFree();
			}

			foreach (Node2D node_player in Players)
			{
				Player player = (Player)node_player;
				Label playerLabel = new Label();
				playerLabel.Text = $"{player.name}    {player.kills}    {player.deaths}";
				if (player.team == "1")
				{
					vbox_1.AddChild(playerLabel);
				}
				if (player.team == "2")
				{
					vbox_2.AddChild(playerLabel);
				}
			}


		}
		else
		{
			team1list.Visible = false;
			team2list.Visible = false;
		}
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Players = GetTree().GetNodesInGroup("Player");
		connectPlayers();
		vbox_1 = FindChild("team1score", true, false).GetNode<VBoxContainer>("VBoxContainer");
		vbox_2 = FindChild("team2score", true, false).GetNode<VBoxContainer>("VBoxContainer");

		team1list = (Control)FindChild("team1score", true, false);
		team2list = (Control)FindChild("team2score", true, false);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		showScoreboard();
	}
}
