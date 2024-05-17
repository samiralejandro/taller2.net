# taller2.net

taller2.net es una aplicación .NET que gestiona productos y su inventario. Permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) sobre los productos y también cuenta con una funcionalidad para reabastecer productos cuando su stock cae por debajo de un umbral crítico.

## Requisitos

- .NET 8.0 SDK o superior
- Base de datos MySQL
- `ReplenishmentSimulator` (otro proyecto .NET que simula el sistema de reabastecimiento)

## Configuración

1. Clona el repositorio:

    ```sh
    git clone https://github.com/samiralejandro/taller2.net
    cd taller2.net
    ```

2. Configura la cadena de conexión en `appsettings.json`:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "DefaultConnection": "server=localhost;port=3306;database=tallernetdos;user=username;password=password"
      }
    }
    ```

3. Restaura los paquetes NuGet y ejecuta la aplicación:

    ```sh
    dotnet restore
    dotnet run
    ```

## Endpoints

- **GET** `/products` - Obtiene todos los productos.
- **GET** `/products/{id}` - Obtiene un producto por su ID.
- **POST** `/products` - Crea un nuevo producto.
- **PUT** `/products/{id}` - Actualiza un producto existente.
- **DELETE** `/products/{id}` - Elimina un producto por su ID.
- **PUT** `/products/{id}/replenish` - Reabastece un producto si su stock cae por debajo del umbral crítico.

## Ejemplo de Producto

```json
{
    "id": 1,
    "name": "Sample Product",
    "price": 10.99,
    "description": "This is a sample product",
    "stock": 50
}
