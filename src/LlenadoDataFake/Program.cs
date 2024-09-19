using Bogus;
using System.Data.SqlClient;

namespace LlenadoDataFake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GeneradorClientes.InsertarClientesEnBaseDeDatos(
                "Server=localhost;Database=paginacion;integrated security=true;TrustServerCertificate=true;",
                GeneradorClientes.GenerarClientes(50000));
        }
    }

    public class Cliente
    {
        public int ClienteId { get; set; } // Primary Key
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Empresa { get; set; }
    }

    public static class GeneradorClientes
    {
        public static List<Cliente> GenerarClientes(int cantidad)
        {
            var faker = new Faker<Cliente>()
                .RuleFor(c => c.Nombre, f => f.Name.FirstName())
                .RuleFor(c => c.Apellido, f => f.Name.LastName())
                .RuleFor(c => c.Direccion, f => f.Address.FullAddress())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.FechaNacimiento, f => f.Date.Past(50))
                .RuleFor(c => c.FechaCreacion, f => f.Date.Past(10))
                .RuleFor(c => c.Empresa, f => f.Company.CompanyName());

            return faker.Generate(cantidad);
        }

        public static void InsertarClientesEnBaseDeDatos(string connectionString, List<Cliente> clientes)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var cliente in clientes)
                {
                    var command = new SqlCommand("INSERT INTO Clientes (Nombre, Apellido, Direccion, Email, FechaNacimiento, FechaCreacion, Empresa) " +
                                                 "VALUES (@Nombre, @Apellido, @Direccion, @Email, @FechaNacimiento, @FechaCreacion, @Empresa)", connection);

                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    command.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                    command.Parameters.AddWithValue("@FechaCreacion", cliente.FechaCreacion);
                    command.Parameters.AddWithValue("@Empresa", cliente.Empresa);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
