using System;

public class MembresiaLogica
{
    private MembresiaDatos datos = new MembresiaDatos();

    public static string[] MetodosPago = { "Efectivo", "Debito", "Credito", "Transferencia" };

    public Membresia Vender(Socio socio, Plan plan, Empleado empleado, string metodoPago)
    {
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
        m.FechaTermino = plan.CalcularTermino(m.FechaInicio);
        m.MontoPagado = plan.PrecioBase;
        m.MetodoPago = metodoPago;

        datos.Insertar(m);
        return m;
    }
}