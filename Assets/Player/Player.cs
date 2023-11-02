using Godot;
using System;
using System.Diagnostics;

public partial class Player : Godot.CharacterBody2D
{
	bool moving = false;
	int speed = 200;
	State playerState = new State();
	PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/bullet.tscn");
	int health = 100;
	bool can_fire = true;
	Gun gun = new Gun(15.0f, 30, 25);
	System.Diagnostics.Stopwatch gunStopwatch = new System.Diagnostics.Stopwatch();
	float max_recoil = .3f;
	float recoil = 0f;
	float recoil_per_shot = .075f;
	Vector2 recoil_direction;
	Random random_generator = new Random();


	void _Move(Vector2 direction)
	{   
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

	void setSprint()
	{
		if(Input.IsActionPressed("Shift"))
		{
			// speed = Convert.ToInt32(speed * 1.5f);
			speed = 300;
		}
		else
		{
			speed = 200;
		}

	}

	void shoot(Vector2 direction)
	{
		if (Input.IsActionPressed("left_click") & can_fire)
		{
			gunStopwatch.Start();
			GD.Print("ass");
			Bullet bullet = (Bullet)bullet_loader.Instantiate();
			// Bullet bullet = new Bullet(Position + direction * 25, direction, gun.damage);
			recoil_direction = new Vector2(-direction.Y, direction.X);
			float recoil_magnitude = (float)(random_generator.NextDouble() * (2 * recoil) - recoil);

			bullet.direction = (direction + recoil_direction * recoil_magnitude).Normalized();
			
			bullet.init_position = Position + direction * 25;
			bullet.LookAt(direction * 50);
			GetParent().AddChild(bullet);
			if (recoil <= max_recoil)
			{
				recoil += recoil_per_shot;
			}
			can_fire = false;
			
		}
		if (gunStopwatch.ElapsedMilliseconds > (1000 / gun.firerateRoundsPerSecond))
		{
			gunStopwatch.Reset();
			can_fire = true;
		}
		if(!Input.IsActionPressed("left_click"))
		{
			recoil = 0f;
		}
		GD.Print(gunStopwatch.ElapsedMilliseconds);

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

	void remove_recoil()
	{
		if (moving == false)
		{
			recoil -= .5f;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerState.test();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _physics_process(double delta)
	{
		// GD.Print(Position);
		LookAt(GetGlobalMousePosition());
		Vector2 direction = _GetMovementDirection();
		setSprint();
		_Move(direction);
		shoot((GetGlobalMousePosition() - Position).Normalized());

		
	}
}
