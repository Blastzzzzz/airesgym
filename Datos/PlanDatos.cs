using System;
using System.Collections.Generic;
using MySqlConnector;

public class PlanDatos
{
    private string cadena = "Server=localhost;Port=3306;Database=airesgymdb;User=root;Password=;";
    public List<Plan> ListarTodos()
    {
        List<Plan> lista = new List<Plan>();

        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();
            string sql = "SELECT p.id_plan, p.nombre, p.precio_base, d.horas_validez, m.cantidad_meses " +
                         "FROM plan p " +
                         "LEFT JOIN plan_diario d ON d.id_plan = p.id_plan " +
                         "LEFT JOIN plan_por_mes m ON m.id_plan = p.id_plan " +
                         "ORDER BY p.id_plan";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            MySqlDataReader lector = comando.ExecuteReader();
            
            while (lector.Read())
            {
                Plan plan;

                if (lector["horas_validez"] != DBNull.Value)
                {
                    PlanDiario diario = new PlanDiario();
                    diario.HorasValidez = (int)lector["horas_validez"];
                    plan = diario;
                }
                else
                {
                    PlanPorMes mensual = new PlanPorMes();
                    mensual.CantidadMeses = (int)lector["cantidad_meses"];
                    plan = mensual;
                }

                plan.IdPlan = (int)lector["id_plan"];
                plan.Nombre = (string)lector["nombre"];
                plan.PrecioBase = (decimal)lector["precio_base"];
                lista.Add(plan);
            }
        }

        return lista;
    }
        public void Insertar(Plan plan)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();
            MySqlTransaction transaccion = conexion.BeginTransaction();

            try
            {
                string sqlPlan = "INSERT INTO plan (nombre, precio_base) VALUES (@nombre, @precio)";
                MySqlCommand cmdPlan = new MySqlCommand(sqlPlan, conexion, transaccion);
                cmdPlan.Parameters.AddWithValue("@nombre", plan.Nombre);
                cmdPlan.Parameters.AddWithValue("@precio", plan.PrecioBase);
                cmdPlan.ExecuteNonQuery();
                long idNuevo = cmdPlan.LastInsertedId;

                if (plan is PlanDiario)
                {
                    PlanDiario diario = (PlanDiario)plan;
                    string sql = "INSERT INTO plan_diario (id_plan, horas_validez) VALUES (@id, @horas)";
                    MySqlCommand cmd = new MySqlCommand(sql, conexion, transaccion);
                    cmd.Parameters.AddWithValue("@id", idNuevo);
                    cmd.Parameters.AddWithValue("@horas", diario.HorasValidez);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    PlanPorMes mensual = (PlanPorMes)plan;
                    string sql = "INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (@id, @meses)";
                    MySqlCommand cmd = new MySqlCommand(sql, conexion, transaccion);
                    cmd.Parameters.AddWithValue("@id", idNuevo);
                    cmd.Parameters.AddWithValue("@meses", mensual.CantidadMeses);
                    cmd.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }
    }
        public void Actualizar(Plan plan)
    {
        using (MySqlConnection conexion = new MySqlConnection(cadena))
        {
            conexion.Open();

            string sql = "UPDATE plan SET nombre = @nombre, precio_base = @precio WHERE id_plan = @id";
            MySqlCommand comando = new MySqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", plan.Nombre);
            comando.Parameters.AddWithValue("@precio", plan.PrecioBase);
            comando.Parameters.AddWithValue("@id", plan.IdPlan);
            comando.ExecuteNonQuery();
        }
    }
}