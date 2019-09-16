using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FWManager
{
    public partial class FWManager : Form
    {
        Firmware firmware = null;

        public FWManager()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    this.firmware = Firmware.ReadFromStream(stream);
                }
            }
            propertyGrid1.SelectedObject = this.firmware;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        firmware.WriteToStream(stream);
                    }
                }
            }
        }
    }
}
