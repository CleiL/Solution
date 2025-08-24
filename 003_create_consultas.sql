create table Consultas (
	ConsultaId text primary key, 
	MedicoId   text not null,
	PacienteId text not null,
	DataHora   text not null,       

	foreign key (MedicoId)   references Medicos(MedicoId),
	foreign key (PacienteId) references Pacientes(PacienteId)
);