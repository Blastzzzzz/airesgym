    // Socio.cs
using System;

public class Socio
{
    public int IdSocio { get; set; }
    public string Rut { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Estado { get; set; }
    public DateTime FechaRegistro { get; set; }

    public bool EstaActivo()
    {
        return Estado == "Activo";
    }
        public void CambiarEstado()
        {
            if (EstaActivo())
            {
                Estado = "Inactivo";
            }
            else
            {
                Estado = "Activo";
            }
    }
}
public abstract class Plan
{
    public int IdPlan { get; set; }
    public string Nombre { get; set; }
    public decimal PrecioBase { get; set; }

    public abstract DateTime CalcularTermino(DateTime fechaInicio);
}
public class PlanDiario : Plan
{
    public int HorasValidez { get; set; }

    public override DateTime CalcularTermino(DateTime fechaInicio)
    {
        return fechaInicio.AddHours(HorasValidez);
    }
}
public class PlanPorMes : Plan
{
    public int CantidadMeses { get; set; }

    public override DateTime CalcularTermino(DateTime fechaInicio)
    {
        return fechaInicio.AddMonths(CantidadMeses);
    }
}
public class Empleado
{
    public int IdEmpleado { get; set; }
    public string Rut { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public bool EsDueno { get; set; }
    public bool Activo { get; set; }
}public class Membresia
{
    public int IdMembresia { get; set; }
    public int IdSocio { get; set; }
    public int IdPlan { get; set; }
    public int IdEmpleado { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaTermino { get; set; }
    public decimal MontoPagado { get; set; }
    public string MetodoPago { get; set; }

    public bool EstaVigente()
    {
        return FechaTermino.Date >= DateTime.Today;
    }

    public int DiasRestantes()
    {
        return (FechaTermino.Date - DateTime.Today).Days;
    }
        public string NombreSocio { get; set; }
    public string RutSocio { get; set; }
    public string NombrePlan { get; set; }

    public bool EstaVencida()
    {
        return FechaTermino.Date < DateTime.Today;
    }

    public bool EstaPorVencer()
    {
        return EstaVigente() && DiasRestantes() <= 7;
    }

    public int DiasDesdeVencimiento()
    {
        return (DateTime.Today - FechaTermino.Date).Days;
    }
}