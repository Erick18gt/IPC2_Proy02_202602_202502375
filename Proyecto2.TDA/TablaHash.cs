namespace Proyecto2.TDA
{
   
    public class TablaHash
    {
        private NodoCasilla primeraCasilla; // cabeza de la cadena de casillas
        private int tamano;

        public TablaHash(int tamano)
        {
            this.tamano = tamano;
            ConstruirCasillas();
        }

        private void ConstruirCasillas()
        {
            NodoCasilla anterior = null;

            for (int i = 0; i < tamano; i++)
            {
                NodoCasilla nueva = new NodoCasilla();

                if (primeraCasilla == null)
                    primeraCasilla = nueva; // la primera casilla que creamos
                else
                    anterior.Siguiente = nueva; // conectamos la anterior con esta nueva

                anterior = nueva;
            }
        }

      
        private NodoCasilla ObtenerCasilla(int indice)
        {
            NodoCasilla actual = primeraCasilla;
            int contador = 0;

            while (contador < indice)
            {
                actual = actual.Siguiente;
                contador++;
            }

            return actual;
        }

        // Función hash: convierte el ISBN en un índice válido
        private int Hash(int isbn)
        {
            int pos = isbn % tamano;
            if (pos < 0) pos += tamano;
            return pos;
        }

        // Inserta un libro al inicio de la lista de su casilla.
        public void Insertar(Libro libro)
        {
            int pos = Hash(libro.ISBN);
            NodoCasilla casilla = ObtenerCasilla(pos);

            NodoLibro nuevo = new NodoLibro(libro);
            nuevo.Siguiente = casilla.PrimerLibro;
            casilla.PrimerLibro = nuevo;
        }

        // Busca un libro por ISBN: va a su casilla y recorre solo esa
        // lista pequeña 
        public Libro Buscar(int isbn)
        {
            int pos = Hash(isbn);
            NodoCasilla casilla = ObtenerCasilla(pos);

            NodoLibro actual = casilla.PrimerLibro;
            while (actual != null)
            {
                if (actual.Dato.ISBN == isbn)
                    return actual.Dato;
                actual = actual.Siguiente;
            }

            return null;
        }

        // Elimina un libro por ISBN de la lista de su casilla.
        public bool Eliminar(int isbn)
        {
            int pos = Hash(isbn);
            NodoCasilla casilla = ObtenerCasilla(pos);

            NodoLibro actual = casilla.PrimerLibro;
            NodoLibro anterior = null;

            while (actual != null)
            {
                if (actual.Dato.ISBN == isbn)
                {
                    if (anterior == null)
                        casilla.PrimerLibro = actual.Siguiente;
                    else
                        anterior.Siguiente = actual.Siguiente;
                    return true;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }

            return false;
        }

       
        public NodoLibro ObtenerTodosOrdenadosPorIsbn()
        {
            NodoLibro cabezaOrdenada = null;
            NodoCasilla casillaActual = primeraCasilla;

            while (casillaActual != null)
            {
                NodoLibro actual = casillaActual.PrimerLibro;
                while (actual != null)
                {
                    cabezaOrdenada = InsertarOrdenado(cabezaOrdenada, actual.Dato);
                    actual = actual.Siguiente;
                }
                casillaActual = casillaActual.Siguiente;
            }

            return cabezaOrdenada;
        }

        private NodoLibro InsertarOrdenado(NodoLibro cabeza, Libro libro)
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

        public Libro ObtenerMenor()
        {
            NodoLibro lista = ObtenerTodosOrdenadosPorIsbn();
            return lista == null ? null : lista.Dato;
        }

        public Libro ObtenerMayor()
        {
            NodoLibro lista = ObtenerTodosOrdenadosPorIsbn();
            if (lista == null) return null;

            while (lista.Siguiente != null)
                lista = lista.Siguiente;

            return lista.Dato;
        }
    }
}
