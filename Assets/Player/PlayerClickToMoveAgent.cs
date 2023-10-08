using Godot;
using System;

public partial class PlayerClickToMoveAgent : Godot.CharacterBody2D
{
	bool moving = false;
	Vector2 target_position;
	public int speed = 200;

	
	public Vector2 getPosition()
	{
		
			// GD.Print("click");
			// GD.Print(GetGlobalMousePosition());
			Vector2 mouse_position = GetGlobalMousePosition();
			
			return mouse_position;
			
	}
	
	public void moveToTarget(Vector2 target_position)
		{
		
		if (Position.DistanceTo(target_position) > 3)
			{
				// GD.Print("This guy should be moving");
				// GD.Print(target_position - Position);
				Vector2 direction_vector = (target_position - Position).Normalized();
				Velocity = direction_vector * speed;
				// GD.Print(Velocity);
				MoveAndSlide();
			}
		}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		target_position = new Vector2(Position.X, Position.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _physics_process(double delta)
	{
		if (Input.IsActionJustPressed("left_click"))
		{
			target_position = getPosition();
		}
		
		moveToTarget(target_position);
	}
}
