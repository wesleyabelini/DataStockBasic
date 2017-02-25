using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataStock
{
    public partial class FormPrincipal : Form
    {
        Cadastro cadastro = new Cadastro();
        Funcao funcao = new Funcao();
        public FormPrincipal()
        {
            InitializeComponent();
            listaEstoque();
        }

        private void produtoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCadProdutos fProdutos = new FormCadProdutos();
            fProdutos.Show();
        }

        private void entradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormEstoque fEstoque = new FormEstoque())
            {
                if(fEstoque.ShowDialog() == DialogResult.OK)
                {
                    fEstoque.Close();

                    listaEstoque();
                    dataGridView1_Click(dataGridView1, new EventArgs());
                    atualizaLimiteEstoque();
                }
            }
        }

        private void sobreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBoxSobre fSobre = new AboutBoxSobre();
            fSobre.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripLabel1.Text = DateTime.Now.ToString();
        }

        private void listaEstoque()
        {
            string cmdSelect = @"SELECT IDPRODUTO AS 'ID', QUANTIDADE AS 'QNT',  TIPO, MARCA, DESCRICAO AS 'DESCRIÇÃO' FROM PRODUTO";
            cadastro.listaTable(cmdSelect, dataGridView1, 2);
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count >1 && Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value) > 0)
            {
                DateTime data = DateTime.Now;

                int scalaChart = 0;
                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisY.Minimum = 0;

                string produto = "Estoque";
                Series series = chart1.Series.Add(produto);
                series.ChartType = SeriesChartType.Spline;

                //função destinada ao estoque

                if (funcao.isok(maskedTextBox1.Text) == true && funcao.isok(maskedTextBox2.Text) == true)
                {
                    string data1 = Convert.ToDateTime(maskedTextBox1.Text).ToString("yyyy/MM/dd");
                    string data2 = Convert.ToDateTime(maskedTextBox2.Text).ToString("yyyy/MM/dd");

                    string cmdSelect = @"SELECT * FROM LOGS WHERE IDPRODUTO=" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + " AND DATAS BETWEEN '" +
                        data1 + "' AND '" + data2 + "';";

                    periodo(series, cmdSelect);
                }
                else
                {
                    int janeiro = meses("01", "Janeiro", series, 0, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int fevereiro = meses("02", "Fevereiro", series, janeiro, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int marco = meses("03", "Março", series, fevereiro, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int abril = meses("04", "Abril", series, marco, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int maio = meses("05", "Maio", series, abril, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int junho = meses("06", "Junho", series, maio, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int julho = meses("07", "Julho", series, junho, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int agosto = meses("08", "Agosto", series, julho, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int setembro = meses("09", "Setembro", series, agosto, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int outubro = meses("10", "Outubro", series, setembro, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int novembro = meses("11", "Novembro", series, outubro, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());
                    int dezembro = meses("12", "Dezembro", series, novembro, "LOGS", "ESTOQUE", numericUpDownAno.Value.ToString());

                    int[] mesesA = { janeiro, fevereiro, marco, abril, maio, julho, julho, agosto, setembro, outubro, novembro, dezembro };

                    for (int i = 0; i < mesesA.Length; i++)
                    {
                        if (scalaChart < mesesA[i])
                        {
                            scalaChart = mesesA[i];
                        }
                    }

                    int scalaChart2 = scalaChart + ((scalaChart * 10) / 100);
                    int resto = scalaChart2 % 10;

                    while (resto != 0 && scalaChart < scalaChart2)
                    {
                        scalaChart2--;
                    }

                    chart1.ChartAreas[0].AxisY.Maximum = scalaChart2;
                }

                //função destinado à venda


                if (funcao.isok(maskedTextBox1.Text) == true && funcao.isok(maskedTextBox2.Text) == true)
                {
                    string venda = "Venda";
                    Series vendas = chart1.Series.Add(venda);
                    vendas.ChartType = SeriesChartType.Spline;

                    string data1 = Convert.ToDateTime(maskedTextBox1.Text).ToString("yyyy/MM/dd");
                    string data2 = Convert.ToDateTime(maskedTextBox2.Text).ToString("yyyy/MM/dd");

                    string cmdSelect = @"SELECT * FROM VENDA WHERE IDPRODUTO=" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + " AND DATAS BETWEEN '" +
                        data1 + "' AND '" + data2 + "';";

                    periodo(vendas, cmdSelect);
                }
                else
                {
                    int anoAtual = Convert.ToInt16(numericUpDownAno.Value);
                    int anos = Convert.ToInt16(numericUpDownAnos.Value);

                    for (int i = anos; i > 0; i--)
                    {
                        string venda = "Venda " + anoAtual.ToString();
                        Series vendas = chart1.Series.Add(venda);
                        vendas.ChartType = SeriesChartType.Spline;

                        int janeiro = meses("01", "Janeiro", vendas, 0, "VENDA", "", anoAtual.ToString());
                        int fevereiro = meses("02", "Fevereiro", vendas, janeiro, "VENDA", "", anoAtual.ToString());
                        int marco = meses("03", "Março", vendas, fevereiro, "VENDA", "", anoAtual.ToString());
                        int abril = meses("04", "Abril", vendas, marco, "VENDA", "", anoAtual.ToString());
                        int maio = meses("05", "Maio", vendas, abril, "VENDA", "", anoAtual.ToString());
                        int junho = meses("06", "Junho", vendas, maio, "VENDA", "", anoAtual.ToString());
                        int julho = meses("07", "Julho", vendas, junho, "VENDA", "", anoAtual.ToString());
                        int agosto = meses("08", "Agosto", vendas, julho, "VENDA", "", anoAtual.ToString());
                        int setembro = meses("09", "Setembro", vendas, agosto, "VENDA", "", anoAtual.ToString());
                        int outubro = meses("10", "Outubro", vendas, setembro, "VENDA", "", anoAtual.ToString());
                        int novembro = meses("11", "Novembro", vendas, outubro, "VENDA", "", anoAtual.ToString());
                        int dezembro = meses("12", "Dezembro", vendas, novembro, "VENDA", "", anoAtual.ToString());

                        anoAtual--;

                        int[] mesesA = { janeiro, fevereiro, marco, abril, maio, julho, julho, agosto, setembro, outubro, novembro, dezembro };

                        for (int j = 0; j < mesesA.Length; j++)
                        {
                            if (scalaChart < mesesA[j])
                            {
                                scalaChart = mesesA[j];
                            }
                        }

                        int scalaChart2 = scalaChart + ((scalaChart * 10) / 100);
                        int resto = scalaChart2 % 10;

                        while (resto != 0 && scalaChart < scalaChart2)
                        {
                            scalaChart2--;
                        }

                        chart1.ChartAreas[0].AxisY.Maximum = scalaChart2;
                    }
                }
            }
        }

        private int meses(string mes, string mesescrito, Series series, int ultimovalor, string tabela, string tipoGrafico, string anoAtual)
        {
            string fDia = getDia(mes, Convert.ToInt16(anoAtual));

            int valor = 0;

            DateTime data = DateTime.Now;
            string month = "";

            if(data.Month > 9)
            {
                month = data.Month.ToString();
            }
            else
            {
                month = "0" + (data.Month).ToString();
            }

            int ano = data.Year;

            string cmdSelectMes = @"SELECT * FROM " + tabela + " WHERE IDPRODUTO=" + dataGridView1.CurrentRow.Cells[0].Value.ToString() +
                " AND DATAS BETWEEN '" + anoAtual + "/" + mes + "/01' AND '" + anoAtual + "/" + mes + "/" + fDia + "'";

            string[] estoque = cadastro.dados(cmdSelectMes, "QUANTIDADE");
            string[] datas = cadastro.dados(cmdSelectMes, "DATAS");
            string[] diaEstoque= new string[Convert.ToInt16(fDia)];
            int fEstoque = 0;

            if (estoque.Length > 0)
            {
                diaEstoque = estoqueDia(fDia, estoque, anoAtual, mes, datas, diaEstoque, ultimovalor);

                if(Convert.ToInt32(anoAtual) == ano && mes == month)
                {
                    fEstoque = Convert.ToInt32(diaEstoque[diaEstoque.Length - 1]);
                }
                else
                {
                    fEstoque = frequenciaEstoque(diaEstoque);
                }
            }
            else
            {
                fEstoque = ultimovalor;
            }
            
            for(int i = 0; i<estoque.Length; i++)
            {
                valor +=Convert.ToInt32(estoque[i]);
            }

            if(tipoGrafico=="ESTOQUE")
            {
                series.Points.AddXY(mesescrito, fEstoque);
                ultimovalor = fEstoque;
            }
            else
            {
                if (estoque.Length > 0)
                {
                    series.Points.AddXY(mesescrito, valor);
                    ultimovalor = valor;
                }
                else if (estoque.Length == 0)
                {
                    series.Points.AddXY(mesescrito, 0);
                    ultimovalor = 0;
                }
            }
            
            return ultimovalor;
        }

        private string getDia(string mes, int ano)
        {
            DateTime data = DateTime.Now;

            string fDia ="";

            if (mes == "02")
            {
                if(DateTime.IsLeapYear(ano))
                {
                    fDia = "29";
                }
                else
                {
                    fDia = "28";
                }
            }
            else if (mes == "04" || mes == "06" || mes == "09" || mes == "11")
            {
                fDia = "30";
            }
            else
            {
                fDia = "30";
            }

            return fDia;
        }

        private int frequenciaEstoque(string[] diaEstoque)
        {
            int min =Convert.ToInt16(diaEstoque[0]);
            int max = Convert.ToInt16(diaEstoque[0]);
            int frequencia = 0;
            int media = 0;

            for(int i=0; i<diaEstoque.Length; i++)
            {
                if(Convert.ToInt16(diaEstoque[i]) < min)
                {
                    min = Convert.ToInt16(diaEstoque[i]);
                }

                if(Convert.ToInt16(diaEstoque[i]) > max)
                {
                    max = Convert.ToInt16(diaEstoque[i]);
                }
            }

            int[] fx = { 0, 0, 0, 0, 0, 0, 0 };

            frequencia = ((max - min) / 7) + 1;

            int a = min;
            int a1 = min + frequencia - 1;

            int b = a1 + 1;
            int b1 = b + frequencia - 1;

            int c = b1 + 1;
            int c1 = c + frequencia - 1;

            int d = c1 + 1;
            int d1 = d + frequencia - 1;

            int e = d1 + 1;
            int e1 = e + frequencia - 1;

            int f = e1 + 1;
            int f1 = f + frequencia - 1;

            int g = f1 + 1;
            int g1 = g + frequencia - 1;

            for(int i = 0; i < diaEstoque.Length; i++)
            {
                if(Convert.ToInt32(diaEstoque[i]) >= a && Convert.ToInt32(diaEstoque[i]) < a1)
                {
                    fx[0]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= b && Convert.ToInt32(diaEstoque[i]) < b1)
                {
                    fx[1]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= c && Convert.ToInt32(diaEstoque[i]) < c1)
                {
                    fx[2]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= d && Convert.ToInt32(diaEstoque[i]) < d1)
                {
                    fx[3]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= e && Convert.ToInt32(diaEstoque[i]) < e1)
                {
                    fx[4]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= f && Convert.ToInt32(diaEstoque[i]) < f1)
                {
                    fx[5]++;
                }
                else if(Convert.ToInt32(diaEstoque[i]) >= g && Convert.ToInt32(diaEstoque[i]) <= g1)
                {
                    fx[6]++;
                }
            }

            int j = 0;
            for(int i = 0; i < 7; i++)
            {
                while(fx[j] < fx[i])
                {
                    if(j==6)
                    {
                        if(i==0)
                        {
                            media = (a + a1) / 2;
                        }
                        else if(i==1)
                        {
                            media = (b + b1) / 2;
                        }
                        else if(i==2)
                        {
                            media = (c + c1) / 2;
                        }
                        else if(i==3)
                        {
                            media = (d + d1) / 2;
                        }
                        else if(i==4)
                        {
                            media = (e + e1) / 2;
                        }
                        else if(i==5)
                        {
                            media = (f + f1) / 2;
                        }
                        else if(i==6)
                        {
                            media = (g + g1) / 2;
                        }
                    }

                    j++;
                }

                if(i==6)
                {
                    if (j == 0)
                    {
                        media = (a + a1) / 2;
                    }
                    else if (j == 1)
                    {
                        media = (b + b1) / 2;
                    }
                    else if (j == 2)
                    {
                        media = (c + c1) / 2;
                    }
                    else if (j == 3)
                    {
                        media = (d + d1) / 2;
                    }
                    else if (j == 4)
                    {
                        media = (e + e1) / 2;
                    }
                    else if (j == 5)
                    {
                        media = (f + f1) / 2;
                    }
                    else if (j == 6)
                    {
                        media = (g + g1) / 2;
                    }
                }
            }

            return media;
        }

        private string[] estoqueDia(string fDia, string[] estoque, string anoAtual, string mes, string[] datas, string[] diaEstoque, int ultimovalor)
        {
            int dia = 01;
            int j = 0;
            int i = 0;
            while (i < Convert.ToInt32(fDia))
            {
                if (j < estoque.Length)
                {
                    DateTime dataEstoque = Convert.ToDateTime(anoAtual + "/" + mes + "/" + dia);
                    int x = DateTime.Compare(dataEstoque, Convert.ToDateTime(datas[j]));

                    if (j == 0 && x < 0)
                    {
                        diaEstoque[i] = ultimovalor.ToString();
                        dia++;
                        i++;
                    }
                    else
                    {
                        if (x < 0)
                        {
                            diaEstoque[i] = estoque[j - 1];
                            dia++;
                            i++;
                        }
                        else if (x == 0)
                        {
                            diaEstoque[i] = estoque[j];
                            dia++;
                            i++;

                            if(j < estoque.Length)
                            {
                                j++;
                            }
                        }
                        else
                        {
                            if (j < estoque.Length)
                            {
                                j++;
                            }
                        }
                    }
                }
                else
                {
                    while (i < Convert.ToInt16(fDia))
                    {
                        diaEstoque[i] = estoque[j - 1];
                        i++;
                    }
                }
            }

            return diaEstoque;
        }

        private void periodo(Series series, string cmdSelect)
        {
            string[] datas = cadastro.dados(cmdSelect, "DATAS");
            string[] valores = cadastro.dados(cmdSelect, "QUANTIDADE");

            int scalaChart = 0;

            for (int i = 0; i < valores.Length; i++)
            {
                series.Points.AddXY(datas[i], valores[i]);

                if(scalaChart < Convert.ToInt32(valores[i]))
                {
                    scalaChart = Convert.ToInt32(valores[i]);
                }
            }

            chart1.ChartAreas[0].AxisY.Maximum = scalaChart + ((scalaChart * 10) / 100);
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataStockDataSet.COMPRA_VENDA' table. You can move, or remove it, as needed.
            //this.cOMPRA_VENDATableAdapter.Fill(this.dataStockDataSet.COMPRA_VENDA);  //Gerar grafico automatico

            button1_Click(buttonCusto, new EventArgs()); //gerar grafico manualmente
            atualizaLimiteEstoque();
        }

        private void atualizaLimiteEstoque()
        {
            string[] idproduto = alertaEstoque("id");
            string[] venda = alertaEstoque("");

            if (idproduto.Length > 0 && venda.Length > 0)
            {
                for (int i = 0; i < idproduto.Length - 1; i++)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    int estoque = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    int vendas = Convert.ToInt32(venda[i]);

                    if (vendas > estoque)
                    {
                        //AJUSTAR A COR CASO O ESTOQUE ESTEJA ABAIXO DO ESPERADO PARA O MES SEGUINTE

                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        private void textBoxAno_Click(object sender, EventArgs e)
        {
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
        }

        private void maskedTextBox2_DoubleClick(object sender, EventArgs e)
        {
            maskedTextBox2.Text = DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Grafico de custo

            DateTime data = DateTime.Now;

            double max = 0;
            double min = 0;

            decimal valorEstoque = 0;

            chart1.Series.Clear();

            string cmdSelect = @"SELECT SUM(C.VALORCOMPRA) AS 'COMPRA', SUM(V.VALORVENDA) AS 'VENDA', SUM(V.VALORVENDA) - SUM(C.VALORCOMPRA) AS 'LUCRO' FROM  PRODUTO AS P 
INNER JOIN VENDA AS V ON V.IDPRODUTO = P.IDPRODUTO 
INNER JOIN COMPRA AS C ON C.IDPRODUTO = P.IDPRODUTO ";

            string cmdIdProduto = @"SELECT IDPRODUTO AS 'ID' FROM PRODUTO";

            string[] idproduto = cadastro.dados(cmdIdProduto, "ID");


            if (funcao.isok(maskedTextBox1.Text) == true && funcao.isok(maskedTextBox2.Text) == true)
            {
                string data1 = Convert.ToDateTime(maskedTextBox1.Text).ToString("yyyy/MM/dd");
                string data2 = Convert.ToDateTime(maskedTextBox2.Text).ToString("yyyy/MM/dd");

                cmdSelect += @"WHERE C.DATAS BETWEEN '" + data1 + "' AND '" + data2 + "'";
            }
            else
            {
                cmdSelect += @"WHERE C.DATAS BETWEEN '" + numericUpDownAno.Value.ToString() + "/01/01' AND '" + numericUpDownAno.Value.ToString() + "/12/31'";
            }
            
            Series compra = chart1.Series.Add("Compra");
            Series venda = chart1.Series.Add("Venda");
            Series valorestoque = chart1.Series.Add("Valor Estoque");
            Series lucro = chart1.Series.Add("Lucro");

            for(int i = 0; i < idproduto.Length; i++)
            {
                string valor = "";
                string cmdValorEstoque = @"SELECT TOP 1 (P.QUANTIDADE *(V.VALORVENDA / V.QUANTIDADE)) AS 'VALORESTOQUE' FROM VENDA AS V
INNER JOIN PRODUTO AS P ON P.IDPRODUTO = V.IDPRODUTO
WHERE P.IDPRODUTO =" + idproduto[i] + "ORDER BY IDVENDA DESC";

                valor = cadastro.getString(cmdValorEstoque, "VALORESTOQUE");

                if(valor !="")
                {
                    valorEstoque += Convert.ToDecimal(valor);
                }
            }

            string compraA = cadastro.getString(cmdSelect, "COMPRA");
            string vendaA = cadastro.getString(cmdSelect, "VENDA");
            string valorEstoqueA = valorEstoque.ToString();
            string lucroA = cadastro.getString(cmdSelect, "LUCRO");

            if(compraA=="")
            {
                compraA = "0";
            }

            if(vendaA=="")
            {
                vendaA = "0";
            }

            if(lucroA=="")
            {
                lucroA = "0";
            }

            if(valorEstoqueA =="")
            {
                valorEstoqueA = "0";
            }

            string[] valores = new string[4];
            valores[0] = compraA;
            valores[1] = vendaA;
            valores[2] = lucroA;
            valores[3] = valorEstoqueA;

            for(int i =0; i < valores.Length; i++)
            {
                if(max < Convert.ToDouble(valores[i]))
                {
                    max =Convert.ToDouble(valores[i]);
                }

                if(min > Convert.ToDouble(valores[i]))
                {
                    min = Convert.ToDouble(valores[i]);
                }
            }

            max = max + ((max * 10) / 100);
            min =Convert.ToInt32(min + ((min * 10) / 100));

            int resto =Convert.ToInt32(max) % 10;

            for(int i = Convert.ToInt32(max); resto!=0; i--)
            {
                resto = i % 10;
                max = i;
            }

            if(max == 0 && min == 0)
            {
                min = 0;
                max = 100;
            }

            chart1.ChartAreas[0].AxisY.Maximum =max;
            chart1.ChartAreas[0].AxisY.Minimum = min;

            compra.Points.Add(Convert.ToDouble(compraA));
            venda.Points.Add(Convert.ToDouble(vendaA));
            valorestoque.Points.Add(Convert.ToDouble(valorEstoqueA));
            lucro.Points.Add(Convert.ToDouble(lucroA));

            compra.IsValueShownAsLabel = true;
            venda.IsValueShownAsLabel = true;
            valorestoque.IsValueShownAsLabel = true;
            lucro.IsValueShownAsLabel = true;
        }

        private string[] alertaEstoque(string opcao)
        {
            string[] listidProdutos = new string[dataGridView1.Rows.Count];
            string[] vendaProdutos = new string[dataGridView1.Rows.Count];

            DateTime data = DateTime.Now;
            string anoBase = (Convert.ToInt16(data.Year) - 1).ToString();
            string mesBase = (Convert.ToInt16(data.Month) + 1).ToString();
            string diaBase = getDia(mesBase, Convert.ToInt16(anoBase));

            string anoBase2 = Convert.ToInt16(data.Year).ToString();
            string mesBase2 = (Convert.ToInt16(data.Month) - 1).ToString();

            int soma = 0;

            if (Convert.ToInt16(mesBase) < 10)
            {
                mesBase = "0" + mesBase;
            }

            string idProduto = "";
            int totalVenda = 0;

            string cmdSelectMovimentacao = @"SELECT * FROM VENDA WHERE DATAS BETWEEN '" + anoBase + "/" + mesBase + "/01' AND '" + anoBase + "/" + mesBase + 
                "/" + diaBase + "'";

            if(cadastro.verificaTrue(cmdSelectMovimentacao)==true)
            {
                int k = 0;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    idProduto = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                    string cmdSelect = @"SELECT * FROM VENDA WHERE IDPRODUTO=" + idProduto + " AND DATAS BETWEEN '" + anoBase + "/" + mesBase + "/01' AND '" +
                        anoBase + "/" + mesBase + "/" + diaBase + "'";

                    diaBase = getDia(mesBase, Convert.ToInt16(anoBase2));
                    string cmdSelect2 = @"SELECT * FROM VENDA WHERE IDPRODUTO=" + idProduto + " AND DATAS BETWEEN '" + anoBase2 + "/01/01' AND '" + anoBase2 + 
                        "/" + mesBase2 + "/" + diaBase + "'";

                    string[] venda = cadastro.dados(cmdSelect, "QUANTIDADE");
                    string[] vendaAtual = cadastro.dados(cmdSelect2, "QUANTIDADE");

                    if (venda.Length > 0 && vendaAtual.Length > 0)
                    {
                        for (int ia = 0; ia < vendaAtual.Length; ia++)
                        {
                            soma += Convert.ToInt32(vendaAtual[ia]);
                        }

                        soma = soma / Convert.ToInt16(mesBase2); //valor referente a media da venda do ano atual

                        for (int j = 0; j <= venda.Length; j++)
                        {
                            totalVenda += Convert.ToInt32(venda[j]); //valor referente a venda do ano passado no mes a frente ao atual
                        }

                        if(soma > 0)
                        {
                            soma = totalVenda * 100 / soma;
                        }

                        listidProdutos[k] = (totalVenda) + ((totalVenda * soma)/100).ToString();
                        vendaProdutos[k] = idProduto;

                        k++;
                    }
                    else if(venda.Length > 0)
                    {
                        for (int j = 0; j < venda.Length; j++)
                        {
                            totalVenda += Convert.ToInt32(venda[j]); //valor referente a venda do ano passado no mes a frente ao atual
                        }

                        listidProdutos[k] = totalVenda.ToString();
                        vendaProdutos[k] = idProduto;

                        k++;
                    }
                    else if(vendaAtual.Length >0)
                    {
                        for (int ia = 0; ia < vendaAtual.Length; ia++)
                        {
                            soma += Convert.ToInt32(vendaAtual[ia]);
                        }

                        soma = soma / Convert.ToInt16(mesBase2);

                        listidProdutos[k] = soma.ToString();
                        vendaProdutos[k] = idProduto;

                        k++;
                    }
                }
            }

            if(opcao=="id")
            {
                return vendaProdutos;
            }
            else
            {
                return listidProdutos;
            }
        }

        private void relatórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRelatorio relatorio = new FormRelatorio();
            relatorio.Show();
        }

        private void buttonProduto_Click(object sender, EventArgs e)
        {
            FormCadProdutos fCadProduto = new FormCadProdutos();
            fCadProduto.Show();
        }

        private void buttonEstoque_Click(object sender, EventArgs e)
        {
            FormEstoque fEstoque = new FormEstoque();
            fEstoque.Show();
        }

        private void buttonRelatorio_Click(object sender, EventArgs e)
        {
            FormRelatorio relatorio = new FormRelatorio();
            relatorio.Show();
        }

        private void buttonGrafico_Click(object sender, EventArgs e)
        {
            button1_Click(buttonGrafico, new EventArgs());
        }
    }
}
