using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public interface INavigationTarget
	{
		void RequestNavigate(object view, NavigationType type);
	}
}
