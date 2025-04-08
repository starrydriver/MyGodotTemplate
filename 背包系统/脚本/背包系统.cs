using Godot;
using System;

public partial class 背包系统 : Control
{
	// 当前跟随鼠标的物品节点
    public static 物品格子 followingItem;
    // 跟随物品的原始父节点
    public static Panel originalParent;
    // 跟随物品在原始父节点中的位置
    public static Vector2 originalPosition;
	// 保存物品原始大小
    public static Vector2 originalSize;
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
