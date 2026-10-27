using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToodedUl_IKTpv
{
    public static class Kaustad
    {
        public static readonly string Juur =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ToodedApp");

        public static readonly string Pildid =
            Path.Combine(Juur, "Images");

        public static readonly string Arved =
            Path.Combine(Juur, "Arved");

        public static void Loo()
        {
            Directory.CreateDirectory(Pildid);
            Directory.CreateDirectory(Arved);
        }

        public static Image LaePilt(string failinimi)
        {
            if (string.IsNullOrEmpty(failinimi))
                return null;

            string tee = Path.Combine(Pildid, failinimi);

            if (!File.Exists(tee))
                return null;

            using (Image ajutine = Image.FromFile(tee))
            {
                return new Bitmap(ajutine);
            }
        }
    }
}