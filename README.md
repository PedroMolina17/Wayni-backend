# Wayni Backend

Este es el backend para el proyecto Wayni, desarrollado con .NET 8. Proporciona APIs para la gestión de datos y la lógica del servidor.

## Requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/) (versión 8.0 o superior)

## Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/PedroMolina17/Wayni-backend.git
```

### 2. Configurar la Base de Datos
Crear la Base de Datos:

Ejecuta el siguiente comando en MySQL:
```bash
CREATE DATABASE wayni_db;
Configurar la Cadena de Conexión:
```

Abre appsettings.json y actualiza la cadena de conexión:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=wayni_db;User=root;Password=yourpassword;"
}
```

###3. Restaurar Dependencias y Ejecutar

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

https://github.com/user-attachments/assets/c3d09491-9be0-4a24-ba9f-0775891fd918



