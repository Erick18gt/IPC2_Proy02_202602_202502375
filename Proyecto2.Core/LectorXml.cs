using System.Xml.Linq;

namespace Proyecto2.Core
{
 
    public class LectorXml
    {
        public void Cargar(string rutaArchivo, Catalogo catalogo)
        {
            XDocument documento = XDocument.Load(rutaArchivo);
            XElement raiz = documento.Root; // el elemento <config>

            XElement listaCategorias = raiz.Element("listaCategorias");
            if (listaCategorias != null)
            {
                CargarCategorias(listaCategorias, catalogo);
            }

            XElement listaLibros = raiz.Element("listaLibros");
            if (listaLibros != null)
            {
                CargarLibros(listaLibros, catalogo);
            }
        }

      
        private void CargarCategorias(XElement listaCategorias, Catalogo catalogo)
        {
            bool huboAvanceEnEstaVuelta = true;

            while (huboAvanceEnEstaVuelta)
            {
                huboAvanceEnEstaVuelta = false;

                foreach (XElement categoriaXml in listaCategorias.Elements("categoria"))
                {
                    string nombre = categoriaXml.Value.Trim();

                    XAttribute atributoPadre = categoriaXml.Attribute("padre");
                    string nombrePadre = atributoPadre == null ? null : atributoPadre.Value;

                    if (catalogo.BuscarCategoria(nombre) != null)
                        continue; // ya se había agregado (en esta carga o en una anterior)

                    bool seAgrego = catalogo.AgregarCategoria(nombre, nombrePadre);
                    if (seAgrego)
                        huboAvanceEnEstaVuelta = true;
                }
            }
        }

        private void CargarLibros(XElement listaLibros, Catalogo catalogo)
        {
            foreach (XElement libroXml in listaLibros.Elements("libro"))
            {
                int isbn = int.Parse(libroXml.Element("ISBN").Value.Trim());
                string titulo = libroXml.Element("titulo").Value.Trim();
                string autor = libroXml.Element("autor").Value.Trim();
                string categoria = libroXml.Element("categoria").Value.Trim();

                catalogo.RegistrarLibro(isbn, titulo, autor, categoria);
            }
        }
    }
}
