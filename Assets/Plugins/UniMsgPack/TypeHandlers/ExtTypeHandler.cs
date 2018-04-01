﻿using System;

namespace UniMsgPack
{
	public interface IExtTypeHandler : ITypeHandler
	{
		sbyte ExtType { get; }

		object ReadExt(uint length, FormatReader reader);
	}
}
