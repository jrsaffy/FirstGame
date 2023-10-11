using Godot;
using System;

public partial class Bullet : Node2D
{

	int speed = 10;
	public Vector2 init_position = new Vector2(0,0);
	public Vector2 direction;

	Vector2 velocity;
	// public Bullet(Vector2 _direction, Vector2 _init_position)
	// {
	//     this.init_position = _init_position;
	//     this.direction = _direction;
	// }
	

	public void test()
	{
		GD.Print("Hello, I am bullet");
	}

	public override void _Ready()
		{
			Position = init_position;
			velocity = direction * speed;
			test();
			
		}

		public override void _PhysicsProcess(double delta)
		{
		   Position = Position + velocity;
		   LookAt(Position + direction);
		}


	


}
