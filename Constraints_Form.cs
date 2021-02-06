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
    public partial class Constraints_Form : Form
    {
        string Table_Name;
        List<string> New_Columns_Name;
        public Constraints_Form(string name, List<string> ll)
        {
            
            InitializeComponent();
            Table_Name = name;
            New_Columns_Name = ll;

            //Fill combo Box
            for (int i = 0; i < New_Columns_Name.Count; i++)
            {
                col_name_combo.Items.Add(New_Columns_Name[i]);
            }
        }

        private void Constraints_Form_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            this.Hide();
            f.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Save_Identity_btn_Click(object sender, EventArgs e)
        {
            bool error = false;

            if (col_name_combo.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Incorrect Column Name ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            if (Seed_txt.Text.ToString() == "")
            {
                MessageBox.Show("Seed Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
            if (Increment_txt.Text.ToString() == "")
            {
                MessageBox.Show("Increment Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
            if (error == false)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Table_Name);
                XmlNodeList l2 = doc.GetElementsByTagName("Name");

                XmlNodeList li = doc.GetElementsByTagName("StartValue");
                XmlNodeList l = doc.GetElementsByTagName("IncrementValue");
                for (int i = 0; i < li.Count; i++)
                {

                    if (l2[i].InnerText.ToString() == col_name_combo.SelectedItem.ToString())
                    {

                        li[i].InnerText = Seed_txt.Text;
                        l[i].InnerText = Increment_txt.Text;
                    }

                    doc.Save(Table_Name);

                }



                //XmlDocument doc = new XmlDocument();

                //XmlElement sequence = doc.CreateElement("Sequence");

                //XmlElement node = doc.CreateElement("StartValue");
                //node.InnerText = Seed_txt.Text.ToString();
                //sequence.AppendChild(node);



                //node = doc.CreateElement("IncrementValue");
                //node.InnerText = Increment_txt.Text.ToString();
                //sequence.AppendChild(node);


                //doc.Load(Table_Name);

                ////return all the columns
                //XmlNodeList list = doc.GetElementsByTagName("Column");
                //for (int i = 0; i < list.Count; i++)
                //{
                //    //childs of each column
                //    XmlNodeList l = list[0].ChildNodes;

                //    //l[o] Column name tag   // check that the name of this column equal the column name that the user want to add sequence in it
                //    if (l[0].InnerText == col_name_combo.SelectedItem.ToString())
                //    {
                //        //l[2] constraints tag // append sequence tag to Constraints tag in this column

                //        l[2].AppendChild(sequence);///////////
                //        break;
                //    }

                //}

                //doc.Save(Table_Name);
            }
        }

        private void Save_constraints_btn_Click(object sender, EventArgs e)
        {
            string constr_Expression = Constraints_Exp_txt.Text.ToString();
            //to check that the expression has < OR  > symbol
            string[] words1 = constr_Expression.Split('<');
            string[] words2 = constr_Expression.Split('>');
            string str1 = "";
            string str2 = "";
            for (int i = 0; i < words1.Count(); i++)
            {
                str1 += words1[i];
            }

            for (int i = 0; i < words2.Count(); i++)
            {
                str2 += words2[i];
            }


          
            if (str1 != constr_Expression) // if == that mean constr_expression does not split because it doesn't have < symbol
            {
                if (!New_Columns_Name.Contains(words1[0]))  //word[0] store column name
                {
                    //check column name
                    MessageBox.Show("Incorrect Column Name ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (words1[1] == "")  //words1[1] store value
                    MessageBox.Show("Value can not be null ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(Table_Name);
                    XmlNodeList lest_name_col = doc.GetElementsByTagName("Name");
                    XmlNodeList li = doc.GetElementsByTagName("constr");
                    for (int i = 0; i < li.Count; i++)
                    {
                        if (lest_name_col[i].InnerText.ToString() ==words1[0].ToString())
                        //if (li[i].InnerText.ToString() == col_name_combo.SelectedItem.ToString())
                        {
                            li[i].InnerText = "<" +' '+ words1[1]; 
                          
                        }

                        doc.Save(Table_Name);

                    }

                }

            }
            else if (str2 != constr_Expression) // if == that mean constr_expression does not split because it doesn't have > symbol
            {
                if (!New_Columns_Name.Contains(words2[0]))
                {
                    MessageBox.Show("Incorrect Column Name ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (words2[1] == "")  //words2[1] store value
                    MessageBox.Show("Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {


                    XmlDocument doc = new XmlDocument();
                    doc.Load(Table_Name);
                    XmlNodeList lest_name_col = doc.GetElementsByTagName("Name");
                    XmlNodeList li = doc.GetElementsByTagName("constr");
                    for (int i = 0; i < li.Count; i++)
                    {
                        if (lest_name_col[i].InnerText.ToString() == words2[0].ToString())
                       
                        {
                            li[i].InnerText = ">" +' '+ words2[1];
                           
                        }

                        doc.Save(Table_Name);

                    }


                    
                }

            }
            else
            {
                MessageBox.Show("Invalid Expression ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void Constraints_Exp_txt_TextChanged(object sender, EventArgs e)
        {
            //To view constraints format in the text box
            if (Constraints_Exp_txt.Text == "")
                label3.Show();
            else
                label3.Hide();
        }
    }
}
