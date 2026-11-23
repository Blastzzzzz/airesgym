using System;
using System.Collections.Generic;
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
        private string consultaBase =
        "SELECT m.id_membresia, m.id_socio, m.id_plan, m.id_empleado, m.fecha_inicio, " +
        "m.fecha_termino, m.monto_pagado, m.metodo_pago, " +
        "s.nombre AS nombre_socio, s.rut AS rut_socio, p.nombre AS nombre_plan " +
        "FROM membresia m " +
        "INNER JOIN socio s ON s.id_socio = m.id_socio " +
        "INNER JOIN plan p ON p.id_plan = m.id_plan ";
        public List<Membresia> ListarPorVencer(DateTime hoy, DateTime limite)
    {
        List<Membresia> lista = new List<Membresia>();

        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = consultaBase +
                         "WHERE DATE(m.fecha_termino) BETWEEN @hoy AND @limite " +
                         "AND NOT EXISTS (SELECT 1 FROM membresia m2 " +
                         "WHERE m2.id_socio = m.id_socio AND m2.fecha_termino > m.fecha_termino) " +
                         "ORDER BY m.fecha_termino";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@hoy", hoy.Date);
            comando.Parameters.AddWithValue("@limite", limite.Date);
            lista = LeerMembresias(comando);
        }
        return lista;
    }
        public List<Membresia> ListarVencidas(DateTime hoy)
    {
        List<Membresia> lista = new List<Membresia>();

        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = consultaBase +
                         "WHERE DATE(m.fecha_termino) < @hoy " +
                         "AND NOT EXISTS (SELECT 1 FROM membresia m2 " +
                         "WHERE m2.id_socio = m.id_socio AND m2.fecha_termino > m.fecha_termino) " +
                         "ORDER BY m.fecha_termino DESC";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@hoy", hoy.Date);
            lista = LeerMembresias(comando);
        }
        return lista;
    }
        private List<Membresia> LeerMembresias(MySqlCommand comando)
    {
        List<Membresia> lista = new List<Membresia>();
        MySqlDataReader lector = comando.ExecuteReader();

        while (lector.Read())
        {
            Membresia m = new Membresia();
            m.IdMembresia = (int)lector["id_membresia"];
            m.IdSocio = (int)lector["id_socio"];
            m.IdPlan = (int)lector["id_plan"];
            m.IdEmpleado = (int)lector["id_empleado"];
            m.FechaInicio = (DateTime)lector["fecha_inicio"];
            m.FechaTermino = (DateTime)lector["fecha_termino"];
            m.MontoPagado = (decimal)lector["monto_pagado"];
            m.MetodoPago = (string)lector["metodo_pago"];
            m.NombreSocio = (string)lector["nombre_socio"];
            m.RutSocio = (string)lector["rut_socio"];
            m.NombrePlan = (string)lector["nombre_plan"];
            lista.Add(m);
        }
        return lista;
    }
}