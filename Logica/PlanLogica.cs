using System;
using System.Collections.Generic;

public class PlanLogica
{
    private PlanDatos datos = new PlanDatos();

    public List<Plan> Listar()
    {
        return datos.ListarTodos();
    }
}