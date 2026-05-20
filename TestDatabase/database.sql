-- Crear la base de datos
CREATE DATABASE coopcontrol_db;
GO

-- Seleccionar la base de datos
USE coopcontrol_db;
GO


-- =============================================
-- 1. TABLAS BASE E INDEPENDIENTES
-- =============================================

-- 1. Tabla base: Socios
CREATE TABLE [dbo].[Socios] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Cedula] NVARCHAR(MAX) NOT NULL,
    [FechaNacimiento] DATE NOT NULL,
    [FechaIngreso] DATE NOT NULL,
    [Nombre] NVARCHAR(MAX) NOT NULL,
    [Apellido] NVARCHAR(MAX) NOT NULL,
    [Sexo] NVARCHAR(MAX) NULL,
    [EstadoCivil] NVARCHAR(MAX) NULL,
    [TipoSangre] NVARCHAR(MAX) NULL,
    [Nacionalidad] NVARCHAR(MAX) NULL,
    [Telefono] NVARCHAR(MAX) NULL,
    [Email] NVARCHAR(MAX) NULL,
    [Direccion] NVARCHAR(MAX) NULL,
    [Estado] BIT NOT NULL DEFAULT 1
);

-- 2. Tabla: User
CREATE TABLE [dbo].[User] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Email] VARCHAR(MAX) NOT NULL,
    [PasswordHash] VARCHAR(MAX) NOT NULL,
    [Role] VARCHAR(MAX) NOT NULL,
    [Estado] BIT NOT NULL DEFAULT 1,
    [FechaRegistro] DATETIME NOT NULL DEFAULT GETDATE(),
    [Nombre] VARCHAR(MAX) NULL,
    [Apellido] VARCHAR(MAX) NULL
);

-- 3. Tabla: Configuraciones
CREATE TABLE [dbo].[Configuraciones] (
    [Clave] NVARCHAR(255) NOT NULL PRIMARY KEY,
    [Valor] NVARCHAR(MAX) NOT NULL,
    [Descripcion] NVARCHAR(MAX) NULL
);


-- =============================================
-- 2. TABLAS DEPENDIENTES (CON LLAVES FORÁNEAS)
-- =============================================

-- 4. Tabla: Ahorros (depende de Socios)
CREATE TABLE [dbo].[Ahorros] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SocioId] INT NOT NULL,
    [Tipo] INT NOT NULL,
    [Saldo] DECIMAL(18,2) NOT NULL,
    [TasaInteresAnual] DECIMAL(18,2) NOT NULL,
    [FechaCreacion] DATETIME2 NOT NULL,
    [FechaVencimiento] DATETIME2 NULL,
    [Notas] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK__Ahorros__SocioId__29221CFB] FOREIGN KEY ([SocioId]) REFERENCES [dbo].[Socios]([Id])
);

-- 5. Tabla: Aportes (depende de Socios)
CREATE TABLE [dbo].[Aportes] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SocioId] INT NOT NULL,
    [Fecha] DATE NOT NULL,
    [Monto] DECIMAL(18,2) NOT NULL,
    [Concepto] NVARCHAR(MAX) NULL,
    [Tipo] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK__Aportes__SocioId__60A75C0F] FOREIGN KEY ([SocioId]) REFERENCES [dbo].[Socios]([Id])
);

-- 6. Tabla: Prestamo (depende de Socios)
CREATE TABLE [dbo].[Prestamo] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SocioId] INT NOT NULL,
    [Monto] DECIMAL(18,2) NOT NULL,
    [TasaInteres] DECIMAL(18,2) NULL,
    [PlazoMeses] INT NULL,
    [FechaPrestamo] DATE NOT NULL,
    [FechaDevolucion] DATE NULL,
    [Estado] NVARCHAR(MAX) NOT NULL DEFAULT 'Activo',
    [Observaciones] NVARCHAR(MAX) NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME NULL,
    [MetodoPagoPreferido] NVARCHAR(MAX) NULL,
    [TipoPrestamo] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK_Prestamo_Socio] FOREIGN KEY ([SocioId]) REFERENCES [dbo].[Socios]([Id])
);

