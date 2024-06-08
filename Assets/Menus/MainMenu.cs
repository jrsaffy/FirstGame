using Godot;
using System;


public partial class MainMenu : Control
{
	PackedScene host_loader = (PackedScene)ResourceLoader.Load("res://Assets/Menus/HostMenu.tscn");
	PackedScene join_loader = (PackedScene)ResourceLoader.Load("res://Assets/Menus/JoinMenu.tscn");

	PackedScene portForwardingLoader = GD.Load<PackedScene>("res://Assets/port_fowarder.tscn");

	public void moveToHostScreen()
	{
		PortFowarder port_fowarder = (PortFowarder)portForwardingLoader.Instantiate();
		GetTree().Root.AddChild(port_fowarder);

		MultiMenu host_scene = (MultiMenu)host_loader.Instantiate();
		host_scene.fowarded_port = port_fowarder.getExternalIP();
		GetTree().Root.AddChild(host_scene);
		
		QueueFree();
	}

	public void moveToJoinScreen()
	{
		Control join_scene = (Control)join_loader.Instantiate();
		GetTree().Root.AddChild(join_scene);
		QueueFree();
	}

	public void OnHost()
	{
		moveToHostScreen();
	}

	public void OnJoin()
	{
		moveToJoinScreen();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}


