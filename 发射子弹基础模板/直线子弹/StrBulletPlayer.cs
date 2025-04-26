using Godot;
using System;

public partial class StrBulletPlayer : Sprite2D
{
	[Export] public RayCast2D rayCast;
	private PackedScene myBulletScene;
	private MyBullet myBullet;
	public override void _Ready()
	{
		myBulletScene = GD.Load<PackedScene>("res://发射子弹基础模板/my_bullet.tscn");
		// _ = PrintRayCastPositionAsync();//print raycast position
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.IsPressed())
			{
				fireBullets();
			}
		}
	}
	public override void _Process(double delta)
	{
		QueueRedraw();// redraw every frame
	}
	public override void _Draw()
	{
		updateTrajectory();
	}
	private async System.Threading.Tasks.Task PrintRayCastPositionAsync()
	{
		while (true)
		{
			GD.Print(rayCast.GlobalPosition);
			await ToSignal(GetTree().CreateTimer(2.0), "timeout");
		}
	}
	public void fireBullets()
	{
		myBullet = myBulletScene.Instantiate<MyBullet>();
		GetTree().Root.AddChild(myBullet);
		myBullet.GlobalPosition = GlobalPosition;
		myBullet.LinearVelocity = 500 * getForwardDirection();
	}
	public Vector2 getForwardDirection()
	{
		return GlobalPosition.DirectionTo(GetGlobalMousePosition());
	}
	public void updateTrajectory()
	{
		Vector2 lineStart = GlobalPosition;
		Vector2 lineEnd;
		float timeStep = 10f;
		var colors = new Color[] { Colors.Yellow, new Color(1, 1, 1, 0.0f) };
		Vector2 direction = getForwardDirection();
		for(int i = 0; i < 100; i++)
		{
			lineEnd = lineStart + direction * timeStep;
			rayCast.GlobalPosition = lineStart;
			rayCast.TargetPosition = lineEnd - lineStart;
			rayCast.ForceRaycastUpdate(); 
			if (rayCast.IsColliding())
			{
				break;
			}
			DrawLineGlobal(lineStart, lineEnd, colors[i % 2], 2);
			lineStart = lineEnd;
		}
	}
	public void DrawLineGlobal(Vector2 pointA, Vector2 pointB, Color color, int width)
	{
		 DrawLine(ToLocal(pointA), ToLocal(pointB), color, width);
	}
}
