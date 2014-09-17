using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO{
    /**
     * Clase main de la práctica, leemos los ficheros que definen el entorno
     * y vemos en que fase nos encontramos y llamamos a la clase que resuelva dicha clase
     * */
    class Program{
    
        static void Main(string[] args){
            int nPlayer = Convert.ToInt32(args[0]);
            string phase = args[1];
            string actionFileName = "accionJ"+nPlayer+".sbt";
            string mapFileName = "mapaJ"+nPlayer+".sbt";
            string mechsFileName = "mechsJ"+nPlayer+".sbt";
            string configFileName = "configJ" + nPlayer + ".sbt";
            
            string[] mapData = System.IO.File.ReadAllLines(mapFileName);
            string[] mechsData = System.IO.File.ReadAllLines(mechsFileName);
            string[] configData = System.IO.File.ReadAllLines(configFileName);

            environment theWorld = new environment(nPlayer,mapData,mechsData);

            if (phase == "Movimiento") {
                movement mov = new movement(theWorld);
                Console.Out.WriteLine(mov.jump);
                mov.go();
                mov.writeMovement();
            }

            else if (phase == "Reaccion") {
                reaction rea = new reaction(theWorld);
                rea.writeReaction();
            }

            else if (phase == "AtaqueArmas") {
                weaponsAttack wAtt = new weaponsAttack(theWorld);
                wAtt.writeWeaponsAttack();
            }

            else if (phase == "AtaqueFisico") {
                physicalAttack pAtt = new physicalAttack(theWorld);
                pAtt.writePhysicalAttack();
            }

            else if (phase == "FinalTurno")
            {
                endOfTurn EOT = new endOfTurn(theWorld);
                EOT.writeEndOfTurn();
            }

            else {
                //Test.printMechs(theWorld);
                Test.testNeigs(theWorld);
            }
        }
    }
}
