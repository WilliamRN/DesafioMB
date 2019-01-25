using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using Desafio_MB_Core.Models;
using System.Security.Cryptography;
using NHibernate.Criterion;

namespace Desafio_MB_Core.Controllers
{
    public static class ApiDesafioMBConstants
    {
        // Definições.

        public static int LOGIN_TYPE_PASSWORD = 1;
        public static int LOGIN_TYPE_LINKEDIN = 2;
        public static int LOGIN_TYPE_FACEBOOK = 3;
        /* Tipo de Login
        1 - Login/Senha
        2 - Linkedin
        3 - Facebook
        */

        public static int ACCESS_LEVEL_NONE = 0;
        public static int ACCESS_LEVEL_VISITOR = 1;
        public static int ACCESS_LEVEL_ADMIN_LOW = 2;
        public static int ACCESS_LEVEL_ADMIN_HIGH = 3;
        public static int ACCESS_LEVEL_MANAGER_LOW = 4;
        public static int ACCESS_LEVEL_MANAGER_HIGH = 5;
        /* Tipo de Acesso
        1 - Visitante:
        2 - Administrador Nível 1:
        3 - Administrador Nível 2:
        4 - Gerente de Eventos Nível 1:
        5 - Gerente de Eventos Nível 2:
        */

        public static int SEX_FEMALE = 1;
        public static int SEX_MALE = 2;
        /* Sexo
        1 - Feminino
        2 - Masculino
        */

        public static int STATE_ACTIVE = 1;
        public static int STATE_DEACTIVATED = 0;
        /* Desativado ou ativo.
        1 - Ativo
        0 - Desativado
        */

        public static int TICKET_PAID = 1;
        public static int TICKET_USED = 2;
        /* Validade de Ingresso
        1 - Pago
        2 - Utilizado
        */

        public static int MAX_RECOMENDATION = 5;
        //Máxima quantida de recomendações.

        public static int SUBORDINATION_NONE = 0;
        //Usuário sem subordinação
    }


    public class ApiDesafioMB
    {
        // Definições

        private int _SESSION_ID_SIZE = 10;
        private Configuration _cfg;
        private ISessionFactory _sessionFactory;
        
        /// <summary>
        /// Cria e mantem conexões com o BD, além das funcionalidades do projeto em cima destas.
        /// </summary>
        public ApiDesafioMB()
        {
            _cfg = new Configuration();
            _cfg.Configure();
            _cfg.AddAssembly(Assembly.GetExecutingAssembly());
            _sessionFactory = _cfg.BuildSessionFactory();
        }

        // Sessão Processo 1 - Usuário

        /// <summary>
        /// Obtem uma lista de usuários, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public List<Usuario> GetUsers(string sessionId)
        {
            List<Usuario> result = new List<Usuario>();
            Usuario usuario = GetUserBySessionID(sessionId);

            //1 - Visitante:
            //Só pode acessar a sí próprio.
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                result.Add(usuario);
                return result;
            }
            //2 - Administrador Nível 1:
            //Todos os gerentes
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>()
                            .Where(Restrictions.Or(
                                Restrictions.Where<Usuario>(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH),
                                Restrictions.Where<Usuario>(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW))
                                )
                            .Where(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
                            .List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //3 - Administrador Nível 2:
            //Todos os usuários.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>().List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //4 - Gerente de Eventos Nível 1:
            //Ninguem.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                return result;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.Subordinado == usuario.Id).List();
                        if (list.Count == 1)
                            result = list.ToList();
                        tx.Commit();
                    }

