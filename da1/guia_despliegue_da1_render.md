**Guía de despliegue de DA1**

**Introducción:**

[Cloud Computing](https://cloud.google.com/learn/what-is-cloud-computing?hl=es), o computación en la nube, es un modelo de entrega de servicios de computación (como servidores, almacenamiento, bases de datos, redes, software, etc.) a través de Internet. En lugar de instalar y mantener estos servicios en una computadora personal o servidor físico, se accede a ellos cuando se necesitan.

**Beneficios de usar la nube**

-  Accesible desde cualquier lugar: solo necesitas Internet para acceder a tus servicios.  
-  Modelo de pago por uso: no es necesario comprar servidores físicos costosos. Solo se pagan los recursos utilizados.  
- Escalabilidad y flexibilidad: podés aumentar o disminuir los recursos según la demanda (por ejemplo, más capacidad cuando tu app tiene muchos usuarios).  
- Seguridad y respaldo: los proveedores ofrecen mecanismos de seguridad integrados, copias de seguridad automáticas y recuperación ante desastres.  
- Despliegue rápido: se puede tener una base de datos o una app corriendo en cuestión de minutos.

**Requisitos previos:**

1. Contar con una cuenta de [**Azure**](https://azure.microsoft.com/es-es/get-started/azure-portal/) **For Students**. La facultad cuenta con un convenio que otorga 100 dólares anuales en créditos de Azure. Se puede activar a través del siguiente enlace:  
   [https://azure.microsoft.com/es-es/free/students](https://azure.microsoft.com/es-es/free/students)

   **Es importante que el registro se realice con el mail de Microsoft de la facultad (fi365)**. 

   **IMPORTANTE: Al ser créditos limitados, es necesario que se eliminen todos los recursos cuando ya no vayan a ser utilizados. Sobre el final de esta guía se especifica cómo se puede realizar.**

2. Contar con Docker instalado.

## Creación del servidor SQL (SQL Server):

1. Ingresar a Azure Portal: <https://portal.azure.com/>. Es importante haber habilitado la suscripción **Azure For Students**.
2. Seleccionar “Crear un recurso” para proceder con la creación del **Servidor de SQL**, en donde se encontrará la base de datos a crear:

![](./assets/image10.png)
   
Una vez seleccionada esa opción, buscamos SQL Server y seleccionamos la opción de **SQL server (logical server)**:

![](./assets/image19.png)

3. Se procederá con la configuración del servidor. Para ello se selecciona en **Suscripción** “Azure for Students” y en **Grupo de recursos** seleccionamos crear uno nuevo y le asignamos un nombre a elección. 

Un recurso de Azure es cualquier servicio que puedas crear o usar dentro de la plataforma Azure, como una máquina virtual, una base de datos o una cuenta de almacenamiento.

Un grupo de recursos es un contenedor lógico que agrupa varios recursos relacionados para facilitar su administración, monitoreo y control de acceso. Todos los recursos dentro de un grupo comparten el mismo ciclo de vida.


![](./assets/image1.png)

En “Detalles del servidor”, le asignamos un nombre a nuestro servidor y en **región seleccionamos West US 2.**

![](./assets/image18.png)

Como método de autenticación seleccionamos **Uso de la autenticación de SQL**. **Es importante recordar el usuario y la contraseña** (ya que las necesitaremos luego. Se sugiere utilizar una contraseña segura para evitar posibles ataques).

![](./assets/image21.png) 

4. En la sección de Redes, **habilitar las Reglas de firewall**:

![](./assets/image5.png)
   
**Este paso es fundamental para que el servidor de SQL pueda interactuar con la aplicación.**

4. Verificar la configuración previa a crear el recurso:

![](./assets/image4.png)

![](./assets/image13.png)

1. Seleccionar **Crear** y esperar a que se cree el recurso. Esto puede tomar unos minutos.

![](./assets/image7.png)

**Creación de la base de datos SQL:**

1\. En la home de Azure Portal (https://portal.azure.com/#home), seleccionamos el recurso de SQL Server recién creado. Seleccionamos la opción de Crear base de datos.

![](./assets/image15.png)

2\. Configurar la base de datos. Seleccionar Apply Offer para que la base de datos sea gratuita:

![](./assets/image20.png)

![](./assets/image9.png)

Acá es importante considerar que este plan gratuito presenta limitaciones. El plan incluye 100.000 *vCore-seconds* por mes, lo que equivale al uso de 1 núcleo virtual de CPU durante 27,7 horas (de tiempo de uso, no tiempo real). Esta cuota **solo se consume cuando hay actividad en la base de datos** (consultas, escritura, procesamiento). Si no se usa, **no se gasta**. Es clave monitorear el uso y optimizar las consultas, ya que al superar el límite pueden aplicarse cargos adicionales o limitarse el servicio.

**Alternativamente, si no se puede acceder al plan gratuito, puede utilizarse esta configuración que no presenta estas limitaciones, pero va a consumir 4 créditos al mes (de los incluidos en el plan de Azure):**

![](./assets/image8.png)

![](./assets/image17.png)

![](./assets/image11.png)


Es **importante** seleccionar como entorno de carga de trabajo la opción de **Implementación** para evitar gastos elevados. Si se seleccionó este plan, validar en esa misma pantalla que los costos sean similares a los siguientes previo a la creación de la base de datos (recordar que son los créditos que se descontarán a los 100 incluidos en el plan de estudiantes):

![](./assets/image14.png)

4\. Una vez configurada la base de datos, seleccionar **Revisar y crear,** y proceder con la creación del recurso. Esto puede tomar unos minutos.

**Consideraciones**

5\. Desde Azure Portal ingresar al recurso de la base de datos recién creada. 

1. El **Connection String** se puede obtener en la sección de **Configuración -> Cadenas de conexión**. El string a utilizar es el correspondiente a la autenticación de SQL. **Es importante modificar el atributo de Password con la contraseña que se ingresó al momento de crear el SQL Server.

![](./assets/image23.png)

**Importante:** Si se desea utilizar esta base de datos desde el proyecto se deberá modificar el archivo appsettings.json. Esto es optativo y nos permitirá acceder a la base de datos recién creada (que se encuentra en la nube), incluso cuando se levanta la aplicación de forma local (sin despliegue).

1. Se pueden **ejecutar scripts de SQL en la sección de Editor de Consultas**:


![](./assets/image12.png)

Previamente nos va a requerir autenticación para realizar las consultas. Las credenciales a utilizar son las mismas creadas para la autenticación al momento de crear el SQL Server.

**IMPORTANTE: Una vez que no se vaya a utilizar más este recurso, eliminar el SQL Server y la Base de Datos para evitar gastos innecesarios de los créditos de Azure**


## Uso y modificación del DockerFile

1. Agregar en el proyecto del obligatorio, un archivo dockerfile en el root del proyecto:

El dockerfile deberá seguir la esta estructura:
```docker
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE\_ENVIRONMENT=Development

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los proyectos
COPY ./Domain/Domain.csproj Domain/
COPY ./FileReaders/FileReaders.csproj FileReaders/
COPY ./UserInterface/UserInterface.csproj UserInterface/
COPY ./BusinessLogic/BusinessLogic.csproj BusinessLogic/
COPY ./Repository/Repository.csproj Repository/
COPY ./IRepository/IRepository.csproj IRepository/
COPY ./IBusinessLogicImport/IBusinessLogicImport.csproj IBusinessLogicImport/

# Restaurar paquetes
RUN dotnet restore UserInterface/UserInterface.csproj

# Copiar el resto de los archivos
COPY ./Domain/ Domain/
COPY ./UserInterface/ UserInterface/
COPY ./BusinessLogic/ BusinessLogic/
COPY ./Repository/ Repository/
COPY ./FileReaders/ FileReaders/
COPY ./IRepository/ IRepository/
COPY ./IBusinessLogicImport/ IBusinessLogicImport/

WORKDIR /src/UserInterface
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR /src/UserInterface
RUN dotnet publish UserInterface.csproj -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserInterface.dll"]
```

2. Modificar el dockerfile de modo que, cuando se realice la **copia de los proyectos**, los archivos y los directorios se correspondan con los del proyecto. Esto depende de la estructura elegida. Como modo de ejemplo, este dockerfile corresponde a la siguiente estructura de carpetas:

![](./assets/image2.png)

La misma consideración se deberá tener cuando se hace la copia del resto de archivos. Además, se deberá sustituir las apariciones de UserInterface por el nombre del proyecto que contenga el punto de entrada a la solución (en otras palabras, el proyecto que contiene el archivo [Program.cs](http://program.cs)).

## Despliegue en Render:

3\. Ingresar a la web de [Render](https://render.com/%20)  
4\. Crear una cuenta en caso de no contar con una. Seleccionar la opción gratuita. **No es necesario seleccionar alguna opción de pago.**   
5\. Una vez creada la cuenta, navegar al dashboard: [https://dashboard.render.com/](https://dashboard.render.com/)   
6\. Seleccionar Add new y la opción de **Web Service:**

![](./assets/render-dash.png)

7\. Permitir el acceso a Git, seleccionar la opción de Git Provider en Source Code. Seleccionar el repositorio en el cuál se encuentre el código a deployar.

![](./assets/render-select-repo.png)
   
8\. Configurar el web service a deployar. Modificar los siguientes campos:  

**Language:** Docker  

**Branch:** Main (La rama sobre la cuál queremos que se realice el deploy. Asumiendo que queremos deployar el ambiente de producción, seleccionamos Main). Es importante mencionar que cada vez que se realice un push sobre la rama seleccionada, se ejecutará de forma automática el proceso para realizar el despliegue con los nuevos cambios.  

**Root Directory:** Colocar el path hacia el root de nuestro proyecto de .NET. En este caso en donde se encontrará nuestro Dockerfile.

![](./assets/render-config.png)

En Instance Type, **seleccionar el plan gratuito:**

![](./assets/render-config-plan.png)

En Environment Variables, se setea el **Connection String** (obtenido previamente en el paso de la creación de la base de datos). 

Es importante que el nombre de la variable de entorno siga este formato: 

**`ConnectionStrings__<NombreLógico>`**  
donde `<NombreLógico>` es el nombre definido en el archivo `appsettings.json` bajo la sección `ConnectionStrings`.

Por ejemplo, en el appSettings.json del proyecto utilizado para la elaboración de esta guía, se encuentra la siguiente configuración para el connection string:  

![](./assets/render1.png)

Acá se puede ver que el nombre lógico es PharmaGo, por lo cual en la configuración de Render, el nombre de la variable de entorno para el Connection String es el siguiente:

![](./assets/render-env.png)

Una vez realizada la configuración, seleccionar Deploy Web Service. Esperar a que se realice. Si transcurrió con éxito, se puede acceder a la pestaña de events, en la cuál nos indicará los estados de los últimos deploys realizados y se nos proveerá la Base URL sobre la cuál estarán deployados nuestros servicios:

![](./assets/render-final.png)

# Importante: Pausar y eliminar recursos**


Como los créditos son limitados, es **fundamental** pausar los recursos mientras no se haga uso de ellos. Para esto, se deberá ingresar al Portal de Azure, ir hacia el grupo de recursos creados y seleccionar la aplicación contenedora:

Ver si hay algun script para bajar/subir


![](./assets/image3.png)

Una vez en el recurso, se deberá seleccionar detener (o iniciar, en el caso de que se le quiera dar uso):

![](./assets/image6.png) 

Detener e iniciar la aplicación contenedora al momento de usarla es fundamental para no gastar los 100 créditos de Azure y poder utilizarlos para otras asignaturas o proyectos.

Una vez que no se deseé hacer más uso de los recursos (por ejemplo al finalizar el semestre, luego de la defensa), se podrá eliminar el resource group para evitar posibles gastos de créditos:

![](./assets/image22.png)
