using Godot;
using System;

public partial class NextButton : Button
{
	[Export] public DialogueBox myDialogueBox;
	public override void _Ready()
	{
		this.ButtonUp += myDialogueBox.NextButtonClick;
	}
	public override void _Process(double delta)
	{
	}
}
