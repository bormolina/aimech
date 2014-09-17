using System;
using System.Collections.Generic;
using System.Text;



namespace PracticaICO
{
    /**
     * Clase que representa un hexágono del tablero
     * */
    class node
    {
        public string id="0000";
        public int nivel=0;
        public int tipo=0; //0 abierto, 1 pavimentado, 2 agua, 3 pantanoso
        public int objeto=0;//0 escombros, 1 bosque, 2 bosqueDenso, 3 ligero, 4 medio, 5 pesado, 6 reforzado, 7 bunker, 255 nada
        public int fce=0;
        public bool edificioDerrumbado=false;
        public bool fuego=false;
        public bool humo=false;
        public int nGarrotes =0;
        public bool[] rios={false,false,false,false,false,false};
        public bool[] carreteras={false,false,false,false,false,false};

        public node(string id) {
            this.id = id;
        }
        /**
         * Obtiene las coordenadas numericas de un nodo 
         */
        public int[] getC(){ 
            int h=Convert.ToInt32(this.id.Substring(0,2));
            int w=Convert.ToInt32(this.id.Substring(2,2));
            int[] coordinates = { h, w };
            return coordinates;
            
        }

        public void setData(string[] data) {
            this.nivel = Convert.ToInt32(data[0]);
            this.tipo = Convert.ToInt32(data[1]);
            this.objeto = Convert.ToInt32(data[2]);
            this.fce = Convert.ToInt32(data[3]);
            this.edificioDerrumbado = Convert.ToBoolean(data[4]);
            this.fuego = Convert.ToBoolean(data[5]);
            this.humo = Convert.ToBoolean(data[6]);
            this.nGarrotes = Convert.ToInt32(data[7]);
            for (int i = 0; i < 6; i++) { this.rios[i] = Convert.ToBoolean(data[i + 8]); }
            for (int i = 0; i < 6; i++) { this.carreteras[i] = Convert.ToBoolean(data[i + 14]); }
        }

        public void print() {
            Console.Out.WriteLine("\ncoordenadas: "+this.id);
            Console.Out.WriteLine("datos:");
            Console.Out.WriteLine("\tnivel:"+this.nivel);
            Console.Out.WriteLine("\ttipo:" + this.tipo);
            Console.Out.WriteLine("\tobjeto:" + this.objeto);
            Console.Out.WriteLine("\tfce:" + this.fce);
            Console.Out.WriteLine("\tedificio derrumbado:" + this.edificioDerrumbado);
            Console.Out.WriteLine("\tfuego:" + this.fuego);
            Console.Out.WriteLine("\thumo:" + this.humo);
            Console.Out.WriteLine("\tnum garrotes:" + this.nGarrotes);
            Console.Out.Write("\trios:");
            //utilities.printBooleanArray(this.rios);
            Console.Out.Write("\tcarreteras:");
            //utilities.printBooleanArray(this.carreteras);
        }
    }
}
