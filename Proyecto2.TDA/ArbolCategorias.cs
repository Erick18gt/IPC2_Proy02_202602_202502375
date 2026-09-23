namespace Proyecto2.TDA
{

    public class ArbolCategorias
    {
        private NodoCategoria raiz;

       
        public NodoCategoria Raiz => raiz;

        // Busca un nodo por nombre en TODO el árbol.
      
        public NodoCategoria Buscar(string nombre)
        {
            return BuscarDesde(raiz, nombre);
        }

        private NodoCategoria BuscarDesde(NodoCategoria nodo, string nombre)
        {
            if (nodo == null) return null;
            if (nodo.Nombre == nombre) return nodo;

            // Primero intentamos encontrarlo entre los descendientes (bajando)
            NodoCategoria encontradoEnHijos = BuscarDesde(nodo.PrimerHijo, nombre);
            if (encontradoEnHijos != null) return encontradoEnHijos;

            // Si no estaba ahí, seguimos buscando entre los hermanos (mismo nivel)
            return BuscarDesde(nodo.SiguienteHermano, nombre);
        }

        
        public bool AgregarCategoria(string nombre, string nombrePadre)
        {
            if (Buscar(nombre) != null)
                return false; // el enunciado exige nombres únicos en todo el árbol

            NodoCategoria nuevo = new NodoCategoria(nombre);

            if (nombrePadre == null)
            {
                if (raiz == null)
                {
                    raiz = nuevo; // es la primera categoría de todas
                }
                else
                {
                    InsertarOrdenadoEntreHermanos(ref raiz, nuevo);
                }
                return true;
            }

            NodoCategoria padre = Buscar(nombrePadre);
            if (padre == null)
                return false; // el padre que indicaron no existe en el árbol

            InsertarOrdenadoEntreHermanos(ref padre.PrimerHijo, nuevo);
            return true;
        }

       
        private void InsertarOrdenadoEntreHermanos(ref NodoCategoria cabezaHermanos, NodoCategoria nuevo)
        {
            if (cabezaHermanos == null || string.Compare(nuevo.Nombre, cabezaHermanos.Nombre) < 0)
            {
                nuevo.SiguienteHermano = cabezaHermanos;
                cabezaHermanos = nuevo; // el nuevo nodo pasa a ser el primero de la cadena
                return;
            }

            NodoCategoria actual = cabezaHermanos;
            while (actual.SiguienteHermano != null &&
                   string.Compare(actual.SiguienteHermano.Nombre, nuevo.Nombre) < 0)
            {
                actual = actual.SiguienteHermano; // avanzamos hasta el lugar correcto
            }
            nuevo.SiguienteHermano = actual.SiguienteHermano;
            actual.SiguienteHermano = nuevo;
        }

        // Agrega un libro a la categoría indicada, dejando la lista de
        // libros de esa categoría ordenada ascendentemente por ISBN.
        public bool AgregarLibroACategoria(string nombreCategoria, Libro libro)
        {
            NodoCategoria categoria = Buscar(nombreCategoria);
            if (categoria == null) return false;

            categoria.PrimerLibro = InsertarLibroOrdenado(categoria.PrimerLibro, libro);
            return true;
        }

        private NodoLibro InsertarLibroOrdenado(NodoLibro cabeza, Libro libro)
        {
            NodoLibro nuevo = new NodoLibro(libro);

            if (cabeza == null || libro.ISBN < cabeza.Dato.ISBN)
            {
                nuevo.Siguiente = cabeza;
                return nuevo;
            }

            NodoLibro actual = cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Dato.ISBN < libro.ISBN)
            {
                actual = actual.Siguiente;
            }
            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
            return cabeza;
        }

        public string MostrarEstructura(string nombreInicio = null)
        {
            NodoCategoria inicio = nombreInicio == null ? raiz : Buscar(nombreInicio);
            if (inicio == null) return "(categoría no encontrada, o el árbol todavía está vacío)";

            return ConstruirTexto(inicio, 0);
        }

        private string ConstruirTexto(NodoCategoria nodo, int nivel)
        {
            if (nodo == null) return "";

            string texto = new string(' ', nivel * 2) + "- " + nodo.Nombre + "\n";
            texto += ConstruirTexto(nodo.PrimerHijo, nivel + 1); // bajamos a los hijos, un nivel más adentro
            texto += ConstruirTexto(nodo.SiguienteHermano, nivel); // seguimos con los hermanos, mismo nivel
            return texto;
        }
    }
}
