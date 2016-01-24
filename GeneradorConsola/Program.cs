using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorConsola
{
    class Program
    {
        public static void Presentacion()
        {
            //Generador ASCII
            //http://www.network-science.de/ascii/

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine(@"          ______  _______      ___       __       _______ .______      ");
            Console.WriteLine(@"         /      ||   ____|    /   \     |  |     |   ____||   _  \     ");
            Console.WriteLine(@"        |  ,----'|  |__      /  ^  \    |  |     |  |__   |  |_)  |    ");
            Console.WriteLine(@"        |  |     |   __|    /  /_\  \   |  |     |   __|  |      /     ");
            Console.WriteLine(@"        |  `----.|  |____  /  _____  \  |  `----.|  |____ |  |\  \----.");
            Console.WriteLine(@"         \______||_______|/__/     \__\ |_______||_______|| _| `._____|");
            Console.WriteLine(@"                                                                       ");
            Console.WriteLine(@"           .___________.  ______     ______    __           _______.");
            Console.WriteLine(@"           |           | /  __  \   /  __  \  |  |         /       |");
            Console.WriteLine(@"           `---|  |----`|  |  |  | |  |  |  | |  |        |   (----`");
            Console.WriteLine(@"               |  |     |  |  |  | |  |  |  | |  |         \   \    ");
            Console.WriteLine(@"               |  |     |  `--'  | |  `--'  | |  `----..----)   |   ");
            Console.WriteLine(@"               |__|      \______/   \______/  |_______||_______/    ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[*] Ejemplo de creación de clases DAL: new dal ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ejemplo");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[*] Ejemplo de creación de clases BOL: new bol ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ejemplo");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[*] Para todos los comandos: help ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            Presentacion();
            Cuerpo_Clase cuerpo = new Cuerpo_Clase();
            string comando = "";

            while (comando != "exit")
            {
                Console.Write("cealer_tools > ");
                comando = Console.ReadLine();

                if (comando.ToUpper().StartsWith("NEW DAL ALL DATABASE"))
                {
                    cuerpo.Tipo = "DAL";
                    cuerpo.namespace_Capa = "ACCESO_DATOS";
                    cuerpo.Generar_DAL_BD();
                }

                else if (comando.ToUpper().StartsWith("--F NEW DAL ALL DATABASE"))
                {
                    cuerpo.Tipo = "DAL";
                    cuerpo.namespace_Capa = "ACCESO_DATOS";
                    cuerpo.Generar_DAL_BD();
                }

                else if (comando.ToUpper().StartsWith("NEW BOL ALL DATABASE"))
                {
                    cuerpo.Tipo = "BOL";
                    cuerpo.namespace_Capa = "BOL";
                    cuerpo.Generar_BOL_BD();
                }

                else if (comando.ToUpper().StartsWith("NEW DAL"))
                {
                    //Indicar que es un DAL
                    cuerpo.Tipo = "DAL";
                    cuerpo.namespace_Capa = "DAL";
                    //Obtener nombre de la clase DAL
                    string nom = FirstCharToUpper(comando.Substring(8).ToLower());
                    cuerpo.CrearCuerpo(nom);
                }

                else if (comando.ToUpper().StartsWith("NEW BOL"))
                {
                    //Indicar que es un BOL
                    cuerpo.Tipo = "BOL";
                    cuerpo.namespace_Capa = "BOL";
                    //Obtener nombre de la clase BOL
                    string nom = FirstCharToUpper(comando.Substring(8).ToLower());
                    cuerpo.CrearCuerpo(nom);
                }

                else if (comando.StartsWith("SET DATABASE ") || comando.StartsWith("set database "))
                {
                    //Obtener nombre de la base de datos
                    string nom = comando.Substring(13);
                    Opciones.DataBase = nom;
                }

                else if (comando.ToUpper().StartsWith("SET DAL "))
                {
                    //Obtener nombre de la base de datos
                    string nom = comando.Substring(8).ToLower();
                    Opciones.DirectorioDAL = nom;
                }

                else if (comando.ToUpper().StartsWith("SET BOL "))
                {
                    //Obtener nombre de la base de datos
                    string nom = comando.Substring(8).ToLower();
                    Opciones.DirectorioBOL = nom;
                }

                else if (comando.StartsWith("NEW INTERFACE ") || comando.StartsWith("new interface "))
                {
                    string nom = comando.Substring(14);
                    Opciones.NomInterfaz = nom;
                    cuerpo.CrearInterfaz();
                }

                else if (comando.ToUpper().StartsWith("HELP"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("[*] Los comandos SET son temporales hasta termine el proceso");
                    Console.WriteLine("[*] Para que sea permanente modificar los valores en Opciones");
                    Console.WriteLine("Asignar base de datos actual: SET DATABASE Ejemplo");
                    Console.WriteLine("Asignar directorio actual DAL: SET DAL Ejemplo");
                    Console.WriteLine("Asignar directorio actual BOL: SET BOL Ejemplo");
                    Console.WriteLine("Crear todos las clases de una Base de datos: NEW DAL ALL DATABASE");
                    Console.WriteLine("Crear todos las clases de una Base de datos: NEW BOL ALL DATABASE");
                    Console.WriteLine("Crear interfaz para CRUD: NEW INTERFACE Ejemplo");
                    Console.WriteLine("Generar clases dal con plantilla con métodos entity framework: --F NEW DAL ALL DATABASE");
                    Console.WriteLine("");
                }

                else if (comando.ToUpper().StartsWith("CLEAR"))
                {
                    Console.Clear();
                    Presentacion();
                }

                else if (comando.ToUpper().StartsWith("EXIT"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("El comando solicitado no existe. :(");
                }
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }
    }
}