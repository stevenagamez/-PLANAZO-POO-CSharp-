namespace PLANAZO_POO_CSharp.Modelos;

public class Usuario
{
    public int Id { get; }
    public string Nombre { get; private set; }

    public Usuario(int id, string nombre)
    {
        if (id <= 0) throw new ArgumentException("El identificador debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es obligatorio.");
        Id = id;
        Nombre = nombre.Trim();
    }

    public override string ToString() => $"ID: {Id} | Nombre: {Nombre}";
}
