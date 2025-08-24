create table Medicos (
	MedicoId text primary key,
	Nome text not null,
	CRM text not null unique,
	Especialidade text not null,
	Email text not null,
	Phone text not null
);