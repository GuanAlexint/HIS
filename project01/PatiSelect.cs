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

namespace AIOllama
{
    public partial class PatiSelect : Form
    {
        private Diagnosis diagnosis;
        Dictionary<string, string> dPati = new Dictionary<string, string>();
        public PatiSelect()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            DataSet ds = new DataSet();
            ds = MysqlHelper.SelectSql("SELECT o.Pid,ib.Pname FROM hrip.pati_out_visit o,hrip.pati_info_basic ib " +
                "where ib.Pid=o.Pid;");
            DataTableReader rdr = ds.CreateDataReader();

            while (rdr.Read())
            {
                listBox1.Items.Add(rdr.GetValue(0));

                dPati.Add(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString());

            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(listBox1.SelectedItem.ToString());
            string pidname;
            bool exists = dPati.TryGetValue(listBox1.SelectedItem.ToString(), out pidname);
            if (exists)
            {
                this.Hide();
                diagnosis = new Diagnosis(listBox1.SelectedItem.ToString(), pidname);
                diagnosis.ShowDialog();
            }


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
