using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace DataStock
{
    class Cadastro
    {
        string sqlconect = ConfigurationManager.ConnectionStrings["DataStock.Properties.Settings.DataStockConnectionString"].ConnectionString;

        public void cadastro(string comando)
        {
            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();

                SqlCommand cmd = new SqlCommand(comando, conexao);
                cmd.ExecuteNonQuery();

                conexao.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        public void listaTable(string comando, DataGridView data, int i)
        {
            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();

                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();

                ad.Fill(table);
                data.DataSource = table;

                data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                data.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                conexao.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        public bool verificaTrue(string comando)
        {
            bool verifica = false;

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();

                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataReader reader = cmd.ExecuteReader();

                verifica = reader.HasRows;

                conexao.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }

            return verifica;
        }

        public int pegaStock(string comando)
        {
            int stock = 0;

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();

                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    stock = Convert.ToInt16(reader["QUANTIDADE"].ToString());
                }

                conexao.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }

            return stock;
        }

        public string getString(string comando, string tabela)
        {
            string dados = "";

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();

                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    dados = reader[tabela].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }

            return dados;
        }

        public string[] dados(string comando, string tabela)
        {
            string[] estoque= { };

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();
                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> lista = new List<string>();

                while(reader.Read())
                {
                    lista.Add(reader[tabela].ToString());
                }

                estoque = lista.ToArray<string>();

                conexao.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }

            return estoque;
        }

        public DataTable gerarReport(string comando)
        {
            DataTable table = new DataTable();

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = sqlconect;

            try
            {
                conexao.Open();
                SqlCommand cmd = new SqlCommand(comando, conexao);
                SqlDataReader reader = cmd.ExecuteReader();

                table.Load(reader);
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }

            return table;
        }
    }
}
