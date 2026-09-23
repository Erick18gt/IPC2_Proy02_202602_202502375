using System.Diagnostics;
using System.IO;
using Proyecto2.TDA;

namespace Proyecto2.Core
{

    public class GeneradorGraphviz
    {
        // Genera la imagen y devuelve la ruta del archivo .png ya creado
      
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

        // Arma el texto en formato DOT
        private string ConstruirTextoDot(NodoCategoria categoria)
        {
            string texto = "digraph G {\n";
            texto += "  \"" + categoria.Nombre + "\" [shape=box, style=filled, fillcolor=lightblue];\n";

            NodoLibro actual = categoria.PrimerLibro;
            while (actual != null)
            {
                string etiquetaLibro = actual.Dato.ISBN + "\\n" + actual.Dato.Titulo;
                texto += "  \"" + categoria.Nombre + "\" -> \"" + etiquetaLibro + "\";\n";
                actual = actual.Siguiente;
            }

            texto += "}\n";
            return texto;
        }

        
        private void EjecutarDot(string rutaDot, string rutaPng)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "dot";
            info.Arguments = "-Tpng \"" + rutaDot + "\" -o \"" + rutaPng + "\"";
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            using (Process proceso = Process.Start(info))
            {
                proceso.WaitForExit(); // esperamos a que Graphviz termine antes de seguir
            }
        }
    }
}
