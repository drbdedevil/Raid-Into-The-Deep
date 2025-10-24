using Godot;
using System;
using System.Collections.Generic;

public partial class PopupNavigator : PopupPanel
{
    [Export]
    public NodePath SceneContainerPath = "MarginContainer/VBoxContainer/SceneContainer";

    [Export]
    public bool PreserveInstances = true;

    [Signal]
    public delegate void StackEmptyEventHandler();

    private Control _container;
    private Stack<Node> _history = new();

    public override void _Ready()
    {
        _container = GetNode<Control>(SceneContainerPath);
        if (_container == null)
        {
            GD.PrintErr("PopupNavigator: SceneContainer not found at " + SceneContainerPath);
        }
    }

    public void PushScene(PackedScene scenePrefab)
    {
        if (scenePrefab == null)
        {
            return;
        }

        var instance = scenePrefab.Instantiate();
        PushInstance(instance);
    }

    public void PushInstance(Node newInstance)
    {
        if (_container == null || newInstance == null)
        {
            return;
        }

        var current = GetCurrent();
        if (current != null)
        {
            if (PreserveInstances)
            {
                _container.RemoveChild(current);
                CallOnHide(current);
                _history.Push(current);
            }
            else
            {
                _container.RemoveChild(current);
                CallOnHide(current);
                current.QueueFree();
            }
        }

        _container.AddChild(newInstance);
        CallOnShow(newInstance);

        if (newInstance.HasSignal("RequestBack"))
        {
            newInstance.Connect("RequestBack", new Callable(this, nameof(Pop)));
        }
    }
    public void Pop()
    {
        if (_container == null)
        {
            return;
        }

        var current = GetCurrent();
        if (current != null)
        {
            _container.RemoveChild(current);
            CallOnHide(current);
            current.QueueFree();
        }

        if (_history.Count > 0)
        {
            var prev = _history.Pop();
            _container.AddChild(prev);
            CallOnShow(prev);
            GetViewport().GuiReleaseFocus();
        }
        else
        {
            EmitSignal(nameof(StackEmpty));
        }
    }

    public bool CanGoBack() => _history.Count > 0;
    public bool IsHistoryEmpty() => _history.Count == 0;
    public bool IsSomethingOpen() => _container.GetChildCount() != 0;

    public void ClearHistory(bool freeNodes = true)
    {
        while (_history.Count > 0)
        {
            var n = _history.Pop();
            if (freeNodes)
            {
                n.QueueFree();
            }
        }

        var current = GetCurrent();
        if (current != null)
        {
            _container.RemoveChild(current);
            if (freeNodes)
            {
                current.QueueFree();
            }
        }
    }

    public Node GetCurrent()
    {
        if (_container.GetChildCount() == 0)
        {
            return null;
        }
        return _container.GetChild(0);
    }

    private void CallOnShow(Node node)
    {
        if (node == null)
        {
            return;
        }

        if (node is IStackPage sp)
        {
            sp.OnShow();
            return;
        }

        if (node.HasMethod("OnShow"))
        {
            node.Call("OnShow");
        }
        else if (node.HasMethod("on_show"))
        {
            node.Call("on_show");
        }
    }
    private void CallOnHide(Node node)
    {
        if (node == null)
        {
            return;
        }

        if (node is IStackPage sp)
        {
            sp.OnHide();
            return;
        }

        if (node.HasMethod("OnHide"))
        {
            node.Call("OnHide");
        }
        else if (node.HasMethod("on_hide"))
        {
            node.Call("on_hide");
        }
    }

    public void SetPanelLabelName(string NewName)
    {
        Label panelLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
        panelLabel.Text = NewName;
    }
}

public interface IStackPage
{
    void OnShow();
    void OnHide();
}
