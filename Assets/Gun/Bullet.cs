using Godot;
using System;
using Godot.Collections;

public partial class Bullet : Area2D
{

	int speed = 1000;
	public Vector2 init_position = new Vector2(0,0);
	public Vector2 direction;
	Array<Area2D> overlappingAreas;
	Array<Node2D> overlappingBodies;
	Vector2 velocity;
	int damage = 25;
	public Player shooter;

	public void test()
	{
		GD.Print("Hello, I am bullet");
	}

	public override void _Ready()
	{
		Position = init_position;
		velocity = direction * speed;
		// gunshot_audio = GetNode<AudioStreamPlayer2D>("GunshotPlayer");
		// gunshot_audio.Play();
			
	}

	public override void _PhysicsProcess(double delta)
	{
		//Position = Position + velocity;
		Translate(velocity * (float)delta);
		LookAt(Position + direction);

		overlappingAreas = GetOverlappingAreas();
		overlappingBodies = GetOverlappingBodies();
		

		for(int i = 0; i < overlappingAreas.Count; i++)
		{
			Area2D area = overlappingAreas[i];
			if (!(area is Bullet) & !(area is EnemyDetector))
			{
				QueueFree();
			}
			// GD.Print("Area");
			// GD.Print(area);

		}

		for(int i = 0; i < overlappingBodies.Count; i++)
		{
			Node2D body = overlappingBodies[i];
			if(body is Player)
			{
				Player player = (Player)body;
				player.last_player_to_damage = this.shooter;
				player.health -= damage;
				
			}
			QueueFree();
			// GD.Print("Body");
			// GD.Print(body);

		}

		}


	


}
