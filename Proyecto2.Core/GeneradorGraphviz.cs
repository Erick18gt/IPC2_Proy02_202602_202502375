using System;
using System.Diagnostics;
using System.IO;
using Proyecto2.TDA;

namespace Proyecto2.Core
{
    // Genera un archivo .dot (el formato de texto que entiende Graphviz)
    // a partir de los libros de una categoría, y luego llama al programa
    // externo 'dot' (que instalaste aparte en tu PC) para convertirlo en
    // una imagen .png que la página Razor pueda mostrar.
    public class GeneradorGraphviz
    {
        // Genera la imagen y devuelve la ruta del archivo .png ya creado
        // (o null si la categoría no existe).
        public string GenerarImagenDeCategoria(Catalogo catalogo, string nombreCategoria, string carpetaSalida)
        {
            NodoCategoria categoria = catalogo.BuscarCategoria(nombreCategoria);
            if (categoria == null)
                return null;

            string textoDot = ConstruirTextoDot(categoria);

            string nombreArchivo = "grafico_" + nombreCategoria.Replace(" ", "_");
            string rutaDot = Path.Combine(carpetaSalida, nombreArchivo + ".dot");
            string rutaPng = Path.Combine(carpetaSalida, nombreArchivo + ".png");

            File.WriteAllText(rutaDot, textoDot);
            EjecutarDot(rutaDot, rutaPng);

            return rutaPng;
        }

        // Arma el texto en formato DOT (el lenguaje de Graphviz), recorriendo
        // la categoría indicada y TODAS sus subcategorías de forma recursiva.
        // Esto es justo lo que pide el enunciado: "visualizar la estructura
        // organizativa del catálogo desde las categorías generales hasta
        // los elementos más específicos". Antes solo se dibujaban los libros
        // de una única categoría, sin bajar a sus subcategorías.
        private string ConstruirTextoDot(NodoCategoria categoriaInicio)
        {
            string texto = "digraph G {\n";
            texto += "  rankdir=TB;\n";
            texto += "  node [fontname=\"Arial\"];\n";
            texto += ConstruirNodosYRelaciones(categoriaInicio);
            texto += "}\n";
            return texto;
        }

        // Dibuja el nodo de la categoría actual y sus libros (hojas), y
        // luego se llama a sí misma para cada subcategoría (PrimerHijo,
        // SiguienteHermano). Así queda representado TODO el subárbol que
        // cuelga de la categoría inicial, no solo un nivel.
        private string ConstruirNodosYRelaciones(NodoCategoria categoria)
        {
            // Usamos un id interno ("cat_" + nombre) distinto de la etiqueta
            // visible, para evitar cualquier choque de nombres dentro del .dot.
            string idCategoria = "cat_" + categoria.Nombre;
            string texto = "  \"" + idCategoria + "\" [label=\"" + categoria.Nombre +
                "\", shape=folder, style=filled, fillcolor=\"#a9c6f5\"];\n";

            // Libros de ESTA categoría (ya vienen ordenados ascendente por
            // ISBN, porque así los mantiene ArbolCategorias al insertarlos).
            NodoLibro libro = categoria.PrimerLibro;
            while (libro != null)
            {
                string idLibro = "libro_" + libro.Dato.ISBN;
                string etiquetaLibro = libro.Dato.ISBN + "\\n" + libro.Dato.Titulo;
                texto += "  \"" + idLibro + "\" [label=\"" + etiquetaLibro +
                    "\", shape=note, style=filled, fillcolor=\"#fdf3b0\"];\n";
                texto += "  \"" + idCategoria + "\" -> \"" + idLibro + "\";\n";
                libro = libro.Siguiente;
            }

            // Subcategorías (hijos), en orden alfabético (así están
            // encadenadas). Por cada una, dibujamos la flecha hacia ella
            // y repetimos el mismo proceso para SU subárbol (recursión).
            NodoCategoria hijo = categoria.PrimerHijo;
            while (hijo != null)
            {
                string idHijo = "cat_" + hijo.Nombre;
                texto += "  \"" + idCategoria + "\" -> \"" + idHijo + "\";\n";
                texto += ConstruirNodosYRelaciones(hijo);
                hijo = hijo.SiguienteHermano;
            }

            return texto;
        }

        // Ejecuta el programa externo 'dot' pasándole el archivo .dot y
        // pidiéndole que genere un .png. Esto solo funciona si Graphviz
        // está instalado y 'dot' está agregado al PATH del sistema.
        private void EjecutarDot(string rutaDot, string rutaPng)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "dot";
            info.Arguments = "-Tpng \"" + rutaDot + "\" -o \"" + rutaPng + "\"";
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            try
            {
                using (Process proceso = Process.Start(info))
                {
                    proceso.WaitForExit();
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Este error específico significa que Windows no encontró
                // 'dot.exe'. Lanzamos una excepción con un mensaje claro
                // en vez de dejar que se propague el error críptico original.
                throw new Exception("No se encontró el programa 'dot' de Graphviz. " +
                    "Verifica que esté instalado y agregado al PATH del sistema, " +
                    "y reinicia Visual Studio despues de instalarlo.");
            }
        }
    }
}