                    session.Close();
                }
                return result;
            }
            else
                return result;
        }

        /// <summary>
        /// Obtem lista de usuários atívos, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public List<Usuario> GetActiveUsers(string sessionId)
        {
            List<Usuario> result = new List<Usuario>();
            Usuario usuario = GetUserBySessionID(sessionId);

            //1 - Visitante:
            //Só pode acessar a sí próprio.
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                if (usuario.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE)
                    result.Add(usuario);
                return result;
            }
            //2 - Administrador Nível 1:
            //Todos os gerentes
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>()
                            .Where(Restrictions.Or(
                                Restrictions.Where<Usuario>(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH),
                                Restrictions.Where<Usuario>(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW))
                                )
                            .And(u => u.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE)
                            .Where(u => u.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
                            .List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //3 - Administrador Nível 2:
            //Todos os usuários.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE).List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //4 - Gerente de Eventos Nível 1:
            //Ninguem.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                return result;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.Subordinado == usuario.Id)
                            .And(u => u.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE).List();
                        if (list.Count == 1)
                            result = list.ToList();
                        tx.Commit();
                    }

                    session.Close();
                }
                return result;
            }
            else
                return result;
        }

        /// <summary>
        /// Cria um Usuário, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="newUsuario"></param>
        /// <returns></returns>
        public int CreateUser(string sessionId, Usuario newUsuario)
        {
            //Login não pode duplicar,
            //Gerar sem sessão,
            //id não importa.

            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            // 1 OK


            int result = -1;
            Usuario usuario = GetUserBySessionID(sessionId);
            if (usuario == null)
                return -2;

            if (!IsUniqueLogin(newUsuario.Login))
                return -3;

            //Não permitir login vazio.
            if (newUsuario.Login == null)
                return -4;
            else if (newUsuario.Login == "")
                return -4;

            if (newUsuario.Senha == null)
                return -4;
            else if (newUsuario.Senha == "")
                return -4;

            //Garantir criação de usuário limpa, sem erros.
            newUsuario.Id = 0;
            newUsuario.SessionId = "";
            newUsuario.Subordinado = ApiDesafioMBConstants.SUBORDINATION_NONE;
            newUsuario.ContaAtiva = ApiDesafioMBConstants.STATE_ACTIVE;

            //1 - Visitante:
            //Só pode criar em /usuarios/visitante
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Cria gerentes
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                if (newUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH && newUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                    return -2;

                SaveUser(newUsuario);
                return 1;
            }
            //3 - Administrador Nível 2:
            //Todos os usuários.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                SaveUser(newUsuario);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Ninguem.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                return -2;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                if (newUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                {
                    newUsuario.Subordinado = usuario.Id;
                    SaveUser(newUsuario);
                    return 1;
                }
            }
            else
                return -2;

            return result;
        }

        /// <summary>
        /// Atualiza um Usuário, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="newUsuario"></param>
        /// <returns></returns>
        public int UpdateUser(string sessionId, Usuario newUsuario)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            //-5 Usuário não existe.
            // 1 OK

            Usuario oldUsuario = null;
            int result = -1;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.Id == newUsuario.Id).List();
                    if (list.Count == 1)
                        oldUsuario = list[0];
                    else
                        result = -5;
                    tx.Commit();
                }

                session.Close();
            }
            if (result == -5)
                return result;

            Usuario usuario = GetUserBySessionID(sessionId);

            if (newUsuario.Login != oldUsuario.Login)
            {
                if (!IsUniqueLogin(newUsuario.Login))
                    return -3;

                //Não permitir login vazio.
                if (newUsuario.Login == null)
                    return -4;
                else if (newUsuario.Login == "")
                    return -4;

                if (newUsuario.Senha == null)
                    return -4;
                else if (newUsuario.Senha == "")
                    return -4;
            }

            /*
            newUsuario.SessionId = "";
            newUsuario.Subordinado = 0;
            newUsuario.ContaAtiva = 1;
            */

            //1 - Visitante:
            //Só pode criar em /usuarios/visitante
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Cria gerentes
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                if (oldUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH || oldUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                {
                    newUsuario.TipoAcesso = oldUsuario.TipoAcesso;
                    SaveUser(newUsuario);
                    return 1;
                }
                else
                    return -2;

            }
            //3 - Administrador Nível 2:
            //Todos os usuários.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                SaveUser(newUsuario);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Ninguem.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                return -2;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                if (oldUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                {
                    newUsuario.TipoAcesso = oldUsuario.TipoAcesso;
                    //Garantir que a subordinação é identica.
                    newUsuario.Subordinado = oldUsuario.Subordinado;
                    SaveUser(newUsuario);
                    return 1;
                }
                else
                {
                    //Não mostrar que o usuário existe, por segurança.
                    return -5;
                }
            }
            else
                return -2;

            return result;
        }

        /// <summary>
        /// Desabilita um Usuário, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public int DisableUser(string sessionId, int usuarioId)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Login já existe.
            //-4 Login inválido.
            //-5 Usuário não existe.
            // 1 OK

            Usuario oldUsuario = null;
            int result = -1;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.Id == usuarioId).List();
                    if (list.Count == 1)
                        oldUsuario = list[0];
                    else
                        result = -5;
                    tx.Commit();
                }

                session.Close();
            }
            if (result == -5)
                return result;

            Usuario usuario = GetUserBySessionID(sessionId);

            /*
            newUsuario.SessionId = "";
            newUsuario.Subordinado = 0;
            newUsuario.ContaAtiva = 1;
            */

            //1 - Visitante:
            //Só pode criar em /usuarios/visitante
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Cria gerentes
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                if (oldUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH && oldUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                    return -2;
                oldUsuario.ContaAtiva = ApiDesafioMBConstants.STATE_DEACTIVATED;
                oldUsuario.SessionId = null;
                SaveUser(oldUsuario);
                return 1;
            }
            //3 - Administrador Nível 2:
            //Todos os usuários.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                oldUsuario.ContaAtiva = ApiDesafioMBConstants.STATE_DEACTIVATED;
                oldUsuario.SessionId = null;
                SaveUser(oldUsuario);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Ninguem.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                return -2;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                if (oldUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW && oldUsuario.Subordinado == usuario.Id)
                {
                    oldUsuario.ContaAtiva = ApiDesafioMBConstants.STATE_DEACTIVATED;
                    oldUsuario.SessionId = null;
                    SaveUser(oldUsuario);
                    return 1;
                }
            }
            else
                return -2;

            return result;
        }

        /// <summary>
        /// Insere/Atualiza dados do Usuário
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public bool SaveUser(Usuario newUser)
        {
            bool isValid = false;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    if (newUser.Id > 0)
                    {
                        session.Update(newUser);
                    }
                    else
                    {
                        session.Save(newUser);
                    }
                    tx.Commit();
                    isValid = true;
                }

                session.Close();
            }
            return isValid;
        }

        // Sessão Processo 2 - Evento

        /// <summary>
        /// Retorna uma lista de eventos, baseado na sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public List<Evento> GetEventos(string sessionId)
        {
            List<Evento> result = new List<Evento>();
            Usuario usuario = GetUserBySessionID(sessionId);

            //1 - Visitante:
            //Visualiza todos os eventos ativos e que ainda não ocorreram.
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Evento> list = session.QueryOver<Evento>()
                            .Where(e => e.EventoAtivo == ApiDesafioMBConstants.STATE_ACTIVE)
                            .And(e => e.DataEvento > DateTime.Now)
                            .List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //2 - Administrador Nível 1:
            //Não visualiza eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                return result;
            }
            //3 - Administrador Nível 2:
            //visualiza todos os eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Evento> list = session.QueryOver<Evento>()
                            .List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //4 - Gerente de Eventos Nível 1:
            //Visualiza Eventos gerados por este.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Evento> list = session.QueryOver<Evento>()
                            .Where(e => e.IdOrganizador == usuario.Id)
                            .List();
                        if (list.Count >= 1)
                            result = list.ToList();
                        tx.Commit();
                    }
                    session.Close();
                }
                return result;
            }
            //5 - Gerente de Eventos Nível 2:
            //Visualiza os eventos gerados por este e seus subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                //Aparentemente o NHibernate não fuciona muito bem com o SQL 'in' statement,
                // então faço uma busca pelos usuários subordinados ao gerente N2 e procuro
                // os eventos de cada um deles.

                List<Usuario> gerentesN1 = GetGerentesNivel1ByUsuario(usuario);

                gerentesN1.Add(usuario);

                foreach(Usuario u in gerentesN1)
                {
                    using (var session = _sessionFactory.OpenSession())
                    {
                        using (var tx = session.BeginTransaction())
                        {
                            IList<Evento> list = session.QueryOver<Evento>()
                                .Where(e => e.IdOrganizador == u.Id)
                                .List();
                            if (list.Count >= 1)
                                result.AddRange(list.ToList());
                            tx.Commit();
                        }
                        session.Close();
                    }
                }

                
                return result;
            }
            else
                return result;
        }
        
        /// <summary>
        /// Cria um Evento, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="newEvento"></param>
        /// <returns></returns>
        public int CreateEvento(string sessionId, Evento newEvento)
        {

            //-1 Erro
            //-2 Permissão errada.
            //-3 Data inválida.
            //-4 Preço inválido.
            //-5 Nome vazio. (Removido p/ teste)
            //-6 Descrição vazia. (Removido p/ teste)
            // 1 OK


            int result = -1;
            Usuario usuario = GetUserBySessionID(sessionId);

            if (usuario == null)
                return -2;

            if (newEvento.DataEvento < DateTime.Now)
                return -3;

            if (newEvento.Preco > 9999999999.99 || newEvento.Preco < 0)
                return -4;

            /* -5 Não permitir nome vazio.
            if (newEvento.NomeEvento == null)
                return -5;
            else
            {
                if (newEvento.NomeEvento.Length == 0)
                    return -5;
            }
            */

            /* -6 Não permitir Descrição vazia.
            if (newEvento.Descricao == null)
                return -6;
            else
            {
                if (newEvento.Descricao.Length == 0)
                    return -6;
            }
            */

            //Garantir criação de evento limpo, sem erros.
            newEvento.Id = 0;
            newEvento.QuantidadeVendida = 0;
            newEvento.EventoAtivo = ApiDesafioMBConstants.STATE_ACTIVE;

            //1 - Visitante:
            //Não cria Evento.
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Não cria Evento.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                return -2;
            }
            //3 - Administrador Nível 2:
            //Cria, Altera e Desativa Eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                SaveEvento(newEvento);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Cria e Altera eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                newEvento.IdOrganizador = usuario.Id;
                SaveEvento(newEvento);
                return 1;
            }
            //5 - Gerente de Eventos Nível 2:
            //Cria, Altera e Desativa Eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                newEvento.IdOrganizador = usuario.Id;
                SaveEvento(newEvento);
                return 1;
            }
            else
                return -2;

            return result;
        }

        /// <summary>
        /// Altera um Evento, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="newEvento"></param>
        /// <returns></returns>
        public int UpdateEvento(string sessionId, Evento newEvento)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Data inválida.
            //-4 Preço inválido.
            //-5 Nome vazio. (Removido p/ teste)
            //-6 Descrição vazia. (Removido p/ teste)
            //-7 Evento não existe.
            // 1 OK


            int result = -1;
            Usuario usuario = GetUserBySessionID(sessionId);

            if (usuario == null)
                return -2;

            if (newEvento.DataEvento < DateTime.Now)
                return -3;

            if (newEvento.Preco > 9999999999.99 || newEvento.Preco < 0)
                return -4;

            /* -5 Não permitir nome vazio.
            if (newEvento.NomeEvento == null)
                return -5;
            else
            {
                if (newEvento.NomeEvento.Length == 0)
                    return -5;
            }
            */

            /* -6 Não permitir Descrição vazia.
            if (newEvento.Descricao == null)
                return -6;
            else
            {
                if (newEvento.Descricao.Length == 0)
                    return -6;
            }
            */


            Evento oldEvento = null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Evento> list = session.QueryOver<Evento>().Where(e => e.Id == newEvento.Id).List();
                    if (list.Count == 1)
                        oldEvento = list[0];
                    else
                        result = -7;
                    tx.Commit();
                }

                session.Close();
            }
            if (result == -7)
                return result;

            
            //1 - Visitante:
            //Não altera Evento
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Não altera Evento
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                return -2;
            }
            //3 - Administrador Nível 2:
            //Altera todos os Eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                SaveEvento(newEvento);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Altera seus próprios eventos.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                if (oldEvento.IdOrganizador == usuario.Id && newEvento.IdOrganizador == oldEvento.IdOrganizador)
                {
                    SaveEvento(newEvento);
                    return 1;
                }

                //Se não for encontrado um gerente para o evento não mostrar que o Evento existe, por segurança.
                return -7;
            }
            //5 - Gerente de Eventos Nível 2:
            //Gerentes Nível 1 subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                //Aparentemente o NHibernate não fuciona muito bem com o SQL 'in' statement,
                // então faço uma busca pelos usuários subordinados ao gerente N2 e procuro
                // os eventos de cada um deles.

                List<Usuario> gerentesN1 = GetGerentesNivel1ByUsuario(usuario);
                gerentesN1.Add(usuario);

                foreach(Usuario u in gerentesN1)
                {
                    if (u.Id == oldEvento.IdOrganizador)
                    {
                        //Garantir que o organizador não muda.
                        newEvento.IdOrganizador = oldEvento.IdOrganizador;
                        SaveEvento(newEvento);
                        return 1;
                    }
                }

                //Se não for encontrado um gerente para o evento não mostrar que o Evento existe, por segurança.
                return -7;
            }
            else
                return -2;

            return result;
        }

        /// <summary>
        /// Desabilita um Evento, baseado no nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        public int DisableEvento(string sessionId, int eventoId)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-5 Evento não existe.
            // 1 OK

            Evento oldEvento = null;
            int result = -1;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Evento> list = session.QueryOver<Evento>().Where(u => u.Id == eventoId).List();
                    if (list.Count == 1)
                        oldEvento = list[0];
                    else
                        result = -5;
                    tx.Commit();
                }

                session.Close();
            }
            if (result == -5)
                return result;

            Usuario usuario = GetUserBySessionID(sessionId);

            //1 - Visitante:
            //Não desativa Evento.
            if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
            {
                return -2;
            }
            //2 - Administrador Nível 1:
            //Não desativa Evento.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_LOW)
            {
                return -2;
            }
            //3 - Administrador Nível 2:
            //Desativa qualquer Evento.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                oldEvento.EventoAtivo = ApiDesafioMBConstants.STATE_DEACTIVATED;
                SaveEvento(oldEvento);
                return 1;
            }
            //4 - Gerente de Eventos Nível 1:
            //Desativa o próprio Evento.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                if (oldEvento.IdOrganizador == usuario.Id)
                {
                    oldEvento.EventoAtivo = ApiDesafioMBConstants.STATE_DEACTIVATED;
                    SaveEvento(oldEvento);
                    return 1;
                }
                else
                    //Se não for encontrado um gerente para o evento não mostrar que o Evento existe, por segurança.
                    return -5;
            }
            //5 - Gerente de Eventos Nível 2:
            //Desativa o próprio evento e os de seus subordinados.
            else if (usuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                List<Usuario> gerentesN1 = GetGerentesNivel1ByUsuario(usuario);
                gerentesN1.Add(usuario);

                foreach (Usuario u in gerentesN1)
                {
                    if (u.Id == oldEvento.IdOrganizador)
                    {
                        oldEvento.EventoAtivo = ApiDesafioMBConstants.STATE_DEACTIVATED;
                        SaveEvento(oldEvento);
                        return 1;
                    }
                }

                //Se não for encontrado um gerente para o evento não mostrar que o Evento existe, por segurança.
                return -5;
            }
            else
                return -2;

            //return result;
        }

        /// <summary>
        /// Insere/Atualiza dados do Evento
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public bool SaveEvento(Evento newEvento)
        {
            bool isValid = false;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    if (newEvento.Id > 0)
                    {
                        session.Update(newEvento);
                    }
                    else
                    {
                        session.Save(newEvento);
                    }
                    tx.Commit();
                    isValid = true;
                }

                session.Close();
            }
            return isValid;
        }

        /// <summary>
        /// Descobrir se um Visitante já possui um ingresso para o Evento.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        public Ingresso GetIngressoEventoByVisitante(string sessionId, Evento evento)
        {
            Ingresso result = null;
            Usuario oldUsuario = GetUserBySessionID(sessionId);

            //Somente Visitantes podem comprar ingresso
            if (oldUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
                return null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Ingresso> list = session.QueryOver<Ingresso>()
                        .Where(i => i.IdUsuario == oldUsuario.Id)
                        .And(i => i.IdEvento == evento.Id)
                        .List();
                    if (list.Count == 1)
                        result = list[0];
                    tx.Commit();
                }

                session.Close();
            }

            return result;
        }

        // Sessão Processo 3 - Validar Participação

        /// <summary>
        /// Valida o ingresso de um Visitante para uma sessão de Gerente de Evento.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="ingressoId"></param>
        /// <returns></returns>
        public int ValidateIngresso(string sessionId, int ingressoId)
        {
            Usuario mngUsuario = null;
            Evento mngEvento = null;
            Ingresso mngIngresso = null;
            int result = -1;

            //-2 Usuario não encontrado.
            //-3 ingresso não encontrado.
            //-4 Evento não encontrado.

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> listUsuarios = session.QueryOver<Usuario>().Where(u => u.SessionId == sessionId)
                        .And(u => u.ContaAtiva == 1)
                        .List();
                    if (listUsuarios.Count == 1)
                    {
                        mngUsuario = listUsuarios[0];
                    }
                    else
                        result = -2;

                    tx.Commit();
                }
                session.Close();
            }

            if (result == -2)
                return result;

            if (mngUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH &&
                mngUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH &&
                mngUsuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
                // Por segurança não mostrar que o ingresso existe.
                return -3;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Ingresso> listIngresso = session.QueryOver<Ingresso>().Where(i => i.Id == ingressoId)
                        .And(i => i.Validade == ApiDesafioMBConstants.TICKET_PAID)
                        .And(i => i.ValidoAte > DateTime.Now)
                        .List();
                    if (listIngresso.Count == 1)
                    {
                        mngIngresso = listIngresso[0];
                    }
                    else
                        result = -3;

                    tx.Commit();
                }
                session.Close();
            }

            if (result == -3)
                return result;

            //Descobrir se o usuário da sessão tem permissão para validar o ingresso apresentado.
            //Deve ser ou o gerente que criou o evento ou subordinado de um mesmo gerente Nível2.
            //ou Admin nível 2.

            if (mngUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_ADMIN_HIGH)
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Evento> listEvento = session.QueryOver<Evento>().Where(e => e.Id == mngIngresso.IdEvento)
                            .And(e => e.EventoAtivo == ApiDesafioMBConstants.STATE_ACTIVE)
                            .List();
                        if (listEvento.Count == 1)
                        {
                            mngEvento = listEvento[0];
                        }
                        else
                            result = -4;

                        tx.Commit();
                    }
                    session.Close();
                }
                if (result == -4)
                    return result;

                mngIngresso.Validade = ApiDesafioMBConstants.TICKET_USED;
                SaveIngresso(mngIngresso);
                return 1;
            }

            List<Usuario> gerentes = new List<Usuario>();
            Usuario gerenteN2 = null;
            int subordinacao = 0;

            if (mngUsuario.Subordinado != 0 && mngUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_LOW)
            {
                subordinacao = mngUsuario.Subordinado;
                gerenteN2 = GetUserById(subordinacao);
                gerentes.Add(gerenteN2);

                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> listUsuarios = session.QueryOver<Usuario>().Where(u => u.Subordinado == gerenteN2.Id)
                            .And(u => u.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE)
                            .List();
                        if (listUsuarios.Count >= 1)
                        {
                            gerentes.AddRange(listUsuarios);
                        }

                        tx.Commit();
                    }
                    session.Close();
                }

            }
            else if (mngUsuario.TipoAcesso == ApiDesafioMBConstants.ACCESS_LEVEL_MANAGER_HIGH)
            {
                gerentes.Add(mngUsuario);

                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> listUsuarios = session.QueryOver<Usuario>().Where(u => u.Subordinado == mngUsuario.Id)
                            .And(u => u.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE)
                            .List();
                        if (listUsuarios.Count >= 1)
                        {
                            gerentes.AddRange(listUsuarios);
                        }

                        tx.Commit();
                    }
                    session.Close();
                }
            }

            bool isValid = false;

            foreach(Usuario g in gerentes)
            {
                if (mngIngresso != null)
                {
                    using (var session = _sessionFactory.OpenSession())
                    {
                        using (var tx = session.BeginTransaction())
                        {
                            IList<Evento> listEvento = session.QueryOver<Evento>().Where(e => e.Id == mngIngresso.IdEvento)
                                .And(e => e.IdOrganizador == g.Id)
                                .And(e => e.EventoAtivo == ApiDesafioMBConstants.STATE_ACTIVE)
                                .List();
                            if (listEvento.Count == 1)
                            {
                                mngEvento = listEvento[0];
                                isValid = true;
                            }

                            tx.Commit();
                        }
                        session.Close();
                    }
                }
            }
            
            //se nenhum gerente é responsavel pelo evento, mostrar que o evento existe, por segurança.
            if (!isValid)
                return -4;
            else
            {
                mngIngresso.Validade = ApiDesafioMBConstants.TICKET_USED;
                SaveIngresso(mngIngresso);
                return 1;
            }

            //return result;
        }

        // Sessão Processo 4 - Login

        /// <summary>
        /// Login de Usuario, cria uma sessão.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="senha"></param>
        /// <returns>sessionId</returns>
        public string LoginUser(string usuario, string senha)
        {
            string sessionId = "";

            //TODO: Conectar com servidor facebook/linkedin e requerir token de acesso permitido.

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> listUsuarios = session.QueryOver<Usuario>().Where(i => i.Login == usuario)
                        .And(i => i.Senha == senha)
                        //Conta desativada não pode mais fazer login.
                        .And(i => i.ContaAtiva == ApiDesafioMBConstants.STATE_ACTIVE)
                        .List();
                    if (listUsuarios.Count == 1)
                    {
                        Usuario u = listUsuarios[0];
                        sessionId = GetSessionID();
                        u.SessionId = sessionId;
                        session.Update(u);
                    }

                    tx.Commit();
                }

                session.Close();
            }
            return sessionId;
        }

        /// <summary>
        /// Efetua o Logout da sessão, remove a sessão do BD.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public int LogoutUser(string sessionId)
        {
            int result = -1;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> listUsuarios = session.QueryOver<Usuario>().Where(i => i.SessionId == sessionId)
                        .List();
                    if (listUsuarios.Count == 1)
                    {
                        result = 1;
                        Usuario u = listUsuarios[0];
                        u.SessionId = null;
                        session.Update(u);
                    }

                    tx.Commit();
                }

                session.Close();
            }
            return result;
        }

        // Sessão Processo 5 - Ingresso

        /// <summary>
        /// Efetua a compra do Ingresso para o usuário, baseado na permissão da sessão, id de evento e no cvv enviado pelo usuário.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="eventoid"></param>
        /// <param name="cvv"></param>
        /// <returns></returns>
        public int BuyIngresso(string sessionId, int eventoid, string cvv)
        {
            //-1 Erro
            //-2 Permissão errada.
            //-3 Evento não existe.
            //-4 Ingresso já existe.
            // 1 OK

            int result = -1;
            Usuario usuario = GetUserBySessionID(sessionId);

            //Somente Visitantes podem comprar Ingressos.
            if (usuario.TipoAcesso != ApiDesafioMBConstants.ACCESS_LEVEL_VISITOR)
                return -2;

            Evento evento = GetEventoById(eventoid);

            if (evento == null)
                return -3;
            else if (evento.EventoAtivo == ApiDesafioMBConstants.STATE_DEACTIVATED || evento.DataEvento < DateTime.Now)
                return -3;

            Ingresso ingresso = null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Ingresso> list = session.QueryOver<Ingresso>().Where(i => i.IdUsuario == usuario.Id)
                        .And(i => i.IdEvento == evento.Id)
                        .List();
                    if (list.Count == 1)
                    {
                        result = -4;
                    }
                    else
                    {
                        ingresso = new Ingresso();
                        ingresso.Id = 0;
                        ingresso.IdEvento = evento.Id;
                        ingresso.IdUsuario = usuario.Id;
                        ingresso.Validade = ApiDesafioMBConstants.TICKET_PAID;
                        ingresso.ValidoAte = evento.DataEvento;
                        ingresso.NotaFiscalPath = PayForIngresso(evento.Preco, cvv, usuario);
                        ingresso.DataPagamento = DateTime.Now;

                        SaveIngresso(ingresso);

                        evento.QuantidadeVendida = evento.QuantidadeVendida + 1;
                        SaveEvento(evento);

                        result = 1;
                    }

                    tx.Commit();
                }

                session.Close();
            }

            return result;
        }

        /// <summary>
        /// STUB para a compra utilizando o cartão de crédito.
        /// </summary>
        /// <param name="preco"></param>
        /// <param name="cvv"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string PayForIngresso(Double preco, string cvv, Usuario usuario)
        {
            // Efetuar a compra usando os dados de cartão de crédito do usuário
            // junto com o cvv do cartão que não pode ser inserido em BD, por segurança.
            // A compra gera uma nota fiscal eletrónica em formato XML, que deve ser
            // armazenada para comprovar a compra e para fins fiscais.

            return "d:\\notafiscal.xml";
        }

        /// <summary>
        /// Salva novo Ingresso no BD.
        /// </summary>
        /// <param name="newIngresso"></param>
        /// <returns></returns>
        public bool SaveIngresso(Ingresso newIngresso)
        {
            bool isValid = false;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    if (newIngresso.Id > 0)
                        session.Update(newIngresso);
                    else
                        session.Save(newIngresso);
                    tx.Commit();
                    isValid = true;
                }

                session.Close();
            }
            return isValid;
        }

        // Sessão Processo 6 - Recomendação

        /// <summary>
        /// Obtem recomendações para um Usuário, baseado na sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public List<Evento> GetRecommendation(string sessionId)
        {
            List<Evento> result = new List<Evento>();

            //TODO: Critério que personaliza a busca baseado no usuário.

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Evento> listEvento = session.QueryOver<Evento>()
                        .Where(e => e.DataEvento > DateTime.Now)
                        .And(e => e.EventoAtivo == ApiDesafioMBConstants.STATE_ACTIVE)
                        .OrderBy(e => e.Id).Desc
                        .List();
                    if (listEvento.Count > 1)
                    {
                        int j = ApiDesafioMBConstants.MAX_RECOMENDATION;
                        foreach (Evento k in listEvento)
                        {
                            j--;
                            result.Add(k);
                            if (j <= 0)
                                break;
                        }
                    }

                    tx.Commit();
                }
                session.Close();
            }
            return result;
        }

        // Sessão Utils

        /// <summary>
        /// Retorna uma SessionID válida para um usuário utilizar.
        /// </summary>
        /// <returns>string: SessionId</returns>
        public string GetSessionID()
        {
            string sessionId = "";
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int length = _SESSION_ID_SIZE;
            StringBuilder res = new StringBuilder();
            bool isValid = false;

            while (!isValid)
            {
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] uintBuffer = new byte[sizeof(uint)];

                    while (length-- > 0)
                    {
                        rng.GetBytes(uintBuffer);
                        uint num = BitConverter.ToUInt32(uintBuffer, 0);
                        res.Append(valid[(int)(num % (uint)valid.Length)]);
                    }
                }

                sessionId = res.ToString();

                using (var session = _sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        IList<Usuario> usuarios = session.Query<Usuario>().Where(i => i.SessionId == sessionId).ToList();
                        if (usuarios.Count == 0)
                            isValid = true;
                    }
                    session.Close();
                }
            }

            return sessionId;
        }

        /// <summary>
        /// Verifica se o login do usuário é unico.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool IsUniqueLogin(string login)
        {
            bool result = false;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.Login == login).List();
                    if (list.Count == 0)
                        result = true;
                    tx.Commit();
                }

                session.Close();
            }
            return result;
        }

        /// <summary>
        /// Verifica se já existe uma sessão aberta com este parâmetro. True se existe.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool IsSessionOpen (string sessionId)
        {
            bool result = false;

            if (sessionId == null)
                return false;

            if (sessionId.Length == 0)
                return false;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.SessionId == sessionId).List();
                    if (list.Count == 1)
                        result = true;
                    tx.Commit();
                }

                session.Close();
            }
            return result;
        }

        /// <summary>
        /// Get Usuários que estão utilizando essa id de sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Usuario GetUserBySessionID(string sessionId)
        {
            Usuario result = null;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.SessionId == sessionId).List();
                    if (list.Count == 1)
                        result = list[0];
                    tx.Commit();
                }

                session.Close();
            }
            return result;
        }

        /// <summary>
        /// Obtem o nível de acesso da sessão.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public int GetAccessBySessionID(string sessionId)
        {
            int result = 0;
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(u => u.SessionId == sessionId).List();
                    if (list.Count == 1)
                        result = list[0].TipoAcesso;
                    tx.Commit();
                }

                session.Close();
            }
            return result;
        }

        /// <summary>
        /// Obter os gerentes subordinados a um Gerente de Eventos Nivel 2.
        /// </summary>
        /// <param name="gerenteN2"></param>
        /// <returns></returns>
        public List<Usuario> GetGerentesNivel1ByUsuario (Usuario gerenteN2)
        {
            List<Usuario> result = new List<Usuario>();

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>()
                        .Where(u => u.Subordinado == gerenteN2.Id)
                        .List();
                    if (list.Count >= 1)
                        result = list.ToList();
                    tx.Commit();
                }
                session.Close();
            }

            return result;
        }

        /// <summary>
        /// Obtem um Evento pela sua Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Evento GetEventoById (int id)
        {
            Evento result = null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Evento> list = session.QueryOver<Evento>().Where(i => i.Id == id)
                        .List();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }

                    tx.Commit();
                }

                session.Close();
            }

            return result;
        }

        /// <summary>
        /// Obtem um Ingresso pela sua Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ingresso GetIngressoById (int id)
        {
            Ingresso result = null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Ingresso> list = session.QueryOver<Ingresso>().Where(i => i.Id == id)
                        .List();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }

                    tx.Commit();
                }

                session.Close();
            }

            return result;
        }

        /// <summary>
        /// Obtem um Usuario pela sua Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario GetUserById(int id)
        {
            Usuario result = null;

            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Usuario> list = session.QueryOver<Usuario>().Where(i => i.Id == id)
                        .List();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }

                    tx.Commit();
                }

                session.Close();
            }

            return result;
        }

    }
}
