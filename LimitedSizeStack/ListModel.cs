using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit { get; set; }
	private readonly LimitedSizeStack<ICommand> _historyCommands;
	
	
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		_historyCommands = new LimitedSizeStack<ICommand>(undoLimit);
	}
	
	public void AddItem(TItem item)
	{
		ExecuteCommand(new AddItemToListCommand<TItem>(new ActionReceiver<TItem>(Items), Items, item));
	}

	public void RemoveItem(int index)
	{
		ExecuteCommand(new RemoveItemFromListCommand<TItem>
			(new ActionReceiver<TItem>(Items), Items, Items[index], index));
	}
	
	public bool CanUndo()
	{
		return _historyCommands.Count > 0;
	}

	public void Undo()
	{
		if (!CanUndo()) return;
		var lastCommand = _historyCommands.Pop();
		lastCommand.Undo();
	}
	
	private void ExecuteCommand(ICommand command)
	{
		command.Execute();
		_historyCommands.Push(command);
	}
}

public interface ICommand
{
	public void Execute();
	public void Undo();
}

public class AddItemToListCommand<TItem> : ICommand
{
	private readonly ActionReceiver<TItem> _actionReceiver;
	private readonly List<TItem> _listItems;
	private readonly TItem _item;
	private int _index;
	
	public AddItemToListCommand
		(ActionReceiver<TItem> actionReceiver, List<TItem> listItems, TItem item)
	{
		_actionReceiver = actionReceiver;
		_listItems = listItems;
		_item = item;
	}

	public void Execute()
	{
		_index = _listItems.Count;
		_actionReceiver.AddItemToList(_item);
	}

	public void Undo()
	{
		if (_index >= 0 && _index < _listItems.Count)
		{
			_actionReceiver.RemoveItemFromList(_index);
		}
	}
}

public class RemoveItemFromListCommand<TItem> : ICommand
{
	private readonly ActionReceiver<TItem> _actionReceiver;
	private readonly List<TItem> _listItems;
	private TItem _item;
	private readonly int _index;

	public RemoveItemFromListCommand
		(ActionReceiver<TItem> actionReceiver, List<TItem> listItems, TItem item, int index)
	{
		_actionReceiver = actionReceiver;
		_listItems = listItems;
		_item = item;
		_index = index;
	}

	public void Execute()
	{
		if (_index < 0 || _index >= _listItems.Count) return;
		_item = _listItems[_index];
		_actionReceiver.RemoveItemFromList(_index);
	}

	public void Undo()
	{
		if (_item != null)
			_actionReceiver.AddItemAfterRemove(_index, _item);
	}
}

public class ActionReceiver<TItem>
{
	private readonly List<TItem> _items;

	public ActionReceiver(List<TItem> items)
	{
		_items = items;
	}
	
	public void AddItemToList(TItem item)
	{
		_items.Add(item);
	}

	public void RemoveItemFromList(int index)
	{
		if (index >= 0 && _items.Count > 0)
			_items.RemoveAt(index);
	}

	public void AddItemAfterRemove(int index, TItem item)
	{
		if (index < 0) index = 0; 
		if (index > _items.Count) index = _items.Count;
		_items.Insert(index, item);
	}
}