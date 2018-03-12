﻿using System;
using UnityEngine;

namespace UniMsgPack
{
	public class IntHandler : ITypeHandler
	{
		public object Read(Format format, FormatReader reader)
		{
			if(format.IsPositiveFixInt) return (int)reader.ReadPositiveFixInt(format);
			if(format.IsUInt8) return (int)reader.ReadUInt8();
			if(format.IsUInt16) return (int)reader.ReadUInt16();
			if(format.IsUInt32) return Convert.ToInt32(reader.ReadUInt32());
			if(format.IsNegativeFixInt) return (int)reader.ReadNegativeFixInt(format);
			if(format.IsInt8) return (int)reader.ReadInt8();
			if(format.IsInt16) return (int)reader.ReadInt16();
			if(format.IsInt32) return reader.ReadInt32();
			throw new FormatException();
		}

		public void Write(object obj, FormatWriter writer)
		{
			int value = Convert.ToInt32(obj);
			Format format = writer.GetFormatForInt(value);
			writer.WriteFormat(format);
			if(format.IsPositiveFixInt) writer.WritePositiveFixInt((byte)value);
			if(format.IsUInt8) writer.WriteUInt8((byte)value);
			if(format.IsUInt16) writer.WriteUInt16((ushort)value);
			if(format.IsUInt32) writer.WriteUInt32((uint)value);
			if(format.IsNegativeFixInt) writer.WriteNegativeFixInt((sbyte)value);
			if(format.IsInt8) writer.WriteInt8((sbyte)value);
			if(format.IsInt16) writer.WriteInt16((short)value);
			if(format.IsInt32) writer.WriteInt32(value);
			throw new FormatException();
		}
	}
}