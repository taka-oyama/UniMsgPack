﻿using System;
using System.Collections;
using UnityEngine;

namespace SouthPointe.Serialization.MessagePack
{
	public class DictionaryHandler : ITypeHandler
	{
		readonly Type type;
		readonly ITypeHandler keyHandler;
		readonly ITypeHandler valueHandler;

		public DictionaryHandler(SerializationContext context, Type type)
		{
			Type[] innerTypes = type.GetGenericArguments();
			this.type = type;
			this.keyHandler = context.typeHandlers.Get(innerTypes[0]);
			this.valueHandler = context.typeHandlers.Get(innerTypes[1]);
		}

		public object Read(Format format, FormatReader reader)
		{
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(type);
			if(format.IsNil) {
				return dictionary;
			}
			int size = reader.ReadMapLength(format);
			while(size > 0) {
				object key = keyHandler.Read(reader.ReadFormat(), reader);
				object value = valueHandler.Read(reader.ReadFormat(), reader);
				dictionary.Add(key, value);
				size = size - 1;
			}
			return dictionary;
		}

		public void Write(object obj, FormatWriter writer)
		{
			if(obj == null) {
				writer.WriteNil();
				return;
			}
			IDictionary dictionary = (IDictionary)obj;
			writer.WriteMapHeader(dictionary.Count);
			foreach(DictionaryEntry kv in dictionary) {
				keyHandler.Write(kv.Key, writer);
				valueHandler.Write(kv.Value, writer);
			}
		}
	}
}