-- 7. Tabla: Certificados (depende de Socios)
CREATE TABLE [dbo].[Certificados] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SocioId] INT NOT NULL,
    [Tipo] NVARCHAR(MAX) NOT NULL,
    [Monto] DECIMAL(18,2) NOT NULL,
    [FechaEmision] DATETIME2 NOT NULL,
    [FechaVencimiento] DATETIME2 NOT NULL,
    [Estado] NVARCHAR(MAX) NOT NULL,
    [Observaciones] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK_Certificados_Socios_SocioId] FOREIGN KEY ([SocioId]) REFERENCES [dbo].[Socios]([Id])
);

-- 8. Tabla: PagoPrestamo (depende de Prestamo)
CREATE TABLE [dbo].[PagoPrestamo] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [PrestamoId] INT NOT NULL,
    [FechaPago] DATE NOT NULL,
    [Monto] DECIMAL(18,2) NOT NULL,
    [Metodo] NVARCHAR(MAX) NULL,
    [Observaciones] NVARCHAR(MAX) NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME NULL,
    [EsAbonoExtraordinario] BIT NOT NULL DEFAULT 0,
    [UsuarioRegistro] NVARCHAR(MAX) NULL,
    [ComprobanteUrl] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK_PagoPrestamo_Prestamo] FOREIGN KEY ([PrestamoId]) REFERENCES [dbo].[Prestamo]([Id])
);

-- 9. Tabla: Depositos (depende de Socios, Aportes, Prestamo y Ahorros)
CREATE TABLE [dbo].[Depositos] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SocioId] INT NOT NULL,
    [AporteId] INT NULL,
    [PrestamoId] INT NULL,
    [AhorroId] INT NULL,
    [FechaDeposito] DATE NOT NULL,
    [Monto] DECIMAL(18,2) NOT NULL,
    [TipoDeposito] NVARCHAR(MAX) NOT NULL,
    [MetodoPago] NVARCHAR(MAX) NULL,
    [ReferenciaExterna] NVARCHAR(MAX) NULL,
    [Observaciones] NVARCHAR(MAX) NULL,
    [ComprobanteUrl] NVARCHAR(MAX) NULL,
    [EstaProcesado] BIT NOT NULL DEFAULT 1,
    [UsuarioRegistro] NVARCHAR(MAX) NULL,
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME NULL,
    [UsuarioModificacion] NVARCHAR(MAX) NULL,
    CONSTRAINT [FK_Depositos_Socio] FOREIGN KEY ([SocioId]) REFERENCES [dbo].[Socios]([Id]),
    CONSTRAINT [FK_Depositos_Aporte] FOREIGN KEY ([AporteId]) REFERENCES [dbo].[Aportes]([Id]),
    CONSTRAINT [FK_Depositos_Prestamo] FOREIGN KEY ([PrestamoId]) REFERENCES [dbo].[Prestamo]([Id]),
    CONSTRAINT [FK_Depositos_Ahorro] FOREIGN KEY ([AhorroId]) REFERENCES [dbo].[Ahorros]([Id])
);

-- =============================================
-- 3. ÍNDICES PARA OPTIMIZACIÓN
-- =============================================
-- Crear índices para mejorar performance
CREATE INDEX [IX_Ahorros_SocioId] ON [dbo].[Ahorros]([SocioId]);
CREATE INDEX [IX_Aportes_SocioId] ON [dbo].[Aportes]([SocioId]);
CREATE INDEX [IX_Certificados_SocioId] ON [dbo].[Certificados]([SocioId]);
CREATE INDEX [IX_Depositos_SocioId] ON [dbo].[Depositos]([SocioId]);
CREATE INDEX [IX_Depositos_AporteId] ON [dbo].[Depositos]([AporteId]);
CREATE INDEX [IX_Depositos_PrestamoId] ON [dbo].[Depositos]([PrestamoId]);
CREATE INDEX [IX_Depositos_AhorroId] ON [dbo].[Depositos]([AhorroId]);
CREATE INDEX [IX_PagoPrestamo_PrestamoId] ON [dbo].[PagoPrestamo]([PrestamoId]);
CREATE INDEX [IX_Prestamo_SocioId] ON [dbo].[Prestamo]([SocioId]);

