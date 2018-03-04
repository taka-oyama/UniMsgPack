﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UniMsgPack
{
	public static class MapResolver
	{
		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField;
		static readonly Dictionary<Type, Dictionary<string, FieldInfo>> cache;

		static MapResolver()
		{
			cache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
		}

		public static Dictionary<string, Type> GetFieldTypes(Type type)
		{
			if(!cache.ContainsKey(type)) {
				cache[type] = ResolveType(type);
			}
			Dictionary<string, Type> types = new Dictionary<string, Type>();
			foreach(KeyValuePair<string, FieldInfo> kv in cache[type]) {
				types.Add(kv.Key, kv.Value.FieldType);
			}
			return types;
		}

		public static FieldInfo GetField(Type type, string field)
		{
			if(!cache.ContainsKey(type)) {
				cache[type] = ResolveType(type);
			}
			if(cache[type].ContainsKey(field)) {
				return cache[type][field];
			}
			return null;
		}

		static Dictionary<string, FieldInfo> ResolveType(Type type)
		{
			FieldInfo[] fields = type.GetFields(flags);
			Dictionary<string, FieldInfo> infos = new Dictionary<string, FieldInfo>(fields.Length);
			foreach(FieldInfo info in fields) {
				infos[info.Name] = info;
			}
			return infos;
		}

		public static void Clear()
		{
			cache.Clear();
		}
	}
}
