using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIOllama.common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AIOllama
{
    public partial class PatientCard : Form
    {
        public PatientCard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ErrorInput = false;
            if (textBox3.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入就诊卡号");
                ErrorInput = true;
            }
            if (textBox1.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入姓名");
                ErrorInput = true;
            }
            if (textBox2.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入性别");
                ErrorInput = true;
            }

            if (textBox4.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入身份证号");
                ErrorInput = true;
            }
            if (textBox5.Text.Equals(string.Empty))
            {
                MessageBox.Show("请输入电话");
                ErrorInput = true;
            }
            if (ErrorInput == false)
            {

                DateTime dateValue;
                string format = "yyyy-MM-dd";
                //if (DateTime.TryParseExact(dateTimePicker1.Value, format, null, DateTimeStyles.None, out dateValue))
                //{
                string sqlstr = "insert into hrip.pati_info_basic values" +
                "('" + textBox3.Text + "','" + textBox1.Text + "','" + textBox2.Text
                + "','" + textBox4.Text + "','" + dateTimePicker1.Value + "','" + textBox5.Text + "')";

                MysqlHelper.ExecuteSql(sqlstr);
                this.Close();

                Form1.instance.Show();
                //}
            }
            else
            {
                MessageBox.Show("请完善患者信息");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
