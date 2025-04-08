using Godot;
using System;

public partial class 格子容器 : Panel
{
    public override void _Ready()
    {
        // 连接输入事件
        GuiInput += HandleInputEvent;
    }

    public override void _Process(double delta)
    {
        // 如果有物品跟随鼠标，更新物品位置
        if (背包系统.followingItem != null)	
        {
            背包系统.followingItem.Position = GetViewport().GetMousePosition() - 背包系统.followingItem.Scale * (背包系统.followingItem as Control).Size ;
        }
    }
    private void HandleInputEvent(InputEvent @event)
	{
		// 检测鼠标左键点击
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			GD.Print("点击格子");
			// 优先处理放下物品的情况
			if (背包系统.followingItem != null)
			{
				GD.Print("放下物品");
				PlaceItem();
			}
			else
			{
				// 只有没有物品跟随时才尝试拿起物品
				GD.Print("点击格子，尝试拿起物品");
				if (GetChildCount() > 0)
				{
					GD.Print("拿起物品");
					PickupItem();
				}
			}
		}
	}
    private void PickupItem()
    {
        // 获取第一个子节点作为物品
        物品格子 item = GetChild(0) as 物品格子;
        item.ResCount.Visible = false;
        // 保存原始信息
        背包系统.originalParent = this;
        背包系统.originalPosition = item.Position;
		背包系统.originalSize = item.Size; // 保存物品原始大小
        // 从当前格子移除物品
        RemoveChild(item);
        // 将物品添加到场景根节点，使其能在任何地方显示
        GetTree().Root.AddChild(item);
        // 设置物品大小为格子大小
        item.Size = Size;
        // 设置为跟随物品
        背包系统.followingItem = item;
        // 更新物品位置到鼠标位置
        背包系统.followingItem.Position = GetViewport().GetMousePosition() - 背包系统.followingItem.Scale * (背包系统.followingItem as Control).Size / 2;
    }
    private void PlaceItem()
    {
        //如果格子为空，则直接放下物品
        if (GetChildCount() == 0)
        {
            // 从根节点移除物品
            GetTree().Root.RemoveChild(背包系统.followingItem);
            var item = 背包系统.followingItem;
            item.ResCount.Visible = true;
            // 恢复物品原始大小
            item.Size = 背包系统.originalSize;
            // 将物品添加到当前格子
            AddChild(item);
            // 设置物品位置为原始位置
            item.Position = 背包系统.originalPosition;
            // 假设我们希望物品居中显示在格子中
            item.Position = new Vector2(Size.X / 2 - (item as Control).Size.X / 2, 
                                        Size.Y / 2 - (item as Control).Size.Y / 2);
            // 清空跟随物品引用
            背包系统.followingItem = null;
        }
        else// 如果格子不为空，则尝试合并物品或交换物品
        {
            // 获取第一个子节点作为物品
            物品格子 item1 = GetChild(0) as 物品格子;
            物品格子 item2 = 背包系统.followingItem;
            if(item1.Res.Id == item2.Res.Id)
            {
                // 如果物品ID相同，则尝试合并物品
                mergeItem(item1, item2);
            }
            else
            {
                // 如果物品ID不同，则尝试交换物品
                exchangeItem(item1, item2);
            }
        }
    }
    private void mergeItem(物品格子 item1, 物品格子 item2)
    {
        // 从根节点移除物品
        GetTree().Root.RemoveChild(背包系统.followingItem);
        // 合并物品数量
        item1.ResCount.Text = (item1.Res.Count + item2.Res.Count).ToString();
        item1.Res.Count += item2.Res.Count;
        // 清空跟随物品引用
        背包系统.followingItem = null;
    }
    private void exchangeItem(物品格子 item1, 物品格子 item2)
    {
        // 1. 从当前格子移除item1
        RemoveChild(item1);
        
        // 2. 从根节点移除跟随的item2
        GetTree().Root.RemoveChild(背包系统.followingItem);
        
        // 3. 将item2放入当前格子
        AddChild(item2);
        item2.ResCount.Visible = true;
        item2.Size = 背包系统.originalSize;
        item2.Position = new Vector2(Size.X / 2 - item2.Size.X / 2, 
                                Size.Y / 2 - item2.Size.Y / 2);
        
        // 4. 设置item1为新的跟随物品
        背包系统.followingItem = item1;
        item1.ResCount.Visible = false;
        item1.Size = Size;
        
        // 5. 将item1添加到场景根节点
        GetTree().Root.AddChild(item1);
        
        // 6. 更新位置
        背包系统.followingItem.Position = GetViewport().GetMousePosition() - item1.Scale * item1.Size / 2;
        
        // 7. 更新原始信息
        背包系统.originalParent = this;
        背包系统.originalPosition = item2.Position;
        背包系统.originalSize = item2.Size;
    }
}
