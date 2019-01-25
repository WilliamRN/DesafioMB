using Microsoft.AspNetCore.Mvc;

namespace Desafio_MB_Core.Controllers.IngressoController
{
    public class RequestData
    {
        public string sessionId { get; set; }
        public int eventId { get; set; }
        public string cvv { get; set; }
    }

    public class ResponseData
    {
        public string result { get; set; }
    }

    [Route("desafiomb/ingresso/[controller]")]
    [ApiController]
    public class ComprarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResponseData> Post([FromBody] RequestData data)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Evento não existe.
            //-4 Ingresso já existe.
            // 1 OK

            ResponseData result = new ResponseData();

            if (!apidesafiomb.IsSessionOpen(data.sessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }

            int res = 0;
            res = apidesafiomb.BuyIngresso(data.sessionId, data.eventId, data.cvv);
            switch (res)
            {
                //-1 Erro
                //-2 Permissão errada.
                //-3 Evento não existe.
                //-4 Ingresso já existe.
                // 1 OK

                case 1:
                    result.result = "Ingresso comprado com sucesso!";
                    break;
                case 0:
                    result.result = "Sessão expirada.";
                    break;
                case -2:
                    result.result = "Falha na compra do Ingresso, permissão negada.";
                    break;
                case -3:
                    result.result = "Falha na compra do Ingresso, Evento não existe.";
                    break;
                case -4:
                    result.result = "Falha na compra do Ingresso, ingresso já comprado.";
                    break;
                default:
                    result.result = "Falha na compra do Ingresso.";
                    break;
            }

            return result;
        }
    }
}
