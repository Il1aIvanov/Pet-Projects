using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		private readonly Dictionary<char, Action<IVirtualMachine>> _commands = 
			new Dictionary<char, Action<IVirtualMachine>>();
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute) => _commands[symbol] = execute;

		public void Run()
		{
			for (; InstructionPointer < Instructions.Length; InstructionPointer++)
			{
				var command = Instructions[InstructionPointer];
				if (_commands.TryGetValue(command, out var value))
					value(this);
			}
		}
	}
}