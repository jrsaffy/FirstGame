using Godot;
using System;

public partial class gunshot_player : AudioStreamPlayer2D

{

	System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

	public override void _Ready()
	{
		// GD.Print("Bullet");
		// stopwatch.Start();
		Play();
		Callable OnAudioFinished = new Callable(this, MethodName._OnAudioFinished);
		Connect("finished", OnAudioFinished);
		
	}
	private void _OnAudioFinished()
	{   
		// GD.Print("Im Dying");
		// stopwatch.Stop();
		// GD.Print(stopwatch.ElapsedMilliseconds);
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print("I'm still alive");
	}

}

