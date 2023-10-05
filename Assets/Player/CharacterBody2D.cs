using Godot;
using System;

public partial class Player : Godot.CharacterBody2D
{
	bool moving = false;
	Vector2 target_position;

	public int speed;

	
	public Vector2 getPosition()
	{
		if (Input.IsActionJustPressed("left_click"))
		{
			Vector2 mouse_possition = GetGlobalMousePosition();
			return (mouse_possition - Position).Normalized();
		}
		else
		{
			return Position;
		}
	}
	
	public void moveToTarget(Vector2 target_position)
		{
		if (Position.DistanceTo(target_position) < 3)
			{
				this.Velocity = target_position * speed
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
		moveToTarget(target_position);
	}
}
