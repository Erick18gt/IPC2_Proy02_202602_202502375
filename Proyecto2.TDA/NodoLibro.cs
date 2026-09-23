namespace Proyecto2.TDA
{
    
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
