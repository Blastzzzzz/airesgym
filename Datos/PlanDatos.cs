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
}