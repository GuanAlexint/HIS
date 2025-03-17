namespace AIOllama
{
    public partial class Form1 : Form
    {

        public static Form1 instance;
        private Login lg = null;
        private PatientCard pc = null;
        private Registration rg = null;
        private Diagnosis dg = null;
        private PatiSelect patiS = null;

        public Form1()
        {
            InitializeComponent();
            instance = this;
            lg = new Login();
            lg.ShowDialog();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            pc = new PatientCard();
            pc.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            rg = new Registration();
            rg.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            patiS = new PatiSelect();
            patiS.ShowDialog();
        }
    }
}
