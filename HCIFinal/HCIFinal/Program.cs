using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCIFinal
{
    public class GlobalMouseHandler : IMessageFilter
    {

        private const int WM_LBUTTONDOWN = 0x201;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                Console.Write("1");
            }
            return false;
        }
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalMouseHandler globalClick = new GlobalMouseHandler();
            Application.AddMessageFilter(globalClick);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
