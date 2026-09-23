using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto2.Core;

namespace IPC2_Proy2.Pages
{
    public class CargarXmlModel : PageModel
    {
        private readonly Catalogo catalogo;
        private readonly IWebHostEnvironment entorno;

        // ASP.NET Core nos "inyecta" automáticamente el Catalogo (el mismo
        // objeto, compartido durante toda la vida de la aplicación porque
        // lo registramos como Singleton en Program.cs) y el entorno de
        // hosting (para saber dónde está la carpeta wwwroot en disco).
        public CargarXmlModel(Catalogo catalogo, IWebHostEnvironment entorno)
        {
            this.catalogo = catalogo;
            this.entorno = entorno;
        }

        [BindProperty]
        public IFormFile ArchivoXml { get; set; }

        public string Mensaje { get; set; }
        public bool Exito { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (ArchivoXml == null || ArchivoXml.Length == 0)
            {
                Mensaje = "Debes seleccionar un archivo XML.";
                Exito = false;
                return;
            }

            // Guardamos el archivo subido en una carpeta temporal dentro de
            // wwwroot, porque XDocument.Load necesita una ruta real en disco.
            string carpetaTemporal = Path.Combine(entorno.WebRootPath, "temp");
            Directory.CreateDirectory(carpetaTemporal);
            string rutaArchivo = Path.Combine(carpetaTemporal, ArchivoXml.FileName);

            using (FileStream stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await ArchivoXml.CopyToAsync(stream);
            }

            try
            {
                LectorXml lector = new LectorXml();
                lector.Cargar(rutaArchivo, catalogo);
                Mensaje = "Archivo cargado correctamente.";
                Exito = true;
            }
            catch (Exception ex)
            {
                Mensaje = "Ocurrió un error al leer el archivo: " + ex.Message;
                Exito = false;
            }
        }
    }
}
