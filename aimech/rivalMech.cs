using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que representa a un mech rival, hereda de la clase abstracta de mech y define la forma en la que se lee un mech rival
     * que es distinta a la de como se lee un mech propio
     * */
    class rivalMech : mech
    {
        public rivalMech(Queue<string> data, int nPlayers) { 
            this.narc = new bool[nPlayers];
            this.inarc = new bool[nPlayers];

            this.player=Convert.ToInt32(data.Dequeue());
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
           
            for (int i = 0; i < 11; i++) {
                this.armor[i] = Convert.ToInt32(data.Dequeue());
            }
            
            for (int i = 0; i < 8; i++) {
                this.armor[i] = Convert.ToInt32(data.Dequeue());
            }

            for (int i = 0; i < nPlayers; i++) {
                this.narc[i] = Convert.ToBoolean(data.Dequeue());
            }

            for (int i = 0; i < nPlayers; i++){
                this.inarc[i] = Convert.ToBoolean(data.Dequeue());
            }
        }
    }
}
