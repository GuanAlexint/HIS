using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIOllama.common;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AIOllama
{
    public partial class Diagnosis : Form
    {
        string pid = "";
        string pidname = "";
        public Diagnosis()
        {
            InitializeComponent();
            //listBox1.Items.Clear();
            //DataSet ds = new DataSet();
            //ds = MysqlHelper.SelectSql("SELECT o.Pid FROM hrip.pati_out_visit o,hrip.pati_info_basic ib where ib.Pid=o.Pid;;");
            //DataTableReader rdr = ds.CreateDataReader();
            //while (rdr.Read())
            //{
            //    listBox1.Items.Add(rdr.GetValue(0));
            //}
        }
        public Diagnosis(string pidstr, string pidname)
        {
            InitializeComponent();
            this.pid = pidstr;
            this.pidname = pidname;
            label5.Text = pid;
            label6.Text = pidname;


            richTextBox1.Clear();
            DataSet ds = new DataSet();
            ds = MysqlHelper.SelectSql("SELECT * FROM hrip.medicalrecord where patiid='" + pid + "' and patiname='" + pidname + "';");
            DataTableReader rdr = ds.CreateDataReader();
            while (rdr.Read())
            {
                richTextBox1.Text = rdr.GetValue(3).ToString();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            MedicalRecordGeneration(textBox1.Text);
        }
        public async void CheckItem(String checkstr)
        {
            var uri = new Uri("http://localhost:11434");
            var ollama = new OllamaApiClient(uri);
            String OllamaChatstr = "";
            // select a model which should be used for further operations
            ollama.SelectedModel = "deepseek-r1:7b";
            Chat ollamachat = new Chat(ollama, "以医生的专业口吻作答，直接输出结果,不用建议,不要诊断结果");

            await foreach (var stream in ollamachat.SendAsync("患者主诉为" + checkstr + "检查项目应该有什么，按照医疗检查格式，格式统一，按序号排序，直接输出结果"))
                OllamaChatstr += stream;

            string substringToFind = "</think>";
            string resultToFind = "";
            string substringFind1 = "检查项目：";
            string resultToFind1 = "";

            int index = OllamaChatstr.IndexOf(substringToFind);
            if (index > -1)
            {
                resultToFind = OllamaChatstr.Substring(index + substringToFind.Length);
            }

            int index01 = resultToFind.IndexOf(substringFind1);
            if (index01 > -1)
            {
                resultToFind1 = resultToFind.Substring(index01 + substringFind1.Length);
                richTextBox2.Text = resultToFind1;
            }
            else
            {
                richTextBox2.Text = resultToFind;
            }

            int count = resultToFind.Count(c => c == '.');

            List<string> checkItems = new List<string>();
            MatchCollection matches = Regex.Matches(resultToFind, @"\d+\.\s*(.*)");
            foreach (Match match in matches)
            {
                checkItems.Add(match.Groups[1].Value.Trim());
            }
            CheckItemlistBox.Items.Clear();
            for (int i = 0; i < checkItems.Count; i++)
            {
                CheckItemlistBox.Items.Add(checkItems[i]);
            }


        }

        public async void MedicalRecordGeneration(String Medical)
        {
            var uri = new Uri("http://localhost:11434");
            var ollama = new OllamaApiClient(uri);
            String OllamaChatstr = "";
            // select a model which should be used for further operations
            ollama.SelectedModel = "deepseek-r1:7b";
            Chat ollamachat = new Chat(ollama, "以医生的专业口吻作答，直接输出结果");




            await foreach (var stream in ollamachat.SendAsync("患者诊断为" + Medical + "按照门诊病历格式，书写一份门诊病历，格式统一," + "患者姓名是" + pidname + "年龄是18岁，性别是男 "))
                OllamaChatstr += stream;

            string substringToFind = "</think>\n\n";
            int index = OllamaChatstr.IndexOf(substringToFind);
            if (index > -1)
            {
                richTextBox1.Text = OllamaChatstr.Substring(index + substringToFind.Length);
            }
            //string output = Regex.Replace(OllamaChatstr, pattern, string.Empty);


            //Console.Write(stream.Response);
        }

        private void Diagnosis_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Form1.instance.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckItem(textBox2.Text);
        }

        private void medicalsave_Click(object sender, EventArgs e)
        {
            DateTime dateValue=DateTime.Now;
            string sqlstr = "insert into hrip.medicalrecord values" +
                    "('" + label5.Text + "','" + label5.Text + "','" + label6.Text
                    + "','" + richTextBox1.Text + "','" + dateValue  + "')";
            MysqlHelper.ExecuteSql(sqlstr);
            this.Close();

            Form1.instance.Show();
        }
    }
}
