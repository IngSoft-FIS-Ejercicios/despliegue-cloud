#!/bin/bash
set -e  # Sale si hay cualquier error

#!/bin/bash
set -e  # Sale si hay cualquier error

# Preguntar variables al usuario
read -p "🔧 Ingresá el nombre del Azure Container Registry (ACR_NAME): " ACR_NAME
read -p "📦 Ingresá el nombre de la Container App (APP_NAME): " APP_NAME
read -p "📁 Ingresá el nombre del Resource Group (RESOURCE_GROUP): " RESOURCE_GROUP
read -p "🌍 Ingresá la región (LOCATION): " LOCATION
read -p "🏞️ Ingresá el nombre del entorno de Container App (ENV_NAME): " ENV_NAME

read -p "🛢️ Ingresá el nombre del servidor SQL (SQL_SERVER_NAME): " SQL_SERVER_NAME
read -p "📚 Ingresá el nombre de la base de datos SQL (SQL_DB_NAME): " SQL_DB_NAME
read -p "👤 Ingresá el nombre del usuario administrador SQL (SQL_ADMIN_USER): " SQL_ADMIN_USER
read -s -p "🔒 Ingresá la contraseña del administrador SQL (SQL_ADMIN_PASS): " SQL_ADMIN_PASS
echo

# ----------------------
# Detectar opción --redeploy
# ----------------------
REDEPLOY=false

for arg in "$@"; do
  if [ "$arg" == "--redeploy" ]; then
    REDEPLOY=true
  fi
done

# ----------------------
# Rollback en caso de error
# ----------------------
rollback() {
  echo "⚠️ Algo falló. Revirtiendo cambios..."

  if [ "$REDEPLOY" = false ]; then
    echo "⛔ Eliminando Azure Container Registry (si fue creado)..."
    az acr delete --name $ACR_NAME --resource-group $RESOURCE_GROUP --yes || true

    echo "⛔ Eliminando Container App (si fue creada)..."
    az containerapp delete --name $APP_NAME --resource-group $RESOURCE_GROUP --yes || true

    echo "⛔ Eliminando Container App Environment (si fue creado)..."
    az containerapp env delete --name $ENV_NAME --resource-group $RESOURCE_GROUP --yes || true
  fi

  echo "⚠️ La imagen enviada al ACR no se elimina automáticamente."
  echo "ℹ️ Podés eliminarla con:"
  echo "az acr repository delete --name $ACR_NAME --image $APP_NAME:latest --yes"

  echo "❌ Rollback completo."
  exit 1
}

trap rollback ERR

# ----------------------
# Variables (ya cargadas con export)
# ----------------------
ACR_NAME=${ACR_NAME}
APP_NAME=${APP_NAME}
RESOURCE_GROUP=${RESOURCE_GROUP}
LOCATION=${LOCATION}
ENV_NAME=${ENV_NAME}

SQL_SERVER_NAME=${SQL_SERVER_NAME}
SQL_DB_NAME=${SQL_DB_NAME}
SQL_ADMIN_USER=${SQL_ADMIN_USER}
SQL_ADMIN_PASS=${SQL_ADMIN_PASS}

# Registro de proveedores (por si falta alguno)
echo "Registrando proveedores necesarios..."
az provider register -n Microsoft.OperationalInsights --wait
az provider register --namespace Microsoft.Web --wait

# Crear el ACR (si no es redeploy)
if [ "$REDEPLOY" = false ]; then
  echo "Creando Azure Container Registry (ACR)..."
  az acr create \
    --resource-group $RESOURCE_GROUP \
    --name $ACR_NAME \
    --sku Basic \
    --admin-enabled true
fi

# Loguearse en el ACR
echo "Logueando Docker en el ACR..."
az acr login --name $ACR_NAME

echo "Obteniendo login server del ACR..."
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)

# Verificación y asignación de roles
ASSIGNEE=$(az ad signed-in-user show --query userPrincipalName -o tsv)
ACR_SCOPE=$(az acr show --name $ACR_NAME --query id -o tsv)
ROL_ASIGNADO=$(az role assignment list --assignee $ASSIGNEE --scope $ACR_SCOPE --query "[?roleDefinitionName=='AcrPush'] | length(@)" -o tsv)

if [ "$ROL_ASIGNADO" -eq 0 ]; then
  echo "Asignando rol AcrPush al usuario $ASSIGNEE en el ACR $ACR_NAME..."
  az role assignment create --assignee $ASSIGNEE --role AcrPush --scope $ACR_SCOPE
fi

# Buildeo y push de la imagen Docker
echo "Construyendo y pusheando imagen con Docker buildx..."
docker buildx build \
  --platform linux/amd64 \
  -t $ACR_LOGIN_SERVER/$APP_NAME:latest \
  --push .

# Obtener credenciales del ACR
echo "Obteniendo credenciales del ACR..."
REGISTRY_USERNAME=$(az acr credential show --name $ACR_NAME --query username -o tsv)
REGISTRY_PASSWORD=$(az acr credential show --name $ACR_NAME --query 'passwords[0].value' -o tsv)

# Crear entorno para Container App (si no es redeploy)
if [ "$REDEPLOY" = false ]; then
  echo "Creando entorno para Container App..."
  az containerapp env create \
    --name $ENV_NAME \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION
fi

# Crear o actualizar la Container App
if [ "$REDEPLOY" = false ]; then
  echo "Creando la Container App..."
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
else
  echo "Actualizando Container App con nueva imagen..."
  az containerapp update \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --image $ACR_LOGIN_SERVER/$APP_NAME:latest
fi

# Mostrar URL de la aplicación
APP_URL=$(az containerapp show \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --query properties.configuration.ingress.fqdn \
  -o tsv)

echo "✅ Despliegue completo. Tu aplicación ya debería estar corriendo en:"
echo "🌐 https://$APP_URL"
