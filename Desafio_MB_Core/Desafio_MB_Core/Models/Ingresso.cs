using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio_MB_Core.Models
{
    public class Ingresso
    {
        public virtual int Id { get; set; }
        public virtual int IdEvento { get; set; }
        public virtual int IdUsuario { get; set; }
        public virtual int Validade { get; set; }
        public virtual DateTime ValidoAte { get; set; }
        public virtual String NotaFiscalPath { get; set; }
        public virtual DateTime DataPagamento { get; set; }

    }
}
