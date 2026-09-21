namespace Proyecto2.TDA
{
    // Nodo del árbol de categorías, usando la técnica
    // "primer hijo, siguiente hermano". Esto permite que un nodo tenga
    // CUALQUIER número de hijos sin necesitar List<NodoCategoria>.
    //
    // Ejemplo visual:
    //
    //   Ficcion
    //     |
    //   PrimerHijo -> Fantasia -> SiguienteHermano -> Terror -> SiguienteHermano -> CienciaFiccion
    //
    // "Fantasia", "Terror" y "CienciaFiccion" son subcategorías de "Ficcion"
    // (están conectadas entre sí como una cadena horizontal de hermanos).
    // Cada una de ellas, a su vez, puede tener su propia cadena de hijos.
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
