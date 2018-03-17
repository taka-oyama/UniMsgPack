﻿using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace UniMsgPack
{
	public class FormatWriter
	{
		readonly Stream stream;
		readonly byte[] staticBuffer = new byte[9];

		public FormatWriter(Stream stream)
		{
			this.stream = stream;
		}

		public void WriteFormat(byte formatValue)
		{
			stream.WriteByte(formatValue);
		}

		public void WriteFormat(Format format)
		{
			stream.WriteByte(format.value);
		}

		public void WriteNil()
		{
			stream.WriteByte(Format.Nil);
		}

		public void WriteTrue()
		{
			stream.WriteByte(Format.True);
		}

		public void WriteFalse()
		{
			stream.WriteByte(Format.False);
		}

		public void WritePositiveFixInt(byte i)
		{
			if(i >= 0 || i <= sbyte.MaxValue) {
				stream.WriteByte(i);
			}
			throw new OverflowException(i + " is out of range for PositiveFixInt");
		}

		public void WriteUInt8(byte i)
		{
			stream.WriteByte(i);
		}

		public void WriteUInt16(ushort i)
		{
			staticBuffer[0] = (byte)(i >> 8);
			staticBuffer[1] = (byte)i;
			stream.Write(staticBuffer, 0, 2);
		}

		public void WriteUInt32(uint i)
		{
			staticBuffer[0] = (byte)(i >> 24);
			staticBuffer[1] = (byte)(i >> 16);
			staticBuffer[2] = (byte)(i >> 8);
			staticBuffer[3] = (byte)i;
			stream.Write(staticBuffer, 0, 4);
		}

		public void WriteUInt64(ulong i)
		{
			staticBuffer[0] = (byte)(i >> 56);
			staticBuffer[1] = (byte)(i >> 48);
			staticBuffer[2] = (byte)(i >> 40);
			staticBuffer[3] = (byte)(i >> 32);
			staticBuffer[4] = (byte)(i >> 24);
			staticBuffer[5] = (byte)(i >> 16);
			staticBuffer[6] = (byte)(i >> 8);
			staticBuffer[7] = (byte)i;
			stream.Write(staticBuffer, 0, 8);
		}

		public void WriteNegativeFixInt(sbyte i)
		{
			if(i >= -32 && i <= -1) {
				stream.WriteByte((byte)(Format.NegativeFixIntMin | (byte)i));
			}
			throw new OverflowException(i + " is out of range for NegativeFixInt");
		}

		public void WriteInt8(sbyte i)
		{
			stream.WriteByte((byte)i);
		}

		public void WriteInt16(short i)
		{
			staticBuffer[0] = (byte)(i >> 8);
			staticBuffer[1] = (byte)i;
			stream.Write(staticBuffer, 0, 2);
		}

		public void WriteInt32(int i)
		{
			staticBuffer[0] = (byte)(i >> 24);
			staticBuffer[1] = (byte)(i >> 16);
			staticBuffer[2] = (byte)(i >> 8);
			staticBuffer[3] = (byte)i;
			stream.Write(staticBuffer, 0, 4);
		}

		public void WriteInt64(long i)
		{
			staticBuffer[0] = (byte)(i >> 56);
			staticBuffer[1] = (byte)(i >> 48);
			staticBuffer[2] = (byte)(i >> 40);
			staticBuffer[3] = (byte)(i >> 32);
			staticBuffer[4] = (byte)(i >> 24);
			staticBuffer[5] = (byte)(i >> 16);
			staticBuffer[6] = (byte)(i >> 8);
			staticBuffer[7] = (byte)i;
			stream.Write(staticBuffer, 0, 8);
		}

		public void WriteFloat32(float f)
		{
			byte[] bytes = BitConverter.GetBytes(f);
			if(BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}
			stream.Write(bytes, 0, 4);
		}

		public void WriteFloat64(double f)
		{
			byte[] bytes = BitConverter.GetBytes(f);
			if(BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}
			stream.Write(bytes, 0, 8);
		}

		public void WriteString(string s)
		{
			if(s.Length <= 31) {
				WriteFormat((byte)(Format.FixStrMin | (byte)s.Length));
			}
			else if(s.Length <= byte.MaxValue) {
				WriteFormat(Format.UInt8);
				WriteUInt8((byte)s.Length);
			}
			else if(s.Length <= ushort.MaxValue) {
				WriteFormat(Format.UInt16);
				WriteUInt16((ushort)s.Length);
			}
			else {
				WriteFormat(Format.UInt32);
				WriteUInt32((uint)s.Length);
			}

			byte[] stringAsBytes = Encoding.UTF8.GetBytes(s);
			stream.Write(stringAsBytes, 0, stringAsBytes.Length);
		}

		public void WriteBinary(byte[] bytes)
		{
			if(bytes.Length <= byte.MaxValue) stream.WriteByte(Format.Bin8);
			else if(bytes.Length <= ushort.MaxValue) stream.WriteByte(Format.Bin16);
			else stream.WriteByte(Format.Bin32);
			stream.Write(bytes, 0, bytes.Length);
		}

		public void WriteArrayLength(int length)
		{
			if(length < 16) WriteFormat((byte)(length | Format.FixArrayMin));
			else if(length <= ushort.MaxValue) WriteFormat(Format.Array16);
			else WriteFormat(Format.Array32);
		}

		public void WriteMapLength(int length)
		{
			if(length < 16) WriteFormat((byte)(length | Format.FixMapMin));
			else if(length <= ushort.MaxValue) WriteFormat(Format.Map16);
			else WriteFormat(Format.Map32);
		}

		public void WriteExtHeader(uint length, sbyte extType)
		{
			if(length == 1) stream.WriteByte(Format.FixExt1);
			if(length == 2) stream.WriteByte(Format.FixExt2);
			if(length == 4) stream.WriteByte(Format.FixExt4);
			if(length == 8) stream.WriteByte(Format.FixExt8);
			if(length == 16) stream.WriteByte(Format.FixExt16);
			if(length <= byte.MaxValue) {
				stream.WriteByte(Format.Ext8);
				stream.WriteByte((byte)length);
			}
			if(length <= byte.MaxValue) {
				stream.WriteByte(Format.Ext16);
				stream.WriteByte((byte)length);
			}
			stream.WriteByte((byte)extType);
		}

		public Format GetFormatForInt(byte value)
		{
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			return new Format((byte)(value | Format.PositiveFixIntMin));
		}

		public Format GetFormatForInt(ushort value)
		{
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			return new Format((byte)(value | Format.PositiveFixIntMin));
		}

		public Format GetFormatForInt(uint value)
		{
			if(value > ushort.MaxValue) return new Format(Format.UInt32);
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			return new Format((byte)(value | Format.PositiveFixIntMin));
		}

		public Format GetFormatForInt(ulong value)
		{
			if(value > uint.MaxValue) return new Format(Format.UInt64);
			if(value > ushort.MaxValue) return new Format(Format.UInt32);
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > (ulong)sbyte.MaxValue) return new Format(Format.UInt8);
			return new Format((byte)(value | Format.PositiveFixIntMin));
		}

		public Format GetFormatForInt(sbyte value)
		{
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			if(value >= 0) return new Format((byte)((byte)value | Format.PositiveFixIntMin));
			if(value >= -32) return new Format((byte)((byte)value | Format.NegativeFixIntMin));
			return new Format(Format.Int8);
		}

		public Format GetFormatForInt(short value)
		{
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			if(value >= 0) return new Format((byte)(value | Format.PositiveFixIntMin));
			if(value >= -32) return new Format((byte)(value | Format.NegativeFixIntMin));
			if(value >= sbyte.MinValue) return new Format(Format.Int8);
			return new Format(Format.Int16);
		}

		public Format GetFormatForInt(int value)
		{
			if(value > ushort.MaxValue) return new Format(Format.UInt32);
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			if(value >= 0) return new Format((byte)(value | Format.PositiveFixIntMin));
			if(value >= -32) return new Format((byte)(value | Format.NegativeFixIntMin));
			if(value >= sbyte.MinValue) return new Format(Format.Int8);
			if(value >= short.MinValue) return new Format(Format.Int16);
			return new Format(Format.Int32);
		}

		public Format GetFormatForInt(long value)
		{
			if(value > uint.MaxValue) return new Format(Format.UInt64);
			if(value > ushort.MaxValue) return new Format(Format.UInt32);
			if(value > byte.MaxValue) return new Format(Format.UInt16);
			if(value > sbyte.MaxValue) return new Format(Format.UInt8);
			if(value >= 0) return new Format((byte)(value | Format.PositiveFixIntMin));
			if(value >= -32) return new Format((byte)(value | Format.NegativeFixIntMin));
			if(value >= sbyte.MinValue) return new Format(Format.Int8);
			if(value >= short.MinValue) return new Format(Format.Int16);
			if(value >= int.MinValue) return new Format(Format.Int32);
			return new Format(Format.Int64);
		}
	}
}
