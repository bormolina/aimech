using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que contiene la información necesaria para realizar un ataque con un arma
     * Se le dará uso cuando la clase ataque con armas esté completada
     * */
    class singleWeaponAttack
    {
        public string Wplace;
        public int Wslot;
        public string dCad = "False";
        public string Aplace;
        public int Aslot;
        public string target;
        public string targetType = "Mech";

        public singleWeaponAttack(string wp, int ws, string ap, int amS, string t) {
            this.Wplace = wp;
            this.Wslot = ws;
            this.Aplace = ap;
            this.Aslot = amS;
            this.target = t;
        }
    }

    
}
