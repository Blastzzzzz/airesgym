using System;
using MySqlConnector;

public class EmpleadoDatos
{
    private string cadena = "Server=localhost;Port=3306;Database=airesgymdb;User=root;Password=;";

    public Empleado BuscarPorRut(string rut)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "SELECT id_empleado, rut, nombre, telefono, es_dueno, activo " +
                         "FROM empleado WHERE rut = @rut";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@rut", rut);
            MySqlDataReader lector = comando.ExecuteReader();

            if (lector.Read())
            {
                Empleado e = new Empleado();
                e.IdEmpleado = (int)lector["id_empleado"];
                e.Rut = (string)lector["rut"];
                e.Nombre = (string)lector["nombre"];
                e.Telefono = (string)lector["telefono"];
                e.EsDueno = Convert.ToBoolean(lector["es_dueno"]);
                e.Activo = Convert.ToBoolean(lector["activo"]);
                return e;
            }
            return null;
        }
    }
}