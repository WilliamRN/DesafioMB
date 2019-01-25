public class TestPost
    {
        public string name { get; set; }
    }


--http://localhost:57623/desafiomb/usuario/visitante

nomes em ingles, no código,
portugues para usuário e api
ni45ZVMZCR

--testar ingresso
--testar evento  
--Sessão expirada em cada controller para sessão = null
referencias para cada get/post



SQL users

select (id,login,senha,tipo_acesso,session_id,subordinado) from usuarios

/* Tipo de Acesso
	1 - Visitante:
	2 - Administrador Nível 1:
	3 - Administrador Nível 2:
	4 - Gerente de Eventos Nível 1:
	5 - Gerente de Eventos Nível 2:
	*/
	
	
	
//Temp dados

{
	"sessionid" : "UONKkByVWq",
	"usrId" : 16
}

{
	
}

id,activo,access




// 4;
// 3;
// 5;
// 1;
// "2019-09-01 17:00:00";
// "d:\\notafiscal.xml";
// "2018-12-01 17:00:00"
	
	

//SQLS

--select (id,conta_ativa,login,senha,tipo_acesso,session_id,subordinado) from usuarios order by id

select (id,conta_ativa,tipo_acesso,login,session_id) from usuarios order by id DESC

--delete from usuarios where id = 15
		

"(7,1,4,celestinop,kjcWnnzcbD)"
"(6,1,2,orianab,JggKCG7SeY)"
"(5,1,1,solomeb,z2oYuXuRrr)"
"(4,1,1,teresac,wAuHRjPraD)"
"(3,1,5,edgarf,y78L7f1VF1)"
"(2,1,5,eloib,R8HsbHQkuP)"
"(1,1,3,admin,Az3bUn5D6Z)"




-- SQL


insert into ingressos values (1,1,1,1,'2019-01-17 20:30:00',"d:\\",'2019-01-17 20:30:00')

insert into ingressos values (nextval('seq_ingresso_id'),1,2,1,'2019-01-17 20:30:00',"d:\\",current_timestamp)
values(,'text')

select * from ingressos

select current_timestamp

create sequence seq_ingressos_id increment by 1 minvalue 1 no maxvalue start with 1;
create sequence seq_eventos_id increment by 1 minvalue 1 no maxvalue start with 1;
create sequence seq_usuario_id increment by 1 minvalue 1 no maxvalue start with 1;

select setval('seq_ingresso_id', 1, true)

select * from seq_ingresso_id


/* tipo de Login
1 - Login/Senha
2 - Linkedin
3 - Facebook
*/

/* tipo de Acesso
1 - Visitante:
2 - Administrador Nível 1:
3 - Administrador Nível 2:
4 - Gerente de Eventos Nível 1:
5 - Gerente de Eventos Nível 2:
*/




-- Dados

  [Datas]
'2019-02-06 00:00:00'
'2019-03-12 00:00:00'
'2019-04-22 00:00:00'
'2019-05-12 00:00:00'

'1982-01-06 00:00:00'
'1984-03-14 00:00:00'
'1982-05-22 00:00:00'
'1985-07-14 00:00:00'
'1989-08-08 00:00:00'


CEP:29180084
Endere:Avenida Castro Alves
Bairro:Belvedere
Cidade:Serra
Estado:ES

CEP:75053757
Endere:Rua Senador Paranhos
Bairro:Loteamento Guanabara
Cidade:Anápolis
Estado:GO

CEP:30642440
Endere:Rua Baltazar Pyramo
Bairro:Santa Helena (Barreiro)
Cidade:Belo Horizonte
Estado:MG

CEP:69036668
Endere:Rua Seringal
Bairro:Santo Agostinho
Cidade:Manaus
Estado:AM

-- Notepad




-- Celestino Pontes (celestinop/123456) - Gerente de Eventos Nível 1 (access4)
INSERT INTO usuarios (id, rg, cpf, conta_ativa, nome, endereco_rua, endereco_bairro, endereco_complemento,
 endereco_cep, endereco_cidade, endereco_estado, endereco_pais, email, data_nascimento, sexo,
 tipo_login, imagem_profile_path, last_token, login, senha, tipo_acesso, numero_cartao,
 nome_titular, data_exp, tipo_cartao, endereco_cobrabca_rua, endereco_cobrabca_bairro,
 endereco_cobrabca_complemento, endereco_cobrabca_cep, endereco_cobrabca_cidade,
 endereco_cobrabca_estado, endereco_cobrabca_pais)
