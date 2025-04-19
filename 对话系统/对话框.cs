using Godot;
using System;
using System.Threading.Tasks;
public partial class 对话框 : Control
{
	[Export]public string myJson="res://对话系统/json实现/txt1.json";
	[Export]public Label myLabel;
	[Export]public Button myButton;
	public override void _Ready()
	{
		// 1. 使用 Godot 的 FileAccess 读取文件
    if (!FileAccess.FileExists(myJson))
    {
        GD.PrintErr($"文件不存在: {myJson}");
        return;
    }

    using var file = FileAccess.Open(myJson, FileAccess.ModeFlags.Read);
    string jsonText = file.GetAsText();
    file.Close(); // 确保关闭文件

    // 2. 解析 JSON
    try
    {
        var jsonData = Json.ParseString(jsonText);
        if (jsonData.VariantType == Variant.Type.Nil)
        {
            GD.PrintErr("JSON 解析失败");
            return;
        }

        // 3. 提取 dialogue_only -> text1
        var dialogueDict = jsonData.AsGodotDictionary()["dialogue_only"].AsGodotDictionary();
        string text1 = dialogueDict["text1"].AsString();
        _ = DisplayTextOneByOne(myLabel, text1);
    }
    catch (Exception ex)
    {
        GD.PrintErr($"JSON 解析失败: {ex.Message}");
    }
	}
	public override void _Process(double delta)
	{

	}
    public async Task DisplayTextOneByOne(Label label, string myText, float interval = 0.1f)
    {
        // 清空初始文本
        label.Text = ""; 
        // 逐个字符显示
        for (int i = 0; i < myText.Length; i++)
        {
            label.Text += myText[i];
            await Task.Delay((int)(interval * 1000)); // 转换为毫秒
        }
    }
}
