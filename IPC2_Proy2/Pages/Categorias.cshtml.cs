using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto2.Core;

namespace IPC2_Proy2.Pages
{
    public class CategoriasModel : PageModel
    {
        private readonly Catalogo catalogo;

        public CategoriasModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }

        public string Estructura { get; set; }

        [BindProperty]
        public string NombreNuevaCategoria { get; set; }

        [BindProperty]
        public string NombreCategoriaPadre { get; set; }

        public string Mensaje { get; set; }
        public bool Exito { get; set; }

        // OnGet se ejecuta cuando entras a la página normalmente (sin enviar
        // ningún formulario todavía), así que aquí solo mostramos lo que
        // ya existe en el catálogo.
        public void OnGet()
        {
            Estructura = catalogo.MostrarEstructura();
        }

        // OnPostAgregar se ejecuta cuando envías el formulario que tiene
        // asp-page-handler="Agregar". El nombre del método sigue el patrón
        // OnPost + NombreDelHandler.
        public void OnPostAgregar()
        {
            string padre = string.IsNullOrWhiteSpace(NombreCategoriaPadre) ? null : NombreCategoriaPadre.Trim();
            bool seAgrego = catalogo.AgregarCategoria(NombreNuevaCategoria.Trim(), padre);

            if (seAgrego)
            {
                Mensaje = "Categoría agregada correctamente.";
                Exito = true;
            }
            else
            {
                Mensaje = "No se pudo agregar: el nombre ya existe o el padre indicado no existe.";
                Exito = false;
            }

            // Volvemos a pedir la estructura actualizada para que se vea
            // la categoría recién agregada en la misma respuesta.
            Estructura = catalogo.MostrarEstructura();
        }
    }
}
