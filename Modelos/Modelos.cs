using System;

public class Socio
{
    private int idSocio;
    private string rut;
    private string nombre;
    private string telefono;
    private string estado;
    private DateTime fechaRegistro;

    public int IdSocio { get { return idSocio; } set { idSocio = value; } }
    public string Rut { get { return rut; } set { rut = value; } }
    public string Nombre { get { return nombre; } set { nombre = value; } }
    public string Telefono { get { return telefono; } set { telefono = value; } }
    public string Estado { get { return estado; } set { estado = value; } }
    public DateTime FechaRegistro { get { return fechaRegistro; } set { fechaRegistro = value; } }

    public bool EstaActivo()
    {
        return estado == "Activo";
    }

    public void CambiarEstado()
    {
        if (EstaActivo())
        {
            estado = "Inactivo";
        }
        else
        {
            estado = "Activo";
        }
    }
}

public abstract class Plan
{
    private int idPlan;
    protected string nombre;
    protected decimal precioBase;

    public int IdPlan { get { return idPlan; } set { idPlan = value; } }
    public string Nombre { get { return nombre; } set { nombre = value; } }
    public decimal PrecioBase { get { return precioBase; } set { precioBase = value; } }

    public abstract DateTime CalcularTermino(DateTime fechaInicio);
}

public class PlanDiario : Plan
{
    private int horasValidez;

    public int HorasValidez { get { return horasValidez; } set { horasValidez = value; } }

    public override DateTime CalcularTermino(DateTime fechaInicio)
    {
        return fechaInicio.AddHours(horasValidez);
    }
}

public class PlanPorMes : Plan
{
    private int cantidadMeses;

    public int CantidadMeses { get { return cantidadMeses; } set { cantidadMeses = value; } }

    public override DateTime CalcularTermino(DateTime fechaInicio)
    {
        return fechaInicio.AddMonths(cantidadMeses);
    }
}

public class Empleado
{
    private int idEmpleado;
    private string rut;
    private string nombre;
    private string telefono;
    private bool esDueno;
    private bool activo;

    public int IdEmpleado { get { return idEmpleado; } set { idEmpleado = value; } }
    public string Rut { get { return rut; } set { rut = value; } }
    public string Nombre { get { return nombre; } set { nombre = value; } }
    public string Telefono { get { return telefono; } set { telefono = value; } }
    public bool EsDueno { get { return esDueno; } set { esDueno = value; } }
    public bool Activo { get { return activo; } set { activo = value; } }

    public bool EstaActivo()
    {
        return activo;
    }

    public void CambiarEstado()
    {
        activo = !activo;
    }
}

public class Membresia
{
    private int idMembresia;
    private int idSocio;
    private int idPlan;
    private int idEmpleado;
    private DateTime fechaInicio;
    private DateTime fechaTermino;
    private decimal montoPagado;
    private string metodoPago;
    private string nombreSocio;
    private string rutSocio;
    private string nombrePlan;

    public int IdMembresia { get { return idMembresia; } set { idMembresia = value; } }
    public int IdSocio { get { return idSocio; } set { idSocio = value; } }
    public int IdPlan { get { return idPlan; } set { idPlan = value; } }
    public int IdEmpleado { get { return idEmpleado; } set { idEmpleado = value; } }
    public DateTime FechaInicio { get { return fechaInicio; } set { fechaInicio = value; } }
    public DateTime FechaTermino { get { return fechaTermino; } set { fechaTermino = value; } }
    public decimal MontoPagado { get { return montoPagado; } set { montoPagado = value; } }
    public string MetodoPago { get { return metodoPago; } set { metodoPago = value; } }
    public string NombreSocio { get { return nombreSocio; } set { nombreSocio = value; } }
    public string RutSocio { get { return rutSocio; } set { rutSocio = value; } }
    public string NombrePlan { get { return nombrePlan; } set { nombrePlan = value; } }

    public bool EstaVigente()
    {
        return fechaTermino.Date >= DateTime.Today;
    }

    public int DiasRestantes()
    {
        return (fechaTermino.Date - DateTime.Today).Days;
    }

    public bool EstaVencida()
    {
        return fechaTermino.Date < DateTime.Today;
    }

    public bool EstaPorVencer()
    {
        return EstaVigente() && DiasRestantes() <= 7;
    }

    public int DiasDesdeVencimiento()
    {
        return (DateTime.Today - fechaTermino.Date).Days;
    }
}