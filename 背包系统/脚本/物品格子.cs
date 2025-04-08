using Godot;
using System;

public partial class 物品格子 : MarginContainer
{
	[Export]
	public Label ResCount { get; set; }
	[Export]
	public TextureRect Icon { get; set; }
	[Export]
	public 背包物品 Res { get; set; }
	public int Id { get; set; }
	public string Description { get; set; }
	public int Count { get; set; }
	public override void _Ready()
	{
		Icon.Texture = Res.Icon;
		ResCount.Text = Res.Count.ToString();
		CallDeferred("set_size", new Vector2(124, 118));
	}
	public override void _Process(double delta)
	{
	}
}
