using Godot;
using System;
using System.Collections.Generic;

public partial class BreakBricksSprite : Sprite2D
{
	public float ExplosideForce = 1000.0f;
	private PackedScene myBulletScene;
	private BreakBricksBullet myBullet;
	public override void _Ready()
	{
		myBulletScene = GD.Load<PackedScene>("res://打砖块基础模板/break_bricks_bullet.tscn");
	}
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.IsPressed())
			{
				fireBullets();
			}
		}
	}
	public override void _Draw()
	{
		updateTrajectory();
	}
	public void fireBullets()
	{
		myBullet = myBulletScene.Instantiate<BreakBricksBullet>();
		GetTree().Root.AddChild(myBullet);

		myBullet.GlobalPosition = GlobalPosition-new Vector2(20,20);
		myBullet.LinearVelocity = ExplosideForce * getForwardDirection();
	}
	public Vector2 getForwardDirection()
	{
		return GlobalPosition.DirectionTo(GetGlobalMousePosition());
	}
	public void updateTrajectory()
	{
		Vector2 velocity = ExplosideForce * getForwardDirection();
		Vector2 lineStart = ToLocal(GlobalPosition);
		Vector2 lineEnd;
		float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
		float drag = (float)ProjectSettings.GetSetting("physics/2d/default_linear_damp");
		float timeStep = 0.02f;
		var colors = new Color[] { Colors.Red,Colors.Blue};
		for(int i = 0; i < 70; i++)
		{
			velocity += Vector2.Down * gravity * timeStep;//
			lineEnd = lineStart + velocity * timeStep;
			velocity = velocity * Mathf.Clamp(1.0f - drag * timeStep, 0.1f, 1.0f);
			var ray = rayCastQuery2d(lineStart, lineEnd);
			if (ray!= null)
			{
				break;
			}
			DrawLine(lineStart, lineEnd, colors[i % 2], 2);
			lineStart = lineEnd;
		}
	}
	public Godot.Collections.Dictionary rayCastQuery2d(Vector2 pointA, Vector2 pointB)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = PhysicsRayQueryParameters2D.Create(pointA, pointB, 1);
		var result = spaceState.IntersectRay(query);
		if (result.Count > 0)
		{
			return result;
		}
		return null;
	}
}
