using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataStock
{
    class Funcao
    {
        public bool isok(string dados)
        {
            bool math = false;

            Regex nome = new Regex(@"^[A-Z0-9\s]*$");
            Regex data = new Regex(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$");
            Regex valor = new Regex(@"^(\d*\,?\d{2}?){1}$");

            if(nome.IsMatch(dados)==true)
            {
                math = true;
            }
            else if(data.IsMatch(dados)==true)
            {
                math = true;
            }
            else if(valor.IsMatch(dados)==true)
            {
                math = true;
            }

            return math;
        }

        public void onEnterCode(KeyEventArgs e, Button botaox)
        {
            if(e.KeyCode == Keys.Enter)
            {
                botaox.PerformClick();
            }
        }

        public string replaceCharacters(string texto)
        {
            texto = texto.Replace("Ã", "A");
            texto = texto.Replace("Õ", "O");
            texto = texto.Replace("Á", "A");
            texto = texto.Replace("É", "E");
            texto = texto.Replace("Ê", "E");
            texto = texto.Replace("Í", "I");
            texto = texto.Replace("Ó", "O");
            texto = texto.Replace("Ú", "U");
            texto = texto.Replace("À", "A");
            texto = texto.Replace("Â", "A");
            texto = texto.Replace("Ç", "C");
            texto = texto.Replace("--", "");

            return texto;
        }
    }
}
