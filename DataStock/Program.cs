using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataStock
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (FormLogin flogin = new FormLogin())
            {
                if(flogin.ShowDialog() == DialogResult.OK)
                {
                    flogin.Close();
                    Application.Run(new FormPrincipal());
                }
            }
        }
    }
}
