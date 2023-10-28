using Godot;
using System;
using Godot.Collections;

public partial class  CollisionDummy: Godot.Area2D
{

	PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/bullet.tscn");
	Bullet bullet;

	Array<Area2D> overlappingAreas;

	
	


	public void test()
	{
		GD.Print("I hit something");
	}

	public override void _Ready()
	{
		bullet = (Bullet)bullet_loader.Instantiate();
	}

	public override void _PhysicsProcess(double delta)
	{
		overlappingAreas = GetOverlappingAreas();

		for(int i = 0; i < overlappingAreas.Count; i++)
		{
			Area2D area = overlappingAreas[i];
			area.QueueFree();
			test();

		}
	}

	


}
