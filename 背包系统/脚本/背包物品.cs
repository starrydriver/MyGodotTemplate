using Godot;
using System;

public partial class 背包物品 : Resource
{
	// 物品的唯一ID
    [Export]
    public string Id { get; set; } = "";
    // 物品名称
    [Export]
    public string Name { get; set; } = "";
    // 物品数量
    [Export]
    public int Count { get; set; } = 1;
    // 物品简介/描述
    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; } = "";
    // 物品图标
    [Export]
    public Texture2D Icon { get; set; }
    // 物品是否可堆叠
    [Export]
    public bool IsStackable { get; set; } = false;
    // 最大堆叠数量
    [Export]
    public int MaxStackSize { get; set; } = 1;
    // 物品类型（可以用枚举替代）
    [Export]
    public string ItemType { get; set; } = "Default";
    
}
