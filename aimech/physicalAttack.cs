using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que toma las decisiones de la fase de ataques físicos
     * */
    class physicalAttack
    {

        public environment world;
        public string nameFile;
        public bool IcanRPunch = false;
        public bool IcanLPunch = false;
        public bool IcanRKick = false;
        //Posición 0 --> BD, 1-->BI, 2-->PD
        public bool[] attacks = new bool[] { false, false, false };
        public string nodeToAttack;
        public string myNode;
        public int mySide;
        public int myBodySide;
        public int bestSideToAttack;
        public int myHeat;

        public physicalAttack(environment m) {
            this.world = m;
            this.nameFile = "accionJ" + this.world.player + ".sbt";
            this.nodeToAttack = this.world.theOthers[0].position;
            this.myNode = this.world.myMech.position;
            this.mySide = this.world.myMech.faceSide;
            this.myBodySide = this.world.myMech.bodySide;
            this.bestSideToAttack = movement.whereFace(this.myNode,this.nodeToAttack);
            this.myHeat = this.world.myMech.heat;
            this.determinateAttack();
        }

        /**
         * Determina, según el entorno, que ataques físicos se pueden realizar
         * */
        public void determinateAttack() {
            bool areNeigs = this.world.land.areNeigs(this.myNode,this.nodeToAttack);
            int circularBodyDist = utilities.circularDist(this.myBodySide, this.bestSideToAttack);
            int diffLevel = this.world.land.diffLevel(this.myNode,this.nodeToAttack);
            
            //Si la diferencia de altura entre nodos es mayor de 1, si no son vecinos directamente no se puede atacar
            if (diffLevel > 1 || diffLevel < -1 || !areNeigs || this.myHeat >9)
            {
                return;
            }

            else {
                //Comprobamos que se cumplen las codiciones para dar un puñetazo con la derecha
                if (diffLevel > -1 && diffLevel < 2 && circularBodyDist > -1 && circularBodyDist < 2 && !this.world.theOthers[0].onGround && this.world.myMech.IHaveRightArm()) {
                    this.IcanRPunch = true;
                    this.attacks[0] = true;
                }
                //Comprobamos que se cumplen las codiciones para dar un puñetazo con la izquierda
                if (diffLevel > -1 && diffLevel < 2 && circularBodyDist > -2 && circularBodyDist < 1 && !this.world.theOthers[0].onGround && this.world.myMech.IHaveLeftArm()) {
                    this.IcanLPunch = true;
                    this.attacks[1] = true;
                }
                //Comprobamos que se cumplen las codiciones para dar patadas
                if (diffLevel > -2 && diffLevel < 1 && circularBodyDist > -2 && circularBodyDist < 2 && this.world.myMech.IHaveLegs()) {
                    this.IcanRKick = true;
                    this.attacks[2] = true;
                }
            }
        }

        /**
         * Cuenta el número de ataques físicos que tenemos pensado hacer
         * */
        public int counAttacks() {
            int nAttacks = 0;
            foreach (bool attack in this.attacks) {
                if (attack) {
                    nAttacks++;
                }
            }
            return nAttacks;
        }

        /**
         * Escribimos los ataques físicos planeados en el fichero accionJx.sbt
         * */
        public void writePhysicalAttack() {
            System.IO.StreamWriter actionFile = new System.IO.StreamWriter(nameFile);
            actionFile.WriteLine(this.counAttacks());
            for (int i = 0; i < 3; i++) {
                if (this.attacks[i]) {
                    if (i == 0) {
                        actionFile.WriteLine("BD");
                        actionFile.WriteLine("1000");
                    }
                    else if (i == 1) {
                        actionFile.WriteLine("BI");
                        actionFile.WriteLine("1000");
                    }
                    else if (i == 2) {
                        actionFile.WriteLine("PD");
                        actionFile.WriteLine("2000");
                    }
                    actionFile.WriteLine(this.nodeToAttack);
                    //Siempre atacamos al mech, no se que ventajas puede tener dar puñetazos al aire. Derribar edificos o arboles?
                    actionFile.WriteLine("Mech");
                }
            }
                actionFile.Close();
        }
    }
}
