using PLANAZO_POO_CSharp.Modelos;

namespace PLANAZO_POO_CSharp.Servicios;

public class GestorReservas
{
    private readonly List<Evento> eventos = new();
    private readonly List<Reserva> reservas = new();
    private int siguienteReservaId = 1;

    public void AgregarEvento(Evento evento)
    {
        ArgumentNullException.ThrowIfNull(evento);
        if (eventos.Any(e => e.Id == evento.Id))
            throw new InvalidOperationException("Ya existe un evento con ese identificador.");
        eventos.Add(evento);
    }

    public List<Evento> ObtenerEventos() => eventos.ToList();

    public Evento BuscarEvento(int id)
    {
        return eventos.FirstOrDefault(e => e.Id == id)
            ?? throw new KeyNotFoundException($"No se encontró un evento con identificador {id}.");
    }

    public Reserva CrearReserva(Usuario usuario, Evento evento, int cantidad)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);

        // Se valida la cantidad antes de modificar los cupos.
        if (cantidad <= 0)
            throw new ArgumentException("No se pueden reservar cero entradas ni cantidades negativas.");

        Evento eventoRegistrado = BuscarEvento(evento.Id);
        if (cantidad > eventoRegistrado.CuposDisponibles)
            throw new InvalidOperationException("La cantidad solicitada supera los cupos disponibles.");

        var reserva = new Reserva(siguienteReservaId, usuario, eventoRegistrado, cantidad);
        eventoRegistrado.ReservarCupos(cantidad);
        reservas.Add(reserva);
        siguienteReservaId++;
        return reserva;
    }

    public List<Reserva> ObtenerReservas() => reservas.ToList();

    public void CancelarReserva(int idReserva)
    {
        Reserva reserva = reservas.FirstOrDefault(r => r.Id == idReserva)
            ?? throw new KeyNotFoundException($"No existe una reserva con identificador {idReserva}.");

        if (!reserva.EstaActiva)
            throw new InvalidOperationException("Esta reserva ya fue cancelada anteriormente.");

        // Solo se devuelven los cupos si la reserva sigue activa.
        reserva.Evento.DevolverCupos(reserva.CantidadEntradas);
        reserva.Cancelar();
    }

    public List<Reserva> ObtenerReservasPorUsuario(int idUsuario) =>
        reservas.Where(r => r.Usuario.Id == idUsuario).ToList();

    public ResumenReservas ObtenerResumen()
    {
        List<Reserva> activas = reservas.Where(r => r.EstaActiva).ToList();
        return new ResumenReservas(
            activas.Count,
            activas.Sum(r => r.CantidadEntradas),
            activas.Sum(r => r.ValorTotal));
    }
}
