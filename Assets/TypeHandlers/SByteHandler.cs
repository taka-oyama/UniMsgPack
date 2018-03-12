﻿using System;
using UnityEngine;

namespace UniMsgPack
{
	public class SByteHandler : ITypeHandler
	{
		public object Read(Format format, FormatReader reader)
		{
			if(format.IsPositiveFixInt) return (sbyte)reader.ReadPositiveFixInt(format);
			if(format.IsUInt8) return Convert.ToSByte(reader.ReadUInt8());
			if(format.IsNegativeFixInt) return reader.ReadNegativeFixInt(format);
			if(format.IsInt8) return reader.ReadInt8();
			throw new FormatException();
		}

		public void Write(object obj, FormatWriter writer)
		{
			sbyte value = Convert.ToSByte(obj);
			Format format = writer.GetFormatForInt(value);
			writer.WriteFormat(format);
			if(format.IsPositiveFixInt) writer.WritePositiveFixInt((byte)value);
			if(format.IsUInt8) writer.WriteUInt8((byte)value);
			if(format.IsNegativeFixInt) writer.WriteNegativeFixInt(value);
			if(format.IsInt8) writer.WriteInt8(value);
			throw new FormatException();
		}
	}
}