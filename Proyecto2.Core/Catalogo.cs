using Proyecto2.TDA;

namespace Proyecto2.Core
{
    
    public class Catalogo
    {
        private ArbolCategorias arbol;
        private TablaHash tabla;

        public Catalogo()
        {
            arbol = new ArbolCategorias();

            
            tabla = new TablaHash(10007);
        }

        // Agrega una categoría nueva. nombrePadre puede ser null si es
        // una categoría de nivel superior (sin padre).
        public bool AgregarCategoria(string nombre, string nombrePadre)
        {
            return arbol.AgregarCategoria(nombre, nombrePadre);
        }

        
        public bool RegistrarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            if (tabla.Buscar(isbn) != null)
                return false; // el ISBN es único, no se permite registrar uno repetido

            Libro libro = new Libro(isbn, titulo, autor, nombreCategoria);

            bool agregadoAlArbol = arbol.AgregarLibroACategoria(nombreCategoria, libro);
            if (!agregadoAlArbol)
                return false; // la categoría indicada no existe en el árbol

            tabla.Insertar(libro);
            return true;
        }

        // Búsqueda rápida por ISBN (usa la tabla hash).
        public Libro BuscarPorIsbn(int isbn)
        {
            return tabla.Buscar(isbn);
        }

       
        public bool EliminarLibro(int isbn)
        {
            Libro libro = tabla.Buscar(isbn);
            if (libro == null) return false; // no existe, no hay nada que eliminar

            tabla.Eliminar(isbn);
            EliminarDeListaDeCategoria(libro.NombreCategoria, isbn);
            return true;
        }

        // Quita el libro de la lista enlazada de su categoría en el árbol.
        private void EliminarDeListaDeCategoria(string nombreCategoria, int isbn)
        {
            NodoCategoria categoria = arbol.Buscar(nombreCategoria);
            if (categoria == null) return;

            NodoLibro actual = categoria.PrimerLibro;
            NodoLibro anterior = null;

            while (actual != null)
            {
                if (actual.Dato.ISBN == isbn)
                {
                    if (anterior == null)
                        categoria.PrimerLibro = actual.Siguiente;
                    else
                        anterior.Siguiente = actual.Siguiente;
                    return;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }
        }

        // Libro con el ISBN más pequeño de todo el catálogo.
        public Libro ObtenerMenor()
        {
            return tabla.ObtenerMenor();
        }

        // Libro con el ISBN más grande de todo el catálogo.
        public Libro ObtenerMayor()
        {
            return tabla.ObtenerMayor();
        }

        // Todos los libros del catálogo, en una lista enlazada ordenada
        // ascendentemente por ISBN.
        public NodoLibro ObtenerTodosOrdenados()
        {
            return tabla.ObtenerTodosOrdenadosPorIsbn();
        }

        // Texto con la estructura de categorías 
        public string MostrarEstructura(string nombreInicio = null)
        {
            return arbol.MostrarEstructura(nombreInicio);
        }

        // Busca un nodo de categoría por nombre
        public NodoCategoria BuscarCategoria(string nombre)
        {
            return arbol.Buscar(nombre);
        }
    }
}
