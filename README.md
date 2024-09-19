# TestPaginacion

## Descripción del Proyecto

**TestPaginacion** es una aplicación web basada en **ASP.NET Core MVC** que implementa diferentes técnicas de paginación sobre una lista de clientes. El objetivo principal del proyecto es demostrar las diferentes formas de realizar paginación eficiente en aplicaciones que manejan grandes volúmenes de datos. Además, incluye un proyecto de consola que utiliza la librería **Bogus** para generar datos falsos y llenar la base de datos con clientes para realizar pruebas de rendimiento.

### Funcionalidades Clave

1. **Paginación Optimizada**: Carga solo los registros necesarios para la página actual usando `Entity Framework` y `LINQ`, optimizando así el uso de recursos.
2. **Paginación con SQL Directo**: Ejecución de consultas SQL directas para obtener los datos paginados, lo que permite una mayor optimización en algunos escenarios.
3. **Carga de Datos en Segundo Plano**: Uso de `JavaScript Fetch` para cargar los datos de manera asíncrona, mejorando los tiempos de respuesta de la interfaz.
4. **Generación de Datos Falsos**: Un proyecto de consola utiliza **Bogus** para generar registros falsos y llenar la base de datos, facilitando las pruebas del sistema.

## Configuración del Proyecto

### Requisitos

- **.NET Core SDK** 6.0 o superior
- **SQL Server** (o cualquier otro servidor de bases de datos compatible con `Entity Framework Core`)
- **Visual Studio 2022** o cualquier editor de tu preferencia

### Instrucciones para Correr el Proyecto

1. Clona el repositorio:

    ```bash
    git clone https://github.com/alexismartinez-ufg/TestPaginacion.git
    cd TestPaginacion
    ```

2. Configura la base de datos:
   
   Asegúrate de tener una instancia de SQL Server corriendo. Puedes modificar la cadena de conexión en el archivo `PruebaDbContext.cs` para que coincida con tu servidor de base de datos.

   ```csharp
   optionsBuilder.UseSqlServer("Server=localhost;Database=prueba;integrated security=true;TrustServerCertificate=true;");