-- =============================================
-- 4. DATOS DE PRUEBA (SEED DATA)
-- =============================================

-- Carga de Socios
SET IDENTITY_INSERT [dbo].[Socios] ON;
INSERT INTO Socios (Id, Cedula, FechaNacimiento, FechaIngreso, Nombre, Apellido, Sexo, EstadoCivil, TipoSangre, Nacionalidad, Telefono, Email, Direccion, Estado) VALUES
(1, '001-0000001-1', '1985-05-15', '2020-01-10', 'Juan Carlos', 'Pérez', 'M', 'Casado', 'O+', 'Dominicano', '809-555-0001', 'juan.perez@email.com', 'Calle A #1, DN', 1),
(2, '001-0000002-2', '1990-08-20', '2020-02-15', 'María Elena', 'Gómez', 'F', 'Soltera', 'A+', 'Dominicana', '809-555-0002', 'maria.gomez@email.com', 'Av. B #2, Santiago', 1),
(3, '001-0000003-3', '1978-03-12', '2020-03-20', 'Pedro', 'Martínez', 'M', 'Divorciado', 'B+', 'Dominicano', '809-555-0003', 'pedro.mtz@email.com', 'Calle C #3, La Vega', 1),
(4, '001-0000004-4', '1995-11-25', '2021-01-05', 'Ana', 'Beltrán', 'F', 'Soltera', 'AB+', 'Dominicana', '809-555-0004', 'ana.bel@email.com', 'Calle D #4, Puerto Plata', 1),
(5, '001-0000005-5', '1982-07-30', '2021-02-10', 'Luis', 'Rodríguez', 'M', 'Casado', 'O-', 'Dominicano', '809-555-0005', 'luis.rod@email.com', 'Calle E #5, Higuey', 1),
(6, '001-0000006-6', '1988-12-05', '2021-03-15', 'Carmen', 'Lora', 'F', 'Viuda', 'A-', 'Dominicana', '809-555-0006', 'carmen.lora@email.com', 'Calle F #6, Moca', 1),
(7, '001-0000007-7', '1992-04-18', '2022-01-20', 'Roberto', 'Sánchez', 'M', 'Soltero', 'B-', 'Dominicano', '809-555-0007', 'roberto.san@email.com', 'Calle G #7, Baní', 1),
(8, '001-0000008-8', '1980-09-10', '2022-02-25', 'Elena', 'Castillo', 'F', 'Casada', 'O+', 'Dominicana', '809-555-0008', 'elena.cas@email.com', 'Calle H #8, Azua', 1),
(9, '001-0000009-9', '1975-06-22', '2022-03-30', 'José', 'Núñez', 'M', 'Casado', 'A+', 'Dominicano', '809-555-0009', 'jose.nun@email.com', 'Calle I #9, Bonao', 1),
(10, '001-0000010-0', '1998-01-14', '2023-01-05', 'Lucía', 'Méndez', 'F', 'Soltera', 'B+', 'Dominicana', '809-555-0010', 'lucia.men@email.com', 'Calle J #10, Mao', 1),
(11, '001-0000011-1', '1983-10-08', '2023-02-10', 'Manuel', 'Ortiz', 'M', 'Divorciado', 'AB-', 'Dominicano', '809-555-0011', 'manuel.ort@email.com', 'Calle K #11, Nagua', 1),
(12, '001-0000012-2', '1991-05-27', '2023-03-15', 'Sofia', 'Reyes', 'F', 'Casada', 'O+', 'Dominicana', '809-555-0012', 'sofia.rey@email.com', 'Calle L #12, Cotuí', 1),
(13, '001-0000013-3', '1986-12-12', '2024-01-20', 'David', 'Blanco', 'M', 'Soltero', 'A-', 'Dominicano', '809-555-0013', 'david.bla@email.com', 'Calle M #13, Dajabón', 1),
(14, '001-0000014-4', '1994-02-28', '2024-02-25', 'Isabel', 'Rojas', 'F', 'Casada', 'B+', 'Dominicana', '809-555-0014', 'isabel.roj@email.com', 'Calle N #14, El Seibo', 1),
(15, '001-0000015-5', '1979-07-15', '2024-03-30', 'Francisco', 'Silva', 'M', 'Viudo', 'O-', 'Dominicano', '809-555-0015', 'fran.sil@email.com', 'Calle O #15, Pedernales', 1),
(16, '001-0000016-6', '1993-09-03', '2024-04-05', 'Valeria', 'Núñez', 'F', 'Soltera', 'A+', 'Dominicana', '809-555-0016', 'valeria.nun@email.com', 'Calle P #16, Samaná', 0);
SET IDENTITY_INSERT [dbo].[Socios] OFF;

