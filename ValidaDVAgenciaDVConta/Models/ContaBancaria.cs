using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidaDVAgenciaDVConta.Models
{
    public class ContaBancaria
    {
        public string Banco { get; set; }
        public string Agencia { get; set; }

        public int DvAgencia { get; set; }
        public string Conta { get; set; }
        public int DvConta { get; set; }
    }
}
