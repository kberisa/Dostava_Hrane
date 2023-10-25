use master
go
drop database if exists dostava_hrane;
go
create database dostava_hrane;
go
use dostava_hrane;

create table kupac(
	sifra int not null primary key identity(1,1),
	korisnicko_ime varchar(50),
	ime varchar(50),
	prezime varchar(50),
	adresa varchar(50),
	telefon varchar(20)
);

create table proizvod(
	sifra int not null primary key identity(1,1),
	naziv varchar(50),
	opis varchar(200),
	cijena dec(18,2),
	dostupnost bit
);

create table dostavljac(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	oib char(11),
	email varchar(50),
	telefon varchar(20)
);

create table kosarica(
	sifra int not null primary key identity(1,1),
	proizvod int,
	kolicina int,
	kupac int,
	adresa_dostave varchar(50),
	status_dostave varchar(20),
	dostavljac int
);

alter table kosarica add foreign key (proizvod) references proizvod(sifra);
alter table kosarica add foreign key (kupac) references kupac(sifra);
alter table kosarica add foreign key (dostavljac) references dostavljac(sifra);

INSERT INTO kupac (korisnicko_ime, ime, prezime, adresa, telefon)
VALUES ('kupac1', 'Ime1', 'Prezime1', 'Adresa1', '123456789');

INSERT INTO kupac (korisnicko_ime, ime, prezime, adresa, telefon)
VALUES ('kupac2', 'Ime2', 'Prezime2', 'Adresa2', '987654321');

INSERT INTO kupac (korisnicko_ime, ime, prezime, adresa, telefon)
VALUES ('kupac3', 'Ime3', 'Prezime3', 'Adresa3', '111111111');

INSERT INTO proizvod (naziv, opis, cijena, dostupnost)
VALUES ('Proizvod1', 'Opis proizvoda 1', 10.99, 1);

INSERT INTO proizvod (naziv, opis, cijena, dostupnost)
VALUES ('Proizvod2', 'Opis proizvoda 2', 19.99, 1);

INSERT INTO proizvod (naziv, opis, cijena, dostupnost)
VALUES ('Proizvod3', 'Opis proizvoda 3', 29.99, 1);

INSERT INTO dostavljac (ime, prezime, oib, email, telefon)
VALUES ('Dostavljac1', 'PrezimeDostavljaca1', '12345678901', 'dostavljac1@example.com', '111222333');

INSERT INTO dostavljac (ime, prezime, oib, email, telefon)
VALUES ('Dostavljac2', 'PrezimeDostavljaca2', '98765432109', 'dostavljac2@example.com', '444555666');

INSERT INTO dostavljac (ime, prezime, oib, email, telefon)
VALUES ('Dostavljac3', 'PrezimeDostavljaca3', '11111111103', 'dostavljac3@example.com', '444444444');

INSERT INTO kosarica (proizvod, kolicina, kupac, adresa_dostave, status_dostave, dostavljac)
VALUES (1, 3, 1, 'AdresaDostave1', 'Na čekanju', 1);

INSERT INTO kosarica (proizvod, kolicina, kupac, adresa_dostave, status_dostave, dostavljac)
VALUES (2, 2, 2, 'AdresaDostave2', 'Otpremljeno', 2);

INSERT INTO kosarica (proizvod, kolicina, kupac, adresa_dostave, status_dostave, dostavljac)
VALUES (3, 2, 3, 'AdresaDostave3', 'Na čekanju', 1);


UPDATE kupac
SET ime = 'Ivan', prezime = 'Ivanov'
WHERE sifra = 1;

DELETE FROM kupac
WHERE sifra = 2;

SELECT ime, prezime FROM kupac WHERE sifra ='1';

SELECT * FROM dostavljac;