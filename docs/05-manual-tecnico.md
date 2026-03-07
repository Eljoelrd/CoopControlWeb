#  Manual Técnico - CoopControl Web

## 1. Introducción
   1.1 Propósito del Documento
   1.2 Alcance del Sistema
   1.3 Glosario de Términos Técnicos

## 2. Arquitectura del Sistema

   2.1 Diagrama de Arquitectura
       ```
       [Cliente] ←→ [Servidor Blazor] ←→ [Base de Datos]
       ```

   2.2 Tecnologías Utilizadas
       | Capa | Tecnología | Versión |
       |------|------------|---------|
       | Frontend | Blazor Web | .NET 9 |
       | UI Framework | Radzen.Blazor | Latest |
       | Backend | .NET 9 | 9.0.x |
       | Base de Datos | SQL Server | 2022 |
       | Control de Versiones | Git | 2.x |

   2.3 Patrón de Diseño
       - Component-based (Blazor)
       - Dependency Injection
       - Repository Pattern (si aplica)

## 3. Estructura del Proyecto

   3.1 Árbol de Directorios
       ```
       CoopControlWeb/
       ├── Components/
       │   ├── Layout/
       │   │   └── MainLayout.razor
       │   └── Pages/
       │       ├── Home.razor
       │       └── ...
       ├── wwwroot/
       │   ├── css/
       │   │   └── app.css
       │   └── images/
       ├── App.razor
       ├── Program.cs
       └── _Imports.razor
       ```

   3.2 Descripción de Carpetas
       - /Components: Componentes Razor
       - /wwwroot: Recursos estáticos
       - /docs: Documentación
       - /.github: Configuración de GitHub

## 4. Configuración del Entorno

   4.1 Requisitos de Desarrollo
       - .NET 9 SDK
       - Visual Studio 2022 / VS Code
       - Git
       - SQL Server

   4.2 Instalación Paso a Paso
       ```bash
       git clone https://github.com/Eljoelrd/CoopControlWeb.git
       cd CoopControlWeb
       dotnet restore
       dotnet ef database update
       dotnet run
       ```

   4.3 Variables de Entorno
       - ConnectionStrings
       - AppSettings
       - Secrets de API

   4.4 Configuración de appsettings.json
       ```json
       {
         "ConnectionStrings": {
           "DefaultConnection": "Server=...;Database=...;"
         },
         "Logging": {
           "LogLevel": {
             "Default": "Information"
           }
         }
       }
       ```

## 5. Componentes Principales

   5.1 MainLayout.razor
       - Estructura del layout
       - Sidebar colapsable
       - Header con iconos
       - Modo oscuro/claro

   5.2 Home.razor
       - Dashboard de estadísticas
       - Cards informativas
       - Botones de acción

   5.3 Componentes Reutilizables
       - RadzenButton
       - RadzenCard
       - RadzenDataGrid
       - RadzenPanelMenu

## 6. Base de Datos

   6.1 Diagrama Entidad-Relación
   6.2 Tablas Principales
       | Tabla | Descripción |
       |-------|-------------|
       | Usuarios | Credenciales y roles |
       | Socios | Información de socios |
       | Cooperativas | Datos de cooperativas |
       | Reportes | Historial de reportes |

   6.3 Migraciones (Entity Framework)
       ```bash
       dotnet ef migrations add InitialCreate
       dotnet ef database update
       ```

## 7. Seguridad

   7.1 Autenticación
       - Login con usuario/contraseña
       - Encriptación BCrypt
       - Sessions/Claims

   7.2 Autorización
       - Roles de usuario
       - Permisos por módulo
       - [Authorize] attributes

   7.3 Buenas Prácticas
       - No exponer secretos en código
       - Usar HTTPS
       - Validar inputs del usuario
       - Proteger contra SQL Injection

## 8. API Endpoints (si aplica)

   | Método | Endpoint | Descripción |
   |--------|----------|-------------|
   | GET | /api/socios | Listar socios |
   | POST | /api/socios | Crear socio |
   | PUT | /api/socios/{id} | Actualizar socio |
   | DELETE | /api/socios/{id} | Eliminar socio |

## 9. Estilos y Temas

   9.1 Variables CSS
       ```css
       :root {
           --bg-primary: #ffffff;
           --text-primary: #1a1a1a;
           --accent-color: #667eea;
       }
       [data-theme="dark"] {
           --bg-primary: #1a1a2e;
           --text-primary: #eaeaea;
       }
       ```

   9.2 Modo Oscuro/Claro
       - Implementación con RadzenAppearanceToggle
       - Persistencia en localStorage
       - Variables CSS dinámicas

## 10. Testing

   10.1 Pruebas Unitarias
       - xUnit / NUnit
       - Cobertura mínima 80%

   10.2 Pruebas de Integración
       - Endpoints API
       - Flujo de autenticación

   10.3 Pruebas Manuales
       - Checklist de QA
       - Casos de prueba críticos

## 11. Despliegue

   11.1 Publicación en IIS
       ```bash
       dotnet publish -c Release -o ./publish
       ```

   11.2 Publicación en Azure
       - Azure App Service
       - Azure SQL Database
       - Configuración de CI/CD

   11.3 Docker (opcional)
       ```dockerfile
       FROM mcr.microsoft.com/dotnet/aspnet:9.0
       COPY . /app
       WORKDIR /app
       ENTRYPOINT ["dotnet", "CoopControlWeb.dll"]
       ```

## 12. Monitoreo y Logs

   12.1 Configuración de Logs
       - Serilog / NLog
       - Niveles de log (Info, Warning, Error)

   12.2 Métricas de Rendimiento
       - Tiempo de respuesta
       - Uso de memoria
       - Requests por segundo

## 13. Solución de Problemas Técnicos

   13.1 Errores Comunes
       | Error | Causa | Solución |
       |-------|-------|----------|
       | 500 Internal Server | Exception no manejada | Revisar logs |
       | 404 Not Found | Ruta incorrecta | Verificar routing |
       | Connection Failed | BD no disponible | Verificar connection string |

   13.2 Debugging
       - Breakpoints en Visual Studio
       - Browser DevTools
       - Logs del servidor

## 14. Mantenimiento

   14.1 Actualizaciones
       - Actualizar .NET SDK
       - Actualizar paquetes NuGet
       - Revisar dependencias

   14.2 Backups
       - Base de datos (diario)
       - Código (GitHub)
       - Configuraciones

   14.3 Escalamiento
       - Horizontal vs Vertical
       - Load Balancing
       - Caching strategies

## 15. Referencias y Enlaces

   - Documentación oficial .NET 9
   - Radzen Blazor Components
   - GitHub del proyecto
   - Stack Overflow tags relevantes