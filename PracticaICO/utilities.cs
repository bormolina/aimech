using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace PracticaICO
{
    /**
     * Clase de utilidades, funciones que necesito a lo largo de la práctica
     * */
    class utilities
    {
        /**
         * Transformas dos enteros en una coordenadas de nuestro mapa hexagonal
         * */
        public static string numbersToCoordinates(int x, int y) {
            StringBuilder coord1 = new StringBuilder(Convert.ToString(x), 2);
            StringBuilder coord2 = new StringBuilder(Convert.ToString(y), 2);
            if (x < 10) {
                coord1.Insert(0,"0");
            }
            if (y < 10)
            {
                coord2.Insert(0, "0");
            }
            string coordinates = coord1.ToString() + coord2.ToString();
            return coordinates;
        }

        /**
         * Dada una lista de enteros la rota nTimes hacia la derecha
         * La usamos a la hora de ver qué lado es mejor para posicionarse
         * */
        public static int[] rotateListRight(int[] l, int nTimes) {
            int size = l.Length;
            int[] newList = new int[size];
            int index = nTimes % size;
            for (int i = 0; i < size; i++) {
                
                newList[index] = l[i];
                index++;
                index %= size;
            }
            return newList;
        }

        /**
         * Nos dice la distancia que hay (en giros necesarios) de un lado del hexagono a otro
         * */
        public static int circularDist(int s1, int s2) {
            int[] baseDist = new int[6];
            for (int i = 0; i < 4; i++) {
                baseDist[i] = i;
            }
            baseDist[4] = -2;
            baseDist[5] = -1;
            int[] newBase = utilities.rotateListRight(baseDist,s1);
            return newBase[s2%6];
        }
        /**
         * Nos dice cuanto elementos element hay en la lista l a partir del indice index
         * */
        public static int howManyElements(List<int> l, int element, int index) {
            int i = index;
            int times = 0;
            bool equal = true;
            int size = l.Count;
            while (equal && i < size) {
                if (l[i] == element) {
                    times++;
                    i++;
                }
                else{
                    equal = false;
                    break;
                }
            }
            return times;

        }

        /**
         * Ejecuta el programa LDVyC y devuelve un entero
         * -1 no tenemos lidea de vision y el enemgio tiene covertura
         * 0 no tenemos linea de visión y el enemigo no tiene covertura 
         * 2 tenemos linea de visión pero el enemigo tiene covertura
         * 3 tenemos linea de visión y el enemigo no tiene covertura
         * Adicionalmente la linea de vision se devuelve en la lista paht que le pasamos por referencia
         * */
        public static int executeLDV(string args, ref List<string> path) {
            
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = @".";
            proc.StartInfo.FileName = "LDVyC.exe";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = false;
            
            proc.Start();
            proc.WaitForExit();
            proc.Close();
            StreamReader fich = new StreamReader("LDV.sbt");
            string[] data = new string[3];
            data[0] = fich.ReadLine();
            data[1] = fich.ReadLine();
            data[2] = fich.ReadLine();
            fich.Close();
            if (!String.IsNullOrEmpty(data[0])) { path = new List<string>(data[0].Split(new char[] { ' ' })); }
            if (data[1] == "False" && data[2] == "False") {
                return 0;
            }
            else if (data[1] == "True" && data[2] == "False") {
                return 2;
            }
            else if (data[1] == "False" && data[2] == "True")
            {
                return -1;
            }
            else if (data[1] == "True" && data[2] == "True") {
                return 1;
            }
            return -2;
        }
        
        public static void printList(List<string> l, string info) {
            if (l.Count == 0) {
                Console.Out.WriteLine("la lista esta vacia");
            }
            Console.Out.WriteLine("("+info+")");
            foreach (string item in l) {
                Console.Out.WriteLine(item);
            }
            Console.Out.WriteLine("#############");
        }
        /*
        public static void printArrayString(string[] a) {
            for (int i = 0; i < a.Length; i++) {
                Console.Out.WriteLine(a[i]);
            }
        }*/
        public static void printIntList(int[] l) {
            for (int i = 0; i < l.Length; i++)
            {
                Console.Out.WriteLine(">>"+l[i]);
            }
        }
        /**
         * Devuelve una lista con el nombre de todos los fichero defmech que el simulador genrará
         * */
        public static List<string> defMechFiles(int players, int nPlayer) {
            List<string> files = new List<string>();
            for (int i = 0; i < players; i++) {
                string name = "defmechJ" + nPlayer + "-" + i + ".sbt";
                files.Add(name);
            }
            return files;
        }
    }
}
