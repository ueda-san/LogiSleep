using System;
using System.Threading;
using System.Windows.Forms;

namespace LogiSleep
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "Global\\LogiSleep@ueda-san", out createdNew)){
                if (createdNew){
                    try {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    } finally {
                        mutex.ReleaseMutex();
                        mutex.Close();
                    }
                }else{
                    MessageBox.Show("Another instance is running", "LogiSleep", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mutex.Close();
                    return;
                }
            }
        }
    }
}
