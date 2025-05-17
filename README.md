ZKTECO ZK9500 Windows Form App

1. Edita el archivo App.config con la configuracion de tu SERVER y BASE DE DATOS
    <value>Server=(localdb)\MSSQLLocalDB;Database=TUDATABASE;Integrated Security=True;TrustServerCertificate=True;</value>

3. Crea tu tabla

CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Huella] [varbinary](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

