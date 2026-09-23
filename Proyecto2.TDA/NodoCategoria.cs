namespace Proyecto2.TDA
{
  
    public class NodoCategoria
    {
        public string Nombre;
        public NodoLibro PrimerLibro;          // lista enlazada de libros que pertenecen a ESTA categoría
        public NodoCategoria PrimerHijo;       // la primera subcategoría (si tiene alguna)
        public NodoCategoria SiguienteHermano; // la siguiente categoría al mismo nivel (mismo padre)

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            PrimerLibro = null;
            PrimerHijo = null;
            SiguienteHermano = null;
        }
    }
}
