using Microsoft.AspNetCore.Mvc;
using Desafio_MB_Core.Models;
using System.Collections.Generic;

namespace Desafio_MB_Core.Controllers.EventoController
{
    public class UpdateEvento
    {
        public string SessionId { get; set; }
        public Evento evt { get; set; }
    }

    public class DisableEvento
    {
        public string SessionId { get; set; }
        public int evtid { get; set; }
    }

    public class ListEvento
    {
        public Evento[] list { get; set; }
        public int[] listingresso { get; set; }
    }

    public class RequestData
    {
        public string sessionId { get; set; }
    }

    public class ResponseData
    {
        public string result { get; set; }
    }

    [Route("desafiomb/evento/[controller]")]
    [ApiController]
    public class CriacaoController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResponseData> Post([FromBody] UpdateEvento data)
        {

            ResponseData result = new ResponseData();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.CreateEvento(data.SessionId, data.evt);
            switch (res)
            {
                //-1 Erro
                //-2 Permissão errada.
                //-3 Data inválida.
                //-4 Preço inválido.
                //-5 Nome vazio. (Removido p/ teste)
                //-6 Descrição vazia. (Removido p/ teste)
                // 1 OK

                case 1:
                    result.result = "Evento criado com sucesso!";
                    break;
                case -2:
                    result.result = "Falha na criação do Evento, permissão insuficiente.";
                    break;
                case -3:
                    result.result = "Falha na criação do Evento, a data do evento deve ser depois da data atual.";
                    break;
                case -4:
                    result.result = "Falha na criação do Evento, preço deve ser entre 0 e 9 999 999 999,99.";
                    break;
                case -5:
                    result.result = "Falha na criação do Evento, nome de evento vazio.";
                    break;
                case -6:
                    result.result = "Falha na criação do Evento, descrição de Evento vazia.";
                    break;
                default:
                    result.result = "Falha na criação do Evento.";
                    break;
            }
            
            return result;
        }
    }

    [Route("desafiomb/evento/[controller]")]
    [ApiController]
    public class AlterarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResponseData> Post([FromBody] UpdateEvento data)
        {
            ResponseData result = new ResponseData();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.UpdateEvento(data.SessionId, data.evt);
            switch (res)
            {
                //-1 Erro
                //-2 Permissão errada.
                //-3 Data inválida.
                //-4 Preço inválido.
                //-5 Nome vazio. (Removido p/ teste)
                //-6 Descrição vazia. (Removido p/ teste)
                //-7 Evento não existe.
                // 1 OK

                case 1:
                    result.result = "Evento alterado com sucesso!";
                    break;
                case -2:
                    result.result = "Falha na alteração do Evento, permissão insuficiente.";
                    break;
                case -3:
                    result.result = "Falha na alteração do Evento, a data do evento deve ser depois da data atual.";
                    break;
                case -4:
                    result.result = "Falha na alteração do Evento, preço deve ser entre 0 e 9 999 999 999,99.";
                    break;
                case -5:
                    result.result = "Falha na alteração do Evento, nome de evento vazio.";
                    break;
                case -6:
                    result.result = "Falha na alteração do Evento, descrição de Evento vazia.";
                    break;
                case -7:
                    result.result = "Falha na alteração do Evento, Evento não existe.";
                    break;
                default:
                    result.result = "Falha na alteração do Evento.";
                    break;
            }

            return result;
        }
    }

    [Route("desafiomb/evento/[controller]")]
    [ApiController]
    public class DesabilitarController : ControllerBase
    {
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ResponseData> Post([FromBody] DisableEvento data)
        {
            ResponseData result = new ResponseData();
            if (!apidesafiomb.IsSessionOpen(data.SessionId))
            {
                result.result = "Sessão expirada.";
                return result;
            }
            int res = apidesafiomb.DisableEvento(data.SessionId, data.evtid);
            switch (res)
            {
                //-1 Erro
                //-2 Permissão errada.
                //-5 Evento não existe.
                // 1 OK

                case 1:
                    result.result = "Evento desativado com sucesso!";
                    break;
                case -2:
                    result.result = "Falha na desativação do Evento, permissão insuficiente.";
                    break;
                case -5:
                    result.result = "Falha na desativação do Evento, Evento não existe.";
                    break;
                default:
                    result.result = "Falha na desativação do Evento.";
                    break;
            }

            return result;
        }
    }

    [Route("desafiomb/evento/[controller]")]
    [ApiController]
    public class VisitanteController : ControllerBase
    {
        //Obter os Eventos para o visitante, e descobrir se existem ingressos para cada um.
        ApiDesafioMB apidesafiomb = new ApiDesafioMB();

        // POST 
        [HttpPost]
        public ActionResult<ListEvento> Post([FromBody] RequestData data)
        {
            ListEvento result = new ListEvento();
            List<Evento> listEvento = new List<Evento>();
            List<int> listIngresso = new List<int>();
            Ingresso aux = null;

            if (!apidesafiomb.IsSessionOpen(data.sessionId))
            {
                return result;
            }

            listEvento = apidesafiomb.GetEventos(data.sessionId);

            foreach(Evento e in listEvento)
            {
                aux = apidesafiomb.GetIngressoEventoByVisitante(data.sessionId, e);
                if (aux != null)
                    listIngresso.Add(aux.Id);
                else
                    listIngresso.Add(-1);
            }

            result.list = listEvento.ToArray();
            result.listingresso = listIngresso.ToArray();

            return result;
        }
    }
}
