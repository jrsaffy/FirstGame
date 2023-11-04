using Godot;
using System;
using System.Diagnostics;

public partial class Gun : Node2D
{

	public float firerateRoundsPerSecond = 15f;
	public int damage = 30;
	int max_ammo = 25;
	int ammo = 0;
	bool can_fire = true;
	System.Diagnostics.Stopwatch gunStopwatch = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch reloadStopwatch = new System.Diagnostics.Stopwatch();
	PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/bullet.tscn");
	float recoil = 0f;
	float max_recoil = .3f;
	float recoil_per_shot = .1f;
	Random random_generator = new Random();
    float reload_time = 1000f;
    bool reloading = false;
	


	public void Shoot(Vector2 direction)
	{
	if (Input.IsActionPressed("left_click") & can_fire & (ammo > 0) & !reloading)
		{
			gunStopwatch.Start();

			GD.Print("ass");

			Bullet bullet = (Bullet)bullet_loader.Instantiate();
			Vector2 recoil_direction = new Vector2(-direction.Y, direction.X);
			float recoil_magnitude = (float)(random_generator.NextDouble() * (2 * recoil) - recoil);

			bullet.direction = (direction + recoil_direction * recoil_magnitude).Normalized();
			bullet.init_position = GlobalPosition + direction * 25;

			bullet.LookAt(direction * 50);
			GetParent().GetParent().AddChild(bullet);

			if (recoil <= max_recoil)
			{
				recoil += recoil_per_shot;
			}
			can_fire = false;
            GD.Print(ammo);

			ammo -= 1;
			
		}
	if (gunStopwatch.ElapsedMilliseconds > (1000 / firerateRoundsPerSecond))
		{
			gunStopwatch.Reset();
			can_fire = true;
            gunStopwatch.Stop();
		}
	// if(!Input.IsActionPressed("left_click"))
	// 	{
	// 		recoil = 0f;
	// 	}
    
	 // GD.Print(gunStopwatch.ElapsedMilliseconds);

    


	}

    void Reload()
    {
        
        if (Input.IsActionJustPressed("R"))
        {   
            reloading = true;
            reloadStopwatch.Start();
        }
        if (reloadStopwatch.ElapsedMilliseconds > reload_time)
        {
            reloadStopwatch.Restart();
            reloadStopwatch.Stop();
            ammo = max_ammo;
            reloading = false;
        }

        GD.Print(reloadStopwatch.ElapsedMilliseconds);

    }

    void reduceRecoil()
    {
        if (recoil >= 0f)
        {
            recoil -= .01f;
        }
        else
        {
            recoil = 0;
        }
    }

    public override void _Ready()
    {
        GD.Print("This is happening");
        ammo = max_ammo;
    }

    
	void _physics_process(double delta)
	{
		Shoot((GetGlobalMousePosition() - GlobalPosition).Normalized());
        reduceRecoil();
        Reload();
        
        // GD.Print(ammo);
		
	}


}
