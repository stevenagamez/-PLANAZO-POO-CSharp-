namespace PLANAZO_POO_CSharp.Modelos;

public class Reserva
{
    public int Id { get; }
    public Usuario Usuario { get; }
    public string NombrePersona => Usuario.Nombre;
    public Evento Evento { get; }
    public int CantidadEntradas { get; }
    public decimal ValorTotal { get; }
    public bool EstaActiva { get; private set; }

    public string Estado => EstaActiva ? "Activa" : "Cancelada";

    public Reserva(int id, Usuario usuario, Evento evento, int cantidadEntradas)
    {
        if (id <= 0) throw new ArgumentException("El identificador de reserva debe ser mayor que cero.");
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);
        if (cantidadEntradas <= 0) throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");

        Id = id;
        Usuario = usuario;
        Evento = evento;
        CantidadEntradas = cantidadEntradas;
        ValorTotal = evento.CalcularPrecioFinal() * cantidadEntradas;
        EstaActiva = true;
    }

    public void Cancelar()
    {
        if (!EstaActiva) throw new InvalidOperationException("Esta reserva ya fue cancelada.");
        EstaActiva = false;
    }

    public override string ToString() =>
        $"Reserva #{Id} | Persona: {NombrePersona} | Evento: {Evento.Nombre} | " +
        $"Entradas: {CantidadEntradas} | Total: {ValorTotal:C} | Estado: {Estado}";
}
