# Gardeny

**Sitio principal:** [https://gardeny.app](https://gardeny.app)  
**Punto de venta (POS):** [https://pos.gardeny.app](https://pos.gardeny.app)  

Gardeny es una plataforma SaaS pensada para **gestión de negocios y puntos de venta**, diseñada para ofrecer control completo sobre productos, ventas y, en el futuro, clientes, proveedores, inventario y órdenes de compra. Actualmente, la aplicación permite administrar **múltiples sucursales** y está enfocada en negocios que requieren un sistema de punto de venta ágil y moderno.

---

## Tecnologías

- Backend: **C# .NET MVC** con **Arquitectura Limpia (Clean Architecture)**  
- ORM: **Entity Framework Core (Code First)**  
- Base de datos: **PostgreSQL**  
- Frontend: Web responsiva, diseñada para adaptarse a dispositivos de escritorio y móviles  

---

## Módulos actuales

1. **Categorías**  
   - Crear, editar y eliminar categorías de productos  
   - Organizar productos por categoría  

2. **Productos**  
   - Administrar información de productos: nombre, descripción, precio, stock inicial, tipo de producto  
   - Relacionar productos con categorías  

3. **Ventas / POS**  
   - Registrar ventas en tiempo real  
   - Generar tickets y facturas  
   - Gestionar ventas por sucursal  

---

## Planes futuros

- **Clientes:** Registro y administración de clientes frecuentes  
- **Proveedores:** Gestión de proveedores y contactos  
- **Inventario:** Control detallado de stock, alertas de productos bajos  
- **Órdenes de compra:** Crear, aprobar y recibir órdenes de compra  
- **Múltiples sucursales:** Consolidación de información y reportes por sucursal  
- **Roles y permisos avanzados:** Control de acceso por usuario y sucursal  

---

## Características destacadas

- **Multi-sucursal:** Gestiona varias sucursales desde un solo panel  
- **Escalable:** Pensado para crecer con tu negocio  
- **Responsivo:** Compatible con escritorio, tablet y móvil  
- **Arquitectura limpia:** Facilita mantenimiento, pruebas y escalabilidad del sistema  

---

## Para desarrolladores

- **Base de datos:** PostgreSQL (con EF Core Code First)  
- **Configuración:** Connection string en `appsettings.json` o mediante variables de entorno  
- **Migraciones:** Se manejan con EF Core Migrations (`dotnet ef migrations add` y `dotnet ef database update`)  
- **Instrucciones generales:**  
  1. Clonar el repositorio  
  2. Configurar `appsettings.json` o variables de entorno con la base de datos  
  3. Aplicar migraciones  
  4. Ejecutar la aplicación en Visual Studio o desde CLI  

---

## Para usuarios finales

- Accede a la aplicación principal en [https://gardeny.app](https://gardeny.app)  
- Accede al POS en [https://pos.gardeny.app](https://pos.gardeny.app)  
- Módulos disponibles: Categorías, Productos y Ventas  
- En futuras actualizaciones se habilitarán más módulos para clientes, proveedores, inventario y órdenes de compra  

---

## Contribuciones y soporte

- Gardeny es un proyecto en crecimiento; las contribuciones son bienvenidas desde el código o sugerencias de funcionalidad.  
