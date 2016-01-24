using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorConsola
{
    public class Atributos
    {


        public string Nombre { get; private set; }
        public string tipoDato { get; set; }
        public string length { get; set; }
        public string TipoNull { get; set; }

        public Atributos()
        {

        }


        public Atributos(string nom, string tip, string len, string nu)
        {
            this.Nombre = nom;
            this.tipoDato = tip;
            this.length = len;
            this.TipoNull = nu;
        }

        public override string ToString() => Nombre + " " + tipoDato + " " + length + " " + TipoNull;


        public List<Atributos> ListaAtributos()
        {
            List<Atributos> lista = new List<Atributos>();
            return lista;
        }

    }
}
