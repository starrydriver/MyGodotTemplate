using Godot;
using System;
using System.Collections.Generic;

public partial class 背包系统 : Control
{
	[Export] public Button SortButton;
	[Export] public GridContainer myGridContainer;
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
		SortButton.ButtonDown += OnSortButtonPressed;
	}
	public override void _Process(double delta)
	{
	}
	void OnSortButtonPressed()
	{
		// 按id排序
		var children = new List<Node>(myGridContainer.GetChildren());
		var myItems = new List<物品格子>();
		for (int i = 0; i < children.Count; i++)
		{
			if(children[i].GetChildCount() == 0)
			{
				continue;
			}
			var item = children[i].GetChild(0) as 物品格子;
			myItems.Add(item);
			children[i].RemoveChild(item);
		}
		//
		//对myItems里元素的Res.Id进行排序
		myItems.Sort((a, b) => a.Res.Id.CompareTo(b.Res.Id));
		//重新排列
		foreach (var child in myGridContainer.GetChildren())
		{
			if(myItems.Count != 0)
			{
				child.AddChild(myItems[0]);
				myItems.RemoveAt(0);	
			}
		}
		// var children = new List<Node>(myGridContainer.GetChildren());
		// var newPanels = new List<Panel>();
		// var myItems = new List<物品格子>();
		// for (int i = 0; i < children.Count; i++)
		// {
		// 	if(children[i].GetChildCount() == 0)
		// 	{
		// 		// 创建一个和当前 Panel 一样属性的新 Panel
		// 		var panel = children[i] as Panel;
		// 		var myPanel = new Panel();
		// 		//为新的Panel添加脚本
		// 		myPanel.SizeFlagsHorizontal = panel.SizeFlagsHorizontal;
		// 		myPanel.SizeFlagsVertical = panel.SizeFlagsVertical;
		// 		myPanel.CustomMinimumSize = panel.CustomMinimumSize;
		// 		// myPanel.SizeFlagsHorizontal = SizeFlags.Fill;
		// 		// myPanel.SizeFlagsVertical = SizeFlags.Fill;
		// 		newPanels.Add(myPanel);
		// 	}
		// 	else
		// 	{
		// 		var item = children[i].GetChild(0) as 物品格子;
		// 		myItems.Add(item);
		// 		children[i].RemoveChild(item);
		// 	}
		// }
		// //
		// GD.Print(myItems[0].Res.Id);
		// GD.Print(myItems[1].Res.Id);
		// GD.Print(myItems[2].Res.Id);
		// //对myItems里元素的Res.Id进行排序
		// myItems.Sort((a, b) => a.Res.Id.CompareTo(b.Res.Id));
		// //重新排列
		// foreach (var child in myGridContainer.GetChildren())
		// {
		// 	myGridContainer.RemoveChild(child);
		// }
		// for (int i = 0; i < children.Count; i++)
		// {
		// 	if(myItems.Count != 0)
		// 	{
		// 		var panel = children[i] as Panel;
		// 		var myPanel = new Panel();
		// 		myPanel.SizeFlagsHorizontal = panel.SizeFlagsHorizontal;
		// 		myPanel.SizeFlagsVertical = panel.SizeFlagsVertical;
		// 		myPanel.CustomMinimumSize = panel.CustomMinimumSize;
		// 		myPanel.AddChild(myItems[0]);
		// 		myGridContainer.AddChild(myPanel);
		// 		myItems.RemoveAt(0);
		// 	}
		// 	else
		// 	{
		// 		myGridContainer.AddChild(newPanels[0]);
		// 		newPanels.RemoveAt(0);
		// 	}	
		// }
		// //统一添加脚本
		// var sc = GD.Load<Script>("res://背包系统/脚本/格子容器.cs");
		// foreach (var child in myGridContainer.GetChildren())
		// {
		// 	child.SetScript(sc);
		// }
	}
}
