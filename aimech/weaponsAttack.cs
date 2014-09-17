using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que determina los ataques a realizar durante la fase de ataque con armas 
     * no está terminada!!! falta mucha tela por cortar
     * */
    class weaponsAttack
    {
        public environment world;
        public string nameFile;
        public List<singleWeaponAttack> attacks = new List<singleWeaponAttack>();
        public int maxHeat;
        public List<component> components;
        public List<component> weapons;
        public List<component> ammo;
        public List<component> equip;
        public string target;
        public string myPos;
        public int myBodySide;
        public bool usedRightArm = false;
        public bool usedRightLeg = false;
        public bool usedLeftArm = false;
        public bool usedLeftLeg = false;

        public weaponsAttack(environment m) {
            this.world = m;
            this.nameFile = "accionJ" + this.world.player + ".sbt";
            this.components = this.world.myMech.components;
            this.weapons = this.world.myMech.weapons;
            this.ammo = this.world.myMech.ammo;
            this.equip = this.world.myMech.equip;
            this.myPos = this.world.myMech.position;
            this.target = this.world.theOthers[0].position;
            this.myBodySide = this.world.myMech.bodySide;
        }

        public bool hasRange(component weapon) {
            int maxRange = weapon.longDistance;
            if (movement.h(this.myPos, this.target) <= maxRange)
            {
                Console.Out.WriteLine(weapon.name+ " "+"true");
                return true;
            }
            else {
                Console.Out.WriteLine(weapon.name + " " + "false");
                return false;
            }
        }

        public bool isOperative(component weapon)
        {
            bool operative = weapon.operative;
            if (operative)
            {
                Console.Out.WriteLine(weapon.name + " " + "true");
                return true;
            }
            else
            {
                Console.Out.WriteLine(weapon.name + " " + "false");
                return false;
            }
        }

        public int getAngle(string goal) {
            
            int[] myPosCoor = new node(this.myPos).getC();
            int[] rivalPosCoor = new node(goal).getC();
            int bestSide = movement.whereFace(this.myPos,goal);
            Console.Out.WriteLine("best side is: " + bestSide);
            return 0;
            /*int dist = utilities.circularDist(this.myBodySide,bestSide);
            if (dist <2) {
                return 1;
            }
            else if (dist == 2)
            {
                return 2;
            }
            else {
                return 3;
            }
            /*int gx = rivalPosCoor[0];
            int gy = rivalPosCoor[1];
            int sx = myPosCoor[0];
            int sy = myPosCoor[1];
            int deltaX = Math.Abs(gx-sx);
            int deltaY = Math.Abs(gy - sy);
            int[] numbers = new int[] { gx, gy, sx, sy, deltaX, deltaY };
            //utilities.printIntList(numbers);
            if (this.myBodySide == 1) {
                if (gy <= sy && deltaY >= deltaX) {
                    return 1;
                }
                if (gx < sx && deltaX > deltaY) {
                    return 4;
                }
                if (gx > sx && deltaX > deltaY) {
                    return 2;
                }
                if (gy >= sy && deltaX >= deltaY) {
                    return 3;
                }
            }
            return -1;*/
        }

        public void writeWeaponsAttack() {
            foreach(component w in this.weapons){
                this.hasRange(w);
            }

            System.IO.StreamWriter actionFile = new System.IO.StreamWriter(nameFile);
            actionFile.WriteLine("False");
            actionFile.WriteLine("0000");
            actionFile.WriteLine("0");
            actionFile.Close();
        }
    }
}
