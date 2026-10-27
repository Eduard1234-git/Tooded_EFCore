using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToodedUl_IKTpv
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Kaustad.Loo();

            Database.SetInitializer(new ToodedInitializer());

            using (var db = new ToodedContext())
            {
                db.Database.Initialize(false);
            }

            Application.Run(new Form2());
        }
    }
}