using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Desafio_MB_Core.Controllers.ValidarIngressoController
{
    public class RequestValidacao
    {
        public string sessionId { get; set; }
        public int ingressoId { get; set; }
    }

    public class ResponseValidacao
    {
        public string result { get; set; }
    }


    [Route("desafiomb/[controller]")]
    [ApiController]
    public class ValidarIngressoController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResponseValidacao> Post([FromBody] RequestValidacao data)
        {
            ResponseValidacao result = new ResponseValidacao();
            if (!apidesafiomb.IsSessionOpen(data.sessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.ValidateIngresso(data.sessionId, data.ingressoId);
            switch (res)
            {
                case 1:
                    result.result = "Ingresso validado com sucesso!";
                    break;
                case -2:
                    result.result = "Usuário não encontrado.";
                    break;
                case -3:
                    result.result = "Ingresso não encontrado.";
                    break;
                case -4:
                    result.result = "Evento não encontrado.";
                    break;
                default:
                    result.result = "Erro na validação do ingresso.";
                    break;
            }

            return result;
        }
    }
}
