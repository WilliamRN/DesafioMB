using Desafio_MB_Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_MB_Core.Controllers.UsuarioController
{
    public class RequestData
    {
        public string SessionId { get; set; }
    }

    public class ListaUsuarios
    {
        public Usuario[] list { get; set; }
    }

    public class UpdateUser
    {
        public string SessionId { get; set; }
        public Usuario usr { get; set; }
    }

    public class DisableUser
    {
        public string SessionId { get; set; }
        public int usrId { get; set; }
    }

    public class ResponseUser
    {
        public string result { get; set; }
    }

    [Route("desafiomb/usuario/[controller]")]
    [ApiController]
    public class ListarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST Usuario
        [HttpPost]
        public ActionResult<ListaUsuarios> Post([FromBody] RequestData data)
        {
            ListaUsuarios result = new ListaUsuarios();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                return result;
            }
            if (data.SessionId != null)
                result.list = apidesafiomb.GetUsers(data.SessionId).ToArray();
            return result;
        }
    }

    [Route("desafiomb/usuario/[controller]")]
    [ApiController]
    public class VisitanteController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST Usuario
        [HttpPost]
        public ActionResult<ResponseUser> Post([FromBody] UpdateUser data)
        {
            ResponseUser result = new ResponseUser();

            result.result = "Falha no cadastro de Visitante.";

            //accesslevel == ApiDesafioMB.ACCESS_LEVEL_NONE || data.usr.TipoAcesso == ApiDesafioMB.ACCESS_LEVEL_VISITOR

            int accesslevel = apidesafiomb.GetAccessBySessionID(data.SessionId);
            //Usuário é Visitante atualizando cadastro ou novo usuário?
            //1 é visitante, 0 não existe ainda.
            if (accesslevel == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR || accesslevel == ApiDesafioMBConstants.ACCESS_LEVEL_NONE)
            {
                //Se Usuário existe, garantir que é a mesma sessão.
                if (data.usr.SessionId != null)
                {
                    if (data.SessionId == data.usr.SessionId && data.usr.SessionId.Length > 0)
                    {
                        apidesafiomb.SaveUser(data.usr);
                        result.result = "Usuário atualizado com sucesso!";
                    }
                }
                //Se novo usuário, garantir que é cadastro de visitante.
                else if (accesslevel == ApiDesafioMBConstants.ACCESS_LEVEL_NONE && data.usr.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
                {
                    //Não permitir login vazio.
                    if (data.usr.Login == null)
                    {
                        result.result = "Login inválido.";
                        return result;
                    }
                    else if (data.usr.Login == "")
                    {
                        result.result = "Login inválido.";
                        return result;
                    }

                    if (data.usr.Senha == null)
                    {
                        result.result = "Login inválido.";
                        return result;
                    }
                    else if (data.usr.Senha == "")
                    {
                        result.result = "Login inválido.";
                        return result;
                    }

                    // Login deve ser unico.
                    if (apidesafiomb.IsUniqueLogin(data.usr.Login))
                    {
                        apidesafiomb.SaveUser(data.usr);
                        result.result = "Novo Usuário cadastrado.";
                    }
                    else
                    {
                        result.result = "Login já esite!";
                    }
                }
            }
            return result;
        }
    }


    [Route("desafiomb/usuario/[controller]")]
    [ApiController]
    public class CriacaoController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST Usuario
        [HttpPost]
        public ActionResult<ResponseUser> Post([FromBody] UpdateUser data)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            // 1 OK
            ResponseUser result = new ResponseUser();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.CreateUser(data.SessionId, data.usr);
            result.result = "Falha na operação, erro ao criar Usuário.";

            switch (res)
            {
                case 1:
                    result.result = "Usuário " + data.usr.Login + " criado com sucesso.";
                    break;
                case -2:
                    result.result = "Falha na operação, permissão insuficiente.";
                    break;
                case -3:
                    result.result = "Falha na operação, login " + data.usr.Login + " já existe.";
                    break;
                case -4:
                    result.result = "Falha na operação, login vazio.";
                    break;
                case -5:
                    result.result = "Falha na operação, Usuário não existe.";
                    break;
                default:
                    result.result = "Falha na operação, erro ao criar Usuário.";
                    break;
            }
            return result;
        }
    }

    [Route("desafiomb/usuario/[controller]")]
    [ApiController]
    public class AtualizarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST Usuario
        [HttpPost]
        public ActionResult<ResponseUser> Post([FromBody] UpdateUser data)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            // 1 OK
            ResponseUser result = new ResponseUser();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.UpdateUser(data.SessionId, data.usr);
            result.result = "Falha na operação, erro ao atualizar Usuário.";

            switch (res)
            {
                case 1:
                    result.result = "Usuário " + data.usr.Login + " atualizado com sucesso.";
                    break;
                case -2:
                    result.result = "Falha na operação, permissão insuficiente.";
                    break;
                case -3:
                    result.result = "Falha na operação, login " + data.usr.Login + " já existe.";
                    break;
                case -4:
                    result.result = "Falha na operação, login vazio.";
                    break;
                case -5:
                    result.result = "Falha na operação, Usuário não existe.";
                    break;
                default:
                    result.result = "Falha na operação, erro ao atualizar Usuário.";
                    break;
            }
            return result;
        }
    }

    [Route("desafiomb/usuario/[controller]")]
    [ApiController]
    public class DesabilitarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST Usuario
        [HttpPost]
        public ActionResult<ResponseUser> Post([FromBody] DisableUser data)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            // 1 OK
            ResponseUser result = new ResponseUser();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.DisableUser(data.SessionId, data.usrId);
            result.result = "Falha na operação, erro ao desabilitar Usuário.";

            switch (res)
            {
                case 1:
                    result.result = "Usuário desativado com sucesso.";
                    break;
                case -2:
                    result.result = "Falha na operação, permissão insuficiente.";
                    break;
                default:
                    result.result = "Falha na operação, erro ao desabilitar Usuário.";
                    break;
            }
            return result;
        }
    }
}
