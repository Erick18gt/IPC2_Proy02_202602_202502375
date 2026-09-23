using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto2.Core;

namespace IPC2_Proy2.Pages
{
    public class GraphvizModel : PageModel
    {
        private readonly Catalogo catalogo;
        private readonly IWebHostEnvironment entorno;

        public GraphvizModel(Catalogo catalogo, IWebHostEnvironment entorno)
        {
            this.catalogo = catalogo;
            this.entorno = entorno;
        }

        [BindProperty]
        public string NombreCategoria { get; set; }

        // Ruta relativa que se usa directo en el atributo src de la imagen.
        public string RutaImagen { get; set; }
        public string Mensaje { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string carpetaGraficos = Path.Combine(entorno.WebRootPath, "graficos");
            Directory.CreateDirectory(carpetaGraficos);

            GeneradorGraphviz generador = new GeneradorGraphviz();
            string rutaFisica = generador.GenerarImagenDeCategoria(catalogo, NombreCategoria.Trim(), carpetaGraficos);

            if (rutaFisica == null)
            {
                Mensaje = "No se encontró esa categoría.";
                return;
            }

            string nombreArchivo = Path.GetFileName(rutaFisica);

            // Agregamos un valor único al final de la URL para que el
            // navegador no muestre una imagen vieja guardada en caché
            // con el mismo nombre de archivo.
            RutaImagen = "/graficos/" + nombreArchivo + "?v=" + DateTime.Now.Ticks;
        }
    }
}