-- Usuarios del Sistema
SET IDENTITY_INSERT [dbo].[User] ON;
INSERT INTO [User] (Id, Email, PasswordHash, Role, Estado, Nombre, Apellido) VALUES
(1, 'admin@coopcontrol.com', '$2a$11$vgF3arqABJWvovUVo06gyOeM1EexN3GdyuL9lAANhIlU7eNFC33lS', 'Admin', 1, 'Administrador', 'del Sistema'),
(2, 'cajero1@coopcontrol.com', '$2a$11$mGp0WP4pDW8HgSLtGDDSP.tA8KVC3oAvRo.wPlOsEI9Bzt/bpH5ZK', 'Cajero', 1, 'María', 'Fernández'),
(3, 'cajero2@coopcontrol.com', '$2a$11$llyrMeOsrqovQ95vFuDzXuOvumVaD/89n1pVBGhHkb.QctamBBuN6', 'Cajero', 1, 'Carlos', 'Ramírez'),
(4, 'juan.perez@email.com', '$2a$11$SHUYy3yjXgQzWPAixE9p9eHnDBrTm5SrT8hKg/k0m02VW9QJgsZhC', 'Socio', 1, 'Juan Carlos', 'Pérez'),
(5, 'maria.gomez@email.com', '$2a$11$nnhWFWzZ3lACo3ngj2og6eTg3opZ2s8vlpi7onBhc3OC/B58PZvR2', 'Socio', 1, 'María Elena', 'Gómez');
SET IDENTITY_INSERT [dbo].[User] OFF;

-- Configuraciones Iniciales
INSERT INTO Configuraciones (Clave, Valor, Descripcion) VALUES
('MultiplicadorPrestamo', '3', 'Capacidad de préstamo basada en aportes'),
('TasaInteresAhorroNormal', '2.5', 'Tasa base para ahorros');

