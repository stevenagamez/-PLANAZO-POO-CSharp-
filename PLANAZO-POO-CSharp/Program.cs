using System.Globalization;
using PLANAZO_POO_CSharp.Modelos;
using PLANAZO_POO_CSharp.Servicios;

namespace PLANAZO_POO_CSharp;

internal class Program
{
    private static readonly GestorReservas gestor = new();
    private static readonly List<Usuario> usuarios = new();
    private static int siguienteUsuarioId = 1;

    private static void Main()
    {
        ConfigurarCultura();
        CargarEventosIniciales();

        bool continuar = true;
        while (continuar)
        {
            MostrarMenu();
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1": VerEventosDisponibles(); break;
                    case "2": ConsultarEvento(); break;
                    case "3": ReservarEntradas(); break;
                    case "4": VerReservasRealizadas(); break;
                    case "5": CancelarReserva(); break;
                    case "6": MostrarResumen(); break;
                    case "7": ConsultarReservasUsuario(); break;
                    case "0": continuar = false; Console.WriteLine("Gracias por usar PLANAZO."); break;
                    default: Console.WriteLine("Opción no válida. Intente nuevamente."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo completar la operación: {ex.Message}");
            }

            if (continuar)
            {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private static void ConfigurarCultura()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("es-CO");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("es-CO");
    }

    private static void MostrarMenu()
    {
        Console.WriteLine("================================");
        Console.WriteLine("       PLANAZO - EVENTOS");
        Console.WriteLine("================================");
        Console.WriteLine("1. Ver eventos disponibles");
        Console.WriteLine("2. Consultar evento");
        Console.WriteLine("3. Reservar entradas");
        Console.WriteLine("4. Ver reservas realizadas");
        Console.WriteLine("5. Cancelar una reserva");
        Console.WriteLine("6. Ver resumen de reservas");
        Console.WriteLine("7. Consultar reservas por usuario");
        Console.WriteLine("0. Salir");
        Console.WriteLine("================================");
    }

    private static void CargarEventosIniciales()
    {
        gestor.AgregarEvento(new Cine(1, "Aventura en el espacio", new DateTime(2026, 11, 5), 18000m, 80));
        gestor.AgregarEvento(new Cine(2, "Misterio en la ciudad", new DateTime(2026, 11, 8), 22000m, 60));
        gestor.AgregarEvento(new Concierto(3, "Noche de Rock", new DateTime(2026, 11, 12), 50000m, 150));
        gestor.AgregarEvento(new Concierto(4, "Festival de Música Latina", new DateTime(2026, 11, 20), 70000m, 200));
        gestor.AgregarEvento(new Taller(5, "Taller de fotografía", new DateTime(2026, 11, 15), 40000m, 25));
        gestor.AgregarEvento(new Taller(6, "Taller de programación", new DateTime(2026, 11, 18), 60000m, 30));
        gestor.AgregarEvento(new EventoDeportivo(7, "Torneo de fútbol", new DateTime(2026, 11, 25), 30000m, 100));
    }

    private static void VerEventosDisponibles()
    {
        Console.WriteLine("\n--- EVENTOS DISPONIBLES ---");
        foreach (Evento evento in gestor.ObtenerEventos())
        {
            Console.WriteLine(evento);
        }
    }

    private static void ConsultarEvento()
    {
        int id = LeerEntero("Ingrese el identificador del evento: ");
        Evento evento = gestor.BuscarEvento(id);
        Console.WriteLine("\n--- INFORMACIÓN DEL EVENTO ---");
        Console.WriteLine(evento);
        Console.WriteLine($"Precio base: {evento.PrecioBase:C}");
        Console.WriteLine($"Precio final por entrada: {evento.CalcularPrecioFinal():C}");
    }

    private static void ReservarEntradas()
    {
        Console.WriteLine("\n--- RESERVAR ENTRADAS ---");
        int idEvento = LeerEntero("Identificador del evento: ");
        Evento evento = gestor.BuscarEvento(idEvento);
        Console.WriteLine(evento);

        string nombre = LeerTexto("Nombre completo de la persona: ");
        int cantidad = LeerEntero("Cantidad de entradas: ");

        Usuario usuario = usuarios.FirstOrDefault(u =>
            u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))!;

        if (usuario is null)
        {
            usuario = new Usuario(siguienteUsuarioId++, nombre);
            usuarios.Add(usuario);
        }

        Reserva reserva = gestor.CrearReserva(usuario, evento, cantidad);
        Console.WriteLine("\n¡Reserva realizada correctamente!");
        Console.WriteLine(reserva);
        Console.WriteLine($"Cupos restantes: {evento.CuposDisponibles}");
    }

    private static void VerReservasRealizadas()
    {
        Console.WriteLine("\n--- RESERVAS REALIZADAS ---");
        List<Reserva> reservas = gestor.ObtenerReservas();
        if (reservas.Count == 0)
        {
            Console.WriteLine("Todavía no hay reservas registradas.");
            return;
        }

        foreach (Reserva reserva in reservas)
            Console.WriteLine(reserva);
    }

    private static void CancelarReserva()
    {
        Console.WriteLine("\n--- CANCELAR RESERVA ---");
        int id = LeerEntero("Identificador de la reserva: ");
        gestor.CancelarReserva(id);
        Console.WriteLine("La reserva fue cancelada y los cupos fueron devueltos.");
    }

    private static void MostrarResumen()
    {
        ResumenReservas resumen = gestor.ObtenerResumen();
        Console.WriteLine("\n--- RESUMEN DE RESERVAS ACTIVAS ---");
        Console.WriteLine($"Reservas activas: {resumen.CantidadReservasActivas}");
        Console.WriteLine($"Entradas reservadas: {resumen.CantidadEntradasReservadas}");
        Console.WriteLine($"Valor total: {resumen.ValorTotal:C}");
    }

    private static void ConsultarReservasUsuario()
    {
        if (usuarios.Count == 0)
        {
            Console.WriteLine("Todavía no hay usuarios registrados.");
            return;
        }

        Console.WriteLine("\n--- USUARIOS ---");
        foreach (Usuario usuario in usuarios)
            Console.WriteLine($"ID: {usuario.Id} | Nombre: {usuario.Nombre}");

        int idUsuario = LeerEntero("Identificador del usuario: ");
        Usuario seleccionado = usuarios.FirstOrDefault(u => u.Id == idUsuario)
            ?? throw new ArgumentException("No existe un usuario con ese identificador.");

        List<Reserva> reservas = gestor.ObtenerReservasPorUsuario(seleccionado.Id);
        Console.WriteLine($"\nReservas de {seleccionado.Nombre}:");
        if (reservas.Count == 0)
            Console.WriteLine("Este usuario no tiene reservas registradas.");
        else
            foreach (Reserva reserva in reservas)
                Console.WriteLine(reserva);
    }

    private static int LeerEntero(string mensaje)
    {
        Console.Write(mensaje);
        if (!int.TryParse(Console.ReadLine(), out int valor))
            throw new ArgumentException("Debe ingresar un número entero válido.");
        return valor;
    }

    private static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        string? valor = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Este campo no puede quedar vacío.");
        return valor.Trim();
    }
}
