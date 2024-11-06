using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor.CustomControls
{
    internal static class ControlExtension
    {
        public static void Bind(this Control target, string targetProperty, object source, string sourceProperty, Func<object?, object?> expression)
        {
            var binding = new Binding(targetProperty, source, sourceProperty, true, DataSourceUpdateMode.Never);
            binding.Format += (sender, e) => e.Value = expression(e.Value);
            target.DataBindings.Add(binding);
        }
    }
}
