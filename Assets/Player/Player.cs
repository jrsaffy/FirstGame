using Godot;
using System;

public partial class Player : Godot.CharacterBody2D
{
	bool moving = false;
	int speed = 200;
	State playerState = new State();
	// PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/bullet.tscn");
	PackedScene gun_loader = GD.Load<PackedScene>("res://Assets/gun.tscn");
	int health = 100;
	bool can_fire = true;

	Gun gun;


	void _Move(Vector2 direction)
	{   

		if(Input.IsActionPressed("Shift"))
		{
			speed = 300;
		}
		else
		{
			speed = 200;
		}

		Velocity = speed * direction;

		if (Velocity.Length() > 0)
		{
			moving = true;
			MoveAndSlide();
			GetNode<AnimatedSprite2D>("Legs").Play();
		}

		else
		{
			moving = false;
			GetNode<AnimatedSprite2D>("Legs").Stop();
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

	void Die()
	{
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// playerState.test();
		Gun gun = (Gun)gun_loader.Instantiate();
		AddChild(gun);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _physics_process(double delta)
	{
		// GD.Print(Position);
		LookAt(GetGlobalMousePosition());
		Vector2 direction = _GetMovementDirection();
		_Move(direction);
		

		
	}
}
