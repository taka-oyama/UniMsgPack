﻿using System;
using System.IO;
using UnityEngine;

namespace MessagePack
{
	public class FormatException : System.FormatException
	{
		public FormatException() { }

		public FormatException(string message) : base(message) { }

		public FormatException(ITypeHandler handler, Format format, FormatReader reader) : base(string.Format("{0}: Undefined Format {1} at position {2}", handler.GetType(), format, reader.stream.Position)) { }

		public FormatException(Format format, FormatReader reader) : base(string.Format("Undefined Format {0} at Position: {1}", format, reader.stream.Position)) { }
	}
}
