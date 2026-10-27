using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToodedUl_IKTpv
{
    public class Kategooria
    {
        public int Id { get; set; }

        public string Kategooria_nimetus { get; set; }

        public string Kirjeldus { get; set; }

        public virtual ICollection<Toode> Tooded { get; set; }
            = new List<Toode>();
    }
}