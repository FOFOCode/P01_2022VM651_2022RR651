CREATE DATABASE ParkingDB;
USE ParkingDB;

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) UNIQUE NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) CHECK (Rol IN ('Cliente', 'Empleado')) NOT NULL
);
SELECT * FROM Usuarios

-- Tabla de Sucursales
CREATE TABLE Sucursales (
    SucursalID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(255) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    AdministradorID INT NOT NULL,
    NumeroEspacios INT NOT NULL,
    FOREIGN KEY (AdministradorID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Espacios de Parqueo
CREATE TABLE EspaciosParqueo (
    EspacioID INT PRIMARY KEY IDENTITY(1,1),
    SucursalID INT NOT NULL,
    NumeroEspacio INT NOT NULL,
    Ubicacion NVARCHAR(100) NOT NULL,
    CostoPorHora DECIMAL(10,2) NOT NULL,
    Estado NVARCHAR(20) CHECK (Estado IN ('Disponible', 'Ocupado')) NOT NULL,
    FOREIGN KEY (SucursalID) REFERENCES Sucursales(SucursalID)
);

-- Tabla de Reservas
CREATE TABLE Reservas (
    ReservaID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    EspacioID INT NOT NULL,
    FechaReserva DATETIME NOT NULL,
    CantidadHoras INT NOT NULL,
    Estado NVARCHAR(20) CHECK (Estado IN ('Activa', 'Cancelada', 'Completada')) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (EspacioID) REFERENCES EspaciosParqueo(EspacioID)
);

--INSERTS
-- Insertar datos en Usuarios
INSERT INTO Usuarios (Nombre, Correo, Telefono, Contraseña, Rol) VALUES
('Juan Pérez', 'juanperez@email.com', '555-1234', 'hashed_password1', 'Cliente'),
('Ana Gómez', 'anagomez@email.com', '555-5678', 'hashed_password2', 'Empleado'),
('Carlos López', 'carloslopez@email.com', '555-9012', 'hashed_password3', 'Cliente'),
('María Rodríguez', 'mariarodriguez@email.com', '555-3456', 'hashed_password4', 'Empleado'),
('Luis Fernández', 'luisfernandez@email.com', '555-7890', 'hashed_password5', 'Cliente');

-- Insertar datos en Sucursales
INSERT INTO Sucursales (Nombre, Direccion, Telefono, AdministradorID, NumeroEspacios) VALUES
('Sucursal Centro', 'Calle 123, Ciudad', '555-0001', 2, 10),
('Sucursal Norte', 'Avenida 456, Ciudad', '555-0002', 4, 15),
('Sucursal Sur', 'Boulevard 789, Ciudad', '555-0003', 2, 12),
('Sucursal Este', 'Carretera 321, Ciudad', '555-0004', 4, 8),
('Sucursal Oeste', 'Paseo 654, Ciudad', '555-0005', 2, 20);

-- Insertar datos en Espacios de Parqueo
INSERT INTO EspaciosParqueo (SucursalID, NumeroEspacio, Ubicacion, CostoPorHora, Estado) VALUES
(1, 1, 'Nivel 1, Zona A', 10.00, 'Disponible'),
(1, 2, 'Nivel 1, Zona B', 10.00, 'Ocupado'),
(2, 3, 'Nivel 2, Zona A', 12.00, 'Disponible'),
(3, 4, 'Nivel 3, Zona B', 8.50, 'Ocupado'),
(4, 5, 'Nivel 4, Zona C', 9.00, 'Disponible');

-- Insertar datos en Reservas
INSERT INTO Reservas (UsuarioID, EspacioID, FechaReserva, CantidadHoras, Estado) VALUES
(1, 1, '2024-02-24 08:00:00', 2, 'Activa'),
(2, 2, '2024-02-24 09:30:00', 3, 'Completada'),
(3, 3, '2024-02-25 10:00:00', 1, 'Activa'),
(4, 4, '2024-02-26 11:15:00', 2, 'Cancelada'),
(5, 5, '2024-02-27 12:45:00', 4, 'Activa');
