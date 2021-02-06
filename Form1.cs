using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_project_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddNewTable_Form f = new AddNewTable_Form();
            this.Hide();
            f.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            add_new__col a = new add_new__col();
            a.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            delete_column d = new delete_column();
            d.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            insert__data i = new insert__data();
            i.Show();
            this.Hide();
        }
    }
}
