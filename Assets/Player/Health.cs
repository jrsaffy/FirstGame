using Godot;
using System;

public partial class Health : AnimatedSprite2D
{
	Player player;
	int health;
	// Called when the node enters the scene tree for the first time.
	int frame = 7;
	public override void _Ready()
	{
		player = (Player)(GetParent().GetParent());
		
	}

	void setHealth()
	{
		float frame_float = ((float)health / 100f) * 7f;
		frame = (int)Math.Floor(frame_float);
		// GD.Print($"health: {health} frame: {frame}, player_health: {player.health}");
		SetFrameAndProgress(frame, 0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GD.Print(player);
		health = player.health;
		setHealth();
	}
}
