using Godot;
using System;

public partial class CRTShaderLayer : CanvasLayer
{
	[Export] public ColorRect CRTFilter = null;

	public override void _Ready()
	{
		this.Layer = 1025;

		// Enable click-through for child nodes
		if (CRTFilter != null)
		{
			CRTFilter.MouseFilter = Control.MouseFilterEnum.Ignore;
		}
	}

	public override void _Process(double delta)
	{
	}
}
