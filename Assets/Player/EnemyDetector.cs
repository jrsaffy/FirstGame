using Godot;
using Godot.Collections;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class EnemyDetector : Area2D
{
	Array<Node2D> overlappingBodies;
	PackedScene detector_loader = GD.Load<PackedScene>("res://Assets/Player/detector.tscn");
	MultiplayerSynchronizer multiplayer_synchronizer;

	
	 
	void detectEnemies()
	{
		overlappingBodies = GetOverlappingBodies();
		
		foreach(var body in overlappingBodies)
		{
			
			
			if (body is Player player)
			{
				
				if(!(player.Id == GetParent<Player>().Id))
				{
					// GD.Print($"{GetParent<Player>().name} detected {player.name}");
					
					var spaceState = GetWorld2D().DirectSpaceState;
					PhysicsRayQueryParameters2D parameters = new PhysicsRayQueryParameters2D();
					parameters.From = GlobalPosition;
					parameters.To = player.GlobalPosition;
					
					// DrawLine(GlobalPosition, player.GlobalPosition,Color.Color8(255,0,0,1));
					var intersect = spaceState.IntersectRay(parameters);
					Node2D collision = (Node2D)intersect["collider"];
					if(collision is Player)
					{
						player.Visible = true;
					}
					else
					{
						player.Visible = false;
					}
				}

				
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		multiplayer_synchronizer = GetParent().GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			detectEnemies();
		}
	}
	
}
