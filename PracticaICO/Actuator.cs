using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase para representar un actuador del mech
     * */
    class Actuator
    {
        public int code;
        public string name;
        public int location;
        public bool operative;
        public int nImpacts;
        public int slot = -1;
        public int amount = -1;

        public void print(){
            Console.Out.WriteLine("Código: "+this.code);
            Console.Out.WriteLine("nombre: " + this.name);
            Console.Out.WriteLine("sitio: " + this.location);
            Console.Out.WriteLine("operativo: " + this.operative);
            Console.Out.WriteLine("nImpactos: " + this.nImpacts);
            Console.Out.WriteLine("slot: " + this.slot);
        }
    }

    
}
