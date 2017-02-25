using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataStock
{
    public partial class FormEstoque : Form
    {
        Cadastro cadastro = new Cadastro();
        Funcao funcao = new Funcao();

        public FormEstoque()
        {
            InitializeComponent();
            maskedTextBox1.Text = DateTime.Now.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                this.Text = "Compra";
                dataGridViewProduto.Columns.Clear();
                buttonBuscar.PerformClick();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked==true)
            {
                this.Text = "Venda";
                dataGridViewProduto.Columns.Clear();
                buttonBuscar.PerformClick();
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            string cmdSelect = "";
            string produto = funcao.replaceCharacters(textBoxProduto.Text);

            if (textBoxProduto.Text!="" && funcao.isok(produto)==true)
            {
                if(radioButton1.Checked==true)
                {
                    cmdSelect = @"SELECT IDPRODUTO AS 'ID', TIPO, MARCA, DESCRICAO AS 'DESCRIÇÃO' FROM PRODUTO WHERE TIPO LIKE '%" + produto + "%';";
                }
                else if(radioButton2.Checked==true)
                {
                    cmdSelect = @"SELECT IDPRODUTO AS 'ID', QUANTIDADE AS 'QNT', TIPO, MARCA, DESCRICAO AS 'DESCRIÇÃO'FROM PRODUTO WHERE TIPO LIKE '%" + 
                        produto + "%' AND QUANTIDADE > 0;";
                }
            }
            else if(textBoxProduto.Text=="")
            {
                if(radioButton1.Checked==true)
                {
                    cmdSelect = @"SELECT IDPRODUTO AS 'ID', TIPO, MARCA, DESCRICAO AS 'DESCRIÇÃO' FROM PRODUTO";
                }
                else if(radioButton2.Checked==true)
                {
                    cmdSelect = @"SELECT IDPRODUTO AS 'ID', QUANTIDADE AS 'QNT',  TIPO, MARCA, DESCRICAO AS 'DESCRIÇÃO' FROM PRODUTO WHERE QUANTIDADE > 0";
                }
            }

            if(radioButton1.Checked==true)
            {
                cadastro.listaTable(cmdSelect, dataGridViewProduto, 1);
            }
            else if(radioButton2.Checked==true)
            {
                cadastro.listaTable(cmdSelect, dataGridViewProduto, 2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //manipulação de dados de entrada e saida no estoque

            if(textBoxIdProduto.Text!="" && funcao.isok(textBoxValor.Text)==true)
            {
                string data = Convert.ToDateTime(maskedTextBox1.Text).ToString("MM /dd/yyyy");

                if (radioButton1.Checked == true)
                {
                    Compra(data);
                }
                else if (radioButton2.Checked == true)
                {
                    int estoque =Convert.ToInt32(dataGridViewProduto.CurrentRow.Cells[1].Value.ToString());

                    int valor = Convert.ToInt32(numericUpDownQuantidade.Value);
                    if (estoque >= valor) // >> VERIFICAR SE A QNTIDADE É SUFICIENTE PARA A VENDA
                    {
                        estoque = estoque - valor;
                        Venda(data, estoque);
                    }
                    else
                    {
                        MessageBox.Show("Quantidade insuficiente para a venda.", "Venda insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            limpaCampo();
            buttonBuscar.PerformClick();
            dataGridViewProduto_Click(dataGridViewProduto, new EventArgs());
        }

        private void Compra(string data)
        {
            string compra = (Convert.ToDecimal(textBoxValor.Text) * Convert.ToDecimal(numericUpDownQuantidade.Value)).ToString();
            string cmdInsert = @"INSERT INTO COMPRA VALUES (" + textBoxIdProduto.Text + ", " + numericUpDownQuantidade.Value.ToString() + ", '" + 
                compra.Replace(",", ".") + "', '" + data + "');";

            cadastro.cadastro(cmdInsert);

            int estoque = cadastro.pegaStock("SELECT * FROM PRODUTO WHERE IDPRODUTO=" + textBoxIdProduto.Text);

            int total = estoque + Convert.ToInt16(numericUpDownQuantidade.Value);
            string cmdEstoque = @"UPDATE PRODUTO SET QUANTIDADE=" + total + " WHERE IDPRODUTO=" + textBoxIdProduto.Text + ";";
            string cmdLog = @"INSERT INTO LOGS VALUES (" + textBoxIdProduto.Text + ", " + total + ", '" + data + "')";

            cadastro.cadastro(cmdEstoque);
            cadastro.cadastro(cmdLog);
        }

        private void Venda(string data, int estoque)
        {
            string venda = (Convert.ToDecimal(textBoxValor.Text) * Convert.ToDecimal(numericUpDownQuantidade.Value)).ToString();
            string cmdInsert = @"INSERT INTO VENDA VALUES (" + textBoxIdProduto.Text + ", " + numericUpDownQuantidade.Value.ToString() + ", '" + 
                venda.Replace(",", ".") + "', '" + data + "');";
            string cmdEstoque = @"UPDATE PRODUTO SET QUANTIDADE=" + estoque + " WHERE IDPRODUTO=" + textBoxIdProduto.Text + ";";
            string cmdLog = @"INSERT INTO LOGS VALUES (" + textBoxIdProduto.Text + ", " + estoque + ", '" + data + "')";

            cadastro.cadastro(cmdInsert);
            cadastro.cadastro(cmdEstoque);
            cadastro.cadastro(cmdLog);
        }

        private void maskedTextBox1_DoubleClick(object sender, EventArgs e)
        {
            maskedTextBox1.Text = DateTime.Now.ToString();
        }

        private void dataGridViewProduto_Click(object sender, EventArgs e)
        {
            if(dataGridViewProduto.Rows.Count >1)
            {
                textBoxIdProduto.Text = dataGridViewProduto.CurrentRow.Cells[0].Value.ToString();
            }
        }

        private void limpaCampo()
        {
            textBoxIdProduto.Clear();
            numericUpDownQuantidade.Value = 1;
            textBoxValor.Text = "0,00";
        }

        private void FormEstoque_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FormEstoque_Load(object sender, EventArgs e)
        {
            buttonBuscar.PerformClick();
            dataGridViewProduto_Click(dataGridViewProduto, new EventArgs());
        }

        private void textBoxValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)44)
            {
                e.Handled = true;
            }
        }

        private void textBoxValor_Click(object sender, EventArgs e)
        {
            textBoxValor.Clear();
        }

        private void textBoxValor_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, button2);
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonBuscar);
        }

        private void textBoxProduto_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonBuscar);
        }
    }
}
