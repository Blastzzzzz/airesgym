using System;
// CAPA DE LÓGICA DE NEGOCIO: aquí van las reglas (qué datos son válidos y qué operaciones se permiten). Revisa todo antes de pedirle a la capa de Datos que guarde. No muestra nada en pantalla ni escribe SQL.
public class SocioLogica
{
    private SocioDatos datos = new SocioDatos();
    // Agrega un socio nuevo. Primero revisa las reglas y, si alguna falla, lanza un error y no guarda nada. Si todo está bien, arma el objeto Socio y lo manda a guardar.
        public void Agregar(string rut, string nombre, string telefono)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(telefono))
            throw new Exception("El teléfono es obligatorio.");
    // Regla: el RUT debe ser único. Se busca si ya hay un socio con ese RUT; si la búsqueda devuelve algo distinto de null, ya existe. Así se avisa en español, en vez de dejar que MySQL responda con un error técnico.
        if (datos.BuscarPorRut(rut) != null)
            throw new Exception("Ya existe un socio con ese RUT.");
        // Se arma el objeto Socio con los datos. Trim() quita los espacios sobrantes al principio y al final.
        Socio socio = new Socio();
        socio.Rut = rut;
        socio.Nombre = nombre.Trim();
        socio.Telefono = telefono.Trim();
        socio.Estado = "Activo";
        socio.FechaRegistro = DateTime.Now;
        // Solo si pasó todas las reglas, se le pide a la capa de Datos que lo guarde en MySQL.
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