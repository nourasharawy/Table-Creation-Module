using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace file_project_2
{
    public partial class delete_column : Form
    {
        public delete_column()
        {
            InitializeComponent();
        }

        private void delete_column_Load(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("tables_name.txt", FileMode.Open);
            StreamReader sd = new StreamReader(fs);
            while (sd.Peek() >= 0)
            {

                comboBox_table_name.Items.Add(sd.ReadLine());

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string table_name = comboBox_table_name.SelectedItem.ToString();
            string table_name2 = table_name + ".xml";

          XmlDocument doc = new XmlDocument();
            doc.Load(table_name2);

            XmlNodeList nodes = doc.GetElementsByTagName("PK");
            XmlNodeList listname = doc.GetElementsByTagName("Name");
            XmlNodeList listcol = doc.GetElementsByTagName("Column");
            int num = nodes.Count;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (listname[i].InnerText == textBox_col_name.Text)
                {

                    if (nodes[i].InnerText == "True")
                        MessageBox.Show("Not allow to remove the primary key!! ");
                    else
                    {

                        listcol[i].ParentNode.RemoveChild(listcol[i]);
                        doc.Save(table_name2);
                        MessageBox.Show("deleted succefully!! ");
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
