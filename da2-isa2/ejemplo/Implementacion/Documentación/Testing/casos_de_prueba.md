# Casos de prueba

## 1. Validación de cantidad negativa de un medicamento al reabastecer stock

- **Precondiciones:** Iniciar sesión con usuario de tipo *Employee*  

### 1.1: Agregar un único medicamento con cantidad negativa

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al reabastecer el stock.

#### Pasos:
1. Ir al menú para crear un pedido de reabastecimiento.
2. Seleccionar un medicamento.
3. Ingresar manualmente un número negativo en el campo de cantidad.
4. Presionar el botón "Add".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se pueden agregar cantidades negativas.
- La petición no debe ser agregada a la base de datos.

---

### 1.2: Validación de cantidad negativa de un medicamento junto a una cantidad positiva de otro

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al reabastecer el stock, teniendo ya un medicamento con cantidad positiva en la lista.

#### Pasos:
1. Ir al menú para crear un pedido de reabastecimiento.
2. Seleccionar un medicamento.
3. Ingresar manualmente un número negativo en el campo de cantidad.
4. Presionar el botón "Add".
5. Seleccionar otro medicamento e ingresar una cantidad positiva.
6. Presionar el botón "Add".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se pueden agregar cantidades negativas.
- El sistema no permite realizar la petición si se ingresa una cantidad menor o igual a 0.

---

### 1.3: Ingresar la cantidad con un valor de cero al reabastecer el stock

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al reabastecer el stock, teniendo ya un medicamento con cantidad positiva en la lista.

#### Pasos:
1. Ir al menú para crear un pedido de reabastecimiento.
2. Seleccionar un medicamento.
3. Ingresar manualmente 0 en el campo de cantidad.
4. Presionar el botón "Add".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se puede ingresar una cantidad igual a 0.
- La petición con cantidad 0 no debe ser agregada a la base de datos.

---

### 1.4: Ingresar un valor inválido, cancelar ese valor y realizar la petición correcta

- **Precondición:** Iniciar sesión con usuario de tipo *Employee*.  
- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al reabastecer el stock, teniendo ya un medicamento con cantidad positiva en la lista.

#### Pasos:
1. Ir al menú para crear un pedido de reabastecimiento.
2. Seleccionar un medicamento.
3. Ingresar manualmente un número negativo o 0 en el campo de cantidad.
4. Presionar el botón "Add".
5. Revertir si es necesario cuando se agregó ese elemento.
6. Agregar un elemento válido y proceder a realizar la petición de reabastecimiento.

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se puede ingresar un número negativo.
- El sistema le debe dar la posibilidad al usuario de recuperarse del error y revertirlo.
- La petición con cantidad incorrecta no debe ser agregada a la base de datos, pero sí la petición con el valor correcto.

---

## 2. Validación en cantidades y saldos negativos en compras

**Precondiciones:** 
- Iniciar como un usuario invitado.
- Disponer de medicamentos ya cargados.

### 2.1: Validación de cantidad negativa de un único medicamento al carrito

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al carrito de compras.

#### Pasos:
1. Ir a la pantalla principal.
2. Seleccionar un medicamento con "View Details".
3. Ingresar manualmente un número negativo en el campo de cantidad.
4. Presionar el botón "Add to cart".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se pueden agregar cantidades negativas.
- El medicamento no debe ser agregado al carrito.

---

### 2.2: Validación de cantidad negativa de un medicamento junto a una cantidad positiva de otro

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al momento de realizar una compra para obtener un "descuento", teniendo ya un medicamento con cantidad positiva en la lista.

#### Pasos:
1. Ir a la pantalla principal.
2. Seleccionar un medicamento con "View Details".
3. Ingresar manualmente un número positivo en el campo de cantidad.
4. Presionar el botón "Add to cart".
5. Seleccionar el botón "Add another item".
6. Seleccionar otro medicamento e ingresar una cantidad negativa.
7. Presionar el botón "Add".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se pueden agregar cantidades negativas.
- El sistema no permite agregar al carrito si se ingresa una cantidad menor o igual a 0.

---

### 2.3: Ingresar la cantidad con un valor 0

- **Descripción:** Verificar que el sistema no permita ingresar cantidades negativas de medicamentos al agregar medicamentos al carrito.

#### Pasos:
1. Ir a la pantalla principal.
2. Seleccionar un medicamento con "View Details".
3. Ingresar manualmente 0 en el campo de cantidad.
4. Presionar el botón "Add to cart".

