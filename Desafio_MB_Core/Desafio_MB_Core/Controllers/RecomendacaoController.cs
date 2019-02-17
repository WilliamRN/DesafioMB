using Microsoft.AspNetCore.Mvc;
using Desafio_MB_Core.Models;

namespace Desafio_MB_Core.Controllers.RecomendacaoController
{

    public class RequestRecomendacao
    {
        public string sessionId { get; set; }
    }

    public class ResponseRecomendacao
    {
        public Evento[] result { get; set; }
    }

    [Route("desafiomb/evento/[controller]")]
    [ApiController]
    public class RecomendacaoController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();
        int b = 0;

        // POST 
        [HttpPost]
        public ActionResult<ResponseRecomendacao> Post([FromBody] RequestRecomendacao data)
        {
            ResponseRecomendacao result = new ResponseRecomendacao();
            if (!apidesafiomb.IsSessionOpen(data.sessionId))
            {
                b += 1;
                return result;
            }
            result.result = apidesafiomb.GetRecommendation(data.sessionId).ToArray();
            return result;
        }
    }
}
