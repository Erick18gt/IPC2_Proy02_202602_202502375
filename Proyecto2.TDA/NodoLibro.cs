namespace Proyecto2.TDA
{
    // Nodo de una lista enlazada simple.
    // Cada nodo es una "cajita" que guarda UN libro y una flecha (referencia)
    // hacia el siguiente nodo de la cadena. Encadenando muchos de estos,
    // armamos una lista sin usar List<T> de C#.
    public class NodoLibro
    {
        public Libro Dato;         // el libro que guarda esta cajita
        public NodoLibro Siguiente; // la flecha hacia la siguiente cajita (o null si es la última)

        public NodoLibro(Libro dato)
        {
            Dato = dato;
            Siguiente = null; // al crearse, todavía no apunta a nada
        }
    }
}