#### Resultado esperado:
- El sistema debe mostrar un mensaje de error o advertencia indicando que no se puede ingresar una cantidad igual a 0.
- El sistema no permite agregar al carrito si se ingresa una cantidad menor o igual a 0.

---

## 3. Validación de creación de cosmético

**Precondición:** Usuario logueado como Employee.

### 3.1: Creación de cosmético con código incorrecto

- **Descripción:** Verificar que el sistema no permita la creación de un medicamento con un código distinto a 5 dígitos.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Ingresar un código con una cantidad distinta a 5 dígitos.
3. Ingresar en el resto de los campos datos válidos (descripción < 70 caracteres, nombre alfanumérico < 30 caracteres, precio con hasta dos lugares después de la coma).
4. Presionar el botón para crear el cosmético.

#### Resultado esperado:
- El sistema no debe permitir al usuario crear un cosmético.

---

### 3.2: Creación de cosmético con nombre incorrecto

- **Descripción:** Verificar que el sistema no permita la creación de un medicamento con un nombre mayor a 30 caracteres.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Ingresar un nombre de largo mayor a 30 caracteres.
3. Ingresar en el resto de los campos datos válidos (código de 5 cifras, descripción < 70 caracteres, precio con hasta dos lugares después de la coma).
4. Presionar el botón para crear el cosmético.

#### Resultado esperado:
- El sistema no debe permitir al usuario crear un cosmético.

---

### 3.3: Creación de cosmético con precio incorrecto

- **Descripción:** Verificar que el sistema no permita la creación de un medicamento con un precio con más de dos lugares decimales después de la coma.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Ingresar un precio con más de tres lugares decimales después de la coma.
3. Ingresar en el resto de los campos datos válidos (código de 5 cifras, descripción < 70 caracteres, nombre alfanumérico < 30 caracteres).
4. Presionar el botón para crear el cosmético.

#### Resultado esperado:
- El sistema no debe permitir al usuario crear un cosmético.

---

### 3.4: Creación de cosmético con descripción incorrecta

- **Descripción:** Verificar que el sistema no permita la creación de un medicamento con una descripción de más de 70 caracteres.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Ingresar una descripción de largo mayor a 70 caracteres.
3. Ingresar en el resto de los campos datos válidos (código de 5 cifras, nombre alfanumérico < 30 caracteres, precio con hasta dos lugares después de la coma).
4. Presionar el botón para crear el cosmético.

#### Resultado esperado:
- El sistema no debe permitir al usuario crear un cosmético.

---

### 3.5: Creación de un cosmético

- **Descripción:** Verificar que el sistema crea un cosmético de forma exitosa.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Ingresar datos válidos en todos los campos (código de 5 cifras, descripción < 70 caracteres, nombre alfanumérico < 30 caracteres, precio con hasta dos lugares después de la coma).
3. Presionar el botón para crear el cosmético.

#### Resultado esperado:
- Se crea un cosmético en la tabla de Products y también en la tabla de Cosmetic.
- Se muestra un mensaje de éxito.

---

### 3.6: Creación de cosmético con campos vacíos

- **Descripción:** Como todos los campos son requeridos, se deberá verificar que todos los campos estén llenos.

#### Pasos:
1. Ir a la pantalla de creación de cosmético.
2. Dejar algún campo vacío y presionar el botón para crear el cosmético.

#### Resultado esperado:
- El sistema no debe permitir al usuario crear un cosmético.

---

## 4. Validación de creación de Drug con un id de farmacia asociado

**Precondición:** Usuario logueado como Employee.

### 4.1: Creación de un medicamento con farmacia asociada

- **Descripción:** Verificar que el medicamento se cree con una farmacia asociada.

#### Pasos:
1. Ir a la pantalla de Create Drug.
2. Crear un medicamento.

#### Resultado esperado:
- Se crea el medicamento de forma exitosa.
- En la base de datos queda asociado al `pharmacyId` del usuario.

---

## 5. Validación de acceso a pantalla de login

### 5.1: Ingreso a pantalla de login

- **Descripción:** Verificar que en la interfaz se le dé al usuario la opción de loguearse al sistema.

#### Pasos:
1. Ir a la pantalla de home.
2. Verificar que se muestre un botón para poder hacer login y que redirija a dicha pantalla.

#### Resultado esperado:
- Aparece el botón en la UI y redirige a `/login`.

---

### 5.2: Indicador de usuario logueado

- **Descripción:** Verificar si un usuario se encuentra logueado.

#### Pasos:
1. Loguearse en la app.
2. Verificar que se muestre el nombre del usuario en algún lugar de la app.

#### Resultado esperado:
- Aparece el nombre del usuario y se le da la opción de hacer logout.
