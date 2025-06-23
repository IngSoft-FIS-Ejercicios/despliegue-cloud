# Script despliegue

El presente documento explica los pasos tomados en el archivo [deploy.sh](./ejemplo/Obligatorio_1/deploy.sh) respecto al despliegue en Azure. Este script se utiliza como parte del despliegue en la [guía de despliegue para Diseño de Aplicaciones 1](./guia_despliegue_da1_azure.md). 

Este script automatiza gran parte del proceso de despliegue, ejecutando comandos a través de Azure CLI.

A su vez, se puede ejecutar este comando con la flag --redeploy, la cual permite desplegar nuevamente en el caso de que ya se haya realizado un despliegue previo de la aplicación y se desee desplegar nuevamente ante cualquier modificación de código.

## Glosario:

**[Azure provider:](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-providers-and-types)** Un proveedor de recursos de Azure es un conjunto de operaciones REST que admiten la funcionalidad de un servicio específico de Azure.

**[Resourse Group:](https://learn.microsoft.com/es-es/azure/azure-resource-manager/management/manage-resource-groups-portal)** Un grupo de recursos es un contenedor que almacena los recursos relacionados con una solución de Azure. El grupo de recursos puede incluir todos los recursos de la solución o solo los recursos que desea administrar como grupo.

**[Azure Container Registry (ACR):](https://azure.microsoft.com/es-es/products/container-registry)** Azure Container Registry permite compilar, almacenar y administrar imágenes y artefactos de contenedor en un registro privado para todo tipo de implementaciones de contenedor.

[**Azure Container Apps environments:**](https://learn.microsoft.com/en-us/azure/container-apps/environment) entorno de Azure Container Apps es un "espacio aislado" donde se ejecutan tus aplicaciones en contenedores.  El entorno de ejecución de Container Apps administra cada entorno controlando las actualizaciones del sistema operativo, las operaciones de escalado, los procedimientos de conmutación por error y el equilibrio de recursos.

## Precondición:

Todas las operaciones de este script utilizan el [CLI de Azure](https://learn.microsoft.com/es-es/cli/azure/install-azure-cli?view=azure-cli-latest). Es necesario contar con dicho cliente instalado.

Para ejecutar el script, es necesaio encontrarse logueado en Azure. Se puede realizar este login mediante el Azure CLI, a través del siguiente comando: 

`az login`

Este comando abrirá una ventana del navegador donde se podrán ingresar las credenciales de Azure. Una vez autenticado, la sesión quedará activa para interactuar con los recursos en la nube.

Al momento de ejecutar este script, es **fundamental que Docker se encuentre inicializado**, de lo contrario, el build de la imagen fallará.

## Pasos del script:

1. Inicialmente, el script pedirá el ingreso de valores que tomaran determinadas variables. Esto es fundamental para poder asociar recursos al grupo de recursos creado previamente y también, poder establecer una conexión con la base de datos creada en Azure. En la [guía](https://github.com/IngSoft-FIS-Ejercicios/despliegue-cloud/blob/main/da1/guia_despliegue_da1_azure.md#instalaci%C3%B3n-del-azure-cli-y-despliegue) se especifican que valores tomar y como obtenerlos, en base a la configuración que hayan utilizado al momento de crear el resource group y el servidor de SQL. 

2. Se registran los providers a utilizar:
```bash
az provider register -n Microsoft.OperationalInsights --wait
az provider register --namespace Microsoft.Web --wait
```

3. Se crea un ACR. El ACR es creado dentro del grupo de recursos que fue ingresado previamente. 

```bash
  az acr create \
    --resource-group $RESOURCE_GROUP \
    --name $ACR_NAME \
    --sku Basic \
    --admin-enabled true
```

4. Se realiza un inicio de sesión en el ACR creado, lo cual permite interactuar con el registro y realizar acciones como subir o descargar imágenes de contenedor.

```bash
az acr login --name $ACR_NAME

ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)
```

5. Se asignan los roles necesarios para poder realizar un push de la imagen de la aplicación construida con Docker.

```bash
ASSIGNEE=$(az ad signed-in-user show --query userPrincipalName -o tsv)
ACR_SCOPE=$(az acr show --name $ACR_NAME --query id -o tsv)
ROL_ASIGNADO=$(az role assignment list --assignee $ASSIGNEE --scope $ACR_SCOPE --query "[?roleDefinitionName=='AcrPush'] | length(@)" -o tsv)

if [ "$ROL_ASIGNADO" -eq 0 ]; then
  ACR $ACR_NAME..."
  az role assignment create --assignee $ASSIGNEE --role AcrPush --scope $ACR_SCOPE
fi
```

6. Se crea una imagen de Docker utilizando Docker Buildx, una extensión de Docker CLI que permite la creación de imágenes para varias plataformas, como linux/amd64, linux/arm64, etc.

    `--platform linux/amd64` especifica la plataforma de destino para la imagen Docker que se está construyendo. En este caso, la imagen se construirá para la arquitectura amd64 (es decir, máquinas de 64 bits basadas en arquitectura x86) y sistema operativo Linux.

```bash
docker buildx build \
  --platform linux/amd64 \
  -t $ACR_LOGIN_SERVER/$APP_NAME:latest \
  --push .
```

7. Los siguientes comandos se utilizan para obtener las credenciales para acceder a un Azure Container Registry (ACR). Estas credenciales permiten subir y descargar imágenes.

```bash
REGISTRY_USERNAME=$(az acr credential show --name $ACR_NAME --query username -o tsv)
REGISTRY_PASSWORD=$(az acr credential show --name $ACR_NAME --query 'passwords[0].value' -o tsv)
```

8. El siguiente comando se utiliza para crear un entorno de Azure Container Apps (Azure Container Apps environments
). Este es el entorno en el que se desplegarán y gestionarán las aplicaciones basadas en contenedores.

```bash
az containerapp env create \
    --name $ENV_NAME \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION
fi
```

> Nota: La suscripción de Azure for Students permite únicamente crear un solo Environment.

9. El comando de Azure `az containerapp create` se utiliza para crear una aplicación de contenedor en Azure Container Apps. La variable $APP_NAME define el nombre de la aplicación de contenedor que se está creando.

También se define el puerto donde la aplicación escuchará. En este caso se utilizó el puerto 8080.

La flag --ingress external define que la aplicación tendrá acceso público desde el exterior, es decir, se expone al mundo y será accesible a través de internet.

Las variables de entorno son seteadas junto con la flag env-vars. Para este caso, se configura el Connection String, para que la aplicación pueda acceder a la base de datos en la nube, creada a través de Azure Portal.

```bash
  az containerapp create \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --environment $ENV_NAME \
    --image $ACR_LOGIN_SERVER/$APP_NAME:latest \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $REGISTRY_USERNAME \
    --registry-password $REGISTRY_PASSWORD \
    --target-port 8080 \
    --ingress external \
    --env-vars \
      ConnectionStrings__DefaultConnection="Server=tcp:$SQL_SERVER_NAME.database.windows.net,1433;Initial Catalog=$SQL_DB_NAME;Persist Security Info=False;User ID=$SQL_ADMIN_USER;Password=$SQL_ADMIN_PASS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

10. Por último, se utiliza el siguiente comando para mostrar la url en la cuál la aplicación fue desplegada.

```bash
APP_URL=$(az containerapp show \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --query properties.configuration.ingress.fqdn \
  -o tsv)
```

## Consideraciones:

- Utilizar la flag --redeploy si ya fue realizado el despliegue nuevamente. Esto permitirá sincronizar la nube con cualquier modificación en el código. Esto permite a que no se depliquen recursos.
- El script cuenta con una funcionalidad de rollback que deshace la creación de ciertos recursos en caso de que falle alguno de los pasos. Si falla algún paso, es indicado cuál fue el paso que falló y se le informa al usuario con un mensaje de error correspondiente. En caso de tener que eliminar algún recurso de forma manual, se le indica cuál recurso y como proceder con la eliminación.

