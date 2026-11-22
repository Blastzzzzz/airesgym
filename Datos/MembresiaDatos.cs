using System;
using MySqlConnector;

public class MembresiaDatos
{
    private string cadena = "Server=localhost;Port=3306;Database=airesgymdb;User=root;Password=;";

    public void Insertar(Membresia membresia)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "INSERT INTO membresia (id_socio, id_plan, id_empleado, fecha_inicio, " +
                         "fecha_termino, monto_pagado, metodo_pago) " +
                         "VALUES (@idSocio, @idPlan, @idEmpleado, @inicio, @termino, @monto, @metodo)";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@idSocio", membresia.IdSocio);
            comando.Parameters.AddWithValue("@idPlan", membresia.IdPlan);
            comando.Parameters.AddWithValue("@idEmpleado", membresia.IdEmpleado);
            comando.Parameters.AddWithValue("@inicio", membresia.FechaInicio);
            comando.Parameters.AddWithValue("@termino", membresia.FechaTermino);
            comando.Parameters.AddWithValue("@monto", membresia.MontoPagado);
            comando.Parameters.AddWithValue("@metodo", membresia.MetodoPago);
            comando.ExecuteNonQuery();
        }
    }
}