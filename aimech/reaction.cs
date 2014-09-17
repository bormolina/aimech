using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que determina que acción realizaremos en la fase de reacción
     * */
    class reaction
    {
        
        public environment world;
        public string nameFile;
        public string whereTurn;

        public reaction(environment m) {
            this.world = m;
            this.nameFile = "accionJ" + this.world.player + ".sbt";
            this.determinateSide();
        }

        /**
         * Determina si tenemos que girar hacia la derecha o hacia la izquierda
         * */
        private void determinateSide(){
            int mySide = this.world.myMech.faceSide;
            string rivalPosition = this.world.theOthers[0].position;
            string myPosition = this.world.myMech.position;
            int bestSide = movement.whereFace(myPosition,rivalPosition);
            int distance = utilities.circularDist(mySide,bestSide);
            if (distance == 0 || distance==3) {
                this.whereTurn = "Igual";
            }
            else if (distance > 0)
            {
                this.whereTurn = "Derecha";
            }
            else {
                this.whereTurn = "Izquierda";
            }
        }

        /**
         * Escribimos la reacción en el fichero accionJx.sbt
         * */
        public void writeReaction() {
            System.IO.StreamWriter actionFile = new System.IO.StreamWriter(nameFile);
            actionFile.WriteLine(this.whereTurn);
            actionFile.Close();
        }
    }
}
