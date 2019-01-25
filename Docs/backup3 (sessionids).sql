--
-- PostgreSQL database dump
--

-- Dumped from database version 10.6
-- Dumped by pg_dump version 10.6

-- Started on 2019-01-23 17:32:55

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
-- TOC entry 196 (class 1259 OID 16496)
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
-- TOC entry 197 (class 1259 OID 16502)
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
-- TOC entry 198 (class 1259 OID 16505)
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
-- TOC entry 199 (class 1259 OID 16507)
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
-- TOC entry 200 (class 1259 OID 16509)
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
-- TOC entry 201 (class 1259 OID 16511)
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
-- TOC entry 2168 (class 0 OID 16496)
-- Dependencies: 196
-- Data for Name: eventos; Type: TABLE DATA; Schema: public; Owner: DMBR
--

INSERT INTO public.eventos VALUES (1, 'Aplicações Robóticas', 'Avenida Castro Alves', 'Belvedere', 'Salão A', '29180084', 'Serra', 'ES', 'Brasil', 2, 399.99, 1, '2019-05-12 17:00:00', 500, 1, 'Empresa X fará uma demonstração de seus robôs.');
INSERT INTO public.eventos VALUES (2, 'Soluções Mobile', 'Rua Senador Paranhos', 'Loteamento Guanabara', 'Area Verde', '75053757', 'Anápolis', 'GO', 'Brasil', 2, 299.99, 1, '2019-01-01 17:00:00', 500, 1, 'Palestra sobre como soluções mobile estão revolucionando o mercado.');
INSERT INTO public.eventos VALUES (3, 'Soluções Impossiveis', 'Rua Senador Paranhos', 'Loteamento Guanabara', 'Sala 0021', '75053757', 'Anápolis', 'GO', 'Brasil', 2, 999.99, 0, '2019-09-01 17:00:00', 500, 1, 'Palestra sobre como montar um esquema de piramide.');
INSERT INTO public.eventos VALUES (4, 'Informática em TI', 'Rua Senador Camilo', 'Jardim Henriquetta', 'Sala 113', '47503711', 'Anápolis', 'GO', 'Brasil', 7, 199.99, 1, '2019-08-10 17:00:00', 200, 0, 'Gerenciando computadores Windows para iniciantes.');


--
-- TOC entry 2169 (class 0 OID 16502)
-- Dependencies: 197
-- Data for Name: ingressos; Type: TABLE DATA; Schema: public; Owner: DMBR
--

INSERT INTO public.ingressos VALUES (1, 1, 4, 1, '2019-05-12 17:00:00', 'd:\notafiscal.xml', '2019-01-23 14:53:16.001625');
INSERT INTO public.ingressos VALUES (2, 2, 4, 1, '2019-01-01 17:00:00', 'd:\notafiscal.xml', '2018-12-01 17:00:00');
INSERT INTO public.ingressos VALUES (3, 3, 4, 1, '2019-09-01 17:00:00', 'd:\notafiscal.xml', '2018-12-01 17:00:00');


--
-- TOC entry 2173 (class 0 OID 16511)
-- Dependencies: 201
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: DMBR
--

