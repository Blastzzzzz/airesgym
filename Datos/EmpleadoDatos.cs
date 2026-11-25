using System;
using System.Collections.Generic;
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
                return LeerEmpleado(lector);

            return null;
        }
    }

    public List<Empleado> ListarTodos()
    {
        List<Empleado> lista = new List<Empleado>();

        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "SELECT id_empleado, rut, nombre, telefono, es_dueno, activo " +
                         "FROM empleado ORDER BY es_dueno DESC, nombre";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            MySqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(LeerEmpleado(lector));
            }
        }
        return lista;
    }

    public void Insertar(Empleado empleado)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "INSERT INTO empleado (rut, nombre, telefono, es_dueno, activo) " +
                         "VALUES (@rut, @nombre, @telefono, @esDueno, @activo)";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@rut", empleado.Rut);
            comando.Parameters.AddWithValue("@nombre", empleado.Nombre);
            comando.Parameters.AddWithValue("@telefono", empleado.Telefono);
            comando.Parameters.AddWithValue("@esDueno", empleado.EsDueno);
            comando.Parameters.AddWithValue("@activo", empleado.Activo);
            comando.ExecuteNonQuery();
        }
    }

    public void Actualizar(Empleado empleado)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "UPDATE empleado SET nombre = @nombre, telefono = @telefono WHERE id_empleado = @id";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", empleado.Nombre);
            comando.Parameters.AddWithValue("@telefono", empleado.Telefono);
            comando.Parameters.AddWithValue("@id", empleado.IdEmpleado);
            comando.ExecuteNonQuery();
        }
    }

    public void ActualizarEstado(Empleado empleado)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "UPDATE empleado SET activo = @activo WHERE id_empleado = @id";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@activo", empleado.Activo);
            comando.Parameters.AddWithValue("@id", empleado.IdEmpleado);
            comando.ExecuteNonQuery();
        }
    }

    private Empleado LeerEmpleado(MySqlDataReader lector)
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
}
