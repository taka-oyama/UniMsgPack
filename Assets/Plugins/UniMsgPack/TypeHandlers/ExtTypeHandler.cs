﻿using System;

namespace UniMsgPack
{
	public abstract class ExtTypeHandler : ITypeHandler
	{
		public abstract sbyte ExtType { get; }

		public object Read(Format format, FormatReader reader)
		{
			uint length = reader.ReadExtLength(format);
			if(ExtType == reader.ReadExtType(format)) {
				return Read(length, reader);
			}
			throw new FormatException();
		}

		public abstract object Read(uint length, FormatReader reader);

		public abstract void Write(object obj, FormatWriter writer);
	}
}