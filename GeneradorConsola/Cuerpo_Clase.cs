using System;
using System.IO;
using System.Linq;
using System.Text;
using static System.Console;
namespace GeneradorConsola
{
    public class Cuerpo_Clase
    {

        public string namespace_Capa { get; set; }
        public string Tipo { get; set; }

        public void CrearCuerpo(string tablaBD)
        {
            StringBuilder Clase = new StringBuilder();

            //string directorioActual = Environment.CurrentDirectory;

            //Determinar dónde guardará
            string archivo;
            string tipo = "";

            //Creando el path del archivo
            if (Tipo.StartsWith("DAL"))
            {
                tipo = Opciones.DirectorioDAL;
            }

            else if (Tipo.StartsWith("BOL"))
            {
                tipo = Opciones.DirectorioBOL;
            }

            archivo = tipo + string.Format(@"\{0}{1}.cs", Tipo, tablaBD);

            //string archivo = directorioActual + string.Format(@"\{0}{1}.cs", Tipo, tablaBD);
            string[] namespaces = { "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks" };

            //Agregando namespaces predeterminados
            foreach (var item in namespaces)
            {
                Clase.AppendLine(string.Format("using {0}; ", item));
            }

            if (Tipo.StartsWith("DAL"))
            {
                //Agregando namespace de ENTIDADES
                Clase.AppendLine("using ENTIDADES;");
                Clase.AppendLine("using ACCESO_DATOS;");
                Clase.AppendLine("using System.Data;");
                Clase.AppendLine("");
            }

            else if (Tipo.StartsWith("BOL"))
            {
                //Agregando namespace de DAL
                Clase.AppendLine("using ACCESO_DATOS;");
                Clase.AppendLine("using ENTIDADES;");
            }

            //Escribiendo el código base de la clase
            Clase.AppendLine("");
            Clase.AppendLine($"namespace {namespace_Capa}");
            Clase.AppendLine("{");

            //Determinar si la clase es BOL o DAL
            if (Tipo.StartsWith("DAL"))
            {
                //Agregando la interfaz
                Clase.AppendLine($"    public class {Tipo}{tablaBD} : {Opciones.NomInterfaz}<{tablaBD.ToUpper()}>");

                //Determinar si se completa el codigo de los métodos crud
                if (Opciones.framework == true)
                {
                    CuerpoDALImplementado(Clase, tablaBD);
                }
                else
                {
                    CuerpoDALPorDefecto(Clase, tablaBD);
                }
            }

            else if (Tipo.StartsWith("BOL"))
            {
                Clase.AppendLine($"    public class {Tipo}{tablaBD}");
                CuerpoBOL(Clase, tablaBD);
            }

            //Creando Clase en el directorio indicado
            //Creando archivo .cs
            File.Create(archivo).Close();

            //Escribiendo el cuerpo del archivo .cs
            using (StreamWriter file = new StreamWriter(archivo))
            {
                file.Write(Clase.ToString());
            }

            WriteLine($"Clase {Tipo} creada :)!");
        }

