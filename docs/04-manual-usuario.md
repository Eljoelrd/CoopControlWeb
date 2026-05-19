# Manual de Usuario - CoopControl Web

## 1. Introducción

### 1.1 Propósito del Manual
Este manual tiene como propósito guiar a los usuarios en el uso de **CoopControl Web**, explicando paso a paso cómo acceder al sistema, navegar por la interfaz y utilizar las funcionalidades principales. Busca facilitar la adopción de la plataforma y asegurar que cada usuario pueda aprovechar al máximo sus herramientas.

### 1.2 ¿Qué es CoopControl Web?
**CoopControl Web** es una aplicación en línea desarrollada con tecnología **Blazor Server** que permite la gestión integral de cooperativas. A través de una interfaz moderna y accesible desde cualquier navegador, los usuarios pueden administrar socios, generar reportes, solicitar certificados, realizar retiros y configurar sus preferencias de manera segura y eficiente.

### 1.3 Público Objetivo
El sistema está dirigido a:
- **Socios de cooperativas**: para consultar su información, solicitar certificados y realizar retiros.  
- **Administradores**: para gestionar socios, generar reportes y supervisar operaciones.  
- **Personal de soporte técnico**: para mantener la plataforma y asistir a los usuarios.  
- **Organismos de control**: que requieren reportes confiables para auditorías y supervisión.  

### 1.4 Requisitos del Sistema
- Navegadores compatibles (Chrome, Edge, Firefox, Safari).  
- Resolución mínima recomendada: 1366x768.  
- Conexión estable a internet.  

---

## 2. Primeros Pasos

### 2.1 Acceso al Sistema
- URL de acceso proporcionada por la cooperativa.  
- Credenciales iniciales entregadas al usuario.  

### 2.2 Inicio de Sesión
- Ingresar usuario y contraseña.  
- Recuperar contraseña mediante opción de “Olvidé mi contraseña”.  

### 2.3 Conociendo la Interfaz
- Barra de navegación superior.  
- Menú lateral (sidebar).  
- Área de contenido principal.  
- Pie de página con información adicional.  

---

## 3. Funcionalidades del Sistema

### 3.1 Dashboard / Panel Principal
- Visualización de estadísticas.  
- Tarjetas de información rápida.  
- Gráficos y métricas.  

### 3.2 Gestión de Socios
- Registrar un nuevo socio.  
- Editar información de socio.  
- Buscar socios.  
- Eliminar o desactivar socio.  
- Validaciones de campos requeridos.  

### 3.3 Reportes y Documentos
- **Informe Global**: Resumen ejecutivo de la cartera de aportes, ahorros y préstamos.
- **Comprobantes**: Al realizar un aporte, el sistema permite descargar el recibo oficial.
- **Certificados**: Generación de documentos oficiales de préstamos y membresía en formato PDF.

### 3.4 Gestión de Ahorros
- El sistema permite manejar diversos tipos de ahorros:
  - **Ahorro Navideño**: Con tasa de interés preferencial.
  - **Ahorro Escolar/Infantil**: Para metas específicas.
  - **Inversión a Plazo**: Certificados financieros con vencimiento.
- Consulta de intereses ganados acumulados en tiempo real.

### 3.5 Configuración
- Cambiar contraseña.  
- Actualizar perfil de usuario.  
- Ajustar preferencias del sistema.  
- Activar modo oscuro/claro.  

### 3.6 Certificados
- Solicitar nuevo certificado.  
- Tipos de certificados disponibles:  
  - Certificado de aportaciones.  
  - Certificado de antigüedad.  
  - Certificado de buen standing.  
- Ver estado de solicitud.  
- Descargar certificado emitido.  
- Historial de certificados.  
- Validar certificado mediante código QR.  

### 3.7 Retiros
- Solicitar nuevo retiro.  
- Tipos de retiro disponibles:  
  - Retiro parcial de aportaciones.  
  - Retiro total (cierre de cuenta).  
  - Retiro de intereses/dividendos.  
- Monto mínimo y máximo permitido.  
- Tiempo de procesamiento.  
- **Validación de Garantía**: Si tiene préstamos activos, el sistema bloqueará retiros que dejen la cuenta de aportes por debajo del margen de garantía requerido.
- Ver estado de solicitud.  
- Historial de retiros.  
- Cancelar solicitud pendiente.  