-- Ahorros
SET IDENTITY_INSERT [dbo].[Ahorros] ON;
INSERT INTO Ahorros (Id, SocioId, Tipo, Saldo, TasaInteresAnual, FechaCreacion, FechaVencimiento, Notas) VALUES
(1, 1, 1, 50000.00, 2.50, '2020-01-10 00:00:00', NULL, 'Ahorro Normal'),
(2, 1, 3, 15000.00, 5.00, '2023-01-10 00:00:00', '2023-12-15 00:00:00', 'Ahorro Navideño'),
(3, 2, 1, 25000.00, 2.50, '2020-02-15 00:00:00', NULL, 'Ahorro Normal'),
(4, 2, 8, 100000.00, 8.00, '2023-06-01 00:00:00', '2024-06-01 00:00:00', 'Inversión 1A'),
(5, 3, 2, 12000.00, 3.50, '2020-03-20 00:00:00', NULL, 'Ahorro Especial'),
(6, 4, 4, 8000.00, 4.00, '2021-01-05 00:00:00', '2021-08-30 00:00:00', 'Ahorro Escolar'),
(7, 5, 5, 5000.00, 4.50, '2021-02-10 00:00:00', NULL, 'Ahorro Infantil'),
(8, 6, 6, 20000.00, 6.00, '2021-03-15 00:00:00', '2022-03-15 00:00:00', 'Club de Viajes'),
(9, 7, 7, 50000.00, 7.00, '2023-01-20 00:00:00', '2023-07-20 00:00:00', 'Inversión 6M'),
(10, 8, 9, 200000.00, 10.00, '2022-02-25 00:00:00', '2025-02-25 00:00:00', 'Inversión 3A'),
(11, 9, 1, 30000.00, 2.50, '2022-03-30 00:00:00', NULL, NULL),
(12, 10, 3, 10000.00, 5.00, '2023-01-05 00:00:00', '2023-12-15 00:00:00', NULL),
(13, 11, 2, 15000.00, 3.50, '2023-02-10 00:00:00', NULL, NULL),
(14, 12, 1, 40000.00, 2.50, '2023-03-15 00:00:00', NULL, NULL),
(15, 13, 8, 75000.00, 8.00, '2024-01-20 00:00:00', '2025-01-20 00:00:00', NULL),
(16, 14, 1, 20000.00, 2.50, '2024-02-25 00:00:00', NULL, NULL),
(17, 15, 3, 5000.00, 5.00, '2024-03-30 00:00:00', '2024-12-15 00:00:00', NULL),
(18, 1, 2, 10000.00, 3.50, '2024-04-01 00:00:00', NULL, NULL),
(19, 2, 7, 30000.00, 7.00, '2024-04-01 00:00:00', '2024-10-01 00:00:00', NULL),
(20, 3, 4, 2000.00, 4.00, '2024-04-01 00:00:00', '2024-08-30 00:00:00', NULL);
SET IDENTITY_INSERT [dbo].[Ahorros] OFF;

-- Aportes
SET IDENTITY_INSERT [dbo].[Aportes] ON;
INSERT INTO Aportes (Id, SocioId, Fecha, Monto, Concepto, Tipo) VALUES
(1, 1, '2020-01-10', 1000.00, 'Aporte Inicial', 'Ordinario'),
(2, 2, '2020-02-15', 1000.00, 'Aporte Inicial', 'Ordinario'),
(3, 3, '2020-03-20', 1000.00, 'Aporte Inicial', 'Ordinario'),
(4, 4, '2021-01-05', 1000.00, 'Aporte Inicial', 'Ordinario'),
(5, 5, '2021-02-10', 1000.00, 'Aporte Inicial', 'Ordinario'),
(6, 6, '2021-03-15', 1000.00, 'Aporte Inicial', 'Ordinario'),
(7, 7, '2022-01-20', 1000.00, 'Aporte Inicial', 'Ordinario'),
(8, 8, '2022-02-25', 1000.00, 'Aporte Inicial', 'Ordinario'),
(9, 9, '2022-03-30', 1000.00, 'Aporte Inicial', 'Ordinario'),
(10, 10, '2023-01-05', 1000.00, 'Aporte Inicial', 'Ordinario'),
(11, 1, '2020-02-10', 500.00, 'Aporte Mensual', 'Ordinario'),
(12, 1, '2020-03-10', 500.00, 'Aporte Mensual', 'Ordinario'),
(13, 1, '2020-05-10', 15000.00, 'Aporte Extraordinario Anual', 'Extraordinario'),
(14, 2, '2020-03-15', 500.00, 'Aporte Mensual', 'Ordinario'),
(15, 2, '2020-12-20', 10000.00, 'Aporte Extraordinario', 'Extraordinario'),
(16, 3, '2020-04-20', 500.00, 'Aporte Mensual', 'Ordinario'),
(17, 4, '2021-02-05', 500.00, 'Aporte Mensual', 'Ordinario'),
(18, 5, '2021-03-10', 500.00, 'Aporte Mensual', 'Ordinario'),
(19, 6, '2021-04-15', 500.00, 'Aporte Mensual', 'Ordinario'),
(20, 7, '2022-02-20', 500.00, 'Aporte Mensual', 'Ordinario'),
(21, 8, '2022-03-25', 500.00, 'Aporte Mensual', 'Ordinario'),
(22, 9, '2022-04-30', 500.00, 'Aporte Mensual', 'Ordinario'),
(23, 10, '2023-02-05', 500.00, 'Aporte Mensual', 'Ordinario'),
(24, 11, '2023-02-10', 1000.00, 'Aporte Inicial', 'Ordinario'),
(25, 12, '2023-03-15', 1000.00, 'Aporte Inicial', 'Ordinario'),
(26, 13, '2024-01-20', 1000.00, 'Aporte Inicial', 'Ordinario'),
(27, 14, '2024-02-25', 1000.00, 'Aporte Inicial', 'Ordinario'),
(28, 15, '2024-03-30', 1000.00, 'Aporte Inicial', 'Ordinario'),
(29, 1, '2021-01-10', 500.00, 'Aporte Mensual', 'Ordinario'),
(30, 1, '2022-01-10', 500.00, 'Aporte Mensual', 'Ordinario'),
(31, 2, '2021-01-15', 500.00, 'Aporte Mensual', 'Ordinario'),
(32, 2, '2022-01-15', 500.00, 'Aporte Mensual', 'Ordinario');
SET IDENTITY_INSERT [dbo].[Aportes] OFF;

