namespace PLANAZO_POO_CSharp.Servicios;

public class ResumenReservas
{
    public int CantidadReservasActivas { get; }
    public int CantidadEntradasReservadas { get; }
    public decimal ValorTotal { get; }

    public ResumenReservas(int cantidadReservasActivas, int cantidadEntradasReservadas, decimal valorTotal)
    {
        CantidadReservasActivas = cantidadReservasActivas;
        CantidadEntradasReservadas = cantidadEntradasReservadas;
        ValorTotal = valorTotal;
    }
}
