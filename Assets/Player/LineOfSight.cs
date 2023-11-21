using Godot;
using Godot.Collections;

partial class LineOfSight : Area2D
{
	float detection_range = 10f;
	Array<Area2D> overlappingAreas;
	Array<Node2D> overlappingBodies;
	CollisionShape2D collider;


	void revealEnemies()
	{
		overlappingBodies = GetOverlappingBodies();

		for(int i = 0; i < overlappingBodies.Count; i++)
		{
			Node2D body = overlappingBodies[i];
			// GD.Print(body);

			
		}

		overlappingAreas = GetOverlappingAreas();

		for(int i = 0; i < overlappingAreas.Count; i++)
		{
			Node2D area = overlappingAreas[i];
			// GD.Print(area);
			area.Visible = true;

			
		}


	}

	public override void _Ready()
	{
		collider = GetNode<CollisionShape2D>("LineOfSight_Collider");
		collider.Scale = new Vector2(detection_range, 1f);
	}

	public override void _PhysicsProcess(double delta)
	{
		revealEnemies();
	}


}
