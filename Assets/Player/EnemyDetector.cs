using Godot;
using Godot.Collections;
using System;
// using System.Security.Cryptography.X509Certificates;

public partial class EnemyDetector : Area2D
{
	Array<Node2D> overlappingBodies;
	PackedScene detector_loader = GD.Load<PackedScene>("res://Assets/Player/detector.tscn");
	MultiplayerSynchronizer multiplayer_synchronizer;

		
		

	private void _on_body_entered(Node2D body)
	{
		if (multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
		Player parent = (Player)this.GetParent();
			if (body is Player player)
			{
				Player player_body = (Player)body;
				
				// GD.Print($"{this}:{player_body.Id} entered");
			}
		}
	}

	private void _on_area_exited(Area2D area)
	{
	// Replace with function body.
	}
	
	private void _on_body_exited(Node2D body)
	{
		if (multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
		Player parent = (Player)this.GetParent();
		
			if (body is Player player)
			{
				Player player_body = (Player)body;
				
				// GD.Print($"{this}:{player_body.Id} exited");
				player.Visible = false;
				parent.Visible = true;
			
			
			}
		}
	// Replace with function body.
	}

	 
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
					// GD.Print(intersect);
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





