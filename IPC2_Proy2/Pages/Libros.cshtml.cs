using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto2.Core;
using Proyecto2.TDA;

namespace IPC2_Proy2.Pages
{
    public class LibrosModel : PageModel
    {
        private readonly Catalogo catalogo;

        public LibrosModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }

        // Campos del formulario "Registrar nuevo libro"
        [BindProperty]
        public int IsbnRegistrar { get; set; }
        [BindProperty]
        public string TituloRegistrar { get; set; }
        [BindProperty]
        public string AutorRegistrar { get; set; }
        [BindProperty]
        public string CategoriaRegistrar { get; set; }

        // Campo compartido por los formularios de buscar/eliminar
        [BindProperty]
        public int IsbnConsulta { get; set; }

        public Libro LibroEncontrado { get; set; }
        public Libro LibroMenor { get; set; }
        public Libro LibroMayor { get; set; }

        public string Mensaje { get; set; }
        public bool Exito { get; set; }

        public void OnGet()
        {
        }

        // Cada botón "submit" de la página tiene su propio asp-page-handler,
        // así que cada uno dispara un método OnPost distinto en vez de que
        // todos caigan en un solo OnPost gigante con ifs.
        public void OnPostRegistrar()
        {
            bool seRegistro = catalogo.RegistrarLibro(IsbnRegistrar, TituloRegistrar, AutorRegistrar, CategoriaRegistrar);

            if (seRegistro)
            {
                Mensaje = "Libro registrado correctamente.";
                Exito = true;
            }
            else
            {
                Mensaje = "No se pudo registrar: el ISBN ya existe o la categoría no existe.";
                Exito = false;
            }
        }

        public void OnPostEliminar()
        {
            bool seElimino = catalogo.EliminarLibro(IsbnConsulta);
            Mensaje = seElimino ? "Libro eliminado correctamente." : "No se encontró un libro con ese ISBN.";
            Exito = seElimino;
        }

        public void OnPostBuscar()
        {
            LibroEncontrado = catalogo.BuscarPorIsbn(IsbnConsulta);
            if (LibroEncontrado == null)
            {
                Mensaje = "No se encontró un libro con ese ISBN.";
                Exito = false;
            }
        }

        public void OnPostMenorMayor()
        {
            LibroMenor = catalogo.ObtenerMenor();
            LibroMayor = catalogo.ObtenerMayor();
        }
    }
}
