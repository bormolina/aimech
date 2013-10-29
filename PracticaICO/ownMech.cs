using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que representa al mech el cual manejamos, hereda de la clase abstracta mech y le añade atributos y redefine métodos
     * */
    class ownMech : mech
    {
        public int walk;
        public int run;
        public int jump;
        public int radiatorsON;
        public int radiatorsOFF;
        public int injuries;
        public bool conscious;
        public bool[] impacts = new bool[78];
        public bool[] shootedGuns = new bool[8];
        public int AmmoToDrop;
        public string[] placeAmmoToDrop;
        public int[] slotAmmoToDrop;
        /**
         * Constructor, lee los datos del mech
         * */
        public ownMech(Queue<string> data, int nPlayers)
        {
            this.narc = new bool[nPlayers];
            this.inarc = new bool[nPlayers];

            this.player = Convert.ToInt32(data.Dequeue());
            this.operative = Convert.ToBoolean(data.Dequeue());
            this.powerOff = Convert.ToBoolean(data.Dequeue());
            this.stacked = Convert.ToBoolean(data.Dequeue());
            this.onGround = Convert.ToBoolean(data.Dequeue());
            this.position = data.Dequeue();
            this.faceSide = Convert.ToInt32(data.Dequeue());
            this.bodySide = Convert.ToInt32(data.Dequeue());
            this.heat = Convert.ToInt32(data.Dequeue());
            this.fire = Convert.ToBoolean(data.Dequeue());
            this.club = Convert.ToBoolean(data.Dequeue());
            this.kindClub = Convert.ToInt32(data.Dequeue());

            for (int i = 0; i < 11; i++)
            {
                this.armor[i] = Convert.ToInt32(data.Dequeue());
            }

            for (int i = 0; i < 8; i++)
            {
           
                this.internalArmor[i] = Convert.ToInt32(data.Dequeue());
            }

            this.walk = Convert.ToInt32(data.Dequeue());
            this.run = Convert.ToInt32(data.Dequeue());
            this.jump = Convert.ToInt32(data.Dequeue());
            this.radiatorsON = Convert.ToInt32(data.Dequeue());
            this.radiatorsOFF = Convert.ToInt32(data.Dequeue());
            this.injuries = Convert.ToInt32(data.Dequeue());
            this.conscious = Convert.ToBoolean(data.Dequeue());

            for (int i = 0; i < 78; i++) {
                this.impacts[i] = Convert.ToBoolean(data.Dequeue());
            }

            for (int i = 0; i < 8; i++)
            {
                this.shootedGuns[i] = Convert.ToBoolean(data.Dequeue());
            }

            this.AmmoToDrop = Convert.ToInt32(data.Dequeue());
            if (this.AmmoToDrop > 0)
            {
                this.placeAmmoToDrop = new string[this.AmmoToDrop];
                this.slotAmmoToDrop = new int[this.AmmoToDrop];
            }

            for (int i = 0; i < this.AmmoToDrop; i++) {
                this.placeAmmoToDrop[i] = data.Dequeue();
                this.slotAmmoToDrop[i] = Convert.ToInt32(data.Dequeue());
            }

            for (int i = 0; i < nPlayers; i++)
            {
                this.narc[i] = Convert.ToBoolean(data.Dequeue());
            }

            for (int i = 0; i < nPlayers; i++)
            {
                this.inarc[i] = Convert.ToBoolean(data.Dequeue());
            }
        }

        public void extendedPrint(){
            this.print();
            Console.Out.WriteLine("Puedo anda: "+ this.walk);
            Console.Out.WriteLine("Puedo correr: "+this.run);
            Console.Out.WriteLine("Puedo saltar: "+this.jump);
            Console.Out.WriteLine("radiadores on: "+this.radiatorsON);
            Console.Out.WriteLine("radiadores apagaos: "+this.radiatorsOFF);
            Console.Out.WriteLine("heridas: "+this.injuries);
            Console.Out.WriteLine("estoy consciente: "+this.conscious);
            Console.Out.WriteLine("municion pa tirar: "+this.AmmoToDrop);

            Console.Out.WriteLine("Impactos recibidos:");
            foreach (bool impacted in this.impacts) {
                Console.Out.WriteLine("\t"+impacted);
            }

            Console.Out.WriteLine("Armas usadas:");
            foreach (bool gun in this.shootedGuns)
            {
                Console.Out.WriteLine("\t" + gun);
            }

            Console.Out.WriteLine("Place ammo to drop:");
            for (int i = 0; i < this.AmmoToDrop;i++ )
            {
                Console.Out.WriteLine("\t" + this.placeAmmoToDrop[i]);
            }

            Console.Out.WriteLine("Slot ammo to drop:");
            for (int i = 0; i < this.AmmoToDrop; i++)
            {
                Console.Out.WriteLine("\t" + this.slotAmmoToDrop[i]);
            }
        }
    }
}
