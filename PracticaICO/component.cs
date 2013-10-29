using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase para representar un componente del mech
     * 
     * */
    class component
    {
       
        public int code;
        public string name;
        public string type;
        public bool inBack;
        public int place;
        public int slot=-1;
        public int amount = -1;
        public int secondaryPlace;
        public string typeWeapon;
        public int heatGenerated;
        public int power;
        public int shotsPerTurn;
        public int minimumDistance;
        public int shortDistance;
        public int mediumDistance;
        public int longDistance;
        public bool operative;
        public int codeAmmo;
        public int ammoAmount;
        public string specialAmmo;
        public int trigger;
        public int[] angle = { 0,0,0,0};

        public void print(){
            Console.Out.WriteLine("Codigo: "+this.code);
            Console.Out.WriteLine("nombre: " + this.name);
            Console.Out.WriteLine("tipo: " + this.type);
            Console.Out.WriteLine("detrás: " + this.inBack);
            Console.Out.WriteLine("lugar: " + this.place);
            Console.Out.WriteLine("angulos: ");
            utilities.printIntList(this.angle);
            Console.Out.WriteLine("lugar secundario: " + this.secondaryPlace);
            Console.Out.WriteLine("tipo de arma: " + this.typeWeapon);
            Console.Out.WriteLine("calor: " + this.heatGenerated);
            Console.Out.WriteLine("daño: " + this.power);
            Console.Out.WriteLine("disparos por turno: " + this.shotsPerTurn);
            Console.Out.WriteLine("distancia mínima: " + this.minimumDistance);
            Console.Out.WriteLine("distancia corta: " + this.shortDistance);
            Console.Out.WriteLine("distancia media: " + this.mediumDistance);
            Console.Out.WriteLine("distancia larga: " + this.longDistance);
            Console.Out.WriteLine("operativo: " + this.operative);
            Console.Out.WriteLine("codigo munición: " + this.codeAmmo);
            Console.Out.WriteLine("cantidad munición: " + this.ammoAmount);
            Console.Out.WriteLine("munición especial: " + this.specialAmmo);
            Console.Out.WriteLine("trigger: " + this.trigger);
            Console.Out.WriteLine("slot: " + this.slot);
            Console.Out.WriteLine("cantidad: " + this.amount);
        }
    }
}
