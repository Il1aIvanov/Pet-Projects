using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(VirtualMachine vm, Func<int> read, Action<char> write)
		{
			RegisterArithmeticCommands(vm);
			RegisterPointerCommands(vm);
			RegisterIoCommands(vm, read, write);
			RegisterAsciiCommands(vm);
		}

		private static void RegisterArithmeticCommands(VirtualMachine vm)
		{
			vm.RegisterCommand('+', state =>
			{
				if (state.Memory[state.MemoryPointer] < 255)
				{
					state.Memory[state.MemoryPointer]++;
				}
				else state.Memory[state.MemoryPointer] = 0;
			});
			
			vm.RegisterCommand('-', state =>
			{
				if (state.Memory[state.MemoryPointer] > 0)
				{
					state.Memory[state.MemoryPointer]--;
				}
				else state.Memory[state.MemoryPointer] = 255;
			});
		}

		private static void RegisterPointerCommands(VirtualMachine vm)
		{
			vm.RegisterCommand('>', state =>
			{
				state.MemoryPointer = (state.MemoryPointer + 1) % state.Memory.Length;
			});

			vm.RegisterCommand('<', state =>
			{
				state.MemoryPointer = (state.MemoryPointer + state.Memory.Length - 1) % state.Memory.Length;
			});
		}

		private static void RegisterIoCommands(VirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', state => { write(Convert.ToChar(state.Memory[state.MemoryPointer])); });

			vm.RegisterCommand(',', state => 
				{ state.Memory[state.MemoryPointer] = Convert.ToByte(read()); });
		}

		private static void RegisterAsciiCommands(VirtualMachine vm)
		{
			SaveAsciiCode(vm, 'A', 'Z');
			SaveAsciiCode(vm, 'a', 'z');
			SaveAsciiCode(vm, '0', '9');
		}
		
		private static void SaveAsciiCode(IVirtualMachine vm, char firstSymbol, char lastSymbol)
		{
			for (var symbolType = firstSymbol; symbolType <= lastSymbol; symbolType++)
			{
				var symbol = symbolType;
				vm.RegisterCommand(symbol, state =>
				{
					state.Memory[state.MemoryPointer] = Convert.ToByte(symbol);
				});
			} 
		}
	}
}