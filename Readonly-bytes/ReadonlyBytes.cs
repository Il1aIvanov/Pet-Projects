using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		private readonly byte[] _data;
		private readonly int _hashCode;
		public int Length => _data.Length;

		public ReadonlyBytes(params byte[] data)
		{
			_data = data ?? throw new ArgumentNullException(nameof(data));
			_hashCode = ComputeHash(_data);
		}

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index >= Length)
				{
					throw new IndexOutOfRangeException();
				}
				return _data[index];
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ReadonlyBytes)) return false;
			if(obj.GetType() != GetType()) return false;
			var bytes = obj as ReadonlyBytes;
			if (_hashCode != bytes._hashCode) return false;
			for (var i = 0; i < _data.Length; i++)
			{
				if (_data[i] != bytes[i]) return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		public override string ToString()
		{
			return $"[{string.Join(", ", _data)}]";
		}
		
		public IEnumerator<byte> GetEnumerator()
		{
			for (var i = 0; i < _data.Length; i++)
			{
				yield return _data[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		private static int ComputeHash(byte[] data)
		{
			var hashResult = 1;
			foreach (var bytes in data)
			{
				unchecked
				{
					hashResult *= 16777619;
					hashResult -= bytes;
				}
			}
			return hashResult;
		}
	}
}