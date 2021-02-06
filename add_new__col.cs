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
    public partial class add_new__col : Form
    {
        public add_new__col()
        {
            InitializeComponent();
        }

        private void add_new__col_Load(object sender, EventArgs e)// it will be saved in text file then read it in combo box
        {
            FileStream fs = new FileStream("tables_name.txt", FileMode.Open);  //
            StreamReader sd = new StreamReader(fs);
            while (sd.Peek() >= 0)
            {
                comboBox_Table_name.Items.Add(sd.ReadLine());
            }

            sd.Close();
            fs.Close();
        }

        //////////////////
        //button Save///////////////////
        //----------
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool AllowNull = false, Primary_Key = false;
            bool iserror = false;


            //---------------------------------------------------------------------------------------------------
            //check if table name ||col_name||daat_type is null
            if (comboBox_Table_name.SelectedIndex <= -1)
            {
                MessageBox.Show("Table Name Can not be Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iserror = true;
            }


            if (comboBox_data_type.SelectedIndex <= -1)
            {
                MessageBox.Show("Data Type Can not be Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iserror = true;
            }


            if (textBox_col_name.Text == "")
            {
                MessageBox.Show("Column Name Can not be Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iserror = true;
            }
            //---------------------------------------------------------------------------------------------------

            if (iserror == false)
            {
                string Data_Type = comboBox_data_type.SelectedItem.ToString();

                string table_name = comboBox_Table_name.SelectedItem.ToString();
                string table_name2 = table_name + ".xml";


                XmlDocument doc = new XmlDocument();
                doc.Load(table_name2);



                //-------------------------------------------------------------------
                //check if the table has a column with the same name the user entered 
                XmlNodeList list = doc.GetElementsByTagName("Name");
                for (int i = 0; i < list.Count; i++)
                    if (list[i].InnerText == textBox_col_name.Text)
                    {
                        MessageBox.Show("this Column already exist Sir!  please enter another name");
                        textBox_col_name.Clear();
                        break;
                    }


                //-------------------------------------------------------------------
                //create new column
                XmlElement New_Col = doc.CreateElement("Column");

                /////////////////////////////////////////////////
                // NOTE!   /////////////////////////////////
                //                                            //
                //   doc(newcol(node1(node2)))         //
                //                                           //
                ///////////////////////////////////////////////
                //-------------------------------------------------------------------
                //Name of new column
                XmlElement node1 = doc.CreateElement("Name");
                node1.InnerText = textBox_col_name.Text.ToString();
                New_Col.AppendChild(node1);
                //-------------------------------------------------------------------



                //-------------------------------------------------------------------
                //Value of new column
                node1 = doc.CreateElement("Values");
                node1.InnerText = " ";
                New_Col.AppendChild(node1);
                //-------------------------------------------------------------------

                //-------------------------------------------------------------------
                //part of Constraints of new column
                node1 = doc.CreateElement("Constraints");

                XmlNode node2;

                //data type  //-------------------------------------------------------------------
                node2 = doc.CreateElement("DataType");
                node2.InnerText = Data_Type;
                node1.AppendChild(node2);
                //-------------------------------------------------------------------

                //Allow null  //-------------------------------------------------------------------
                if (radioButton_allowNull.Checked)
                {
                    AllowNull = true;
                    node2 = doc.CreateElement("AllowNull");
                    node2.InnerText = "True";
                    node1.AppendChild(node2);
                }
                else
                {
                    AllowNull = false;
                    node2 = doc.CreateElement("AllowNull");
                    node2.InnerText = "False";
                    node1.AppendChild(node2);
                }
                //-------------------------------------------------------------------


                //primary key  //-------------------------------------------------------------------
                if (radioButton_primary_Key.Checked)
                {
                    XmlNodeList li = doc.GetElementsByTagName("PK");
                    for (int i = 0; i < li.Count; i++)
                    {
                        if (li[i].InnerText == "True")
                        {
                            DialogResult result = MessageBox.Show("Are you sure you want to replace the Primary Key", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (result == DialogResult.OK)
                            {
                                li[i].InnerText = "False";
                            }
                            else
                            {
                                Primary_Key = false;
                                node2 = doc.CreateElement("PK");
                                node2.InnerText = "False";
                                node1.AppendChild(node2);
                                break;
                            }

                        }
                    }

                    Primary_Key = true;
                    node2 = doc.CreateElement("PK");
                    node2.InnerText = "True";
                    node1.AppendChild(node2);
                }
                else
                {
                    Primary_Key = false;
                    node2 = doc.CreateElement("PK");
                    node2.InnerText = "False";
                    node1.AppendChild(node2);
                }

                //part of  constrinte ended
                New_Col.AppendChild(node1);

                //-------------------------------------------------------------------

                //---------------------------------------------------------------
                //sequence
                ////////////

                node2 = doc.CreateElement("Sequence");

                XmlNode node3;

                node3 = doc.CreateElement("StartValue");
                node3.InnerText = " ";
                node2.AppendChild(node3);

                node3 = doc.CreateElement("IncrementValue");
                node3.InnerText = " ";
                node2.AppendChild(node3);

                node1.AppendChild(node2);

                //sequence endeed
                //------------------------------------------------------------------------------


                //---------------------------------------------------------------
                //constr
                /////////////
                node2 = doc.CreateElement("constr");
                node2.InnerText = " ";
                node1.AppendChild(node2);

                //constr ended
                //------------------------------------------------------------------






                //---------------------------------------------------------
                //new col ended
                doc.DocumentElement.AppendChild(New_Col);

                //table ended
                MessageBox.Show("Done Sir!");
                doc.Save(table_name2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox_Table_name.ResetText();
            textBox_col_name.Clear();
            comboBox_data_type.ResetText();

            radioButton_primary_Key.Checked = false;
            radioButton_allowNull.Checked = false;

            Seed_txt.Clear();
            Increment_txt.Clear();

        }
        //----------------
        //Button enter  indentiy///////
        //---------------
        private void button6_Click(object sender, EventArgs e)
        {

            string table_name = comboBox_Table_name.SelectedItem.ToString();
            string table_name2 = table_name + ".xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(table_name2);

            int seed = Convert.ToInt32(Seed_txt.Text);  //just to to check about the value
            int increament = Convert.ToInt32(Increment_txt.Text); //just to to check about the value

            bool error = false;

            if (comboBox_data_type.SelectedItem.ToString() == "nchar")
            {
           DialogResult res= MessageBox.Show("Identity Specification just for Int and Float Datatype ..You Can easily  change the Datatype.  Sir!  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
                comboBox_data_type.ResetText();




                if (res == DialogResult.OK)
                {
                    XmlNodeList l2 = doc.GetElementsByTagName("Name");

                    XmlNodeList li = doc.GetElementsByTagName("DataType");
                    for (int i = 0; i < li.Count; i++)
                    {

                        if (l2[i].InnerText.ToString() == textBox_col_name.Text)
                        {

                            li[i].InnerText = comboBox_data_type.SelectedItem.ToString();
                          
                        }

                        doc.Save(table_name2);

                    }
                }
            }
            if (Seed_txt.Text.ToString() == "")
            {
                MessageBox.Show("Seed Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
            else if (seed < 0)
            {
                MessageBox.Show("Seed Value can not be <0 ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                error = true;
            }

            if (Increment_txt.Text.ToString() == "")
            {
                MessageBox.Show("Increment Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
            else if (increament <= 0)
            {
                MessageBox.Show("IncrementValue can not be <=0 ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }



            if (error == false)
            {

                XmlNodeList l22 = doc.GetElementsByTagName("Name");

                XmlNodeList lii = doc.GetElementsByTagName("StartValue");
                XmlNodeList l = doc.GetElementsByTagName("IncrementValue");
                for (int i = 0; i < lii.Count; i++)
                {

                    if (l22[i].InnerText.ToString() == textBox_col_name.ToString())
                    {

                        lii[i].InnerText = Seed_txt.Text;
                        l[i].InnerText = Increment_txt.Text;
                    }

                    doc.Save(table_name2);

                }



            }
        }
        //------------
        //Button enter const//
        //-----------------
        private void button7_Click(object sender, EventArgs e)
        {
            string table_name = comboBox_Table_name.SelectedItem.ToString();
            string table_name2 = table_name + ".xml";

            bool error = false;


            XmlDocument doc = new XmlDocument();
            doc.Load(table_name2);


            string constr_Expression = Constraints_Exp_txt.Text.ToString();
            //to check that the expression has < OR  > symbol
            string[] words1 = constr_Expression.Split('<');
            string[] words2 = constr_Expression.Split('>');
            string str1 = "";
            string str2 = "";


            //-----------------------------------------------------------------------------------------------
            //check if the value have the same data type of column
            int value;
            float v;
            if (comboBox_data_type.SelectedItem.ToString() == "int")
            {
                if (str1 != constr_Expression)  //if datatype is int so value must be int
                {
                    if (int.TryParse(words2[1], out value))
                    // if (int.TryParse(words1[1], out value) || int.TryParse(words2[1], out value))
                    {
                        MessageBox.Show(" yes it is int");
                    }
                    else
                    {
                        MessageBox.Show("vlaue is not valid ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        error = true;
                    }
                }

                else if (str2 != constr_Expression)
                {
                    if (int.TryParse(words1[1], out value))
                    // if (int.TryParse(words1[1], out value) || int.TryParse(words2[1], out value))
                    {
                        MessageBox.Show(" yes it is int");
                    }
                    else
                    {
                        MessageBox.Show("vlaue is not valid ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        error = true;
                    }
                }
            }

            else if (comboBox_data_type.SelectedItem.ToString() == "float")//if datatype is int so value must be float
            {
                if (float.TryParse(words1[1], out v) || float.TryParse(words2[1], out v))
                {
                    MessageBox.Show(" yes it is float");
                }
                else
                {
                    MessageBox.Show("vlaue is not valid ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }
            }
            else  //useless to use > or < with nchar datatype
            {
                {
                    MessageBox.Show("invalid Expression !Tha Datatype is ncher ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }
            }
            //-----------------------------------------------------------------------------------------------


            


            if (str1 != constr_Expression) // if == that mean constr_expression does not split because it doesn't have < symbol
            {

                if (textBox_col_name.Text != (words1[0]))  //word[0] store column name
                {
                    //check column name
                    MessageBox.Show("Incorrect Column Name ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (words1[1] == " ")  //words1[1] store value
                    MessageBox.Show("Value can not be null ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                else
                {
                    if (error == false)
                    {

                        doc.Load(table_name2);
                        XmlNodeList lest_name_col = doc.GetElementsByTagName("Name");
                        XmlNodeList li = doc.GetElementsByTagName("constr");
                        for (int i = 0; i < li.Count; i++)
                        {
                            if (lest_name_col[i].InnerText.ToString() == words1[0].ToString())
                            {
                                li[i].InnerText = "<" + " " + words1[1];

                            }

                            doc.Save(table_name2);

                        }




                    }

                }

            }
            else if (str2 != constr_Expression) // if == that mean constr_expression does not split because it doesn't have > symbol
            {
                if (textBox_col_name.Text != (words2[0]))
                {
                    MessageBox.Show("Incorrect Column Name ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (words2[1] == "")  //words2[1] store value
                    MessageBox.Show("Value can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {
                    if (error == false)
                    {


                        XmlNodeList lest_name_col = doc.GetElementsByTagName("Name");
                        XmlNodeList li = doc.GetElementsByTagName("constr");
                        for (int i = 0; i < li.Count; i++)
                        {
                            if (lest_name_col[i].InnerText.ToString() == words2[0].ToString())
                            {
                                li[i].InnerText = ">" + " " + words2[1];

                            }
                        }
                        doc.Save(table_name2);

                    }



                }
            }
        }
    }
}
