using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		private static readonly Dictionary<int, int> BracketPairs = new Dictionary<int, int>();
		private static readonly Stack<int> BracketIndex = new Stack<int>();
		
		public static void RegisterTo(IVirtualMachine vm)
		{
			MatchBracketPair(vm);
			vm.RegisterCommand('[', state => 
			{
				if (state.Memory[state.MemoryPointer] == 0)
				{
					state.InstructionPointer = BracketPairs[state.InstructionPointer];
				}
			});
			vm.RegisterCommand(']', state =>
			{
				if (state.Memory[state.MemoryPointer] != 0)
				{
					state.InstructionPointer = BracketPairs[state.InstructionPointer];
				}
			});
		}

		private static void MatchBracketPair(IVirtualMachine vm)
		{
			for (var i = 0; i < vm.Instructions.Length; i++)
			{
				if (vm.Instructions[i] == '[')
				{
					BracketIndex.Push(i);
				}
				if (vm.Instructions[i] == ']')
				{
					BracketPairs[i] = BracketIndex.Peek();
					BracketPairs[BracketIndex.Pop()] = i;
				}
			}
		}
	}
}