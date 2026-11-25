using System;
using System.Collections.Generic;

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

    public void ValidarDueno(Empleado usuario)
    {
        if (!usuario.EsDueno)
            throw new Exception("Solo el dueño puede gestionar los empleados.");
    }

    public List<Empleado> Listar(Empleado usuario)
    {
        ValidarDueno(usuario);
        return datos.ListarTodos();
    }

    public void Agregar(Empleado usuario, string rut, string nombre, string telefono)
    {
        ValidarDueno(usuario);

        if (string.IsNullOrWhiteSpace(rut))
            throw new Exception("El RUT es obligatorio.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exception("El teléfono es obligatorio.");

        if (datos.BuscarPorRut(rut.Trim()) != null)
            throw new Exception("Ya existe un empleado con ese RUT.");

        Empleado empleado = new Empleado();
        empleado.Rut = rut.Trim();
        empleado.Nombre = nombre.Trim();
        empleado.Telefono = telefono.Trim();
        empleado.EsDueno = false;
        empleado.Activo = true;

        datos.Insertar(empleado);
    }

    public void Modificar(Empleado usuario, string rut, string nombre, string telefono)
    {
        ValidarDueno(usuario);

        Empleado empleado = datos.BuscarPorRut(rut);

        if (empleado == null)
            throw new Exception("No existe un empleado con ese RUT.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exception("El teléfono es obligatorio.");

        empleado.Nombre = nombre.Trim();
        empleado.Telefono = telefono.Trim();
        datos.Actualizar(empleado);
    }

    public Empleado CambiarEstado(Empleado usuario, string rut)
    {
        ValidarDueno(usuario);

        Empleado empleado = datos.BuscarPorRut(rut);

        if (empleado == null)
            throw new Exception("No existe un empleado con ese RUT.");

        if (empleado.IdEmpleado == usuario.IdEmpleado)
            throw new Exception("No puede cambiar su propio estado.");

        empleado.CambiarEstado();
        datos.ActualizarEstado(empleado);
        return empleado;
    }
}
