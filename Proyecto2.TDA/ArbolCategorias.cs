namespace Proyecto2.TDA
{
    // Árbol N-ario: organiza las categorías y subcategorías del catálogo,
    // igual que las secciones de una biblioteca física o las carpetas
    // de una computadora (una carpeta contiene subcarpetas, que contienen
    // subcarpetas, etc).
    public class ArbolCategorias
    {
        private NodoCategoria raiz;

        // Propiedad de solo lectura para poder ver la raíz desde afuera si hace falta.
        public NodoCategoria Raiz => raiz;

        // Busca un nodo por nombre en TODO el árbol.
        // Es un recorrido recursivo: primero baja por los hijos,
        // y si no encuentra nada, sigue por los hermanos.
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

        // Agrega una categoría nueva. Si nombrePadre es null, se agrega
        // como categoría de nivel superior (sin padre).
        // Devuelve false si el nombre ya existe (deben ser únicos) o si
        // el padre indicado no se encontró.
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

        // Inserta 'nuevo' dentro de la cadena de hermanos que arranca en
        // 'cabezaHermanos', manteniendo orden alfabético (el enunciado
        // pide que las categorías se muestren ordenadas alfabéticamente).
        // Usamos 'ref' porque necesitamos poder CAMBIAR cuál es la cabeza
        // de la cadena si el nuevo nodo debe quedar de primero.
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

        // Devuelve un texto con la estructura completa del árbol (o desde
        // una subcategoría específica si se indica 'nombreInicio'), con
        // sangría según el nivel de profundidad. Pensado para mostrarlo
        // tal cual en una página Razor dentro de un <pre>.
        public string MostrarEstructura(string nombreInicio = null)
        {
            NodoCategoria inicio = nombreInicio == null ? raiz : Buscar(nombreInicio);
            if (inicio == null) return "(categoría no encontrada, o el árbol todavía está vacío)";

            return ConstruirTexto(inicio, 0);
        }

        private string ConstruirTexto(NodoCategoria nodo, int nivel)
        {
            if (nodo == null) return "";

            string texto = ObtenerSangria(nivel) + "- " + nodo.Nombre + "\n";
            texto += ConstruirTexto(nodo.PrimerHijo, nivel + 1); // bajamos a los hijos, un nivel más adentro
            texto += ConstruirTexto(nodo.SiguienteHermano, nivel); // seguimos con los hermanos, mismo nivel
            return texto;
        }

        // Arma el espacio de sangría "a mano", con un ciclo, en vez de
        // usar new string(' ', n) (que por dentro usa un array de char).
        private string ObtenerSangria(int nivel)
        {
            string sangria = "";
            int espacios = nivel * 2;
            for (int i = 0; i < espacios; i++)
            {
                sangria += " ";
            }
            return sangria;
        }
    }
}