-- Préstamos
SET IDENTITY_INSERT [dbo].[Prestamo] ON;
INSERT INTO Prestamo (Id, SocioId, Monto, TasaInteres, PlazoMeses, FechaPrestamo, FechaDevolucion, Estado, Observaciones, MetodoPagoPreferido, TipoPrestamo) VALUES
(1, 1, 30000.00, 18.00, 12, '2023-01-15', '2024-01-15', 'Pagado', 'Préstamo Personal', 'Efectivo', 'Personal'),
(2, 2, 50000.00, 15.00, 24, '2023-02-15', '2025-02-15', 'Activo', 'Préstamo para estudios', 'Transferencia', 'Educativo'),
(3, 3, 100000.00, 12.00, 36, '2022-05-10', '2025-05-10', 'Activo', 'Mejora de Vivienda', 'Transferencia', 'Hipotecario'),
(4, 4, 20000.00, 20.00, 6, '2023-08-01', '2024-02-01', 'Pagado', 'Emergencia médica', 'Efectivo', 'Emergencia'),
(5, 5, 15000.00, 18.00, 12, '2023-03-20', '2024-03-20', 'Vencido', 'En mora', 'Efectivo', 'Personal'),
(6, 6, 25000.00, 15.00, 18, '2023-06-15', '2024-12-15', 'Activo', NULL, 'Efectivo', 'Comercial'),
(7, 7, 40000.00, 12.00, 24, '2023-09-20', '2025-09-20', 'Activo', NULL, 'Transferencia', 'Personal'),
(8, 8, 200000.00, 10.00, 60, '2023-01-25', '2028-01-25', 'Activo', 'Compra Vehículo', 'Transferencia', 'Vehículo'),
(9, 9, 10000.00, 24.00, 4, '2023-11-01', '2024-03-01', 'Vencido', 'Incumplimiento', 'Efectivo', 'Consumo'),
(10, 10, 5000.00, 18.00, 6, '2024-01-05', '2024-07-05', 'Activo', NULL, 'Efectivo', 'Personal'),
(11, 11, 15000.00, 15.00, 12, '2024-02-10', '2025-02-10', 'Activo', NULL, 'Efectivo', 'Personal'),
(12, 12, 30000.00, 12.00, 24, '2024-03-15', '2026-03-15', 'Activo', NULL, 'Transferencia', 'Personal'),
(13, 13, 50000.00, 15.00, 36, '2024-04-01', '2027-04-01', 'Activo', NULL, 'Efectivo', 'Personal'),
(14, 1, 20000.00, 18.00, 12, '2022-01-10', '2023-01-10', 'Pagado', NULL, 'Efectivo', 'Personal'),
(15, 2, 10000.00, 15.00, 12, '2024-04-10', '2025-04-10', 'Activo', NULL, 'Efectivo', 'Personal'),
(16, 14, 25000.00, 18.00, 12, '2024-05-01', NULL, 'Pendiente', 'En construcción', NULL, NULL);
SET IDENTITY_INSERT [dbo].[Prestamo] OFF;

