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
    public partial class FormCadProdutos : Form
    {
        Cadastro cadastro = new Cadastro();
        Funcao funcao = new Funcao();

        public FormCadProdutos()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tipo = funcao.replaceCharacters(textBoxTipo.Text);
            string marca = funcao.replaceCharacters(textBoxMarca.Text);
            string descricao = funcao.replaceCharacters(textBoxDescricao.Text);

            if (textBoxTipo.Text!="" && funcao.isok(tipo)==true)
            {
                string cmdInsert = @"INSERT INTO PRODUTO VALUES('" + tipo + "', '" + marca + "', '" + descricao + "', 0);";

                if (textBoxMarca.Text!="" && funcao.isok(marca)==true && textBoxDescricao.Text!="" && funcao.isok(descricao)==true)
                {
                    cadastro.cadastro(cmdInsert);
                }
                else if((textBoxMarca.Text != "" && funcao.isok(marca) == true) || (textBoxDescricao.Text != "" && funcao.isok(descricao) == true))
                {
                    cadastro.cadastro(cmdInsert);
                }
                else
                {
                    cadastro.cadastro(cmdInsert);
                }

                limpaCampo();
            }
        }

        private void limpaCampo()
        {
            textBoxTipo.Clear();
            textBoxMarca.Clear();
            textBoxDescricao.Clear();
        }

        private void textBoxTipo_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonCadastro);
        }

        private void textBoxMarca_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonCadastro);
        }

        private void textBoxDescricao_KeyDown(object sender, KeyEventArgs e)
        {
            funcao.onEnterCode(e, buttonCadastro);
        }
    }
}