VALUES (nextval('seq_usuario_id'),'297562587','69571152080',1,'Celestino Pontes','Rua Bernardo Horta','Zumbi','','29302235',
'Cachoeiro de Itapemirim','ES','Brasil','celestinop@mail.com','1973-08-02 00:00:00',2,1,'','','celestinop','123456',4,'7777111177774444',
'CELESTINO PONTES','2020-09-01 00:00:00',1,'Rua Bernardo Horta','Zumbi','','29302235',
'Cachoeiro de Itapemirim','ES','Brasil');



CREATE TABLE public.usuarios
(
  id integer NOT NULL,
  rg character varying(11),
  cpf character varying(11),
  conta_ativa integer,
  nome character varying(300),
  endereco_rua character varying(100),
  endereco_bairro character varying(100),
  endereco_complemento character varying(100),
  endereco_cep character varying(16),
  endereco_cidade character varying(100),
  endereco_estado character varying(100),
  endereco_pais character varying(100),
  email character varying(320),
  data_nascimento timestamp without time zone,
  sexo integer,
  tipo_login integer,
  imagem_profile_path character varying(260),
  last_token character varying(1000),
  login character varying(100),
  senha character varying(100),
  tipo_acesso integer,
  numero_cartao character varying(19),
  nome_titular character varying(100),
  data_exp timestamp without time zone,
  tipo_cartao integer,
  endereco_cobrabca_rua character varying(100),
  endereco_cobrabca_bairro character varying(100),
  endereco_cobrabca_complemento character varying(100),
  endereco_cobrabca_cep character varying(16),
  endereco_cobrabca_cidade character varying(100),
  endereco_cobrabca_estado character varying(100),
  endereco_cobrabca_pais character varying(100),
  CONSTRAINT usuarios_pkey PRIMARY KEY (id)
  
  
  



-- Evento 1 desativado. (id:3 - Eloi Boaventura)
INSERT INTO eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento,
endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco,
evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao)
VALUES (nextval('seq_evento_id'),'Soluções Impossiveis','Rua Senador Paranhos','Loteamento Guanabara','','75053757',
'Anápolis','GO','Brasil', 3, 999.99, 0, '2019-09-01 17:00:00', 500, 0, 'Palestra sobre como montar um esquema de piramide.');

VALUES (nextval('seq_evento_id'),'Nome_Evento','Rua Bernardo Horta','Zumbi','','29302235',
'Cachoeiro de Itapemirim','ES','Brasil','id_organizador', preco, evento_ativo, data_evento,
qtd_max_ingressos, qtd_vendida, descricao)
  
  CREATE TABLE public.eventos
(
  id integer NOT NULL,
  nome_evento character varying(300),
  endereco_rua character varying(100),
  endereco_bairro character varying(100),
  endereco_complemento character varying(100),
  endereco_cep character varying(16),
  endereco_cidade character varying(100),
  endereco_estado character varying(100),
  endereco_pais character varying(100),
  id_organizador integer,
  preco numeric(12,2),
  evento_ativo integer,
  data_evento timestamp without time zone,
  qtd_max_ingressos integer,
  qtd_vendida integer,
  descricao character varying(300),
  CONSTRAINT eventos_pkey PRIMARY KEY (id),
  
  
  
  
  
  
  
  
  -- 3 Ingressos (1 ativo e para utilizar, 1 ativo e utilizado, e 1 desativado)

-- id:5 (Teresa Caminha) participa de id:3 (Soluções Impossiveis)
-- 1 desativado
INSERT INTO ingressos (id, id_evento, id_usuario, validade, valido_ate,
nota_fiscal_path, data_pagamento)
VALUES (nextval('seq_ingresso_id'), 3, 5, 1, '2019-09-01 17:00:00',
'd:\\notafiscal.xml', '2018-12-01 17:00:00');
  
  id integer NOT NULL,
  id_evento integer,
  id_usuario integer,
  validade integer,
  valido_ate timestamp without time zone,
  nota_fiscal_path character varying(260),
  data_pagamento timestamp without time zone,
  CONSTRAINT ingressos_pkey PRIMARY KEY (id),
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  