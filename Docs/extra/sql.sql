create table ingresso (
	id integer primary key,
	id_evento integer,
	id_usuario integer,
	validade integer,
	valido_ate timestamp without time zone,
	nota_fiscal_path varchar(260),
	data_pagamento timestamp without time zone,
	CONSTRAINT ingressos_pkey PRIMARY KEY (id)
);

	
	
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
  preco numeric(12,2), -- 9 999 999 999, 99 milhoes eh o maximo (12 numeros, 2 fracoes)
  evento_ativo integer,
  data_evento timestamp without time zone,
  qtd_max_ingressos integer,
  qtd_vendida integer,
  descricao character varying(300),
  CONSTRAINT eventos_pkey PRIMARY KEY (id)
);


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
  session_id character varying(100),
  subordinado integer,
  CONSTRAINT usuarios_pkey PRIMARY KEY (id)
);

ALTER TABLE public.eventos ADD FOREIGN KEY (id_organizador) REFERENCES public.usuarios(id);
ALTER TABLE public.ingressos ADD FOREIGN KEY (id_evento) REFERENCES public.eventos(id);
ALTER TABLE public.ingressos ADD FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id);

create sequence seq_ingressos_id increment by 1 minvalue 1 no maxvalue start with 1;
create sequence seq_eventos_id increment by 1 minvalue 1 no maxvalue start with 1;
create sequence seq_usuario_id increment by 1 minvalue 1 no maxvalue start with 1;


/* Tipo de Login
1 - Login/Senha
2 - Linkedin
3 - Facebook
*/

/* Tipo de Acesso
1 - Visitante:
2 - Administrador Nível 1:
3 - Administrador Nível 2:
4 - Gerente de Eventos Nível 1:
5 - Gerente de Eventos Nível 2:
*/

/* Sexo
1 - Feminino
2 - Masculino
*/

/* Validade de Ingresso
1 - Pago
2 - Utilizado
*/


-- id:1
-- SuperUser Admin (access3)
insert into usuarios (id,conta_ativa,email,tipo_login,login,senha,tipo_acesso) values (nextval('seq_usuario_id'),1,'superuser@eventos.com',1,'admin','123456',3);


-- Uusarios de Teste

-- id:2
-- Eloi Boaventura (eloib/123456) - Gerentes de Evento Nivel 2 (access5)
INSERT INTO usuarios (id,conta_ativa,nome,email,tipo_login,login,senha,tipo_acesso,subordinado)
VALUES (nextval('seq_usuario_id'),1,'Eloi Boaventura','eloib@eventos.com',1,'eloib','123456',5,0);


-- id:3
-- Edgar Figueira (edgarf/123456) - Gerentes de Evento Nivel 2 (access5)
INSERT INTO usuarios (id,conta_ativa,nome,email,tipo_login,login,senha,tipo_acesso,subordinado)
VALUES (nextval('seq_usuario_id'),1,'Edgar Figueira','edgarf@eventos.com',1,'edgarf','123456',5,0);


-- id:4
-- Teresa Caminha (teresac/123456) - Visitante (access1)
INSERT INTO usuarios (id, rg, cpf, conta_ativa, nome, endereco_rua, endereco_bairro,
 endereco_cep, endereco_cidade, endereco_estado, endereco_pais, email, data_nascimento, sexo,
 tipo_login, login, senha, tipo_acesso, numero_cartao,
 nome_titular, data_exp, tipo_cartao, endereco_cobrabca_rua, endereco_cobrabca_bairro,
 endereco_cobrabca_cep, endereco_cobrabca_cidade,
 endereco_cobrabca_estado, endereco_cobrabca_pais,subordinado)
VALUES (nextval('seq_usuario_id'),'371761773','56614783050',1,'Teresa Caminha','Rua Nossa Senhora de Lourdes','Dezoito do Forte','49072636',
'Aracaju','SE','Brasil','teresac@mail.com','1974-08-22 00:00:00',1,1,'teresac','123456',1,'5555222211113333',
'TERESA CAMINHA','2023-07-01 00:00:00',1,'Rua Nossa Senhora de Lourdes','Dezoito do Forte','49072636',
'Aracaju','SE','Brasil',0);


-- id:5
-- Salomé Borges (solomeb/123456) - Visitante (access1)
INSERT INTO usuarios (id, rg, cpf, conta_ativa, nome, endereco_rua, endereco_bairro,
 endereco_cep, endereco_cidade, endereco_estado, endereco_pais, email, data_nascimento, sexo,
 tipo_login, login, senha, tipo_acesso, numero_cartao,
 nome_titular, data_exp, tipo_cartao, endereco_cobrabca_rua, endereco_cobrabca_bairro,
 endereco_cobrabca_cep, endereco_cobrabca_cidade,
 endereco_cobrabca_estado, endereco_cobrabca_pais,subordinado)
