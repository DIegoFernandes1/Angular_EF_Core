create database ProEvento;
use ProEvento;

if not exists(select * from sys.objects where name = 'Evento')
begin
	create table Evento
	(
	IdEvento int identity(1,1) primary key not null,
	Local varchar(500) not null,
	DataEvento datetime not null,
	Tema varchar(100) not null,
	QuantidadePessoas int not null,
	Lote varchar(50),
	ImagemURL varchar(max)
	)
end


--insert into Evento values ('Campinas-SP', GETDATE()+2, 'Angular Workshop', 350, '1° Lote', 'evento1.jfif');
--insert into Evento values ('São Paulo-SP', GETDATE()+30, 'Cafe e bate papo', 500, '2° Lote', 'evento2.jpg');
--insert into Evento values ('Campinas-SP', GETDATE()+15, '.Net Workshop', 500, '8° Lote', 'evento3.png');

