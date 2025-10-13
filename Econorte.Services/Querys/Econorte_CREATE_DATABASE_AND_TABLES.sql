CREATE DATABASE Econorte_Dev;
USE Econorte_Dev;

--Roles
CREATE TABLE Roles 
(
	Id_Role INT IDENTITY NOT NULL CONSTRAINT PK_ROLES PRIMARY KEY,
	[Name] VARCHAR (MAX) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
);

--Usuarios
CREATE TABLE Users 
(
	Id_User INT IDENTITY NOT NULL CONSTRAINT PK_USERS PRIMARY KEY,
	[Name] VARCHAR(MAX) NOT NULL,
	Email VARCHAR(MAX) NOT NULL,
	[Password] VARCHAR(MAX) NOT NULL,
	Phone VARCHAR (MAX) NULL,
	fk_Role INT NOT NULL CONSTRAINT FK_USERS_ROLES FOREIGN KEY REFERENCES Roles(Id_Role),
	Active BIT NOT NULL DEFAULT 1,
	[Login] BIT NOT NULL DEFAULT 0,
	LastLog DATE NULL
);

--APIs
CREATE TABLE APIs (
	Id_API INT IDENTITY NOT NULL CONSTRAINT PK_APIs PRIMARY KEY,
	[Name] VARCHAR(MAX) NOT NULL,
	[URL_Dev] VARCHAR(MAX) NOT NULL,
	[URL_Prod] VARCHAR(MAX) NOT NULL,
	IsGet BIT NOT NULL DEFAULT 0,
	IsPost BIT NOT NULL DEFAULT 0,
);

--Sensors
CREATE TABLE Sensors (
	Id_Sensor INT IDENTITY NOT NULL CONSTRAINT PK_Sensors PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
    [Date] DATETIME DEFAULT GETDATE(),
	fk_User INT NOT NULL CONSTRAINT FK_SENSORS_USERS FOREIGN KEY REFERENCES Users(Id_User),
    Temperature FLOAT,
    Humidity FLOAT,
    Gas_Level INT,
    Vibration INT,
    Earthquake_Status BIT,
    Fire_Status BIT,
    Alarm_Intensity TINYINT
);

--Roles
INSERT INTO [Roles] (Name) VALUES
('Admin'),
('Public');

--Users
INSERT INTO Users VALUES
('MARTINEZ FLORES, ELIAS RAFAEL','21017@virtual.utsc.edu.mx','21017',NULL,1,1,0,NULL),
('IBARRA VELAZQUEZ, JOSE MIGUEL ANGEL','15830@virtual.utsc.edu.mx','15830',NULL,1,1,0,NULL),
('SALAS MENDOZA, FABIAN','18307@virtual.utsc.edu.mx','18307',NULL,1,1,0,NULL),
('SANTIAGO GALLINDO, SAULO','19401@virtual.utsc.edu.mx','19401',NULL,1,1,0,NULL),
('GUZMAN PEREZ, JORDAN YAREL','14236@virtual.utsc.edu.mx','14236',NULL,1,1,0,NULL);

--APIs
INSERT INTO APIs VALUES 
('Login','https://localhost:32775/Services/Login','/Services/Login',0,1),
('Logout','https://localhost:32775/Services/Logout','/Services/Logout',0,1),
('Register','https://localhost:32775/Services/Register','/Services/Register',0,1);

--Actualizar Endpoints para cuando esté en Prod
UPDATE APIs SET URL_Prod = CASE
	WHEN URL_Prod = '/Services/Login' THEN '-' 
	WHEN URL_Prod = '/Services/Logout' THEN '-'
	WHEN URL_Prod = '/Services/Register' THEN '-'
	ELSE URL_Prod
	END
	WHERE URL_Dev IN(
	'/Services/Login',
	'/Services/Logout',
    '/Services/Register'
	);