        //Creacion de cuerpo DAL sin implementar
        public void CuerpoDALPorDefecto(StringBuilder Clase, string tablaBD)
        {
            Clase.AppendLine("    {");
            Clase.AppendLine(string.Format("        {0}Entities contexto = new {0}Entities();", Opciones.DataBase.ToUpper()));
            Clase.AppendLine(string.Format("        public int Insertar({0} aux)", tablaBD.ToUpper()));
            Clase.AppendLine("        {");
            Clase.AppendLine("            throw new NotImplementedException();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine(string.Format("        public int Modificar({0} aux)", tablaBD.ToUpper()));
            Clase.AppendLine("        {");
            Clase.AppendLine("            throw new NotImplementedException();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine(string.Format("        public int Eliminar({0} aux)", tablaBD.ToUpper()));
            Clase.AppendLine("        {");
            Clase.AppendLine("            throw new NotImplementedException();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine("        public System.Data.DataTable ListarCriterio(char Criterio, string Buscado)");
            Clase.AppendLine("        {");
            Clase.AppendLine("            throw new NotImplementedException();");
            Clase.AppendLine("        }");
            Clase.AppendLine("    }");
            Clase.AppendLine("}");
        }

        //Creacion de cuerpo DAL implementando metodos de entityframework
        public void CuerpoDALImplementado(StringBuilder Clase, string tablaBD)
        {
            Clase.AppendLine("    {");
            Clase.AppendLine($"        {Opciones.DataBase.ToUpper()}Entities contexto = new {Opciones.DataBase.ToUpper()}Entities();");
            Clase.AppendLine($"        public void Insertar({tablaBD.ToUpper()} aux)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"            contexto.{tablaBD.ToUpper()}.Add(aux);");
            Clase.AppendLine("             contexto.SaveChanges();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine($"        public void Modificar({tablaBD.ToUpper()} aux)");
            Clase.AppendLine("        {");
            Clase.AppendLine("            var entry = contexto.Entry(aux);");
            Clase.AppendLine("");
            Clase.AppendLine($"            var pkey = contexto.{tablaBD.ToUpper()}.Create().GetType().GetProperty(\"Id{FirstCharToUpper(tablaBD.ToLower())}\").GetValue(aux);");
            Clase.AppendLine("");
            Clase.AppendLine("            if (entry.State == EntityState.Detached)");
            Clase.AppendLine("            {");
            Clase.AppendLine($"                var set = contexto.Set<{tablaBD.ToUpper()}>();");
            Clase.AppendLine($"                {tablaBD.ToUpper()} attachedEntity = set.Find(pkey);");
            Clase.AppendLine("                if (attachedEntity != null)");
            Clase.AppendLine("                {");
            Clase.AppendLine("                    var attachedEntry = contexto.Entry(attachedEntity);");
            Clase.AppendLine("                    attachedEntry.CurrentValues.SetValues(aux);");
            Clase.AppendLine("                }");
            Clase.AppendLine("                else");
            Clase.AppendLine("                {");
            Clase.AppendLine("                    entry.State = EntityState.Modified;");
            Clase.AppendLine("                }");
            Clase.AppendLine("            }");
            Clase.AppendLine("            contexto.SaveChanges();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine($"        public void Eliminar({tablaBD.ToUpper()} aux)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"            var obj = contexto.{tablaBD.ToUpper()}.Find(aux.Id{FirstCharToUpper(tablaBD.ToLower())});");
            Clase.AppendLine($"            contexto.{tablaBD.ToUpper()}.Remove(obj);");
            Clase.AppendLine("             contexto.SaveChanges();");
            Clase.AppendLine("        }");
            Clase.AppendLine("");
            Clase.AppendLine($"        public {tablaBD.ToUpper()} PorId<unknowtype>(unknowtype id)=>contexto.{tablaBD.ToUpper()}.Find(id);");
            Clase.AppendLine("");
            Clase.AppendLine($"        public IEnumerable<{tablaBD.ToUpper()}> List=>contexto.{tablaBD.ToUpper()};");
            Clase.AppendLine("");
            Clase.AppendLine("        public System.Data.DataTable ListarCriterio(char Criterio, string Buscado)");
            Clase.AppendLine("        {");
            Clase.AppendLine("            throw new NotImplementedException();");
            Clase.AppendLine("        }");
            Clase.AppendLine("    }");
            Clase.AppendLine("}");
        }

        //Creacion de cuerpo BOL
        public void CuerpoBOL(StringBuilder Clase, string tablaBD)
        {
            #region Plantilla Registrar
            Clase.AppendLine("    {");
            Clase.AppendLine($"        private DAL{tablaBD.ToUpper()} obj = new DAL{tablaBD.ToUpper()}();");
            Clase.AppendLine("");
            Clase.AppendLine($"        public void Registrar({tablaBD.ToUpper()} aux)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"            if (obj.PorId(aux.Id{FirstCharToUpper(tablaBD.ToLower())}) == null)");
            Clase.AppendLine("            {");
            Clase.AppendLine("                obj.Insertar(aux);");
            Clase.AppendLine("            }");
            Clase.AppendLine("            else");
            Clase.AppendLine("            {");
            Clase.AppendLine("                 obj.Modificar(aux);");
            Clase.AppendLine("            }");
            Clase.AppendLine("        }");
            Clase.AppendLine($"         public IEnumerable<{tablaBD.ToUpper()}> Todos() => obj.List;");
            Clase.AppendLine($"public {tablaBD.ToUpper()} TraerPorId<T>(T id) => obj.PorId(id);");
            Clase.AppendLine($"public void Eliminar({tablaBD.ToUpper()} aux) => obj.Eliminar(aux);");
            Clase.AppendLine("    }");
            Clase.AppendLine("}");
            #endregion

            #region Plantilla

            #endregion
        }

        public void Generar_DAL_BD()
        {
            Conexiones cn = new Conexiones();
            var lista = cn.ObtenerTablas(Opciones.Servidor, Opciones.DataBase);
            Tipo = "DAL";

            foreach (var item in lista)
            {
                CrearCuerpo(item);
            }
        }

        public void Generar_BOL_BD()
        {
            Conexiones cn = new Conexiones();
            var lista = cn.ObtenerTablas(Opciones.Servidor, Opciones.DataBase);
            Tipo = "BOL";

            foreach (var item in lista)
            {
                CrearCuerpo(item);
            }
        }

        public void CrearInterfaz()
        {
            StringBuilder cuerpo = new StringBuilder();
            string[] namespaces = { "System", "System.Collections.Generic", "ENTIDADES" };

            //Agregando namespaces predeterminados
            foreach (var item in namespaces)
            {
                cuerpo.AppendLine($"using {item}; ");
            }

            //Creando el cuerpo de la interfaz
            cuerpo.AppendLine("namespace ACCESO_DATOS");
            cuerpo.AppendLine("{");
            cuerpo.AppendLine(string.Format("public interface {0}<T> where T : class", Opciones.NomInterfaz));
            cuerpo.AppendLine("    {");
            cuerpo.AppendLine("    void Insertar(T aux);");
            cuerpo.AppendLine("    void Modificar(T aux);");
            cuerpo.AppendLine("    void Eliminar(T aux);");
            cuerpo.AppendLine("    T PorId<unknowtype>(unknowtype id);");
            cuerpo.AppendLine("    System.Data.DataTable ListarCriterio(char Criterio, string Buscado);");
            cuerpo.AppendLine("    }");
            cuerpo.AppendLine("}");

            //Creando el path del archivo
            string archivo = Opciones.DirectorioDAL + @"\" + Opciones.NomInterfaz + ".cs";

            //Creando archivo .cs
            File.Create(archivo).Close();

            //Escribiendo el cuerpo de la interfaz
            using (StreamWriter file = new StreamWriter(archivo))
            {
                file.Write(cuerpo.ToString());
            }

            Console.WriteLine("Interfaz creada.");
        }

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}