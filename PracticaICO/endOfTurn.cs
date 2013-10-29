using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase para el cálculo de las acciones a realizar en la fase de fin de turno.
     * */
    class endOfTurn
    {
        public environment world;
        public string nameFile;

        public endOfTurn(environment m) {
            this.world = m;
            this.nameFile = "accionJ" + this.world.player + ".sbt"; 
        }

        public void writeEndOfTurn() {
            System.IO.StreamWriter actionFile = new System.IO.StreamWriter(nameFile);
            actionFile.WriteLine("0");
            actionFile.WriteLine("0");
            actionFile.WriteLine("False");
            actionFile.WriteLine("0");
            actionFile.Close();
        }
    }
}
