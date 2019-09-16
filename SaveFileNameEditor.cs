using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FWManager
{
    /// <summary>
    /// This class is taken from: https://stackoverflow.com/questions/1858960/how-can-i-get-a-propertygrid-to-show-a-savefiledialog
    /// </summary>


    public class SaveFileNameEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                if (value != null)
                {
                    saveFileDialog.FileName = value.ToString();
                }

                saveFileDialog.Title = context.PropertyDescriptor.DisplayName;
                saveFileDialog.Filter = "All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    value = saveFileDialog.FileName;
                }
            }

            return value;
        }
    }
}
