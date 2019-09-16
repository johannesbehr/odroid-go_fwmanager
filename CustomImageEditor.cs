using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWManager
{
    public class CustomImageEditor: ImageEditor
    {

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //return base.EditValue(context, provider, value);

            using (EditImageDialog dialog = new EditImageDialog()) {
                dialog.Image = (Image)value;
                if(dialog.ShowDialog()== System.Windows.Forms.DialogResult.OK) {
                    return (dialog.Image);
                }
            }

            return value;

        }




    }
}
