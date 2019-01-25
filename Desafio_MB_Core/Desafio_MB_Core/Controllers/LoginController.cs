using Microsoft.AspNetCore.Mvc;

namespace Desafio_MB_Core.Controllers.LoginController
{
    public class ResultResponse
    {
        public string result { get; set; }
        public string sessionID { get; set; }
    }

    public class InputLogin
    {
        public string usuario { get; set; }
        public string senha { get; set; }
    }

    public class logoutLogin
    {
        public string sessionId { get; set; }
    }

    [Route("desafiomb/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResultResponse> Post([FromBody] InputLogin data)
        {
            ResultResponse result = new ResultResponse();

            if (data.usuario != null && data.senha != null)
            {
                result.sessionID = apidesafiomb.LoginUser(data.usuario, data.senha);
                if (result.sessionID.Length > 0)
                {
                    result.result = "Login OK.";
                }
                else
                {
                    result.result = "Login fail.";
                }
            }
            return result;
        }
    }

    [Route("desafiomb/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResultResponse> Post([FromBody] logoutLogin data)
        {
            ResultResponse result = new ResultResponse();
            if (!apidesafiomb.IsSessionOpen(data.sessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }

            int logr = apidesafiomb.LogoutUser(data.sessionId);
            if (logr == 1)
                result.result = "Logout OK.";
            else
                result.result = "Logout falhou.";

            return result;
        }
    }
}
