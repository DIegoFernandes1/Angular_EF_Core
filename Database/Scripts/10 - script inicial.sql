IF NOT EXISTS(SELECT name FROM master.dbo.sysdatabases where name = 'ProEvento')
BEGIN
	CREATE DATABASE ProEvento;
END

USE ProEvento;

IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'Evento')
BEGIN
	CREATE TABLE Evento
	(
		Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		Local VARCHAR(500) NOT NULL,
		DataEvento DATETIME NULL,
		Tema VARCHAR(100) NOT NULL,
		QuantidadePessoas INT NOT NULL,
		ImagemURL VARCHAR(MAX),
		Telefone VARCHAR(20) NULL, 
		Email VARCHAR(100) NULL
	)
END

IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'Lote')
BEGIN
	CREATE TABLE Lote
	(
		Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		Nome VARCHAR(50) NOT NULL,
		Preco DECIMAL(8,2) NOT NULL,
		DataInicio DATETIME NULL,
		DataFim DATETIME NULL,
		Quantidade INT NOT NULL,
		IdEvento INT FOREIGN KEY REFERENCES Evento(Id)
	)
END

IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'Palestrante')
BEGIN
	CREATE TABLE Palestrante
	(
		Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		Nome VARCHAR(50) NOT NULL,
		Descricao VARCHAR(100) NOT NULL,
		ImagemURL VARCHAR(MAX),
		Telefone VARCHAR(20) NULL, 
		Email VARCHAR(100) NULL
	)
END

IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'PalestranteEvento')
BEGIN
	CREATE TABLE PalestranteEvento
	(
		Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		IdPalestrante INT NOT NULL FOREIGN KEY REFERENCES Palestrante(Id),
		IdEvento INT NOT NULL  FOREIGN KEY REFERENCES Evento(Id)
	)
END

IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE NAME = 'RedeSocial')
BEGIN
	CREATE TABLE RedeSocial
	(
		Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		Nome VARCHAR(50) NOT NULL,
		URL VARCHAR(MAX) NOT NULL,
		IdPalestrante INT NULL FOREIGN KEY REFERENCES Palestrante(Id),
		IdEvento INT NULL FOREIGN KEY REFERENCES Evento(Id)
	)
END


--IF NOT EXISTS(SELECT 1 FROM Evento)
--BEGIN
--	INSERT INTO Evento VALUES ('Campinas-SP', GETDATE()+2, 'Angular Workshop', 350, 'evento1.jfif','(19)99999-9999','fulanodetal@hotmail.com');
--	INSERT INTO Evento VALUES ('São Paulo-SP', GETDATE()+30, 'Cafe e bate papo', 500, 'evento2.jpg','(11)98888-8888','cliclano@hotmail.com');
--	INSERT INTO Evento VALUES ('Campinas-SP', GETDATE()+15, '.Net Workshop', 500, 'evento3.png','(19)97777-7777','beltrano@hotmail.com');
--END


--IF NOT EXISTS(SELECT 1 FROM Lote)
--BEGIN
--	INSERT INTO Lote VALUES ('Lote 1', 125.35, null, null, 50, 1)
--	INSERT INTO Lote VALUES ('Lote 2', 125.35, null, null, 100, 2)
--	INSERT INTO Lote VALUES ('Lote 4', 125.35, null, null, 100, 3)
--END

--IF NOT EXISTS(SELECT 1 FROM Palestrante)
--BEGIN
--	INSERT INTO Palestrante VALUES ('Fulano de tal', 'Fulano de tal especialista em angular', '', '(19)99999-9999', 'fulanodetal@hotmail.com')
--	INSERT INTO Palestrante VALUES ('Ciclano', 'ciclano especialista em fazer nada', '', '(11)98888-8888', 'cliclano@hotmail.com')
--	INSERT INTO Palestrante VALUES ('Beltrano', 'beltrano especialista em .net', '', '(19)97777-7777', 'beltrano@hotmail.com')
--END

--IF NOT EXISTS(SELECT 1 FROM PalestranteEvento)
--BEGIN
--	INSERT INTO PalestranteEvento VALUES (1,1)
--	INSERT INTO PalestranteEvento VALUES (2,2)
--	INSERT INTO PalestranteEvento VALUES (3,3)
--END

--IF NOT EXISTS(SELECT 1 FROM RedeSocial)
--BEGIN
--	INSERT INTO RedeSocial VALUES ('Facebook', '', 1,1)
--	INSERT INTO RedeSocial VALUES ('Instagram', '', 2,2)
--	INSERT INTO RedeSocial VALUES ('YouTube', '', 3,3)
--END