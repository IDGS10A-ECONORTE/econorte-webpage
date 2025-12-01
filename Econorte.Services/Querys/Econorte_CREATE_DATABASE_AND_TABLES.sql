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

--Métodods
CREATE TABLE Methods(
	Id_Method INT IDENTITY NOT NULL CONSTRAINT PK_METHODS PRIMARY KEY,
	[Name] VARCHAR(MAX) NOT NULL,
);

--APIs
CREATE TABLE APIs (
	Id_API INT IDENTITY NOT NULL CONSTRAINT PK_APIs PRIMARY KEY,
	[Name] VARCHAR(MAX) NOT NULL,
	[URL_Dev] VARCHAR(MAX) NOT NULL,
	[URL_Prod] VARCHAR(MAX) NOT NULL,
	fk_Method INT NOT NULL CONSTRAINT FK_METHODS_APIS FOREIGN KEY REFERENCES Methods(Id_Method),
);

--Sensors
CREATE TABLE Sensors (
	Id_Sensor INT IDENTITY NOT NULL CONSTRAINT PK_Sensors PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
	fk_User INT NOT NULL CONSTRAINT FK_SENSORS_USERS FOREIGN KEY REFERENCES Users(Id_User),
);

--SensorsParameters
CREATE TABLE SensorsParameters (
	Id_Parameters INT IDENTITY NOT NULL CONSTRAINT PK_SENSORSPARAMETERS PRIMARY KEY,
	fk_Sensor INT NOT NULL CONSTRAINT FK_PARAMETERS_SENSORS FOREIGN KEY REFERENCES Sensors(Id_Sensor),
	[Date] DATETIME DEFAULT GETDATE(),
    Temperature FLOAT,
    Humidity FLOAT,
    Gas_Level INT,
    Vibration INT,
    Earthquake_Status BIT,
    Fire_Status BIT,
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

SELECT * FROM Methods;
--Methods
INSERT INTO Methods VALUES
('Get'),
('Post'),
('Delete'),
('Put'),
('Patch');

--APIs
INSERT INTO APIs VALUES 
('Login','https://localhost:7168/Services/Login','/Services/Login',2),
('Logout','https://localhost:7168/Services/Logout','/Services/Logout',2),
('Register','https://localhost:7168/Services/Register','/Services/Register',2),
('Register','https://localhost:7168/Services/CloseSessions','/Services/CloseSessions',2),
('Register','https://localhost:7168/Services/RegisterSensor','/Services/RegisterSensor',2),
('Register','https://localhost:7168/Services/GetSensors/','/Services/GetSensors/',1),
('Register','https://localhost:7168/Services/DeleteSensor/','/Services/DeleteSensor/',3),
('Register','https://localhost:7168/Services/UpdateSensor','/Services/UpdateSensor',4);

--Actualizar Endpoints para cuando esté en Prod
  UPDATE APIs SET URL_Prod = CASE
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/Login' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/Login'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/Logout' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/Logout'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/Register' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/Register'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/CloseSessions' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/CloseSessions'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/RegisterSensor' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/RegisterSensor'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/GetSensors/' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/GetSensors/'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/DeleteSensor/' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/DeleteSensor/'
  WHEN URL_Dev LIKE 'https://localhost:7168/Services/UpdateSensor' THEN 'https://econorteservicesv1-dya3g3ggg6cudqhp.canadaeast-01.azurewebsites.net/Services/UpdateSensor'
  ELSE URL_Prod END
	WHERE Id_API IN(1,2,3,4,5,6,7,8);