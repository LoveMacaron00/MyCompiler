using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crru
{
    public partial class inputbox : Form
    {
        public string data = "";
        public inputbox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data = textBox1.Text;
            //this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void inputbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                data = textBox1.Text;
                this.Close();
            }
        }
    }
}
