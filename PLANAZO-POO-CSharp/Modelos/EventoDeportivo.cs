namespace PLANAZO_POO_CSharp.Modelos;

// Reto adicional: los eventos deportivos agregan un 5 % al precio base.
public class EventoDeportivo : Evento
{
    public EventoDeportivo(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima) { }

    public override decimal CalcularPrecioFinal() => PrecioBase * 1.05m;
}
