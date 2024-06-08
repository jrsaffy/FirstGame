using Godot;
using System;
using System.Diagnostics;

public partial class Gun : Node2D
{
	int player_id;
	public float firerateRoundsPerSecond = 15f;
	public int damage = 30;
	int max_ammo = 25;
	int ammo = 0;
	bool can_fire = true;
	System.Diagnostics.Stopwatch gunStopwatch = new System.Diagnostics.Stopwatch();
	System.Diagnostics.Stopwatch reloadStopwatch = new System.Diagnostics.Stopwatch();
	PackedScene bullet_loader = GD.Load<PackedScene>("res://Assets/Gun/bullet.tscn");
	PackedScene bullet_audio_loader = GD.Load<PackedScene>("res://Assets/Gun/bullet_player.tscn");
	float recoil = 0f;
	float max_recoil = .3f;
	float recoil_per_shot = .05f;
	float recoil_from_movement = .01f;
	Random random_generator = new Random();
	float reload_time = 1000f;
	bool reloading = false;
	
	Random random = new Random();

	AudioStreamPlayer2D reload_audio_player;
	Vector2 prev_pos;
	Vector2 velocity;

	MultiplayerSynchronizer multiplayer_synchronizer;

	float recoil_decay = .02f;



	public void Shoot(Vector2 direction)
	{
		if (Input.IsActionPressed("left_click") & can_fire & (ammo > 0) & !reloading)
			{
				gunStopwatch.Start();

				Vector2 recoil_direction = new Vector2(-direction.Y, direction.X);
				float recoil_magnitude = (float)(random_generator.NextDouble() * (2 * recoil) - recoil);

				// createBullet(direction);
				Rpc("createBullet", (direction + recoil_direction * recoil_magnitude).Normalized());

				playGunshot();

				addRecoil();
			
				can_fire = false;

				ammo -= 1;
			
			}

		if (gunStopwatch.ElapsedMilliseconds > (1000 / firerateRoundsPerSecond))
			{
				gunStopwatch.Reset();
				can_fire = true;
				gunStopwatch.Stop();
			}

		}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	void createBullet(Vector2 direction)
	{
		Bullet bullet = (Bullet)bullet_loader.Instantiate();

		AudioStreamPlayer2D bullet_player = (AudioStreamPlayer2D)bullet_audio_loader.Instantiate();
		AddChild(bullet_player);
		
		

		bullet.direction = direction;
		bullet.init_position = GlobalPosition + direction * 25;

		bullet.LookAt(direction * 50);
		GetParent().GetParent().AddChild(bullet);


	}


	void playGunshot()
	{
		// gunshot_audio = new AudioStreamPlayer2D();
		// AddChild(gunshot_audio);
		// int gunshot_stream_int = random.Next(1,7);
		// AudioStream random_stream = gunshot_randomizer.GetStream(gunshot_stream_int);

		// gunshot_audio.Stream = random_stream;
		// gunshot_audio.Play();
		
		
	}


	void Reload()
	{
		
		if (Input.IsActionJustPressed("R"))
		{   
			reloading = true;
			reloadStopwatch.Start();
			reload_audio_player.Play();
		}
		if (reloadStopwatch.ElapsedMilliseconds > reload_time)
		{
			reloadStopwatch.Restart();
			reloadStopwatch.Stop();
			ammo = max_ammo;
			reloading = false;
		}

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer,CallLocal = true)]
	void addRecoil()
	{
		if (recoil <= max_recoil)
		{
			recoil += recoil_per_shot;
		}
	}

	// [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal =true)]
	void reduceRecoil()
	{
		if (recoil >= 0f)
		{
			recoil -= recoil_decay;
		}
		else
		{
			recoil = 0;
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal =true)]
	void addMovementRecoil()
	{
		if (velocity.Length() > 0 & recoil <= max_recoil)
		{
			recoil += recoil_from_movement;
		}
		else
		{
			reduceRecoil();
		}
	}

	public override void _Ready()
	{
		// GD.Print("This is happening");
		// gunshot_audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		player_id = GetParent<Player>().Id;

		multiplayer_synchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		multiplayer_synchronizer.SetMultiplayerAuthority(player_id);
		prev_pos = GlobalPosition;
		
		reload_audio_player = GetNode<AudioStreamPlayer2D>("ReloadPlayer");
		ammo = max_ammo;
	}

	
	void _physics_process(double delta)
	{
		// GD.Print(GetParent<Player>().name);
		// GD.Print(multiplayer_synchronizer.GetMultiplayerAuthority());
		// GD.Print(Multiplayer.GetUniqueId());
		// GD.Print("--------------------------");
		


		if(multiplayer_synchronizer.GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			Shoot((GetGlobalMousePosition() - GlobalPosition).Normalized());
			// Rpc("Shoot", (GetGlobalMousePosition() - GlobalPosition).Normalized());
			// GD.Print(GlobalPosition);
			// GD.Print(prev_pos);
			velocity = (GlobalPosition - prev_pos) / (float)delta;
			// GD.Print(velocity);
			prev_pos = GlobalPosition;
			Rpc("addMovementRecoil");
			// addMovementRecoil();
			Reload();
		}
		
	
		
	}


}
