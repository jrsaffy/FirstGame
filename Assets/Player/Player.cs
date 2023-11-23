using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public string name;
	public int Id;
	bool moving = false;
	int speed = 200;
	State playerState = new State();
	// PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/bullet.tscn");
	PackedScene gun_loader = GD.Load<PackedScene>("res://Assets/Gun/gun.tscn");
	int health = 100;
	bool can_fire = true;
	Gun gun;
	MultiplayerSynchronizer multiplayer_synchronizer;
	




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

	void ShowEnemies()
	{

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		multiplayer_synchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		multiplayer_synchronizer.SetMultiplayerAuthority(Id);
		Gun gun = (Gun)gun_loader.Instantiate();
		
		AddChild(gun);

		if(!(multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId()))
		{
			Visible = false;
			GetNode<Area2D>("EnemyDetector").GetNode<PointLight2D>("PointLight2D").QueueFree();
			GetNode<Camera2D>("Camera2D").QueueFree();
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _physics_process(double delta)
	{
		// GD.Print($"{name}:{Id}:{Multiplayer.GetUniqueId()}");
		if(multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			LookAt(GetGlobalMousePosition());
			Vector2 direction = _GetMovementDirection();
			_Move(direction);
		}
		else
		{
			
			
		}
	}
}
