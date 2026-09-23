namespace Proyecto2.TDA
{
    // Antes usábamos un array de C# (NodoLibro[]) para representar las
    // "casillas" de la tabla hash. Como no podemos usar arrays, cada
    // casilla ahora es un nodo más, y todas las casillas se encadenan
    // entre sí formando una lista enlazada de tamaño fijo (se arma una
    // sola vez, en el constructor de TablaHash, y ya no cambia de tamaño).
    public class NodoCasilla
    {
        public NodoLibro PrimerLibro;  // cabeza de la lista de libros que cayeron en esta casilla
        public NodoCasilla Siguiente;  // la siguiente casilla de la tabla

        public NodoCasilla()
        {
            PrimerLibro = null;
            Siguiente = null;
        }
    }
}
