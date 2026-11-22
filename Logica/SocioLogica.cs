using System;

public class SocioLogica
{
    private SocioDatos datos = new SocioDatos();
        public void Agregar(string rut, string nombre, string telefono)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exception("El teléfono es obligatorio.");

        if (datos.BuscarPorRut(rut) != null)
            throw new Exception("Ya existe un socio con ese RUT.");

        Socio socio = new Socio();
        socio.Rut = rut;
        socio.Nombre = nombre.Trim();
        socio.Telefono = telefono.Trim();
        socio.Estado = "Activo";
        socio.FechaRegistro = DateTime.Now;

        datos.Insertar(socio);
    }
        public Socio Buscar(string rut)
    {
        return datos.BuscarPorRut(rut);
    }
        public Socio CambiarEstado(string rut)
    {
        Socio socio = datos.BuscarPorRut(rut);

        if (socio == null)
            throw new Exception("No existe un socio con ese RUT.");

        socio.CambiarEstado();
        datos.ActualizarEstado(socio);
        return socio;
    }
        public void Modificar(string rut, string nombre, string telefono)
    {
        Socio socio = datos.BuscarPorRut(rut);

        if (socio == null)
            throw new Exception("No existe un socio con ese RUT.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exception("El teléfono es obligatorio.");

        socio.Nombre = nombre.Trim();
        socio.Telefono = telefono.Trim();
        datos.Actualizar(socio);
    }
}