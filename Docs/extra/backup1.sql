--
-- PostgreSQL database dump
--

-- Dumped from database version 10.6
-- Dumped by pg_dump version 10.6

-- Started on 2019-01-23 15:03:54

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2179 (class 1262 OID 16404)
-- Name: desafiomb; Type: DATABASE; Schema: -; Owner: DMBR
--

CREATE DATABASE desafiomb WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'English_United States.1252' LC_CTYPE = 'English_United States.1252';


ALTER DATABASE desafiomb OWNER TO "DMBR";

\connect desafiomb

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12278)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2181 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 198 (class 1259 OID 16430)
-- Name: eventos; Type: TABLE; Schema: public; Owner: DMBR
--

CREATE TABLE public.eventos (
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
    descricao character varying(300)
);


ALTER TABLE public.eventos OWNER TO "DMBR";

--
-- TOC entry 196 (class 1259 OID 16405)
-- Name: ingressos; Type: TABLE; Schema: public; Owner: DMBR
--

CREATE TABLE public.ingressos (
    id integer NOT NULL,
    id_evento integer,
    id_usuario integer,
    validade integer,
    valido_ate timestamp without time zone,
    nota_fiscal_path character varying(260),
    data_pagamento timestamp without time zone
);


ALTER TABLE public.ingressos OWNER TO "DMBR";

--
-- TOC entry 199 (class 1259 OID 16473)
-- Name: seq_evento_id; Type: SEQUENCE; Schema: public; Owner: DMBR
--

CREATE SEQUENCE public.seq_evento_id
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seq_evento_id OWNER TO "DMBR";

--
-- TOC entry 197 (class 1259 OID 16410)
-- Name: seq_ingresso_id; Type: SEQUENCE; Schema: public; Owner: DMBR
--

CREATE SEQUENCE public.seq_ingresso_id
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seq_ingresso_id OWNER TO "DMBR";

--
-- TOC entry 200 (class 1259 OID 16475)
-- Name: seq_usuario_id; Type: SEQUENCE; Schema: public; Owner: DMBR
--

CREATE SEQUENCE public.seq_usuario_id
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seq_usuario_id OWNER TO "DMBR";

--
-- TOC entry 201 (class 1259 OID 16477)
-- Name: usuarios; Type: TABLE; Schema: public; Owner: DMBR
--

CREATE TABLE public.usuarios (
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
    subordinado integer
);


ALTER TABLE public.usuarios OWNER TO "DMBR";

--
-- TOC entry 2170 (class 0 OID 16430)
-- Dependencies: 198
-- Data for Name: eventos; Type: TABLE DATA; Schema: public; Owner: DMBR
--

COPY public.eventos (id, nome_evento, endereco_rua, endereco_bairro, endereco_complemento, endereco_cep, endereco_cidade, endereco_estado, endereco_pais, id_organizador, preco, evento_ativo, data_evento, qtd_max_ingressos, qtd_vendida, descricao) FROM stdin;
2	Soluções Mobile	Rua Senador Paranhos	Loteamento Guanabara		75053757	Anápolis	GO	Brasil	3	299.99	1	2019-01-01 17:00:00	500	0	Palestra sobre como soluções mobile estão revolucionando o mercado.
5	Evento 2BB	Rua 2	Campinas		29180000	Serra	ES	Brasil	8	399.99	1	2019-05-12 17:00:00	500	0	Evento 2.
4	Evento 1	Rua 1	Campinas		29180000	Serra	ES	Brasil	8	399.99	1	2019-05-12 17:00:00	500	0	Evento 1.
1	Aplicações Robóticas	Avenida Castro Alves	Belvedere		29180084	Serra	ES	Brasil	8	399.99	1	2019-05-12 17:00:00	500	1	Empresa X fará uma demonstração de seus robôs.
3	Soluções Impossiveis	Rua Senador Paranhos	Loteamento Guanabara		75053757	Anápolis	GO	Brasil	3	999.99	0	2019-09-01 17:00:00	500	0	Palestra sobre como montar um esquema de piramide.
\.


--
-- TOC entry 2168 (class 0 OID 16405)
-- Dependencies: 196
-- Data for Name: ingressos; Type: TABLE DATA; Schema: public; Owner: DMBR
--

COPY public.ingressos (id, id_evento, id_usuario, validade, valido_ate, nota_fiscal_path, data_pagamento) FROM stdin;
5	3	6	1	2019-09-01 17:00:00	d:\\notafiscal.xml	2019-01-23 14:32:45.425139
4	3	5	1	2019-09-01 17:00:00	d:\\notafiscal.xml	2018-12-01 17:00:00
3	2	5	1	2019-01-01 17:00:00	d:\\notafiscal.xml	2018-12-01 17:00:00
2	1	5	2	2019-05-12 17:00:00	d:\\notafiscal.xml	2019-01-18 16:57:03.49851
6	1	6	1	2019-05-12 17:00:00	d:\\notafiscal.xml	2019-01-23 14:40:22.754787
\.


--
-- TOC entry 2173 (class 0 OID 16477)
-- Dependencies: 201
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: DMBR
--

