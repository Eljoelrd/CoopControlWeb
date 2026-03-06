/*
using System.Linq;
using CoopControlWeb.Modelos;
using BCrypt.Net;

namespace CoopControlWeb.Modelos
{
    public class UserService
    {
        private readonly AppDBContext context;

        public UserService(AppDBContext context)
        {
            this.context = context;
        }

        // Método para guardar un nuevo usuario
        public bool SaveUser(User user)
        {
            // Verifica si ya existe un usuario con el mismo correo electrónico
            bool isExist = context.Users.Any(x => x.Email.ToLower() == user.Email.ToLower());
            if (!isExist)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        // Método para verificar las credenciales de un usuario
        public User? Verify(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u =>
                u.Email.ToLower() == email.ToLower() && u.Estado);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public User? GetByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public bool UpdatePerfil(string email, string nombre, string apellido, string telefono, string direccion)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            user.Nombre = nombre; // Asignar Nombre
            user.Apellido = apellido; // Asignar Apellido

            // Campos en User o Socio para Teléfono/Dirección
            // user.Telefono = telefono;
            // user.Direccion = direccion;

            context.SaveChanges();
            return true;
        }

        public bool UpdateUser(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
            return true;
        }


    }
}
 


 */