using Godot;
using System;

public partial class NotificationSystem : Control
{
	public static NotificationSystem Instance { get; private set; }
	[Export] public PackedScene MessageScene;

	private VBoxContainer _messagesContainer;

	public override void _Ready()
	{
		if (Instance != null && Instance != this)
		{
			QueueFree();
			return;
		}
		Instance = this;
		
		_messagesContainer = GetNode<VBoxContainer>("CanvasLayer/MarginContainer/VBoxContainer");
	}

	public void ShowMessage(string text, EMessageType messageType = EMessageType.Default)
	{
		NotificationMessage notificationMsg = MessageScene.Instantiate<NotificationMessage>();
		notificationMsg.SetMessage(text, messageType);
		_messagesContainer.AddChild(notificationMsg);
	}
}
