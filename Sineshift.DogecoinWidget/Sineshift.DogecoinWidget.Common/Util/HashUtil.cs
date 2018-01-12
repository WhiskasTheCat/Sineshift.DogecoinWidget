using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class HashUtil
	{
		public static int CombineHashes(int a, int b)
		{
			int hash = 17;
			hash = hash * 31 + a;
			hash = hash * 31 + b;
			return hash;
		}

		public static int CombineHashes(params int[] hashes)
		{
			int hash = 17;
			int factor = 31;
			for (int i = 0; i < hashes.Length; ++i)
			{
				hash = (hash * factor) + hashes[i];
			}
			return hash;
		}
	}
}