---

## 4. Navegación y Atajos
### 4.1 Menú Principal
El sistema utiliza un menú lateral colapsable. Puede alternar su visibilidad haciendo clic en el botón de "hamburguesa" (tres líneas horizontales) situado en la esquina superior izquierda. Las opciones están categorizadas por módulos (Socios, Préstamos, Ahorros, Reportes) para facilitar el acceso.

### 4.2 Búsqueda Rápida
En todas las pantallas de listado (como el buscador de socios), encontrará una barra de filtros superior. Puede escribir nombres, números de cédula o códigos de socio; la tabla se actualizará automáticamente sin necesidad de recargar la página.

### 4.3 Notificaciones
El sistema cuenta con un centro de alertas en tiempo real ubicado en la barra superior (icono de campana). Un indicador numérico verde aparecerá cuando existan eventos pendientes, tales como:
- Préstamos que han entrado en estado de **Mora**.
- Solicitudes de retiro que superan el margen de garantía y requieren revisión.
- Vencimientos próximos de certificados de inversión a plazo.

### 4.4 Cerrar Sesión
Para salir del sistema de forma segura, haga clic en el círculo con su nombre o avatar en la parte superior derecha (Chip de Perfil) y seleccione la opción "Cerrar Sesión". Esto limpiará los datos de su sesión actual.

---

## 5. Preguntas Frecuentes (FAQ)
**¿Cómo recupero mi contraseña?**  
En la pantalla de acceso, haga clic en "¿Olvidó su contraseña?". Se le enviará un correo con las instrucciones. Si no tiene un correo configurado, el administrador de la cooperativa puede resetear su clave desde el panel de gestión de usuarios.

**¿Por qué no puedo ver ciertos módulos?**  
La visibilidad de los módulos de "Configuración" y "Reportes Globales" está restringida según el rol de usuario. Si necesita acceso adicional, solicite al administrador que actualice sus permisos de perfil.

**¿Cómo contacto soporte técnico?**  
Puede enviar un reporte directamente a través del correo soporte@coopcontrol.com o utilizar el formulario de incidencias disponible en la sección de contacto al final del menú lateral.

**¿El sistema funciona en móviles?**  
Sí, **CoopControl Web** está construido con una arquitectura responsiva que se adapta automáticamente a teléfonos inteligentes y tabletas, permitiendo gestionar solicitudes y aprobaciones desde cualquier lugar.

---

## 6. Solución de Problemas Comunes

### 6.1 Error de inicio de sesión
- **Causa**: Credenciales incorrectas o falta de permisos.
- **Solución**: Verifique que su usuario y contraseña sean correctos (recuerde que el sistema distingue entre mayúsculas y minúsculas). Si el problema persiste, utilice la opción "Recuperar contraseña" o contacte al administrador para verificar si su cuenta está activa.

### 6.2 Página no carga correctamente
- **Causa**: Problemas de conexión a internet o caché del navegador desactualizada.
- **Solución**: Intente recargar la página presionando `Ctrl + F5`. Asegúrese de estar utilizando un navegador compatible y que su conexión a internet sea estable.

### 6.3 Datos no se guardan
- **Causa**: Campos obligatorios vacíos o pérdida de sesión por inactividad.
- **Solución**: Revise que todos los campos marcados con asterisco (*) o indicados como requeridos estén completos y sigan el formato correcto. Si ha pasado mucho tiempo inactivo, cierre sesión y vuelva a entrar para refrescar su token de acceso.

### 6.4 Problemas de visualización
- **Causa**: Resolución de pantalla inadecuada o conflictos con el modo oscuro/claro.
- **Solución**: Verifique que su resolución de pantalla sea la mínima recomendada. Si los elementos se ven superpuestos, intente cambiar el tema visual (Modo Oscuro/Claro) desde la configuración para forzar el refresco de los estilos de Radzen.

---

## 7. Contacto y Soporte
- Email de soporte: soporte@coopcontrol.com  
- Horarios de atención: Lunes a viernes, 8:00 AM - 5:00 PM  
- Formulario de incidencias disponible en el sistema  
