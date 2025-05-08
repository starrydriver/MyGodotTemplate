using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class DialogueBox : Control
{
	[Export] public Json  myJson;
	[Export] public Label myLabel1;
    [Export] public Label myLabel2;
    [Export]public int eventId = 0;
    private int diaCurrent = 0;
    private string diaType = "";
    private CancellationTokenSource _currentDisplayCTS;
    private bool isAdd = false;
    private Godot.Collections.Dictionary dialogueDict = new Godot.Collections.Dictionary();
	public override void _Ready()
	{	
        JsonParse();
	}
	public override void _Process(double delta)
	{

	}
    public void JsonParse()
    {
        if (!FileAccess.FileExists(myJson.ResourcePath))
        {
            GD.PrintErr($"文件不存在: {myJson.ResourcePath}");
            return;
        }
        using var file = FileAccess.Open(myJson.ResourcePath, FileAccess.ModeFlags.Read);
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
                diaType = "dialogue_only";
            }
            else if (dict.ContainsKey("dialogue_mutual"))
            {
                dialogueDict = dict["dialogue_mutual"].AsGodotDictionary();
                diaType = "dialogue_mutual";
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
        if(diaType == "dialogue_only")
        {
            if (key.StartsWith("text"))
            {
                var text = dialogueDict[key].AsString();
                _ = DisplayTextOneByOne(myLabel1,text);
            }
        }
        else if(diaType == "dialogue_mutual")
        {
            if (key.StartsWith("textA"))
            {
                var text = dialogueDict[key].AsString();
                _ = DisplayTextOneByOne(myLabel1,text);
            }
            else if (key.StartsWith("textB"))
            {
                var text = dialogueDict[key].AsString();
                _ = DisplayTextOneByOne(myLabel2,text);
            }
        }
    }
    public void NextButtonClick()
    {
        JsonTextParse();
        if(isAdd == false)
        {
            return;
        }
        diaCurrent++;
    }
    public async Task DisplayTextOneByOne(Label label, string myText, float interval = 0.1f)
    {
        // 取消正在进行的显示任务
        _currentDisplayCTS?.Cancel();
        _currentDisplayCTS = new CancellationTokenSource();
        // 清空初始文本
        label.Text = "";
        try
        {
            // 逐个字符显示
            for (int i = 0; i < myText.Length; i++)
            {
                // 检查是否被取消
                if (_currentDisplayCTS.IsCancellationRequested)
                {
                    isAdd = false;
                    label.Text = myText;
                    return;
                }
                isAdd = true; 
                label.Text += myText[i];
                await Task.Delay((int)(interval * 1000), _currentDisplayCTS.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // 正常取消时立即显示完整文本
            label.Text = myText;
        }
    }
}
