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
    public partial class AddNewTable_Form : Form
    {
        //Table Name
        public static string TableName;
        public static string data_type;
        //store columns' name to avoid columns' name repetition
        List<string> New_Columns_Name = new List<string>();

        public AddNewTable_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            this.Hide();
            f.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TableName = TableName_txt.Text.ToString();

            New_Columns_Name = new List<string>();
            string Column_Name, Data_Type;

            bool Is_Error = false, Allow_Null, PK;

            if (TableName == "")
            {
                MessageBox.Show("Table Name Can not be Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (TableCol_grid.Rows.Count == 1)
                MessageBox.Show("Table can not be Empty ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            else
            {
                if (!File.Exists(TableName))
                {
                    //textfile that contains name of tables will append just create new table
                    FileStream fs = new FileStream("tables_name.txt", FileMode.Append);
                    StreamWriter st = new StreamWriter(fs);
                    st.WriteLine(TableName);

                    st.Close();
                    fs.Close();

                    //Check that there isn't an empty  column name or data type or column name that already exist 
                    for (int i = 0; i < TableCol_grid.RowCount - 1; i++)
                    {
                        Column_Name = Convert.ToString(TableCol_grid.Rows[i].Cells[0].Value);
                        if (New_Columns_Name.Contains(Column_Name))
                        {
                            MessageBox.Show("Column Name is already Exist ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Is_Error = true;
                        }
                        else if (Column_Name == "")
                        {
                            MessageBox.Show("Column Name can not be Empty! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Is_Error = true;
                        }
                        else
                        {
                            New_Columns_Name.Add(Column_Name);
                        }
                        Data_Type = Convert.ToString(TableCol_grid.Rows[i].Cells[1].Value);
                        if (Data_Type == "")
                        {
                            MessageBox.Show("You must choose Column's Data Type ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Is_Error = true;
                        }

                        Allow_Null = Convert.ToBoolean(TableCol_grid.Rows[i].Cells[2].Value);
                        PK = Convert.ToBoolean(TableCol_grid.Rows[i].Cells[3].Value);
                        if (Allow_Null == true && PK == true)
                        {
                            MessageBox.Show("PK Can not allow Null! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Is_Error = true;
                        }

                        if (Is_Error == true)
                            break;

                    }

                    if (Is_Error == false)
                    {

                        TableName = TableName + ".xml";



                        XmlWriter writer = XmlWriter.Create(TableName);

                        //Create Root Element
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Table");
                        writer.WriteAttributeString("name", TableName);


                        //Get Data From Grid View
                        for (int i = 0; i < TableCol_grid.RowCount - 1; i++)
                        {
                            //create column
                            writer.WriteStartElement("Column");


                            //Column Name
                            Column_Name = Convert.ToString(TableCol_grid.Rows[i].Cells[0].Value);
                            writer.WriteStartElement("Name");
                            writer.WriteString(Column_Name);
                            writer.WriteEndElement();

                            //values
                            /// inside this tag there will be childs called val 
                            writer.WriteStartElement("Values");
                            writer.WriteString("");
                            writer.WriteEndElement();


                            //constraints
                            /// inside this tag there will be childs called constr 
                            writer.WriteStartElement("Constraints");

                            //Data Type
                            Data_Type = Convert.ToString(TableCol_grid.Rows[i].Cells[1].Value);
                            writer.WriteStartElement("DataType");
                            writer.WriteString(Data_Type);
                            writer.WriteEndElement();


                            //Allow Null
                            Allow_Null = Convert.ToBoolean(TableCol_grid.Rows[i].Cells[2].Value);
                            if (Allow_Null == false)
                            {
                                writer.WriteStartElement("AllowNull");
                                writer.WriteString("False");
                                writer.WriteEndElement();
                            }
                            else
                            {
                                writer.WriteStartElement("AllowNull");
                                writer.WriteString("True");
                                writer.WriteEndElement();
                            }


                            //PK
                            PK = Convert.ToBoolean(TableCol_grid.Rows[i].Cells[3].Value);
                            if (PK == false)
                            {
                                writer.WriteStartElement("PK");
                                writer.WriteString("False");
                                writer.WriteEndElement();
                            }
                            else
                            {
                                writer.WriteStartElement("PK");
                                writer.WriteString("True");
                                writer.WriteEndElement();
                            }


                            //sequence
                          
                                writer.WriteStartElement("sequence");

                                //strartvalue
                                writer.WriteStartElement("StartValue");
                                writer.WriteString(" ");
                                writer.WriteEndElement();

                                //increament
                                writer.WriteStartElement("IncrementValue");
                                writer.WriteString(" ");
                                writer.WriteEndElement();


                                writer.WriteEndElement();


                            //constr

                                writer.WriteStartElement("constr");
                                writer.WriteString(" ");
                                writer.WriteEndElement();

                            //////////////////////////////////////
                            //          FK                     //
                            /////////////////////////////////////               
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();

                        writer.WriteEndDocument();
                        writer.Close();
                        Constraints_Form f = new Constraints_Form(TableName, New_Columns_Name);
                        this.Hide();
                        f.ShowDialog();
                        this.Close();
                    }

                }
                else
                {
                    //There is a table with the same name because there is a file with the same name
                    MessageBox.Show("Table Name is already Exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void AddNewTable_Form_Load(object sender, EventArgs e)
        {

        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {

            TableCol_grid.Rows.Clear();
            TableName_txt.Clear();
        }

        private void TableCol_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // To make PK check Box like radio button
            int row_index = e.RowIndex;
            for (int i = 0; i < TableCol_grid.Rows.Count - 1; i++)
            {
                if (row_index != i)
                {
                    TableCol_grid.Rows[i].Cells["Column4"].Value = false;
                }
            }
        }
    }
}
