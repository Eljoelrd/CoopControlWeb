#  Arquitectura del Sistema
## Diagrama de Arquitectura
┌─────────────────────────────────────────┐
│ Cliente (Navegador)                     │
│ Blazor Web                              │
└─────────────────┬───────────────────────┘
                  │HTTPS
┌─────────────────▼───────────────────────┐
│ Servidor (.NET 9)                       │
│ ┌─────────────────┐                     │
│ │ Radzen UI       │                     │
│ ├─────────────────┤                     │
│ │ Componentes     │                     │
│ ├─────────────────┤                     │
│ │ Servicios       │                     │
│ └─────────────────┘                     │
└─────────────────┬───────────────────────
                  │
┌─────────────────▼───────────────────────┐
│ Base de Datos                           │
│ SQL Server                              │
└─────────────────────────────────────────┘


## Tecnologías
| Capa | Tecnología |
|------|------------|
| Frontend | Blazor Web + Radzen |
| Backend | .NET 9 |
| Base de Datos | SQL Server |

| Hosting | Azure / IIS |
