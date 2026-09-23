namespace Proyecto2.TDA
{
  
    public class Libro
    {
        public int ISBN;
        public string Titulo;
        public string Autor;
        public string NombreCategoria; // guardamos el NOMBRE de la categoría (un texto),
                                        // no una referencia directa al nodo del árbol,
                                        // para no complicar las dependencias entre clases.

        public Libro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            NombreCategoria = nombreCategoria;
        }

        // Sobreescribimos ToString() para que al imprimir un libro
        // (por ejemplo en una página Razor) se vea legible de una vez.
        public override string ToString()
        {
            return "[" + ISBN + "] " + Titulo + " - " + Autor;
        }
    }
}
