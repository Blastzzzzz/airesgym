using System;

public class EmpleadoLogica
{
    private EmpleadoDatos datos = new EmpleadoDatos();

    public Empleado IniciarSesion(string rut)
    {
        Empleado empleado = datos.BuscarPorRut(rut);

        if (empleado == null)
            throw new Exception("No existe un empleado con ese RUT.");

        if (!empleado.Activo)
            throw new Exception("El empleado está inactivo.");

        return empleado;
    }
}