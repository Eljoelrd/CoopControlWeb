
# Manual Técnico - CoopControl Web

## 2. Arquitectura del Sistema

### 2.1 Diagrama de Arquitectura
```
┌────────────────────────────┐
│ Cliente (Navegador)        │
│ Blazor Web                 │
└────────────────────────────┘
            │ HTTPS
            V
┌────────────────────────────┐
│ Servidor (.NET 9)          │
│ ┌────────────────────────┐ │
│ │ Radzen UI              │ │
│ └────────────────────────┘ │
│ ┌────────────────────────┐ │
│ │ Componentes            │ │
│ └────────────────────────┘ │
│ ┌────────────────────────┐ │
│ │ Servicios              │ │
│ └────────────────────────┘ │
└────────────────────────────┘
            │
            V
┌────────────────────────────┐
│ Base de Datos              │
│ SQL Server                 │
└────────────────────────────┘
```

### 2.2 Tecnologías
| Capa         | Tecnología           |
|--------------|----------------------|
| Frontend     | Blazor Web + Radzen  |
| Backend      | .NET 9               |
| Base de Datos| SQL Server           |
| Hosting      | Azure / IIS          |
```

