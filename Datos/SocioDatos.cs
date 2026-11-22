using System;
using MySqlConnector;

public class SocioDatos
{
    private string cadena = "Server=localhost;Port=3306;Database=airesgymdb;User=root;Password=;";
        public void Insertar(Socio socio)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "INSERT INTO socio (rut, nombre, telefono, estado, fecha_registro) " +
                         "VALUES (@rut, @nombre, @telefono, @estado, @fecha)";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
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