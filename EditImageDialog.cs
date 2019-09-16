using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FWManager
{
    public partial class EditImageDialog : Form
    {

        public Image Image {
            get {
                return this.pictureBox1.Image;
            }
            set {
                this.pictureBox1.Image = value;
            }
        }

        public EditImageDialog()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog()) {
                if (diag.ShowDialog() == DialogResult.OK) {
                    pictureBox1.Image = Image.FromFile(diag.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog diag = new SaveFileDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(diag.FileName);
                }
            }
        }
    }
}
