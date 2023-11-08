using Godot;
using Godot.Collections;
using System;

public partial class EnemyDetector : Area2D
{
	Array<Area2D> overlappingAreas;
	PackedScene detector_loader = GD.Load<PackedScene>("res://Assets/Player/detector.tscn");

	
	 
	void detectEnemies()
	{
		overlappingAreas = GetOverlappingAreas();
		// GD.Print("This is running");

		foreach(var area in overlappingAreas)
		{
			// GD.Print(area);
			if (!(area is Bullet) & !(area is Detector) & !(area is EnemyDetector))
			{
				// Vector2 direction = area.GlobalPosition - GlobalPosition;
				// Detector detector_projectile = (Detector)detector_loader.Instantiate();
				// detector_projectile.direction = direction.Normalized();
				// detector_projectile.init_position = GlobalPosition;
				// GetParent().GetParent().AddChild(detector_projectile);
				var spaceState = GetWorld2D().DirectSpaceState;
				PhysicsRayQueryParameters2D parameters = new PhysicsRayQueryParameters2D();
				parameters.From = GlobalPosition;
				parameters.To = area.GlobalPosition;
				var intersect = spaceState.IntersectRay(parameters);
				if(intersect.Count == 0)
				{
					area.Visible = true;
				}
				else
				{
					area.Visible = false;
				}


				
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		detectEnemies();
	}
}
