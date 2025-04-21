using Godot;
using System;
using System.Threading.Tasks;

public partial class DialogueBox : Control
{
	[Export] public string myJson="res://对话系统/json实现/txt1.json";
	[Export] public Label myLabel;
	[Export] public Button myButton1;
    [Export] public Button myButton2;
    [Export]public int eventId = 0;
    private int diaCurrent = 0;
    private Godot.Collections.Dictionary dialogueDict = new Godot.Collections.Dictionary();
	public override void _Ready()
	{	
        JsonParse();
        myButton1.ButtonUp +=  NextButtonClick;
	}
	public override void _Process(double delta)
	{

	}
    public void JsonParse()
    {
        if (!FileAccess.FileExists(myJson))
        {
            GD.PrintErr($"文件不存在: {myJson}");
            return;
        }
        using var file = FileAccess.Open(myJson, FileAccess.ModeFlags.Read);
        string jsonText = file.GetAsText();
        file.Close(); // 确保关闭文件
        try
        {
            var jsonData = Json.ParseString(jsonText);
            if (jsonData.VariantType == Variant.Type.Nil)
            {
                GD.PrintErr("JSON 解析失败");
                return;
            }
            var dict = jsonData.AsGodotDictionary();
            if (dict.ContainsKey("dialogue_only"))
            {
                dialogueDict = dict["dialogue_only"].AsGodotDictionary();
            }
            else if (dict.ContainsKey("dialogue_mutual"))
            {
                dialogueDict = dict["dialogue_mutual"].AsGodotDictionary();
            }
            else
            {
                GD.PrintErr("JSON 格式错误");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"JSON 解析失败: {ex.Message}");
        }
    }
    public void JsonTextParse()
    {
        var keys = new Godot.Collections.Array(dialogueDict.Keys);
        string key = "";
        if (diaCurrent >= 0 && diaCurrent < keys.Count)
        {
            key = keys[diaCurrent].AsString();
        }
        if (key.StartsWith("text"))
        {
            var text = dialogueDict[key].AsString();
            _ = DisplayTextOneByOne(myLabel,text);
        }
    }
    public void NextButtonClick()
    {
        JsonTextParse();
        diaCurrent++;
    }
    public async Task DisplayTextOneByOne(Label label, string myText, float interval = 0.1f)
    {
        // 清空初始文本
        label.Text = ""; 
        // 逐个字符显示
        for (int i = 0; i < myText.Length; i++)
        {
            label.Text += myText[i];
            await Task.Delay((int)(interval * 1000));// 转换为毫秒
        }
    }
}
