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
    public partial class insert__data : Form
    {
        public insert__data()
        {
            InitializeComponent();
        }

        private void insert__data_Load(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("tables_name.txt", FileMode.Open);  //
            StreamReader sd = new StreamReader(fs);
            while (sd.Peek() >= 0)
            {
                comboBox1.Items.Add(sd.ReadLine());

            }
            sd.Close();
            fs.Close();


            XmlDocument doc = new XmlDocument();
            

            /*     XmlNodeList list = doc.GetElementsByTagName("DataType");
                 for (int i = 0; i < list.Count; i++)
                 {
                     comboBox2.Items.Add(list[i].InnerText);
                 }
                 */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            string table_name = comboBox1.SelectedItem.ToString();
            string table_name2 = table_name + ".xml";


            XmlDocument doc = new XmlDocument();
            doc.Load(table_name2);


            List<string> col_list = new List<string>();


            XmlNodeList list = doc.GetElementsByTagName("Name");

            for (int i = 0; i < list.Count; i++)
            {

                col_list.Add(list[i].InnerText);

            }

            if (dataGridView1.ColumnCount == 0)
            {
                for (int i = 0; i < col_list.Count; i++)
                {

                    dataGridView1.Columns.Add(col_list[i], col_list[i]);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string table_name = comboBox1.SelectedItem.ToString();
            string table_name2 = table_name + ".xml";


            XmlDocument doc = new XmlDocument();
            doc.Load(table_name2);




            //--------------identity
            XmlNodeList col_num = doc.GetElementsByTagName("Column");
            /*XmlNodeList list_StartValue = doc.GetElementsByTagName("StartValue");
            XmlNodeList list_increament = doc.GetElementsByTagName("IncrementValue");
            //MessageBox.Show("list_StartValue.Count" + list_StartValue.Count);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int k = 0; k < col_num.Count; k++)
                {
                    if (list_StartValue[i].InnerText != " ")
                    {

                        Int32 start_inlist = Convert.ToInt32(list_StartValue[k].InnerText.ToString());
                        Int32 increament_in_list = Convert.ToInt32(list_increament[k].InnerText.ToString());


                        dataGridView1.Rows[i].Cells[k].Value = start_inlist; //i_in_list;

                        Int32 start_indg = Convert.ToInt32(dataGridView1.Rows[i + 1].Cells[k].Value.ToString());
                        //   if (start_indg < start_inlist)
                        {
                            //     MessageBox.Show("smaller than strat in list ");
             //               iserror = true;
                        }
                        increament_in_list += start_indg;
                        //   dataGridView1.Rows[i].Cells[k].Value = increament_in_list;
                        increament_in_list = 0;
                    }
                    else
                    {
                        //        MessageBox.Show("this index in file is  null");
               //         iserror = true;
                    }
                }
            }

            */








            //-------------ended
            bool iserror = false;
           //just to know num of col


            //---------------------------------------------------------------------------datatype start
            XmlNodeList list_datatype = doc.GetElementsByTagName("DataType");

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int k = 0; k < list_datatype.Count; k++)//1-we have num of col= num of datatype;
                {                                            //2-we have 3 types of datatype (int,float,nchar)"nchar may be number so we wouldnot check on it"
                    //3-we check (int,float)for every cell entered 

                    int x;
                    float y;

                    if (list_datatype[k].InnerText.ToString() == "int")
                    {
                        if (int.TryParse(dataGridView1.Rows[i].Cells[k].Value.ToString(), out x))
                        {
                            iserror = true;
                            //MessageBox.Show("yes it is int");
                        }
                        else
                        {
                            iserror = false;
                            //   MessageBox.Show("no it isnot int");
                            MessageBox.Show("no it isnot int ", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.Rows.RemoveAt(k);

                        }
                    }

                    else if (list_datatype[k].InnerText.ToString() == "float")
                    {
                      //  MessageBox.Show("cell num " + i + ',' + k);
                        if (float.TryParse(dataGridView1.Rows[i].Cells[k].Value.ToString(), out y) && (dataGridView1.Rows[i].Cells[k].Value.ToString() != null))  ////////////
                        {
                           
                            iserror = true;
                          
                        }
                        else
                        {
                            iserror = false;
                         
                            MessageBox.Show("no it isnot float ", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dataGridView1.Rows.RemoveAt(k);
                        }
                    }

                }
            }
            //---------------------------------------------------------------------------------------------data type ended


            //----------------------------------------------------------------------------------------------allow null start
            XmlNodeList list_allownull = doc.GetElementsByTagName("AllowNull");

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int k = 0; k < list_allownull.Count; k++)
                {

                    if (list_allownull[k].InnerText.ToString() == "True" && dataGridView1.Rows[i].Cells[k].Value.ToString() == " "||
                        list_allownull[k].InnerText.ToString() == "True" && dataGridView1.Rows[i].Cells[k].Value.ToString() != " "||
                        list_allownull[k].InnerText.ToString() == "False" && dataGridView1.Rows[i].Cells[k].Value.ToString() != " ")
                    {
                        // MessageBox.Show("yes be null");
                        iserror = false;

                    }
                    else
                    {
                        iserror = true;
                        MessageBox.Show("not allow null ", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    MessageBox.Show("not allow null ");
                    }
                }
            }
            //-------------------------------------------------------------------------------------allow null ended  


            //-------------------------------------------------------------------------------------constr start

         XmlNodeList list_constr = doc.GetElementsByTagName("constr");

           
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int k = 0; k < col_num.Count; k++)
                {
                    if (list_datatype[k].InnerText.ToString() != "nchar")
                    {
                        if (list_constr[k].InnerText != " ")
                        {
                            string[] words1 = list_constr[k].InnerText.Split(' ');
                           
                            string sign = words1[0];
                            string val ;
                              val=Convert.ToString (words1[1].ToString());
                              MessageBox.Show("value from constraints:" + val);
                        

                            double value_of_dg = Convert.ToDouble(dataGridView1.Rows[i].Cells[k].Value.ToString());// because int to compare with float
                            double value = Convert.ToDouble(val.ToString());//value from file
                            

                            if (sign == ">" && value_of_dg > value)
                            {
                                iserror = false;
                              
                            }

                            else if (sign == "<" && value_of_dg < value)
                            {
                               
                                iserror = false;
                            }
                            else
                            {
                                MessageBox.Show(" no value not valid");
                                iserror = true;
                            }
                        }

                    }
                }
            }
            
            //-----------------------------------------------------------------------contr ended






            string col_value;

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                //list is contain all columns in table
                XmlNodeList list = doc.GetElementsByTagName("Column");
                for (int j = 0; j < list.Count; j++)
                {
                    //child contain tags for each col
                    XmlNodeList child = list[j].ChildNodes;
                    //to get chield from tag constrains
                    XmlNodeList constrant_value = child[2].ChildNodes;

                    col_value = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);

                    //check pk is repeted or not 
                    if (constrant_value[2].InnerText == "True")
                    {
                        for (int s = 0; s < child.Count; s++)
                        {
                            XmlNodeList columns_values = child[1].ChildNodes;
                            for (int a = 0; a < columns_values.Count; a++)
                            {
                                if (columns_values[a].InnerText.ToString() == col_value)
                                {
                                    MessageBox.Show("The Col: " + child[0].InnerText + "  is pk and musn't be repeted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    iserror = true;
                                }
                            }
                        }
                    }


                }


                if (iserror == true)
                {
                    break;
                }
            }

            if (iserror == false)
            {
                List<string> col_inner_value = new List<string>(); ;
                //Get Data From Grid View
                for (int p = 0; p < dataGridView1.RowCount - 1; p++)
                {

                    XmlNodeList li = doc.GetElementsByTagName("Column");

                    for (int j = 0; j < li.Count; j++)
                    {
                        XmlNodeList pk = doc.GetElementsByTagName("PK");
                        XmlNodeList list_val = doc.GetElementsByTagName("Values");
                        XmlNodeList col_name = doc.GetElementsByTagName("Name");
                        //int w = 0;
                        string dd = Convert.ToString(dataGridView1.Rows[p].Cells[j].Value);
                        XmlElement val = doc.CreateElement("val");

                        val.InnerText = dd;
                        list_val[j].AppendChild(val);
                        XmlNodeList child = li[j].ChildNodes;
                        


                    }

                }


            }
            doc.Save(table_name2);
            MessageBox.Show("Insertion Done Sucsissfuly ^_^ ", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //-------------------------------------------------------------------------------------------
            //sequence
            /////////////
          /*  XmlNodeList list_StartValue = doc.GetElementsByTagName("StartValue");
            XmlNodeList list_increament = doc.GetElementsByTagName("IncrementValue");
            //MessageBox.Show("list_StartValue.Count" + list_StartValue.Count);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int k = 0; k < col_num.Count; k++)
                {
                    if (list_StartValue[i].InnerText != " ")
                    {

                        Int32 start_inlist = Convert.ToInt32(list_StartValue[k].InnerText.ToString());
                        Int32 increament_in_list = Convert.ToInt32(list_increament[k].InnerText.ToString());


                        dataGridView1.Rows[i].Cells[k].Value = start_inlist; //increament_in_list;

                       Int32 start_indg = Convert.ToInt32(dataGridView1.Rows[i+1].Cells[k].Value.ToString());
                     //   if (start_indg < start_inlist)
                        {
                       //     MessageBox.Show("smaller than strat in list ");
                            iserror = true;
                        }
                        increament_in_list += start_indg;
                     //   dataGridView1.Rows[i].Cells[k].Value = increament_in_list;
                        increament_in_list = 0;
                    }
                    else
                    {
                //        MessageBox.Show("this index in file is  null");
                        iserror = true;
                    }
                }
            }
            */
            //-------------------------------------------------------------------------------------------
            //sequence ended
            /////////////////








        }
    }
}
            
        
    

