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
// Clase abstracta (abstracción): define lo que tienen en común todos los planes y no se puede crear con new. Solo se crean sus hijas, PlanDiario y PlanPorMes.
public abstract class Plan
{
    private int idPlan;
    protected string nombre;
    protected decimal precioBase;

    public int IdPlan { get { return idPlan; } set { idPlan = value; } }
    public string Nombre { get { return nombre; } set { nombre = value; } }
    public decimal PrecioBase { get { return precioBase; } set { precioBase = value; } }

    // Método abstracto: no tiene cuerpo aquí. Cada plan hijo debe escribir su propia versión (polimorfismo).
    public abstract DateTime CalcularTermino(DateTime fechaInicio);
}
// Herencia: PlanDiario "es un" Plan. Recibe IdPlan, Nombre y PrecioBase de la clase madre y agrega su dato propio: las horas de validez.
public class PlanDiario : Plan
{
    private int horasValidez;

    public int HorasValidez { get { return horasValidez; } set { horasValidez = value; } }
    // Versión propia del plan diario: la fecha de término es la fecha de inicio más sus horas de validez.
    public override DateTime CalcularTermino(DateTime fechaInicio)
    {
        return fechaInicio.AddHours(horasValidez);
    }
}
// Herencia: PlanPorMes también "es un" Plan. Su dato propio es la cantidad de meses que dura.
public class PlanPorMes : Plan
{
    private int cantidadMeses;

    public int CantidadMeses { get { return cantidadMeses; } set { cantidadMeses = value; } }
    // Versión propia del plan por mes: la fecha de término es la fecha de inicio más sus meses. El mismo método que PlanDiario, pero con otro comportamiento.
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
// Representa una venta: qué socio compró qué plan, quién la vendió, cuándo empieza y termina, y cuánto pagó. Cada objeto es una fila de la tabla membresia.
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
    // Datos que no están en la tabla membresia: vienen de socio y plan mediante un JOIN. Solo sirven para mostrar en pantalla.
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
// EstaVigente: la fecha de término es hoy o posterior. Se compara solo la fecha, sin la hora.
    public bool EstaVigente()
    {
        return fechaTermino.Date >= DateTime.Today;
    }
// Cuántos días faltan para el término. Si ya venció, el resultado es negativo.
    public int DiasRestantes()
    {
        return (fechaTermino.Date - DateTime.Today).Days;
    }
// Vencida: la fecha de término es anterior a hoy.
    public bool EstaVencida()
    {
        return fechaTermino.Date < DateTime.Today;
    }
// Por vencer: sigue vigente y faltan 7 días o menos para el término. Si ya venció, no está por vencer.
    public bool EstaPorVencer()
    {
        return EstaVigente() && DiasRestantes() <= 7;
    }
// Cuántos días pasaron desde que terminó la membresía.
    public int DiasDesdeVencimiento()
    {
        return (DateTime.Today - fechaTermino.Date).Days;
    }
}