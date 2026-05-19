# Manual Técnico - CoopControl Web

## 1. Introducción
### 1.1 Propósito del Documento
El propósito de este manual es servir como guía técnica para el desarrollo, configuración, despliegue y mantenimiento del sistema **CoopControl Web**. Proporciona a los desarrolladores y administradores la información necesaria para comprender la arquitectura, las tecnologías utilizadas, la estructura del proyecto y las mejores prácticas de seguridad y operación.

### 1.2 Alcance del Sistema
El sistema **CoopControl Web** está diseñado para la gestión integral de cooperativas, incluyendo módulos de administración de socios, emisión de certificados, solicitudes de retiros, generación de reportes y configuración personalizada. El alcance abarca:
- Interfaz web accesible desde navegadores modernos.
- Integración con bases de datos SQL Server.
- Autenticación y autorización basada en roles.
- Funcionalidades de monitoreo, seguridad y escalabilidad.
- Despliegue en entornos locales (IIS).

### 1.3 Glosario de Términos Técnicos
- **Blazor**: Framework de Microsoft para construir aplicaciones web interactivas con C# y .NET.  
- **Radzen.Blazor**: Conjunto de componentes UI para Blazor.  
- **Entity Framework (EF)**: ORM para interactuar con bases de datos SQL.  
- **Repository Pattern**: Patrón de diseño para separar la lógica de acceso a datos.  
- **QuestPDF**: Librería para la generación de documentos PDF estructurados.
- **CI/CD**: Integración y despliegue continuo.  

## 2. Arquitectura del Sistema

### 2.1 Diagrama de Arquitectura
```
[Cliente] ←→ [Servidor Blazor] ←→ [Base de Datos]
```

### 2.2 Tecnologías Utilizadas
| Capa              | Tecnología      | Versión |
|-------------------|-----------------|---------|
| Frontend          | Blazor Web      | .NET 9  |
| UI Framework      | Radzen.Blazor   | Latest  |
| Backend           | .NET 9 / Web API| 9.0.x   |
| PDF Engine        | QuestPDF        | Latest  |
| Base de Datos     | SQL Server      | 2022    |
| Control de Versiones | Git          | 2.x     |

### 2.3 Patrón de Diseño
- Component-based (Blazor)
- Dependency Injection
- Repository Pattern (si aplica)

## 3. Estructura del Proyecto

### 3.1 Árbol de Directorios
```
CoopControlWeb/
├── Controllers/
│   └── ReportesController.cs
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor
│   └── Pages/
│       ├── Home.razor
│       └── ...
├── Modelos/
│   ├── PoliticasCreditoService.cs
│   └── ...
├── wwwroot/
│   ├── css/
│   │   └── app.css
│   └── images/
├── App.razor
├── Program.cs
└── _Imports.razor
```

### 3.2 Descripción de Carpetas
- /Components: Componentes Razor  
- /wwwroot: Recursos estáticos  
- /docs: Documentación  
- /.github: Configuración de GitHub  

### 3.3 Servicios de Lógica de Negocio
- **PoliticasCreditoService.cs**: Contiene el motor de reglas de crédito. Implementa el cálculo de capacidad basado en el multiplicador de aportes (por defecto 3x) y valida las restricciones de garantía para retiros.
- **DepositoService.cs**: Gestiona la orquestación de ingresos de efectivo, vinculando automáticamente los depósitos con la creación de aportes ordinarios o la amortización de cuotas de préstamos mediante transacciones SQL.

## 4. Configuración del Entorno

### 4.1 Requisitos de Desarrollo
- .NET 9 SDK  
- Visual Studio 2022 / VS Code  
- Git  
- SQL Server  

### 4.2 Instalación Paso a Paso
```bash
git clone https://github.com/Eljoelrd/CoopControlWeb.git
cd CoopControlWeb
dotnet restore
dotnet ef database update
dotnet run
```

### 4.3 Variables de Entorno
- ConnectionStrings  
- AppSettings  
- Secrets de API  

### 4.4 Configuración de appsettings.json
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

### 5.1 MainLayout.razor
- Estructura del layout  
- Sidebar colapsable  
- Header con iconos  
- Modo oscuro/claro  

### 5.2 Home.razor
- Dashboard de estadísticas  
- Cards informativas  
- Botones de acción  

