using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
	private readonly List<Clone<int>> _clonesList = new()
	{
		new Clone<int>()
	};

	public string Execute(string query)
	{
		var queryList = query.Split(' ');
		switch (queryList[0])
		{
			case "learn":
				_clonesList[int.Parse(queryList[1]) - 1].Learn(int.Parse(queryList[2]));
				return null;
			case "rollback":
				_clonesList[int.Parse(queryList[1]) - 1].Rollback();
				return null;
			case "relearn":
				_clonesList[int.Parse(queryList[1]) - 1].Relearn();
				return null;
			case "clone":
				_clonesList.Add(new Clone<int>(
					new LinkedStack<int>(_clonesList[int.Parse(queryList[1]) - 1].CopyLearned()),
					new LinkedStack<int>(_clonesList[int.Parse(queryList[1]) - 1].CopyRollback())));
				return null;
			case "check":
				return _clonesList[int.Parse(queryList[1]) - 1].Check();
		}
		return null;
	}
}

public class Clone<T>
{
	private readonly LinkedStack<T> _learnedPrograms;
	private readonly LinkedStack<T> _rollbackPrograms;

	public Clone(LinkedStack<T> learnedPrograms, LinkedStack<T> rollbackPrograms)
	{
		_learnedPrograms = learnedPrograms;
		_rollbackPrograms = rollbackPrograms;
	}

	public Clone()
	{
		_learnedPrograms = new LinkedStack<T>();
		_rollbackPrograms = new LinkedStack<T>();
	}

	public void Learn(T program)
	{
		_learnedPrograms.Push(program);
	}

	public void Rollback()
	{
		if (_learnedPrograms.IsEmpty) 
			return;
		_rollbackPrograms.Push(_learnedPrograms.Pop());
	}

	public void Relearn()
	{
		if(_rollbackPrograms.IsEmpty)
			return;
		_learnedPrograms.Push(_rollbackPrograms.Pop());
	}

	public string Check()
	{
		return _learnedPrograms.IsEmpty ? "basic" : _learnedPrograms.Peek().ToString();
	}

	public LinkedStackItem<T> CopyLearned()
	{
		return _learnedPrograms.Copy();
	}

	public LinkedStackItem<T> CopyRollback()
	{
		return _rollbackPrograms.Copy();
	}
}

public class LinkedStackItem<T>
{
	public T Value { get; set; }
	public LinkedStackItem<T> Next { get; set; }
	public LinkedStackItem(T value)
	{
		Value = value;
	}
}

public class LinkedStack<T>
{
	private LinkedStackItem<T> _head;
	public bool IsEmpty => _head == null;

	public LinkedStack(LinkedStackItem<T> item)
	{
		_head = item;
	}

	public LinkedStack()
	{
		_head = null;
	}
 
	public void Push(T item)
	{
		var linkedStackItem = new LinkedStackItem<T>(item);
		linkedStackItem.Next = _head; 
		_head = linkedStackItem;
	}
	
	public T Pop()
	{
		if (IsEmpty)
			throw new System.InvalidOperationException("Стек пуст");
		var temporaryVariable = _head;
		_head = _head.Next; 
		return temporaryVariable.Value;
	}
	
	public T Peek()
	{
		if (IsEmpty)
			throw new System.InvalidOperationException("Стек пуст");
		return _head.Value;
	}

	public LinkedStackItem<T> Copy()
	{
		return _head;
	}
}