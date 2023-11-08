using Godot;
using System;
using Godot.Collections;

public partial class Detector : Area2D
{

	int speed = 1200;
	public Vector2 init_position = new Vector2(0,0);
	public Vector2 direction;
	Array<Area2D> overlappingAreas;
	Array<Node2D> overlappingBodies;
	Vector2 velocity;
	// AudioStreamPlayer2D gunshot_audio;


	
	// public Bullet(Vector2 _init_position, Vector2 _direction, int _damage)
	// {
	// 	this.init_position = _init_position;
	// 	this.direction = _direction;
	// 	this.damage = _damage;
	// }


	public override void _Ready()
	{
		Position = init_position;
		velocity = direction * speed;
		GD.Print(velocity);
		// gunshot_audio = GetNode<AudioStreamPlayer2D>("GunshotPlayer");
		// gunshot_audio.Play();
			
	}

	public override void _PhysicsProcess(double delta)
	{
		//Position = Position + velocity;
		GD.Print("This is happening");
		Translate(velocity * (float)delta);

		overlappingAreas = GetOverlappingAreas();
		overlappingBodies = GetOverlappingBodies();
		

		for(int i = 0; i < overlappingAreas.Count; i++)
		{
			Area2D area = overlappingAreas[i];
			if (!(area is Detector) & !(area is EnemyDetector) & !(area is Detector))
			{
				QueueFree();
			}
			// GD.Print("Area");
			// GD.Print(area);

		}

		for(int i = 0; i < overlappingBodies.Count; i++)
		{
			Node2D body = overlappingBodies[i];
			if(!(body is Player))
			{
				QueueFree();
			}
			
			// GD.Print("Body");
			// GD.Print(body);

		}

		}


	


}

