CREATE DATABASE VeterinariaDb;
GO
USE VeterinariaDb;
GO

CREATE TABLE Mascota (
    MascotaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Especie NVARCHAR(40) NOT NULL,
    Edad INT NOT NULL CHECK (Edad BETWEEN 0 AND 40),
    CodigoChip NVARCHAR(25) NOT NULL UNIQUE
);

CREATE TABLE Cita (
    CitaId INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora DATETIME2 NOT NULL,
    Motivo NVARCHAR(150) NOT NULL,
    Veterinario NVARCHAR(80) NOT NULL,
    MascotaId INT NOT NULL,
    CONSTRAINT FK_Cita_Mascota FOREIGN KEY (MascotaId) REFERENCES Mascota(MascotaId),
    CONSTRAINT UQ_Cita_Mascota_Fecha UNIQUE (MascotaId, FechaHora)
);

INSERT INTO Mascota (Nombre, Especie, Edad, CodigoChip) VALUES
('Luna', 'Perro', 4, 'CHIP-1001'),
('Michi', 'Gato', 2, 'CHIP-1002'),
('Kiwi', 'Conejo', 1, 'CHIP-1003');

INSERT INTO Cita (FechaHora, Motivo, Veterinario, MascotaId) VALUES
('2026-11-10T09:00:00', 'Vacunación anual', 'Dra. Ramos', 1),
('2026-11-11T16:30:00', 'Control general', 'Dr. Paredes', 2);