VALUES (nextval('seq_usuario_id'),'267631625','56048480008',1,'Salomé Borges','Avenida Dom Pedro I','Cardoso Continuação','74934520',
'Aparecida de Goiânia','GO','Brasil','solomeb@mail.com','1974-08-22 00:00:00',2,1,'solomeb','123456',1,'6666222211113333',
'SALOMÉ BORGES','2020-09-01 00:00:00',1,'Rua Nossa Senhora Aparecida','Nova Parnamirim','59151730',
'Parnamirim','RN','Brasil',0);


-- id:6
-- Oriana Bensaúde (orianab/123456) - Administrador Nível 1 (access2)
INSERT INTO usuarios (id,conta_ativa,nome,email,tipo_login,login,senha,tipo_acesso,subordinado)
VALUES (nextval('seq_usuario_id'),1,'Oriana Bensaúde','orianab@eventos.com',1,'orianab','123456',2,0);


-- id:7
-- Celestino Pontes (celestinop/123456) - Gerente de Eventos Nível 1 (access4) - Subordinado a eloib (id:2)
INSERT INTO usuarios (id,conta_ativa,nome,email,tipo_login,login,senha,tipo_acesso,subordinado)
VALUES (nextval('seq_usuario_id'),1,'Celestino Pontes','celestinop@eventos.com',1,'celestinop','123456',4,2);





-- 4 Eventos (1 ativo e para acontecer, 1 ativo e concluido, 1 desativado, e 1 subordinado)

-- Evento 1 ativo e para acontecer. (id:2 - Eloi Boaventura) - Aplicações Robóticas / id:1
INSERT INTO eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento,
endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco,
evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao)
VALUES (nextval('seq_evento_id'),'Aplicações Robóticas','Avenida Castro Alves','Belvedere','Salão A','29180084',
'Serra','ES','Brasil',2, 399.99, 1, '2019-05-12 17:00:00', 500, 1, 'Empresa X fará uma demonstração de seus robôs.');

-- Evento 1 ativo e concluido. (id:2 - Eloi Boaventura) - Soluções Mobile / id:2
INSERT INTO eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento,
endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco,
evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao)
VALUES (nextval('seq_evento_id'),'Soluções Mobile','Rua Senador Paranhos','Loteamento Guanabara','Area Verde','75053757',
'Anápolis','GO','Brasil', 2, 299.99, 1, '2019-01-01 17:00:00', 500, 1, 'Palestra sobre como soluções mobile estão revolucionando o mercado.');

-- Evento 1 desativado. (id:2 - Eloi Boaventura) - Soluções Impossiveis / id:3
INSERT INTO eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento,
endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco,
evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao)
VALUES (nextval('seq_evento_id'),'Soluções Impossiveis','Rua Senador Paranhos','Loteamento Guanabara','Sala 0021','75053757',
'Anápolis','GO','Brasil', 2, 999.99, 0, '2019-09-01 17:00:00', 500, 1, 'Palestra sobre como montar um esquema de piramide.');

-- Evento 1 subordinado. (id:7 - Celestino Pontes) - Informática em TI / id:4
INSERT INTO eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento,
endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco,
evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao)
VALUES (nextval('seq_evento_id'),'Informática em TI','Rua Senador Camilo','Jardim Henriquetta','Sala 113','47503711',
'Anápolis','GO','Brasil', 7, 199.99, 1, '2019-08-10 17:00:00', 200, 0, 'Gerenciando computadores Windows para iniciantes.');


-- 3 Ingressos (1 ativo e para utilizar, 1 ativo e utilizado, e 1 desativado)

-- id:4 (Teresa Caminha) participa de id:1 (Aplicações Robóticas)
-- 1 ativo e para utilizar
INSERT INTO ingressos (id, id_evento, id_usuario, validade, valido_ate,
nota_fiscal_path, data_pagamento)
VALUES (nextval('seq_ingresso_id'), 1, 4, 1, '2019-05-12 17:00:00',
'd:\notafiscal.xml', current_timestamp);


-- id:4 (Teresa Caminha) participa de id:2 (Soluções Mobile)
-- 1 ativo e utilizado
INSERT INTO ingressos (id, id_evento, id_usuario, validade, valido_ate,
nota_fiscal_path, data_pagamento)
VALUES (nextval('seq_ingresso_id'), 2, 4, 1, '2019-01-01 17:00:00',
'd:\notafiscal.xml', '2018-12-01 17:00:00');


-- id:4 (Teresa Caminha) participa de id:3 (Soluções Impossiveis)
-- 1 desativado
INSERT INTO ingressos (id, id_evento, id_usuario, validade, valido_ate,
nota_fiscal_path, data_pagamento)
VALUES (nextval('seq_ingresso_id'), 3, 4, 1, '2019-09-01 17:00:00',
'd:\notafiscal.xml', '2018-12-01 17:00:00');