### 5.3 Componentes Reutilizables
- RadzenButton  
- RadzenCard  
- RadzenDataGrid  
- RadzenPanelMenu  

## 6. Base de Datos

### 6.1 Diagrama Entidad-Relación
### 6.2 Tablas Principales
| Tabla        | Descripción              |
|--------------|--------------------------|
| Usuarios     | Credenciales y roles     |
| Socios       | Información de socios    |
| Ahorros      | Tipos de ahorro y balances |
| Aportes      | Registro de capital social |
| Pagos        | Amortizaciones de préstamos |
| Prestamos    | Gestión de créditos y cuotas |
| Certificados | Solicitudes y emisión de certificados |
| Retiros      | Solicitudes y procesamiento de retiros |
| Reportes     | Historial de reportes    |

### 6.3 Migraciones (Entity Framework)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 7. Seguridad

### 7.1 Autenticación
- Login con usuario/contraseña  
- Encriptación BCrypt  
- Sessions/Claims  

### 7.2 Autorización
- Roles de usuario  
- Permisos por módulo  
- [Authorize] attributes  

### 7.3 Buenas Prácticas
- No exponer secretos en código  
- Usar HTTPS  
- Validar inputs del usuario  
- Proteger contra SQL Injection  

## 8. API Endpoints (si aplica)

| Método | Endpoint          | Descripción       |
|--------|------------------|-------------------|
| GET    | /api/socios      | Listar socios     |
| POST   | /api/socios      | Crear socio       |
| PUT    | /api/socios/{id} | Actualizar socio  |
| DELETE | /api/socios/{id} | Eliminar socio    |
| GET    | /api/reportes/comprobante/{id} | Genera PDF de recibo de caja |
| GET    | /api/reportes/solicitud/{id}   | Genera PDF de contrato de préstamo |
| GET    | /api/reportes/estado-cuenta/{socioId} | Resumen de movimientos |

## 9. Estilos y Temas

### 9.1 Variables CSS
```css
:root {
    --bg-primary: #ffffff;
    --text-primary: #1a1a1a;
    --accent-color: #2f855a; 
}
[data-theme="dark"] {
    --bg-primary: #111827;
    --text-primary: #eaeaea;
}
```

### 9.2 Modo Oscuro/Claro
- Implementación con RadzenAppearanceToggle  
- Persistencia en localStorage  
- Variables CSS dinámicas  

## 10. Testing

### 10.1 Pruebas Unitarias
- xUnit / NUnit  
- Cobertura mínima 80%  

### 10.2 Pruebas de Integración
- Endpoints API  
- Flujo de autenticación  

### 10.3 Pruebas Manuales
- Checklist de QA  
- Casos de prueba críticos  

## 11. Despliegue

### 11.1 Publicación en IIS
```bash
dotnet publish -c Release -o ./publish
```

### 11.2 Publicación en Azure
- Azure App Service  
- Azure SQL Database  
- Configuración de CI/CD  

### 11.3 Docker (opcional)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "CoopControlWeb.dll"]
```

## 12. Monitoreo y Logs

### 12.1 Configuración de Logs
- Serilog / NLog  
- Niveles de log (Info, Warning, Error)  

### 12.2 Métricas de Rendimiento
- Tiempo de respuesta  
- Uso de memoria  
- Requests por segundo  

## 13. Solución de Problemas Técnicos

### 13.1 Errores Comunes
| Error                | Causa                  | Solución             |
|----------------------|------------------------|----------------------|
| 500 Internal Server  | Exception no manejada  | Revisar logs         |
| 404 Not Found        | Ruta incorrecta        | Verificar routing    |
| Connection Failed    | BD no disponible       | Verificar connection string |

### 13.2 Debugging
- Breakpoints en Visual Studio  
- Browser DevTools  
- Logs del servidor  

## 14. Mantenimiento

### 14.1 Actualizaciones
- Actualizar .NET SDK  
- Actualizar paquetes NuGet  
- Revisar dependencias  

### 14.2 Backups
- Base de datos (diario)  
- Código (GitHub)  
- Configuraciones  

### 14.3 Escalamiento
- Horizontal vs Vertical  
- Load Balancing  
- Caching strategies  

## 15. Referencias y Enlaces
- Documentación oficial .NET 9  
- Radzen Blazor Components  
- GitHub del proyecto  
- Stack Overflow tags relevantes  
```