INSERT INTO public.usuarios VALUES (1, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'superuser@eventos.com', '0001-01-01 00:00:00', 0, 1, NULL, NULL, 'admin', '123456', 3, NULL, NULL, '0001-01-01 00:00:00', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Az3bUn5D6Z', 0);
INSERT INTO public.usuarios VALUES (2, NULL, NULL, 1, 'Eloi Boaventura', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'eloib@eventos.com', '0001-01-01 00:00:00', 0, 1, NULL, NULL, 'eloib', '123456', 5, NULL, NULL, '0001-01-01 00:00:00', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'R8HsbHQkuP', 0);
INSERT INTO public.usuarios VALUES (3, NULL, NULL, 1, 'Edgar Figueira', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'edgarf@eventos.com', '0001-01-01 00:00:00', 0, 1, NULL, NULL, 'edgarf', '123456', 5, NULL, NULL, '0001-01-01 00:00:00', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'y78L7f1VF1', 0);
INSERT INTO public.usuarios VALUES (4, '371761773', '56614783050', 1, 'Teresa Caminha', 'Rua Nossa Senhora de Lourdes', 'Dezoito do Forte', NULL, '49072636', 'Aracaju', 'SE', 'Brasil', 'teresac@mail.com', '1974-08-22 00:00:00', 1, 1, NULL, NULL, 'teresac', '123456', 1, '5555222211113333', 'TERESA CAMINHA', '2023-07-01 00:00:00', 1, 'Rua Nossa Senhora de Lourdes', 'Dezoito do Forte', NULL, '49072636', 'Aracaju', 'SE', 'Brasil', 'wAuHRjPraD', 0);
INSERT INTO public.usuarios VALUES (5, '267631625', '56048480008', 1, 'Salomé Borges', 'Avenida Dom Pedro I', 'Cardoso Continuação', NULL, '74934520', 'Aparecida de Goiânia', 'GO', 'Brasil', 'solomeb@mail.com', '1974-08-22 00:00:00', 2, 1, NULL, NULL, 'solomeb', '123456', 1, '6666222211113333', 'SALOMÉ BORGES', '2020-09-01 00:00:00', 1, 'Rua Nossa Senhora Aparecida', 'Nova Parnamirim', NULL, '59151730', 'Parnamirim', 'RN', 'Brasil', 'z2oYuXuRrr', 0);
INSERT INTO public.usuarios VALUES (6, NULL, NULL, 1, 'Oriana Bensaúde', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'orianab@eventos.com', '0001-01-01 00:00:00', 0, 1, NULL, NULL, 'orianab', '123456', 2, NULL, NULL, '0001-01-01 00:00:00', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'JggKCG7SeY', 0);
INSERT INTO public.usuarios VALUES (7, NULL, NULL, 1, 'Celestino Pontes', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'celestinop@eventos.com', '0001-01-01 00:00:00', 0, 1, NULL, NULL, 'celestinop', '123456', 4, NULL, NULL, '0001-01-01 00:00:00', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'kjcWnnzcbD', 2);


--
-- TOC entry 2182 (class 0 OID 0)
-- Dependencies: 198
-- Name: seq_evento_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_evento_id', 4, true);


--
-- TOC entry 2183 (class 0 OID 0)
-- Dependencies: 199
-- Name: seq_ingresso_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_ingresso_id', 3, true);


--
-- TOC entry 2184 (class 0 OID 0)
-- Dependencies: 200
-- Name: seq_usuario_id; Type: SEQUENCE SET; Schema: public; Owner: DMBR
--

SELECT pg_catalog.setval('public.seq_usuario_id', 7, true);


--
-- TOC entry 2039 (class 2606 OID 16518)
-- Name: eventos eventos_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.eventos
    ADD CONSTRAINT eventos_pkey PRIMARY KEY (id);


--
-- TOC entry 2041 (class 2606 OID 16520)
-- Name: ingressos ingressos_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_pkey PRIMARY KEY (id);


--
-- TOC entry 2043 (class 2606 OID 16522)
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id);


--
-- TOC entry 2044 (class 2606 OID 16523)
-- Name: eventos eventos_id_organizador_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.eventos
    ADD CONSTRAINT eventos_id_organizador_fkey FOREIGN KEY (id_organizador) REFERENCES public.usuarios(id);


--
-- TOC entry 2045 (class 2606 OID 16528)
-- Name: ingressos ingressos_id_evento_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_id_evento_fkey FOREIGN KEY (id_evento) REFERENCES public.eventos(id);


--
-- TOC entry 2046 (class 2606 OID 16533)
-- Name: ingressos ingressos_id_usuario_fkey; Type: FK CONSTRAINT; Schema: public; Owner: DMBR
--

ALTER TABLE ONLY public.ingressos
    ADD CONSTRAINT ingressos_id_usuario_fkey FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id);


-- Completed on 2019-01-23 17:32:56

--
-- PostgreSQL database dump complete
--

