using Godot;
using System;
using Godot.Collections;

public partial class  CollisionDummy: Area2D
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

		foreach(var area in overlappingAreas)
		{
			if(area is Detector || area is Bullet)
			{
				Visible = !Visible;
			}
			
		}
		if(overlappingAreas.Count == 0)
		{
			Visible = false;
		}

		}

		
	}


	



