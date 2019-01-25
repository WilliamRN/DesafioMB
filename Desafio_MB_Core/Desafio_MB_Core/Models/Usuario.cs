using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio_MB_Core.Models
{
    public class Usuario
    {
        public virtual int Id { get; set; }
        public virtual String RG { get; set; }
        public virtual String CPF { get; set; }
        public virtual String EnderecoRua { get; set; }
        public virtual String EnderecoBairro { get; set; }
        public virtual String EnderecoComplemento { get; set; }
        public virtual String EnderecoCEP { get; set; }
        public virtual String EnderecoCidade { get; set; }
        public virtual String EnderecoEstado { get; set; }
        public virtual String EnderecoPais { get; set; }
        public virtual String Email { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual int Sexo { get; set; }
        public virtual int ContaAtiva { get; set; }
        public virtual String Nome { get; set; }
        public virtual int TipoLogin { get; set; }
        public virtual String ImagemProfilePath { get; set; }
        public virtual String LastToken { get; set; }
        public virtual String Login { get; set; }
        public virtual String Senha { get; set; }
        public virtual int TipoAcesso { get; set; }
        public virtual String NumeroCartao { get; set; }
        public virtual String NomeTitular { get; set; }
        public virtual DateTime DataExpiracao { get; set; }
        public virtual int TipoCartao { get; set; }
        public virtual String EnderecoCobrancaRua { get; set; }
        public virtual String EnderecoCobrancaBairro { get; set; }
        public virtual String EnderecoCobrancaComplemento { get; set; }
        public virtual String EnderecoCobrancaCEP { get; set; }
        public virtual String EnderecoCobrancaCidade { get; set; }
        public virtual String EnderecoCobrancaEstado { get; set; }
        public virtual String EnderecoCobrancaPais { get; set; }
        public virtual String SessionId { get; set; }
        public virtual int Subordinado { get; set; }
    }
}
