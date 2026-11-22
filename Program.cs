using System;

public class Program

{
    static Empleado usuario;
    static void Main(string[] args)
    {
         while (usuario == null)
        {
            Console.Write("RUT del empleado: ");
            string rutEmpleado = Console.ReadLine();

            try
            {
                EmpleadoLogica logica = new EmpleadoLogica();
                usuario = logica.IniciarSesion(rutEmpleado);
                Console.WriteLine("Bienvenido, " + usuario.Nombre);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        bool seguir = true;
        while (seguir)
        {
            Console.WriteLine();
            Console.WriteLine("1. Agregar socio");
            Console.WriteLine("2. Buscar socio");
            Console.WriteLine("3. Cambiar estado de un socio");
            Console.WriteLine("4. Modificar socio");
            Console.WriteLine("5. Listar planes");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            string opcion = Console.ReadLine();

            try
            {
                if (opcion == "1")
                    AgregarSocio();
                else if (opcion == "2")
                    BuscarSocio();
                else if (opcion == "3")                          
                    CambiarEstadoSocio();
                else if (opcion == "4")
                    ModificarSocio();
                else if (opcion == "5")
                    ListarPlanes();      
                else if (opcion == "0")
                    seguir = false;
                else
                    Console.WriteLine("Opción no válida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void AgregarSocio()
    {
        Console.Write("RUT: ");
        string rut = Console.ReadLine();
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Teléfono: ");
        string telefono = Console.ReadLine();

        SocioLogica logica = new SocioLogica();
        logica.Agregar(rut, nombre, telefono);
        Console.WriteLine("Socio guardado correctamente.");
    }

    static void BuscarSocio()
    {
        Console.Write("RUT: ");
        string rut = Console.ReadLine();

        SocioLogica logica = new SocioLogica();
        Socio socio = logica.Buscar(rut);

        if (socio != null)
            Console.WriteLine("Socio encontrado: " + socio.Nombre + " (" + socio.Estado + ")");
        else
            Console.WriteLine("No existe un socio con ese RUT.");
    }
        static void CambiarEstadoSocio()
    {
        Console.Write("RUT: ");
        string rut = Console.ReadLine();

        SocioLogica logica = new SocioLogica();
        Socio socio = logica.CambiarEstado(rut);

        Console.WriteLine("El estado de " + socio.Nombre + " ahora es: " + socio.Estado);
    }
        static void ModificarSocio()
    {
        Console.Write("RUT del socio a modificar: ");
        string rut = Console.ReadLine();
        Console.Write("Nuevo nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Nuevo teléfono: ");
        string telefono = Console.ReadLine();

        SocioLogica logica = new SocioLogica();
        logica.Modificar(rut, nombre, telefono);
        Console.WriteLine("Socio modificado correctamente.");
    }
        static void ListarPlanes()
    {
        PlanLogica logica = new PlanLogica();
        List<Plan> planes = logica.Listar();

        foreach (Plan p in planes)
        {
            Console.WriteLine(p.Nombre + " | $" + p.PrecioBase.ToString("N0") +
                              " | termina: " + p.CalcularTermino(DateTime.Now).ToString("dd/MM/yyyy"));
        }
    }
}