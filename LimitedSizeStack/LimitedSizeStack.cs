using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly int _undoLimit;
    private readonly LinkedList<T> _linkedList = new LinkedList<T>();
    public int Count => _linkedList.Count;
	
    public LimitedSizeStack(int undoLimit)
    {
        _undoLimit = undoLimit;
    }

    public void Push(T item)
    {
        _linkedList.AddLast(item);
        if (_linkedList.Count > _undoLimit)
        {
            _linkedList.RemoveFirst();
        }
    }

    public T Pop()
    {
        var lastValue = _linkedList.Last.Value;
        _linkedList.RemoveLast();
        return lastValue;
    }
}