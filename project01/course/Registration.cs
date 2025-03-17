using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIOllama.common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AIOllama
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
            comboBox2.Items.Clear();
            DataSet ds = new DataSet();
            ds = MysqlHelper.SelectSql("select * from hrip.hospitaldepartment;");
            DataTableReader rdr = ds.CreateDataReader();
            while (rdr.Read())
            {
                comboBox2.Items.Add(rdr.GetValue(1));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ErrorInput = false;
            if (textBox1.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入挂号编码");
                ErrorInput = true;
            }
            if (textBox2.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入患者就诊卡号");
                ErrorInput = true;
            }
            if (listBox1.Text.Equals("专家"))
            {
                if (textBox3.Text.Equals(string.Empty))
                {
                    MessageBox.Show("请输入专家医生姓名");
                    ErrorInput = true;
                }
            }

            if (ErrorInput == false)
            {
                if (comboBox1.SelectedIndex == -1)
                { comboBox1.SelectedIndex = 0; }
                if (comboBox2.SelectedIndex == -1)
                { comboBox2.SelectedIndex = 0; }
                string sqlstr = "insert into hrip.pati_out_visit (rid,pid,Rtype,DpmtnNme," +
                "DoctName," +
                "RegistDate) values" +
                "('" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + textBox3.Text + "','" + dateTimePicker1.Text + "')";
                MysqlHelper.ExecuteSql(sqlstr);
                Form1.instance.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("请完善挂号信息");
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
