﻿using System;
using UnityEngine;

namespace UniMsgPack
{
	public static class Extension
	{
		public static bool IsNullable(this Type type)
		{
			if(type.IsValueType) {
				if(Nullable.GetUnderlyingType(type) != null) {
					return true;
				}
			}
			return false;
		}
	}
}