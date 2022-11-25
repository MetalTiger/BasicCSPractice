// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace BasicCS
{
    class Program
    {

        public struct Game
        {

            public string Genre { get; set; }
            public string Clasification { get; set; }

            public Game ()
            {
                Genre = "Terror";
                Clasification = "PEGI-18";
            }
        
        }

        static void Main(string[] args)
        {

            //CSharpBasico();
            //LeerXml();
            //XmlAObjeto();
            linqTest();

            Console.WriteLine("Hola mundo");

            Game silentHill = new Game();






        }

        public static int ContarNumeros(List<int> lista, int number)
        {
            int contador = 0;

            lista.ForEach(x => {
                if (x == number)
                {
                    contador++;
                }

            });

            return contador;
        }
        
        private static void linqTest()
        {

            // Contar numeros
            List<int> numeros = new() { 1, 2, 3, 4, 5, 5, 2, 2, 2, 2 };
            List<int> repNum = new() { };

            numeros.ForEach(x =>
            {
                if (!repNum.Contains(x))
                {
                    repNum.Add(x);
                    Console.WriteLine("La lista tiene " + ContarNumeros(numeros, x) + " numeros " + x);
                }
            });

            // Contar numeros y almacenarlos en un diccionario
            Dictionary<string, int> numbers2 = new();
            numeros.ForEach(x =>
            {

                if (numbers2.ContainsKey(x.ToString()))
                {
                    numbers2[x.ToString()] += 1;
                    //numbers2.Add(x.ToString(), numbers2[x.ToString()]++);
                }
                else 
                {
                    numbers2.Add(x.ToString(), 1);
                }

            });

            foreach (KeyValuePair<String, int> item in numbers2)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }

            // Contar Numeros con Linq
            int cantidad2 = (from d in numeros where d == 2 select d).Count();
            Console.WriteLine(cantidad2);

            // Aggregate
            List<string> nombres = new() { "Pedro", "Carlos", "Pito", "Juan" };

            string nombresSeparadosPorComa = nombres.Aggregate((a, b) => {
                return a + ", " + b;
            }).ToString();



            Console.WriteLine(nombresSeparadosPorComa);
        }

        private static void XmlAObjeto()
        {
            Program program = new Program();
            OrderedItem item = program.DeserializeObject("C:\\Users\\SmacoDevs1\\Desktop\\dotNET\\BasicCS\\BasicCS\\simple.xml");

            Console.WriteLine(
                $"{item.ItemName}\n" +
                $"{item.Description}\n" +
                $"{item.Quantity}\n" +
                $"{item.LineTotal}\n"
            );
        }

        private OrderedItem DeserializeObject(string filename)
        {
            Console.WriteLine("Reading with Stream");
            // Create an instance of the XmlSerializer.
            XmlSerializer serializer = new XmlSerializer(typeof(OrderedItem));

            // Declare an object variable of the type to be deserialized.
            OrderedItem i;

            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                i = (OrderedItem)serializer.Deserialize(reader);
            }

            return i;
        }

        private static void LeerXml()
        {

            XmlDocument xmlFile = new XmlDocument();
            //xmlFile.Load("C:\\Users\\SmacoDevs1\\Desktop\\Facturas\\Prueba\\8F7ED3B1-E208-4F93-8B86-E934D61C3D2C.xml");
            xmlFile.Load("C:\\Users\\SmacoDevs1\\Desktop\\Facturas\\11CD858F-C784-4AD3-812F-92A29B1C3FDC.xml");
            // Generar espacio de nombre cfdi
            string cfdi = "cfdi";
            string tfd = "tfd";

            XmlNamespaceManager nsmgr = new(xmlFile.NameTable);
            nsmgr.AddNamespace(cfdi, "http://www.sat.gob.mx/cfd/3");
            nsmgr.AddNamespace(tfd, "http://www.sat.gob.mx/TimbreFiscalDigital");

            /* Nodos existentes */
            Console.WriteLine("**Nodos**");
            Console.WriteLine($"Existen {xmlFile.DocumentElement.ChildNodes.Count} nodos");

            var childNodes = xmlFile.DocumentElement.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {

                Console.WriteLine(childNode.Name);

                foreach (XmlAttribute attribute in childNode.Attributes)
                {
                    Console.WriteLine($"    {attribute.Name} {attribute.Value}");
                }

                Console.WriteLine("");
            }

            /* Extraer nodo con SelectSingleNode */
            Console.WriteLine("**Extraer nodo mediante SelectSingleNode**");
            // Extraer nodo
            XmlNode nodoReceptor = xmlFile.DocumentElement.SelectSingleNode($"{cfdi}:Receptor", nsmgr);
            Console.WriteLine(nodoReceptor.Name);

            // Recorrer atributos
            foreach (XmlAttribute attribute in nodoReceptor.Attributes)
            {
                Console.WriteLine($"    {attribute.Name} = {attribute.Value}");

            }

            // Extraer atributo (@) con SelectSingleNode
            XmlNode rfcReceptor = nodoReceptor.SelectSingleNode("@Rfc");
            Console.WriteLine($"El {rfcReceptor.Name} del emisor es {rfcReceptor.Value}");


            Console.WriteLine("");

            /* Extraer un nodo mediante [nombre]
             * Este truena si no se pone el nombre del elemento tal cual como esta en el documento, por tanto no se puede usar un namespace abreviado
             */
            Console.WriteLine("**Extraer un nodo mediante [nombre]**");
            // Extraer nodo
            XmlNode nodoEmisor = xmlFile.DocumentElement[$"{cfdi}:Emisor"];
            Console.WriteLine($"{nodoEmisor.Name}");

            // Recorrer atributos
            foreach (XmlAttribute attribute in nodoEmisor.Attributes)
            {
                Console.WriteLine($"    {attribute.Name} = {attribute.Value}");
            }

            // Extraer atributo nombresSeparadosPorComa mano
            XmlNode rfcEmisor = nodoEmisor.Attributes.GetNamedItem("Rfc");
            Console.WriteLine($"El {rfcEmisor.Name} del emisor es {rfcEmisor.Value}");

            Console.WriteLine("");

            /* Sacar conceptos */
            Console.WriteLine("**Sacar Conceptos**");
            XmlNode nodoConceptos = xmlFile.DocumentElement.SelectSingleNode($"{cfdi}:Conceptos", nsmgr);

            // Extraer nodos concepto en lista
            XmlNodeList nodosConcepto = nodoConceptos.SelectNodes($"{cfdi}:Concepto", nsmgr);
            Console.WriteLine($"Existen {nodosConcepto.Count} nodos de concepto.");

            foreach (XmlNode concepto in nodosConcepto)
            {

                if (concepto.ChildNodes.Count == 0)
                {
                    Console.WriteLine($"    El concepto {concepto.Attributes.GetNamedItem("ClaveProdServ").Value} - {concepto.Attributes.GetNamedItem("Descripcion").Value} no genera impuestos.");
                }
                else
                {
                    Console.WriteLine($"    El concepto {concepto.Attributes.GetNamedItem("ClaveProdServ").Value} - {concepto.Attributes.GetNamedItem("Descripcion").Value} si genera impuestos.");

                    XmlNode nodoTraslado = concepto.FirstChild.FirstChild.FirstChild;

                    Console.WriteLine($"        Con valor de {nodoTraslado.Attributes.GetNamedItem("Importe").Value}");

                }

            }

            Console.WriteLine("");


            /* Recorrer atributos de Comprobante */
            Console.WriteLine("**Atributos**");
            Console.WriteLine($"Existen {xmlFile.DocumentElement.Attributes.Count} atributos");
            XmlAttributeCollection attributes = xmlFile.DocumentElement.Attributes;

            foreach (XmlAttribute attribute in attributes)
            {
                Console.WriteLine(attribute.Name);
            }

            Console.WriteLine("");

            /* Sacar timbre de complemento */
            Console.WriteLine("**Sacar timbre de complemento**");
            XmlNode nodoComplemento = xmlFile.DocumentElement.SelectSingleNode($"{cfdi}:Complemento", nsmgr);
            XmlNode nodoTimbre = nodoComplemento.SelectSingleNode($"{tfd}:TimbreFiscalDigital", nsmgr);
            Console.WriteLine(nodoComplemento.Name);
            Console.WriteLine($"    {nodoTimbre.Name}");

            foreach (XmlAttribute attribute in nodoTimbre.Attributes)
            {
                Console.WriteLine($"        {attribute.Name}");
            }


        }

        private static void CSharpBasico()
        {
            DateTime fecha = DateTime.UtcNow;

            Console.WriteLine(fecha);

            string[,] matriz = new string[3, 4];

            Perro perro1 = new Perro("Juan", "2", Color.Negro, tam: Tamanio.Mediano);
            Perro perro2 = new Perro("Pedro", "2", Color.Blanco, tam: Tamanio.Mediano);
            Perro perro3 = new Perro("Lokuas", "2", Color.Rojo, tam: Tamanio.Mediano);
            Perro perro4 = new("Loco", "3", Color.Blanco, Tamanio.Mediano);


            List<Perro> listaPerros = new()
            {
                perro1,
                perro2,
                perro3,
                perro4
            };

            listaPerros.ForEach(perro =>
            {

                Console.WriteLine("Perro Color: ");
                Console.BackgroundColor = (ConsoleColor)perro.Color;
                Console.ForegroundColor = (ConsoleColor)perro.Color;
                Console.WriteLine("adasdasdasdasd");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{perro.Nombre} va a hablar:");
                perro.Hablar();
            });


        }

    }


    public static class Extensions
    {
        public static string Filter(this string str, List<char> charsToRemove)
        {
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), ""));
            return str;
        }
    }


    enum Color
    {
        Blanco = 15,
        Negro = 0,
        Rojo = 12,
    }

    enum Tamanio
    {
        Chico,
        Mediano,
        Grande,
    }


    abstract class Animal
    {
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public Color Color { get; set; }

        public Animal() { }

        public Animal(string nombre, string edad, Color color)
        {
            Nombre = nombre;
            Edad = edad;
            Color = color;
        }

        public void Respirar()
        {
            Console.WriteLine("Inhala y exhala");
        }

        public void Dormir()
        {
            Console.WriteLine("Zzzzzz...");
        }

        public virtual void Hablar()
        {
            Console.WriteLine("Habla");
        }

    }


    class Perro : Animal
    {
        public Tamanio Tamanio { get; set; }

        public Perro() { }

        public Perro(string nombre, string edad, Color color, Tamanio tam)
        {
            Nombre = nombre;
            Edad = edad;
            Color = color;
            Tamanio = tam;
        }

        public override void Hablar()
        {
            Console.WriteLine("Guau");
        }

    }
}