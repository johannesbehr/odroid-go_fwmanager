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
    public partial class EditBinaryDialog : Form
    {
        Byte[] binaryData;

        public Byte[] BinaryData
        {
            get
            {
                return binaryData;
            }
            set
            {
                binaryData = value;
                if (binaryData != null)
                {
                    textBox1.Text = binaryData.Length + " bytes";
                }
                else {
                    textBox1.Text = "Not defined.";
                }
            }
        }


        public EditBinaryDialog()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog()) {
                if (diag.ShowDialog() == DialogResult.OK) {
                    this.BinaryData = File.ReadAllBytes(diag.FileName);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog diag = new SaveFileDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(diag.FileName,this.BinaryData);
                }
            }

        }
    }
}
