﻿namespace Sino.Web.Test
{
    public interface ICalcService
	{
		bool Disposed { get; }
		bool Initialized { get; }

		int Sum(int x, int y);
	}
}