COPY public.usuarios (id, rg, cpf, conta_ativa, nome, endereco_rua, endereco_bairro, endereco_complemento, endereco_cep, endereco_cidade, endereco_estado, endereco_pais, email, data_nascimento, sexo, tipo_login, imagem_profile_path, last_token, login, senha, tipo_acesso, numero_cartao, nome_titular, data_exp, tipo_cartao, endereco_cobrabca_rua, endereco_cobrabca_bairro, endereco_cobrabca_complemento, endereco_cobrabca_cep, endereco_cobrabca_cidade, endereco_cobrabca_estado, endereco_cobrabca_pais, session_id, subordinado) FROM stdin;
3	493618193	21985947064	1	Eloi Boaventura	Via Ana Aparecida de Carvalho	Bortolan		37715300	Poços de Caldas	MG	Brasil	eloib@mail.com	1982-01-06 00:00:00	1	1			eloib	123456	5	0000111122223333	ELOI BOAVENTURA	2022-01-01 00:00:00	1	Via Ana Aparecida de Carvalho	Bortolan		37715300	Poços de Caldas	MG	Brsil	NNpEiW0wgG	0
6	267631625	56048480008	1	Salomé Borges	Avenida Dom Pedro I	Cardoso Continuação	Complemento	74934520	Aparecida de Goiânia	GO	Brasil	solomeb@mail.com	1974-08-22 00:00:00	2	1			solomeb	123456	1	6666222211113333	SALOMÉ BORGES	2020-09-01 00:00:00	1	Rua Nossa Senhora Aparecida	Nova Parnamirim		59151730	Parnamirim	RN	Brasil	ni45ZVMZCR	0
7	441652505	74260643010	1	Oriana Bensaúde	Rua Sebastiana Izídio de Oliveira	Canafístula		57302679	Arapiraca	AL	Brasil	orianab@mail.com	1966-01-22 00:00:00	1	1			orianab	123456	2	5555111177774444	ORIANA BENSAÚDE	2020-09-01 00:00:00	1	Rua Sebastiana Izídio de Oliveira	Canafístula		57302679	Arapiraca	AL	Brasil	m5KVzyyPVm	0
8	297562587	69571152080	1	Celestino Pontes	Rua Bernardo Horta	Zumbi		29302235	Cachoeiro de Itapemirim	ES	Brasil	celestinop@mail.com	1973-08-02 00:00:00	2	1			celestinop	123456	4	7777111177774444	CELESTINO PONTES	2020-09-01 00:00:00	1	Rua Bernardo Horta	Zumbi		29302235	Cachoeiro de Itapemirim	ES	Brasil	fShtoiVJWw	3
2	\N	\N	1	\N	\N	\N	\N	\N	\N	\N	\N	superuser@eventos.com	0001-01-01 00:00:00	0	1	\N	\N	admin	123456	3	\N	\N	0001-01-01 00:00:00	0	\N	\N	\N	\N	\N	\N	\N	UONKkByVWq	0
5	371761773	56614783050	1	Teresa Caminha	Rua Nossa Senhora de Lourdes	Dezoito do Forte		49072636	Aracaju	SE	Brasil	teresac@mail.com	1974-08-22 00:00:00	1	1			teresac	123456	1	5555222211113333	TERESA CAMINHA	2023-07-01 00:00:00	1	Rua Nossa Senhora de Lourdes	Dezoito do Forte		49072636	Aracaju	SE	Brasil	8ZMfsxFNCE	0
16	382572749	43709948029	1	Cláudio Otávio Bernardes	Rua Jardim Magnólia	Alto do Sumaré		59634036	Mossoró	RN	Brasil	claudioo@mail.com	1951-01-13 00:00:00	2	1			claudioo	123456	4	99994444111122222	CLÁUDIO OTÁVIO BERNARDES	2020-09-01 00:00:00	1	Rua Jardim Magnólia	Alto do Sumaré		59634036	Mossoró	RN	Brasil	\N	4
4	288617551	79457437063	1	Edgar Figueira	Rua Sídio Carlos de Couto	Castelo Branco		29140806	Cariacica	ES	Brasil	edgarf@mail.com	1981-05-11 00:00:00	2	1			edgarf	123456	5	4444111122223333	EDGAR FIGUEIRA	2024-03-01 00:00:00	1	Rua Sídio Carlos de Couto	Castelo Branco		29140806	Cariacica	ES	Brasil	7dlbfylwZt	0
\.


--
-- TOC entry 2182 (class 0 OID 0)
-- Dependencies: 199
-- Name: seq_evento_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_evento_id', 5, true);


--
-- TOC entry 2183 (class 0 OID 0)
-- Dependencies: 197
-- Name: seq_ingresso_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_ingresso_id', 6, true);


--
-- TOC entry 2184 (class 0 OID 0)
-- Dependencies: 200
-- Name: seq_usuario_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_usuario_id', 16, true);


--
-- TOC entry 2041 (class 2606 OID 16437)
-- Name: eventos eventos_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.eventos
    ADD CONSTRAINT eventos_pkey PRIMARY KEY (id);


--
-- TOC entry 2039 (class 2606 OID 16439)
-- Name: ingressos ingressos_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_pkey PRIMARY KEY (id);


--
-- TOC entry 2043 (class 2606 OID 16484)
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id);


--
-- TOC entry 2046 (class 2606 OID 16485)
-- Name: eventos eventos_id_organizador_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.eventos
    ADD CONSTRAINT eventos_id_organizador_fkey FOREIGN KEY (id_organizador) REFERENCES public.usuarios(id);


--
-- TOC entry 2044 (class 2606 OID 16463)
-- Name: ingressos ingressos_id_evento_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_id_evento_fkey FOREIGN KEY (id_evento) REFERENCES public.eventos(id);


--
-- TOC entry 2045 (class 2606 OID 16490)
-- Name: ingressos ingressos_id_usuario_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_id_usuario_fkey FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id);


-- Completed on 2019-01-23 15:03:55

--
-- PostgreSQL database dump complete
--