-- Pagos de Préstamo
SET IDENTITY_INSERT [dbo].[PagoPrestamo] ON;
INSERT INTO PagoPrestamo (Id, PrestamoId, FechaPago, Monto, Metodo, Observaciones, EsAbonoExtraordinario, UsuarioRegistro) VALUES
(1, 1, '2023-02-15', 2500.00, 'Efectivo', 'Cuota 1', 0, 'cajero1'),
(2, 1, '2023-03-15', 2500.00, 'Efectivo', 'Cuota 2', 0, 'cajero1'),
(3, 1, '2023-04-15', 2500.00, 'Efectivo', 'Cuota 3', 0, 'cajero1'),
(4, 1, '2023-05-15', 2500.00, 'Efectivo', 'Cuota 4', 0, 'cajero1'),
(5, 1, '2023-06-15', 2500.00, 'Efectivo', 'Cuota 5', 0, 'cajero2'),
(6, 1, '2023-07-15', 2500.00, 'Efectivo', 'Cuota 6', 0, 'cajero2'),
(7, 1, '2023-08-15', 2500.00, 'Efectivo', 'Cuota 7', 0, 'cajero2'),
(8, 1, '2023-09-15', 2500.00, 'Efectivo', 'Cuota 8', 0, 'cajero1'),
(9, 1, '2023-10-15', 2500.00, 'Efectivo', 'Cuota 9', 0, 'cajero1'),
(10, 1, '2023-11-15', 2500.00, 'Efectivo', 'Cuota 10', 0, 'cajero1'),
(11, 1, '2023-12-15', 2500.00, 'Efectivo', 'Cuota 11', 0, 'cajero2'),
(12, 1, '2024-01-15', 2500.00, 'Efectivo', 'Cuota 12 Final', 0, 'cajero2'),
(13, 2, '2023-03-15', 2100.00, 'Transferencia', 'Cuota 1', 0, 'admin'),
(14, 2, '2023-04-15', 2100.00, 'Transferencia', 'Cuota 2', 0, 'admin'),
(15, 2, '2023-05-15', 10000.00, 'Efectivo', 'Abono Extra', 1, 'cajero1'),
(16, 2, '2023-06-15', 2100.00, 'Transferencia', 'Cuota 3', 0, 'admin'),
(17, 2, '2023-07-15', 2100.00, 'Transferencia', 'Cuota 4', 0, 'admin'),
(18, 2, '2023-08-15', 2100.00, 'Transferencia', 'Cuota 5', 0, 'admin'),
(19, 2, '2023-09-15', 2100.00, 'Transferencia', 'Cuota 6', 0, 'admin'),
(20, 2, '2023-10-15', 2100.00, 'Transferencia', 'Cuota 7', 0, 'admin'),
(21, 2, '2023-11-15', 2100.00, 'Transferencia', 'Cuota 8', 0, 'admin'),
(22, 2, '2023-12-15', 2100.00, 'Transferencia', 'Cuota 9', 0, 'admin'),
(23, 2, '2024-01-15', 2100.00, 'Transferencia', 'Cuota 10', 0, 'admin'),
(24, 3, '2022-06-10', 3500.00, 'Transferencia', 'Cuota 1', 0, 'admin'),
(25, 3, '2022-07-10', 3500.00, 'Transferencia', 'Cuota 2', 0, 'admin'),
(26, 3, '2022-08-10', 3500.00, 'Transferencia', 'Cuota 3', 0, 'admin'),
(27, 3, '2022-09-10', 3500.00, 'Transferencia', 'Cuota 4', 0, 'admin'),
(28, 3, '2022-10-10', 3500.00, 'Transferencia', 'Cuota 5', 0, 'admin'),
(29, 10, '2024-02-05', 1000.00, 'Efectivo', 'Cuota 1', 0, 'cajero1'),
(30, 10, '2024-03-05', 1000.00, 'Efectivo', 'Cuota 2', 0, 'cajero1'),
(31, 11, '2024-03-10', 1500.00, 'Efectivo', 'Cuota 1', 0, 'cajero2'),
(32, 11, '2024-04-10', 1500.00, 'Efectivo', 'Cuota 2', 0, 'cajero2');
SET IDENTITY_INSERT [dbo].[PagoPrestamo] OFF;

