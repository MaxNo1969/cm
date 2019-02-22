using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace CM
{
    public class MyListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return (UITypeEditorEditStyle.Modal); }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (FRListEditor f = new FRListEditor(value)) { f.ShowDialog(); }
            return (value);
        }
    }
}