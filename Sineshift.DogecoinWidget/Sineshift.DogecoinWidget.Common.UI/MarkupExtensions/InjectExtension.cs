using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Sineshift.DogecoinWidget.Common.UI
{
    public class InjectExtension : MarkupExtension
    {
        Type type;

        public InjectExtension(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ServiceLocator.Current.Get(type);
        }
    }
}
