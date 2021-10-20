using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Class2_Startup
{
    public partial class ModalDialog : Form
    {
        public ModalDialog()
        {
            InitializeComponent();
        }

        public int NumberX
        {
            get
            {
                return (int)numericUpDown1.Value;
            }

            set
            {
                numericUpDown1.Value = value;
            }
        }

        public int NumberY
        {
            get
            {
                return (int)numericUpDown2.Value;
            }

            set
            {
                numericUpDown2.Value = value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
