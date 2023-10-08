using Godot;
using System;

public partial class Player : Godot.CharacterBody2D
{
	bool moving = false;
	public int speed = 200;

	void _Move(Vector2 direction)
	{   
		Velocity = speed * direction;

		if (Velocity.Length() > 0)
		{
			moving = true;
			MoveAndSlide();
			
		}

		else
		{
			moving = false;
		}

		
	}
	
	Vector2 _GetMovementDirection()
	{
		int right = Convert.ToInt32(Input.IsActionPressed("move_right"));
		int left = -1 * Convert.ToInt32(Input.IsActionPressed("move_left"));
		int up = -1 * Convert.ToInt32(Input.IsActionPressed("move_up"));
		int down = Convert.ToInt32(Input.IsActionPressed("move_down"));
		
		int horizontal = right + left;
		int vertical = up + down;
		Vector2 direction = new Vector2(horizontal, vertical).Normalized();
		return direction;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _physics_process(double delta)
	{
		LookAt(GetGlobalMousePosition());
		Vector2 direction = _GetMovementDirection();
		_Move(direction);
	}
}