-- Depósitos
SET IDENTITY_INSERT [dbo].[Depositos] ON;
INSERT INTO Depositos (Id, SocioId, AhorroId, PrestamoId, AporteId, FechaDeposito, Monto, TipoDeposito, MetodoPago, ReferenciaExterna, Observaciones, EstaProcesado, UsuarioRegistro) VALUES
(1, 1, 1, NULL, NULL, '2024-04-01', 500.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero1'),
(2, 2, 3, NULL, NULL, '2024-04-01', 1000.00, 'AhorroLibre', 'Transferencia', NULL, NULL, 1, 'cajero1'),
(3, 3, 5, NULL, NULL, '2024-04-01', 200.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(4, 1, NULL, 11, NULL, '2024-04-05', 1500.00, 'PagoPrestamo', 'Efectivo', NULL, NULL, 1, 'cajero1'),
(5, 4, NULL, NULL, NULL, '2024-04-05', 50000.00, 'Inversión', 'Transferencia', NULL, NULL, 1, 'admin'),
(6, 5, NULL, NULL, NULL, '2024-04-05', 100.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(7, 6, 8, NULL, NULL, '2024-04-10', 1000.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero1'),
(8, 7, 9, NULL, NULL, '2024-04-10', 500.00, 'AhorroLibre', 'Transferencia', NULL, NULL, 1, 'cajero1'),
(9, 8, NULL, 8, NULL, '2024-04-10', 4000.00, 'PagoPrestamo', 'Transferencia', NULL, NULL, 1, 'admin'),
(10, 9, NULL, NULL, NULL, '2024-04-15', 200.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(11, 10, NULL, 10, NULL, '2024-04-15', 1000.00, 'PagoPrestamo', 'Efectivo', NULL, NULL, 1, 'cajero1'),
(12, 11, 13, NULL, NULL, '2024-04-15', 300.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(13, 12, NULL, NULL, NULL, '2024-04-20', 1000.00, 'AhorroLibre', 'Transferencia', NULL, NULL, 1, 'cajero1'),
(14, 13, NULL, NULL, NULL, '2024-04-20', 500.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(15, 14, NULL, NULL, NULL, '2024-04-20', 100.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(16, 15, NULL, NULL, NULL, '2024-04-25', 1000.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero1'),
(17, 1, NULL, 11, NULL, '2024-04-25', 1500.00, 'PagoPrestamo', 'Transferencia', NULL, NULL, 1, 'admin'),
(18, 2, NULL, NULL, NULL, '2024-04-25', 2000.00, 'Inversión', 'Transferencia', NULL, NULL, 1, 'admin'),
(19, 3, NULL, 3, NULL, '2024-04-30', 3500.00, 'PagoPrestamo', 'Transferencia', NULL, NULL, 1, 'admin'),
(20, 4, NULL, NULL, NULL, '2024-04-30', 500.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero2'),
(21, 5, NULL, NULL, NULL, '2024-04-30', 1000.00, 'AhorroLibre', 'Efectivo', NULL, NULL, 1, 'cajero1');
SET IDENTITY_INSERT [dbo].[Depositos] OFF;