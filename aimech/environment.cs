using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que engloba todos los elementos que conforman el entorno en que estará situado nuestro jugador inteligente
     * */
    class environment
    {
        public map land; //mapa del juego
        public int player; //número del jugador inteligente
        public int nPlayers; //número de jugadores en la partida
        public ownMech myMech; //mech que manejará el jugador inteligente
        public List<rivalMech> theOthers = new List<rivalMech>(); //Lista con los mechs rivales

        /**
         * Constructor de la clase
         * A partir de los datos relativos al mapa y a lo mechs crea una representación interna del entorno
         * */
        public environment(int thePlayer, string[] mapData, string[] mechsData) {
            this.player = thePlayer;
            this.land = new map(mapData);
            //Cola con los datos de los mechs
            Queue<string> data = new Queue<string>(mechsData);
            //El primer elemento es el nombre del fichero, no nos interesa
            data.Dequeue();
            //El segundo elemento es el número de mechs que participan
            this.nPlayers = Convert.ToInt32(data.Dequeue());
            //leemos los datos de cada mech
            for (int i = 0; i < this.nPlayers; i++) {
                if (i == this.player)
                {
                    this.myMech = new ownMech(data, this.nPlayers);
                }
                else {
                    this.theOthers.Add(new rivalMech(data,this.nPlayers));
                }
            }
            //Leemos los datos adicionales de los mechs dónde se nos especifican sus armas
            List<string> files = utilities.defMechFiles(this.nPlayers,this.player);
            for (int i = 0; i < this.nPlayers; i++) {
                if (i == this.player)
                {
                    this.myMech.setComponents(files[i]);
                }
                else
                {
                    int index = i;
                    if (i > this.player) {
                        index--;
                    }
                    this.theOthers[index].setComponents(files[i]);
                }
            }
        }

        /**
         * Nos dice si se puede ir a n2 desde n1
         * */
        public bool reachable (string n1, string  n2){
        //queda por implementar el de si la casilla es muy profunda??
            if (!this.land.areNeigs(n1, n2)) {
                return false;
            }
            //Calculamso el desnivel
            int ramp = Math.Abs(this.land.nodes[n1].nivel - this.land.nodes[n2].nivel);

            if (ramp > 2 || n2==this.theOthers[0].position) {
                return false;
            }
            
            return true;
        }

        /**
         * Lista de nodos accesibles desde uno dado
         * */
        public List<string> listOfReachables(string n1)
        {
            List<string> canGo = new List<string>();
            List<string> neigs = this.land.neigs(n1);
            foreach (string neig in neigs)
            {
                if (this.reachable(n1, neig))
                {
                    canGo.Add(neig);
                }
            }
            return canGo;
        }
    }
}
