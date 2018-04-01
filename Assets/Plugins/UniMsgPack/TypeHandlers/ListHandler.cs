﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniMsgPack
{
	public class ListHandler : ITypeHandler
	{
		readonly Type innerType;
		readonly ITypeHandler innerTypeHandler;

		public ListHandler(Type innerType, ITypeHandler innerTypeHandler)
		{
			this.innerType = innerType;
			this.innerTypeHandler = innerTypeHandler;
		}

		public object Read(Format format, FormatReader reader)
		{
			Type listType = typeof(List<>).MakeGenericType(new[] { innerType });
			IList list = (IList)Activator.CreateInstance(listType);

			if(format.IsArrayFamily) {
				int size = reader.ReadArrayLength(format);
				for(int i = 0; i < size; i++) {
					list.Add(innerTypeHandler.Read(reader.ReadFormat(), reader));
				}
				return list;
			}
			if(format.IsNil) {
				return list;
			}
			throw new FormatException();
		}

		public void Write(object obj, FormatWriter writer)
		{
			if(obj == null) {
				writer.WriteNil();
				return;
			}
			IList values = (IList)obj;
			writer.WriteArrayHeader(values.Count);
			foreach(object value in values) {
				innerTypeHandler.Write(value, writer);
			}
		}
	}
}
