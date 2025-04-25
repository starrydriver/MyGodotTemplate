using Godot;
using System;

public partial class Character : Sprite2D
{
	[Export] public RayCast2D rayCast;
	public override void _Ready()
	{
		_ = PrintRayCastPositionAsync();

	}

	private async System.Threading.Tasks.Task PrintRayCastPositionAsync()
	{
		while (true)
		{
			GD.Print(rayCast.GlobalPosition);
			await ToSignal(GetTree().CreateTimer(2.0), "timeout");
		}
	}
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	public override void _Draw()
	{
		updateTrajectory();
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
			rayCast.GlobalPosition = lineStart; // 关键：每次更新起点位置
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
