﻿using System;
using UnityEngine;

namespace SouthPointe.Serialization.MessagePack
{
	static class CustomExtensions
	{
		internal static bool IsNullable(this Type type)
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
