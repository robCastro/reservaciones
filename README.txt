Dependencias:
    - SQL Server 2008 R2 o superior
    - .Net Core SDK 3.1 o superior (https://dotnet.microsoft.com/download) Testear con el comandos
        dotnet --version
      
Puntos Interesantes en el codigo:
- Configuracion JWT: ./Startup.cs
- Generadores de JWT: Api/JwtCustomModels/* y Api/Controllers/JwtLoginController.cs
- API para CRUD Reservaciones: Api/Controllers/SalaController.cs
- Validaciones para no chocar reservaciones: Models/CustomValidations/ReservacionDesdeValidator.cs
- CRUD de Sala: Pages/Sala/*

Instalación:

1) Abra el documento appsettings.json

2) En la propiedad "ReservacionDevConnection" cambie el nombre del server por el de su computador.

3) Ejecutar el archivo script.sql en sql server, asegurarse de poder loggearse con permisos 
para crear bases de datos y que no haya otra base de datos con nombre reservacion_roberto_castro.

4) Acceder a la ventana de comandos y navegar a la carpeta raíz de este proyecto

5) Ejecutar los siguientes comandos en el orden dado:

    dotnet dev-certs https --trust (Presionar sí en la ventana que emergerá)

    dotnet tool install --global dotnet-ef

    dotnet ef database update (Este comando tardará un rato, está generando la estructura de tablas)

    dotnet restore (Instalará todas las dependencias necesarias)

    dotnet run (Asegurarse que el puerto 5000 y 5001 están libres)



Instrucciones de Uso:

SE RECOMIENDA USAR CHROME, FIREFOX NO CUENTA CON INPUTS DATETIME, NO ME ALCANZÓ EL TIEMPO PARA
IMPLEMENTAR DATEPICKER CON JQUERY UI

Al iniciar la aplicación en el paso anterior, se corrió el archivo Models/Seeder.cs por lo que ya hay
usuarios, salas y reservaciones. 

Las Razor Pages pruebelas en su navegador https://localhost:5001/ estas implementan https
Para evitar problemas con su cliente rest (postman por ejemplo), pruebe la web api en 
http://localhost:5000/ la cual no implementa https

1) Testear CRUD de Salas (Razor Pages):

    1.1) En la barra de navegacion superior clic en Login
         Iniciar sesión como Admin el usuario y la contraseña siguientes:
                admin@localhost
                passReservacion2020
    
    1.2) En la barra de navegación superior se encuentra el enlace a Salas.
         La seguridad solo permite usuarios con rol administrador en estas paginas.

2) Testear CRUD de Salas (Web Api):
    2.1) Utilizando el cliente rest de su preferencia haga una peticion POST a
         
         http://localhost:5000/api/JwtLogin

         Su body deberá ser el siguiente:

                {
                    "UserName":"admin@localhost",
                    "Password":"passReservacion2020"
                }
        Recuerde utilizar header application/json

    2.2) Deberá tener ahora su token, copie el valor del token que está entre comillas.
         Para probar la API deberá poner el header Authorization, con valor Bearer token.
         Ejemplo: Authorization: Bearer jloppoeriyp2f35.kfhogiowoitop3.iehglskndlkndnnf
         ATENCION EL TOKEN TIENE DURACION DE UNA HORA, AL CABO DE LA HORA DEBE GENERAR OTRO

    2.3) Puede probar la API dirigiendo peticiones GET, POST, PUT y DELETE a 
         http://localhost:5000/api/Sala Si no ha provisto el token en los headers, recibirá Unauthorized
         GET, PUT y DELETE admiten un parametro numerico para ejectuar acciones en una sala en especifico
         Ejemplo de Body:
               {
                    "nombre": "Sala de Juntas",
                    "descripcionUbicacion": "Edificio 1"
               }

    2.4) Su Token conlleva su rol, así que puede testear peticiones hacia 
         http://localhost:5000/api/Reservacion las cuales recibirá Unauthorized ya que allí solamente
         tienen acceso usuarios con rol Normal
         

3) Testear CRUD de Reservaciones (Razor Pages):
    3.1) Iniciar Sesion como usuario normal, tiene 3 usuarios posibles, user1, user2, user3
         todos con la misma contraseña, ejemplo:
                user1@localhost
                passReservacion2020
    
    3.2) En la barra de navegación superior se encuentra el enlace a Reservaciones.
         La seguridad solo permite usuarios con rol Normal en estas paginas.

4) Testear CRUD de Reservacion (Web Api):
    4.1) Utilizando el cliente rest de su preferencia haga una peticion POST a
         
         http://localhost:5000/api/JwtLogin

         Su body deberá ser el siguiente:

                {
                    "UserName":"user1@localhost",
                    "Password":"passReservacion2020"
                }
        Recuerde utilizar header application/json, puede probar tambien con user2@localhost
        o user3@localhost; ambos con la misma contraseña.

    4.2) Deberá obtener ahora su token, copie el valor del token que está entre comillas.
         Para probar la API deberá poner el header Authorization, con valor Bearer token.
         Ejemplo: Authorization: Bearer jloppoeriyp2f35.kfhogiowoitop3.iehglskndlkndnnf
         ATENCION EL TOKEN TIENE DURACION DE UNA HORA, AL CABO DE LA HORA DEBE GENERAR OTRO

    4.3) Puede probar la API dirigiendo peticiones GET, POST, PUT y DELETE a 
         http://localhost:5000/api/Reservacion
         GET, PUT y DELETE admiten un parametro numerico para ejectuar acciones en una mesa en especifico
         Las reglas de negocio especificadas en los requerimientos han sido aplicadas, tengalo
         en cuenta al definir fechas. Asegurese de usar formatos de fechas estandares para el body. 
         Ejemplo
               {
                    "reservacionDesde": "2020-04-25 11:39:09",
                    "reservacionHasta": "2020-04-25 15:39:09",
                    "salaId": 1
               }
          La fecha de creacion de la reservacion, asi como el id del usuario son registrados en 
          el backend, puede ver el codigo en Controller/ReservacionController.cs

    4.4) Su Token conlleva su rol, así que puede testear peticiones hacia 
         http://localhost:5000/api/Sala las cuales recibirá un forbidden ya que allí solamente
         tienen acceso usuarios con rol Administrador
    