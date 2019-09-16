using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWManager
{
    public class CustomBinaryEditor: UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //return base.EditValue(context, provider, value);

            using (EditBinaryDialog dialog = new EditBinaryDialog()) {
                dialog.BinaryData = (Byte[])value;
                if(dialog.ShowDialog()== System.Windows.Forms.DialogResult.OK) {
                    return (dialog.BinaryData);
                }
            }

            return value;

        }




    }
}
