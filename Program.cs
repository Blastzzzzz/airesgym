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
            Console.WriteLine("6. Vender membresía");
            Console.WriteLine("7. Membresías por vencer");
            Console.WriteLine("8. Membresías vencidas");
            Console.WriteLine("9. Historial de un socio");
            Console.WriteLine("10. Reporte de ingresos mensual");
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
                else if (opcion == "6")
                    VenderMembresia();
                else if (opcion == "7")
                    ListarPorVencer();
                else if (opcion == "8")
                    ListarVencidas();
                else if (opcion == "9")
                    VerHistorial();
                else if (opcion == "10")
                    VerReporteIngresos();
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
        static void VenderMembresia()
    {
        Console.Write("RUT del socio: ");
        string rut = Console.ReadLine();

        SocioLogica socioLogica = new SocioLogica();
        Socio socio = socioLogica.Buscar(rut);

        if (socio == null)
        {
            Console.WriteLine("No existe un socio con ese RUT. Puede registrarlo con la opción 1.");
            return;
        }
        Console.WriteLine("Socio: " + socio.Nombre + " (" + socio.Estado + ")");
        
                PlanLogica planLogica = new PlanLogica();
        List<Plan> planes = planLogica.Listar();

        for (int i = 0; i < planes.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + planes[i].Nombre + " - $" + planes[i].PrecioBase.ToString("N0"));
        }

        Console.Write("Número de plan: ");
        int numPlan;
        if (!int.TryParse(Console.ReadLine(), out numPlan) || numPlan < 1 || numPlan > planes.Count)
        {
            Console.WriteLine("Plan no válido.");
            return;
        }
        Plan plan = planes[numPlan - 1];
                for (int i = 0; i < MembresiaLogica.MetodosPago.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + MembresiaLogica.MetodosPago[i]);
        }

        Console.Write("Método de pago: ");
        int numMetodo;
        if (!int.TryParse(Console.ReadLine(), out numMetodo) || numMetodo < 1 || numMetodo > MembresiaLogica.MetodosPago.Length)
        {
            Console.WriteLine("Método de pago no válido.");
            return;
        }
        string metodo = MembresiaLogica.MetodosPago[numMetodo - 1];

                Console.WriteLine("Plan: " + plan.Nombre);
        Console.WriteLine("Monto a pagar: $" + plan.PrecioBase.ToString("N0"));
        Console.WriteLine("Termina: " + plan.CalcularTermino(DateTime.Now).ToString("dd/MM/yyyy HH:mm"));
        Console.Write("¿Confirmar la venta? (s/n): ");
        string respuesta = Console.ReadLine();

        if (respuesta != "s")
        {
            Console.WriteLine("Venta cancelada.");
            return;
        }

        MembresiaLogica membresiaLogica = new MembresiaLogica();
        membresiaLogica.Vender(socio, plan, usuario, metodo);
        Console.WriteLine("Venta registrada correctamente.");
    }
        static void ListarPorVencer()
    {
        MembresiaLogica logica = new MembresiaLogica();
        List<Membresia> lista = logica.ListarPorVencer();

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay membresías por vencer.");
            return;
        }

        foreach (Membresia m in lista)
        {
            Console.WriteLine(m.NombreSocio + " | " + m.RutSocio + " | " + m.NombrePlan +
                              " | termina " + m.FechaTermino.ToString("dd/MM/yyyy") +
                              " | faltan " + m.DiasRestantes() + " día(s)");
        }
    }
        static void ListarVencidas()
    {
        MembresiaLogica logica = new MembresiaLogica();
        List<Membresia> lista = logica.ListarVencidas();

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay membresías vencidas.");
            return;
        }

        foreach (Membresia m in lista)
        {
            Console.WriteLine(m.NombreSocio + " | " + m.RutSocio + " | " + m.NombrePlan +
                              " | terminó " + m.FechaTermino.ToString("dd/MM/yyyy") +
                              " | hace " + m.DiasDesdeVencimiento() + " día(s)");
        }
    }
        static void VerHistorial()
    {
        Console.Write("RUT del socio: ");
        string rut = Console.ReadLine();

        SocioLogica socioLogica = new SocioLogica();
        Socio socio = socioLogica.Buscar(rut);

        if (socio == null)
        {
            Console.WriteLine("No existe un socio con ese RUT.");
            return;
        }

        MembresiaLogica membresiaLogica = new MembresiaLogica();
        List<Membresia> historial = membresiaLogica.ObtenerHistorial(socio.IdSocio);

        Console.WriteLine("Historial de " + socio.Nombre);

        if (historial.Count == 0)
        {
            Console.WriteLine("Este socio no tiene membresías.");
            return;
        }

        foreach (Membresia m in historial)
        {
            string estado;
            if (m.EstaVigente())
                estado = "Vigente";
            else
                estado = "Vencida";

            Console.WriteLine(m.NombrePlan + " | " + m.FechaInicio.ToString("dd/MM/yyyy") +
                              " al " + m.FechaTermino.ToString("dd/MM/yyyy") +
                              " | $" + m.MontoPagado.ToString("N0") + " | " + estado);
        }
    }
    static void VerReporteIngresos()
    {
        if (!usuario.EsDueno)
        {
            Console.WriteLine("Esta opción es solo para el dueño.");
            return;
        }

        Console.Write("Año (ej: 2026): ");
        int anio;
        if (!int.TryParse(Console.ReadLine(), out anio))
        {
            Console.WriteLine("Año no válido.");
            return;
        }

        Console.Write("Mes (1-12): ");
        int mes;
        if (!int.TryParse(Console.ReadLine(), out mes))
        {
            Console.WriteLine("Mes no válido.");
            return;
        }

        MembresiaLogica logica = new MembresiaLogica();
        List<Membresia> ventas = logica.ObtenerVentasDelMes(anio, mes);

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas en ese mes.");
            return;
        }

        decimal total = 0;
        foreach (Membresia m in ventas)
        {
            Console.WriteLine(m.FechaInicio.ToString("dd/MM/yyyy") + " | " + m.NombreSocio +
                              " | " + m.NombrePlan + " | $" + m.MontoPagado.ToString("N0"));
            total = total + m.MontoPagado;
        }

        Console.WriteLine("Cantidad de ventas: " + ventas.Count);
        Console.WriteLine("TOTAL DEL MES: $" + total.ToString("N0"));
    }
}
