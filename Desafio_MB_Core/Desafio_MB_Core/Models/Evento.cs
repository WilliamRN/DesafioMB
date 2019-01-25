using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio_MB_Core.Models
{
    public class Evento
    {
        public virtual int Id { get; set; }
        public virtual String NomeEvento { get; set; }
        public virtual String EnderecoRua { get; set; }
        public virtual String EnderecoBairro { get; set; }
        public virtual String EnderecoComplemento { get; set; }
        public virtual String EnderecoCEP { get; set; }
        public virtual String EnderecoCidade { get; set; }
        public virtual String EnderecoEstado { get; set; }
        public virtual String EnderecoPais { get; set; }
        public virtual int IdOrganizador { get; set; }
        public virtual Double Preco { get; set; }
        public virtual int EventoAtivo { get; set; }
        public virtual DateTime DataEvento { get; set; }
        public virtual int QuantidadeMaximaIngressos { get; set; }
        public virtual int QuantidadeVendida { get; set; }
        public virtual String Descricao { get; set; }
    }
}
