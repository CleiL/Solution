create table Pacientes (
	PacienteId text primary key,
	Nome text not null,
	CPF text not null unique,
	Email text not null,
	Phone text not null
);