using System;
// CAPA DE LÓGICA: reglas de las ventas de membresías y de los listados. Valida y calcula; el guardado y las consultas las hace MembresiaDatos.
public class MembresiaLogica
{
    private MembresiaDatos datos = new MembresiaDatos();
    // Métodos de pago permitidos. Son los mismos que acepta el CHECK de la tabla membresia. Es static para usarla sin crear un objeto: MembresiaLogica.MetodosPago.
    public static string[] MetodosPago = { "Efectivo", "Debito", "Credito", "Transferencia" };
    // Registra la venta de una membresía, Primero revisa las reglas, después arma la membresía y la guarda. Devuelve la membresía registrada.
    public Membresia Vender(Socio socio, Plan plan, Empleado empleado, string metodoPago)
    {   // Regla: solo se vende a socios Activos. Si alguna regla falla, throw detiene el método y no se guarda nada.
        if (!socio.EstaActivo())
            throw new Exception("El socio está inactivo, no se le puede vender una membresía.");

        if (plan.PrecioBase <= 0)
            throw new Exception("El plan no tiene un precio válido.");

        if (Array.IndexOf(MetodosPago, metodoPago) < 0)
            throw new Exception("El método de pago no es válido.");

        Membresia m = new Membresia();
        m.IdSocio = socio.IdSocio;
        m.IdPlan = plan.IdPlan;
        m.IdEmpleado = empleado.IdEmpleado;
        m.FechaInicio = DateTime.Now;
        // POLIMORFISMO: plan es de tipo Plan, pero puede ser PlanDiario o PlanPorMes. C# ejecuta la versión de CalcularTermino del tipo real: suma horas o suma meses. Aquí no hace falta saber cuál es.
        m.FechaTermino = plan.CalcularTermino(m.FechaInicio);
        // Regla: el monto pagado es el precio del plan, que es fijo por plan.
        m.MontoPagado = plan.PrecioBase;
        m.MetodoPago = metodoPago;
        // Solo si pasó todas las reglas se pide a la capa de Datos que guarde la venta en MySQL.
        datos.Insertar(m);
        return m;
    }
        public List<Membresia> ListarPorVencer()
    {
        return datos.ListarPorVencer(DateTime.Today, DateTime.Today.AddDays(7));
    }
    // Por vencer = la fecha de término está entre hoy y dentro de 7 días, La Lógica decide las fechas y la capa de Datos ejecuta la consulta.
    public List<Membresia> ListarVencidas()
    {
        return datos.ListarVencidas(DateTime.Today);
    }
        public List<Membresia> ObtenerHistorial(int idSocio)
    {
        return datos.ListarPorSocio(idSocio);
    }
    public List<Membresia> ObtenerVentasDelMes(int anio, int mes)
    {
        if (mes < 1 || mes > 12)
            throw new Exception("El mes debe estar entre 1 y 12.");

        if (anio < 2000 || anio > 2100)
            throw new Exception("El año no es válido.");

        return datos.ListarDelMes(anio, mes);
    }
}
