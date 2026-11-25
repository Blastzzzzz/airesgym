using System;
using MySqlConnector;
// CAPA DE ACCESO A DATOS: esta clase es la única que habla con MySQL para los socios. No tiene reglas de negocio ni muestra nada en pantalla; solo guarda y busca datos.
public class SocioDatos
{   // Cadena de conexión: indica dónde está la base de datos (servidor y puerto), cuál es (airesgymdb) y con qué usuario entrar. Es private porque solo esta clase la necesita.
    private string cadena = "Server=localhost;Port=3306;Database=airesgymdb;User=root;Password=;";
    // Guarda un socio nuevo en la base de datos. Recibe un objeto Socio ya armado por la capa de Lógica y no devuelve nada (void).
        public void Insertar(Socio socio)
    {
        // "using" abre la conexión y la cierra sola al terminar el bloque, aunque ocurra un error. Así no quedan conexiones abiertas.
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();
            // Orden SQL que inserta una fila. Los valores van como parámetros (@rut, @nombre...) y no pegados al texto, para prevenir la inyección SQL 

            string sql = "INSERT INTO socio (rut, nombre, telefono, estado, fecha_registro) " +
                         "VALUES (@rut, @nombre, @telefono, @estado, @fecha)";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            // Cada AddWithValue rellena un parámetro (@) con el dato del socio. El valor viaja aparte de la consulta, y por eso no puede alterarla.
            comando.Parameters.AddWithValue("@rut", socio.Rut);
            comando.Parameters.AddWithValue("@nombre", socio.Nombre);
            comando.Parameters.AddWithValue("@telefono", socio.Telefono);
            comando.Parameters.AddWithValue("@estado", socio.Estado);
            comando.Parameters.AddWithValue("@fecha", socio.FechaRegistro);
            comando.ExecuteNonQuery();
        }
    }
        public Socio BuscarPorRut(string rut)
        {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "SELECT id_socio, rut, nombre, telefono, estado, fecha_registro " +
                         "FROM socio WHERE rut = @rut";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@rut", rut);
            MySqlDataReader lector = comando.ExecuteReader();

            if (lector.Read())
            {
                Socio s = new Socio();
                s.IdSocio = (int)lector["id_socio"];
                s.Rut = (string)lector["rut"];
                s.Nombre = (string)lector["nombre"];
                s.Telefono = (string)lector["telefono"];
                s.Estado = (string)lector["estado"];
                s.FechaRegistro = (DateTime)lector["fecha_registro"];
                return s;
            }
            return null;
             }
         }
             public void ActualizarEstado(Socio socio)
            {
                using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
            conexion.Open();

            string sql = "UPDATE socio SET estado = @estado WHERE id_socio = @id";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@estado", socio.Estado);
            comando.Parameters.AddWithValue("@id", socio.IdSocio);
            comando.ExecuteNonQuery();
        }
    }
    // Modifica el nombre y el teléfono de un socio. El RUT no se modifica porque es lo que identifica al socio.
        public void Actualizar(Socio socio)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "UPDATE socio SET nombre = @nombre, telefono = @telefono WHERE id_socio = @id";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", socio.Nombre);
            comando.Parameters.AddWithValue("@telefono", socio.Telefono);
            comando.Parameters.AddWithValue("@id", socio.IdSocio);
            comando.ExecuteNonQuery();
        }
    }
}