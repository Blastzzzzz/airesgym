using System;
using System.Collections.Generic;

public class PlanLogica
{
    private PlanDatos datos = new PlanDatos();

    public List<Plan> Listar()
    {
        return datos.ListarTodos();
    }
        private void ValidarDueno(Empleado empleado)
    {
        if (!empleado.EsDueno)
            throw new Exception("Solo el dueño puede gestionar los planes.");
    }

    private void ValidarPlan(string nombre, decimal precio, int duracion)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre del plan es obligatorio.");

        if (precio <= 0)
            throw new Exception("El precio debe ser mayor a 0.");

        if (duracion <= 0)
            throw new Exception("La duración debe ser mayor a 0.");

        foreach (Plan p in datos.ListarTodos())
        {
            if (p.Nombre.ToLower() == nombre.Trim().ToLower())
                throw new Exception("Ya existe un plan con ese nombre.");
        }
    }
        public void AgregarDiario(Empleado empleado, string nombre, decimal precio, int horas)
    {
        ValidarDueno(empleado);
        ValidarPlan(nombre, precio, horas);

        PlanDiario plan = new PlanDiario();
        plan.Nombre = nombre.Trim();
        plan.PrecioBase = precio;
        plan.HorasValidez = horas;
        datos.Insertar(plan);
    }
        public void AgregarPorMes(Empleado empleado, string nombre, decimal precio, int meses)
    {
        ValidarDueno(empleado);
        ValidarPlan(nombre, precio, meses);

        PlanPorMes plan = new PlanPorMes();
        plan.Nombre = nombre.Trim();
        plan.PrecioBase = precio;
        plan.CantidadMeses = meses;
        datos.Insertar(plan);
    }
        public void Modificar(Empleado empleado, Plan plan, string nombre, decimal precio)
    {
        ValidarDueno(empleado);

        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre del plan es obligatorio.");

        if (precio <= 0)
            throw new Exception("El precio debe ser mayor a 0.");

        foreach (Plan p in datos.ListarTodos())
        {
            if (p.IdPlan != plan.IdPlan && p.Nombre.ToLower() == nombre.Trim().ToLower())
                throw new Exception("Ya existe otro plan con ese nombre.");
        }

        plan.Nombre = nombre.Trim();
        plan.PrecioBase = precio;
        datos.Actualizar(plan);
    }
}