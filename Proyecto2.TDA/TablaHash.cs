namespace Proyecto2.TDA
{
    
    public class TablaHash
    {
        
        private NodoLibro[] casillas;
        private int tamano;

        public TablaHash(int tamano)
        {
            this.tamano = tamano;      // guardamos el tamaño para poder usarlo después
            casillas = new NodoLibro[tamano]; // reservamos el array de casillas, todas empiezan en null
        }

        private int Hash(int isbn)
        {
            int pos = isbn % tamano;
            if (pos < 0) pos += tamano; // por si acaso llega un ISBN negativo, no debería pasar pero es defensivo
            return pos;
        }

        // Inserta un libro. 
        public void Insertar(Libro libro)
        {
            int pos = Hash(libro.ISBN);
            NodoLibro nuevo = new NodoLibro(libro);
            nuevo.Siguiente = casillas[pos]; // el nuevo nodo apunta a lo que ya había en la casilla
            casillas[pos] = nuevo;           // y la casilla ahora apunta al nuevo nodo (queda primero)
        }

        // Busca un libro por ISBN. Va directo a la casilla correspondiente
        // y solo recorre esa lista pequeña 
        public Libro Buscar(int isbn)
        {
            int pos = Hash(isbn);
            NodoLibro actual = casillas[pos];

            while (actual != null)
            {
                if (actual.Dato.ISBN == isbn)
                    return actual.Dato; // lo encontramos
                actual = actual.Siguiente; // seguimos a la siguiente cajita de esa casilla
            }

            return null; 
        }

        // Elimina un libro por ISBN. Devuelve true si lo encontró y lo quitó.
        public bool Eliminar(int isbn)
        {
            int pos = Hash(isbn);
            NodoLibro actual = casillas[pos];
            NodoLibro anterior = null; 

            while (actual != null)
            {
                if (actual.Dato.ISBN == isbn)
                {
                    if (anterior == null)
                    {
                        // el que vamos a borrar era el primero de la casilla
                        casillas[pos] = actual.Siguiente;
                    }
                    else
                    {
                        // "saltamos" el nodo actual: el de atrás ahora apunta
                        // directo al de adelante, dejando fuera al que borramos
                        anterior.Siguiente = actual.Siguiente;
                    }
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

            for (int i = 0; i < tamano; i++)
            {
                NodoLibro actual = casillas[i];
                while (actual != null)
                {
                    cabezaOrdenada = InsertarOrdenado(cabezaOrdenada, actual.Dato);
                    actual = actual.Siguiente;
                }
            }

            return cabezaOrdenada;
        }

        
        private NodoLibro InsertarOrdenado(NodoLibro cabeza, Libro libro)
        {
            NodoLibro nuevo = new NodoLibro(libro);

            if (cabeza == null || libro.ISBN < cabeza.Dato.ISBN)
            {
                nuevo.Siguiente = cabeza;
                return nuevo; // el nuevo libro es el más chico, pasa a ser la cabeza
            }

            NodoLibro actual = cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Dato.ISBN < libro.ISBN)
            {
                actual = actual.Siguiente; // avanzamos hasta encontrar el lugar correcto
            }
            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
            return cabeza;
        }

        // Devuelve el libro con el ISBN más pequeño de todo el catálogo.
        public Libro ObtenerMenor()
        {
            NodoLibro lista = ObtenerTodosOrdenadosPorIsbn();
            return lista == null ? null : lista.Dato; // ya viene ordenada ascendente: el primero es el menor
        }

        // Devuelve el libro con el ISBN más grande de todo el catálogo.
        public Libro ObtenerMayor()
        {
            NodoLibro lista = ObtenerTodosOrdenadosPorIsbn();
            if (lista == null) return null;

            while (lista.Siguiente != null)
                lista = lista.Siguiente; // avanzamos hasta el último nodo

            return lista.Dato;
        }
    }
}
