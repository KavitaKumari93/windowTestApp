using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace FF.Cockpit.Common
{
    public class EnumBindingExtension : MarkupExtension
    {
        public Type EnumType { get; private set; }

        public EnumBindingExtension(Type type)
        {
            this.EnumType = type;
        }
        public override object ProvideValue(IServiceProvider _serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
