using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace DataStock
{
    public partial class FormRelatorio : Form
    {
        Cadastro cadastro = new Cadastro();
        Funcao funcao = new Funcao();

        public FormRelatorio()
        {
            InitializeComponent();
        }

        private void FormRelatorio_Load(object sender, EventArgs e)
        {

        }

        private void buttonGerar_Click(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataStockDataSet.DataTable1' table. You can move, or remove it, as needed.

            this.reportViewer1.Clear();

            if (textBoxAno.Text != "")
            {
                this.DataTable1TableAdapter.Fill(this.DataStockDataSet.DataTable1, textBoxAno.Text + "/01/01", textBoxAno.Text + "/12/31");
                this.PRODUTO1TableAdapter.Fill(this.DataStockDataSet.PRODUTO1, textBoxAno.Text + "/01/01", textBoxAno.Text + "/12/31");
            }
            else if(funcao.isok(maskedTextBox1.Text)==true && funcao.isok(maskedTextBox2.Text)==true)
            {
                this.DataTable1TableAdapter.Fill(this.DataStockDataSet.DataTable1, maskedTextBox1.Text, maskedTextBox2.Text);
                this.PRODUTO1TableAdapter.Fill(this.DataStockDataSet.PRODUTO1, maskedTextBox1.Text, maskedTextBox2.Text);
            }

            this.reportViewer1.RefreshReport();
        }

        private void textBoxAno_Click(object sender, EventArgs e)
        {
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
        }

        private void maskedTextBox1_Click(object sender, EventArgs e)
        {
            textBoxAno.Clear();
        }

        private void maskedTextBox2_Click(object sender, EventArgs e)
        {
            textBoxAno.Clear();
        }

        private void maskedTextBox2_DoubleClick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            maskedTextBox2.Text = date.ToString();
        }

        private void textBoxAno_DoubleClick(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            textBoxAno.Text = data.Year.ToString();
        }

        private void textBoxAno_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonGerar);
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonGerar);
        }

        private void maskedTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonGerar);
        }
    }
}
