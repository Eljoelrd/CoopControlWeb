using CoopControlWeb.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Servicios
{
    public class PrestamoService
    {
        private readonly AppDBContext context;

        public PrestamoService(AppDBContext context)
        {
            this.context = context;
        }

        public List<Prestamo> GetAll() => context.Prestamos.Include(p => p.Socio).ToList();

        public Prestamo? GetById(int id) => context.Prestamos.Include(p => p.Socio).FirstOrDefault(p => p.Id == id);

        public bool Create(Prestamo prestamo)
        {
            context.Prestamos.Add(prestamo);
            context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var p = context.Prestamos.Find(id);
            if (p == null) return false;
            context.Prestamos.Remove(p);
            context.SaveChanges();
            return true;
        }
    }
}
