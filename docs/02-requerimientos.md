# Documentación de Requerimientos

## 1. Requerimientos Funcionales

### RF01 - Gestión de Usuarios
- El sistema debe permitir registrar usuarios
- El sistema debe permitir autenticar usuarios
- El sistema debe permitir gestionar roles

### RF02 - Gestión de Socios
- El sistema debe permitir registrar socios
- El sistema debe permitir editar información de socios
- El sistema debe permitir eliminar socios

### RF03 - Gestión de Préstamos y Ahorros
- El sistema debe permitir la creación de cuentas de ahorro (Normal, Especial, Navideño, etc.).
- El sistema debe validar la capacidad de préstamo basada en el multiplicador de aportes.
- El sistema debe bloquear solicitudes si el socio tiene préstamos en mora.

### RF04 - Reportes y Comprobantes
- El sistema debe generar certificados de préstamo en PDF.
- El sistema debe generar comprobantes de aportes en PDF.
- El sistema debe generar un informe global consolidado de operaciones.

## 2. Requerimientos No Funcionales

### RNF01 - Rendimiento
- El sistema debe cargar páginas en menos de 3 segundos

### RNF02 - Seguridad
- Las contraseñas deben estar encriptadas
- El sistema debe usar HTTPS

### RNF03 - Usabilidad
- El sistema debe ser responsive
- El sistema debe tener modo oscuro/claro