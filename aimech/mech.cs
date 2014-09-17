using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase abstracta que representa las partes comunes entre los mechs enemigos y el mech que controlarermos
     * */
    abstract class mech
    {
        public int player=-1;
        public bool operative;
        public bool powerOff;
        //atascado
        public bool stacked;
        public bool onGround;
        public string position;
        public int faceSide;
        public int bodySide;
        public int heat;
        public bool fire;
        //Garrote
        public bool club;
        public int kindClub;
        //Ordenados: BI, TI, PI, PD, TD, BD, TC, C, bTI, bTD, bTC
        public int[] armor = new int[11];
        //Ordenados: BI, TI, PI, PD, TD, BD, TC, C
        public int[] internalArmor = new int[8];
        public bool[] narc;
        public bool[] inarc;
        public int tons;
        public List<component> components = new List<component>();
        public List<component> weapons = new List<component>();
        public List<component> ammo = new List<component>();
        public List<component> equip = new List<component>();
        public int nComponents;
        public int nWeapons;
        public int nAmmo;
        public int nEquip;
        public int nActuators;
        public List<Actuator> actuators = new List<Actuator>();

        /**
         * Establece los componentes de un mech a partir del fichero defMechJx-y.sbt
         * */
        public void setComponents(string file) {
            string[] fileData = System.IO.File.ReadAllLines(file);
            Queue<string> data = new Queue<string>(fileData);
            for (int i = 0; i < 40; i++) {
                string line = data.Dequeue();
                if (i == 3) {
                    this.tons = Convert.ToInt32(line);
                }
            }
            //Leemos los componentes
            this.nComponents = Convert.ToInt32(data.Dequeue());
            for (int i = 0; i < this.nComponents; i++) {
                component newComponent = new component();
                newComponent.code = Convert.ToInt32(data.Dequeue());
                newComponent.name = data.Dequeue();
                newComponent.type = data.Dequeue();
                newComponent.inBack = Convert.ToBoolean(data.Dequeue());
                newComponent.place = Convert.ToInt32(data.Dequeue());
                newComponent.secondaryPlace = Convert.ToInt32(data.Dequeue());
                newComponent.typeWeapon = data.Dequeue();
                newComponent.heatGenerated = Convert.ToInt32(data.Dequeue());
                newComponent.power = Convert.ToInt32(data.Dequeue());
                newComponent.shotsPerTurn = Convert.ToInt32(data.Dequeue());
                newComponent.minimumDistance = Convert.ToInt32(data.Dequeue());
                newComponent.shortDistance = Convert.ToInt32(data.Dequeue());
                newComponent.mediumDistance = Convert.ToInt32(data.Dequeue());
                newComponent.longDistance = Convert.ToInt32(data.Dequeue());
                newComponent.operative = Convert.ToBoolean(data.Dequeue());
                newComponent.codeAmmo = Convert.ToInt32(data.Dequeue());
                newComponent.ammoAmount = Convert.ToInt32(data.Dequeue());
                newComponent.specialAmmo = data.Dequeue();
                newComponent.trigger = Convert.ToInt32(data.Dequeue());
                this.components.Add(newComponent);
            }
            //Leemos los actuadores
            data.Dequeue();
            this.nActuators = Convert.ToInt32(data.Dequeue());
            for (int i = 0; i < nActuators; i++) {
                Actuator newActuator= new Actuator();
                newActuator.code = Convert.ToInt32(data.Dequeue());
                newActuator.name = data.Dequeue();
                newActuator.location = Convert.ToInt32(data.Dequeue());
                newActuator.operative = Convert.ToBoolean(data.Dequeue());
                newActuator.nImpacts = Convert.ToInt32(data.Dequeue());
                this.actuators.Add(newActuator);
            }
            //Vemos los slots y cantidades de los distinto componentes y actuadores que componen el mech
            for (int i = 0; i < 8; i++) {
                int nSlots = Convert.ToInt32(data.Dequeue());
                for (int j = 0; j < nSlots; j++) {
                    string type = data.Dequeue();
                    int amount = Convert.ToInt32(data.Dequeue());
                    int code = Convert.ToInt32(data.Dequeue());
                    string name = data.Dequeue();
                    int componentIndex = Convert.ToInt32(data.Dequeue());
                    int actuatorIndex = Convert.ToInt32(data.Dequeue());
                    int damage = Convert.ToInt32(data.Dequeue());
                    if (type == "ARMA" || type == "MUNICION") { 
                        if(this.components[componentIndex].slot==-1){
                            this.components[componentIndex].slot = j;
                            this.components[componentIndex].amount = amount;
                        }
                    }
                    if (type == "ACTUADOR")
                    {
                        if (this.actuators[actuatorIndex].slot == -1)
                        {
                            this.actuators[actuatorIndex].slot = j;
                            this.actuators[actuatorIndex].amount = amount;
                        }
                    }
                }
            }
           //clasificamos los distintos componentes en armas, municiones o equipo
           for (int i = 0; i < this.nComponents; i++){
                component current = this.components[i];
                if (current.type == "ARMA")
                {
                    this.weapons.Add(current);
                }
                else if (current.type == "MUNICION")
                {
                    this.ammo.Add(current);
                }
                else
                {
                    this.equip.Add(current);
                }
            }
            this.nWeapons = this.weapons.Count;
            this.nAmmo = this.ammo.Count;
            this.nEquip = this.equip.Count;
            //Para cada una de las armas determinamos su ángulo
            for (int i = 0; i < this.nWeapons; i++) {
                int place = this.weapons[i].place;
                if (place < 8)
                {
                    this.weapons[i].angle[0] = 1;
                    if (place == 0) {
                        this.weapons[i].angle[3] = 1;
                    }
                    if (place == 5) {
                        this.weapons[i].angle[1] = 1;
                    }
                }
                else {
                    this.weapons[i].angle[2] = 1;
                }
            }
        }

        /**
         * Nos dice si el mech tiene las dos caderas y por tanto puede pegar patadas y asumirse que tiene las dos piernas
         * */
        public bool IHaveLegs() {
            List<Actuator> hips = this.actuators.FindAll(hip => hip.name=="Cadera" && hip.operative);
            if (hips.Count > 1)
            {
                return true;
            }
            else {
                return false;
            }
        }

        /**
         * Nos dice si el mech tiene el hombre derecho y por tanto puede puede usar su brazo derecho
         * */
        public bool IHaveRightArm() {
            List<Actuator> shoulder = this.actuators.FindAll(s => s.name == "Hombro" && s.operative && s.location==5);
            if (shoulder.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /**
        * Nos dice si el mech tiene el hombre izquierdo y por tanto puede puede usar su brazo izquierdo
        * */
        public bool IHaveLeftArm()
        {
            List<Actuator> shoulder = this.actuators.FindAll(s => s.name == "Hombro" && s.operative && s.location == 0);
            if (shoulder.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void print() {
            Console.Out.WriteLine("Jugador: "+this.player);
            Console.Out.WriteLine("Posición en el mapa: " + this.position);
            Console.Out.WriteLine("Posición de la cara: " + this.faceSide);
            Console.Out.WriteLine("Posición del cuerpo: " + this.bodySide);
            Console.Out.WriteLine("Operativo: "+this.operative);
            Console.Out.WriteLine("Apagado: "+this.powerOff);
            Console.Out.WriteLine("Atascado: "+this.stacked);
            Console.Out.WriteLine("En el suelo: "+this.onGround);
            Console.Out.WriteLine("Calor: "+this.heat);
            Console.Out.WriteLine("Ardiendo: "+this.fire);
            Console.Out.WriteLine("Garrote: "+this.club);
            Console.Out.WriteLine("Tipo de garrote: "+this.kindClub);
            Console.Out.WriteLine("Tonelaje: " + this.tons);
            Console.Out.WriteLine("N de componentes: " + this.nComponents);
            Console.Out.WriteLine("Puntos de armadura: ");
            foreach(int point in this.armor){
                Console.Out.WriteLine(point);
            }
            Console.Out.WriteLine("Puntos de armadura interna: ");
            foreach (int point in this.internalArmor)
            {
                Console.Out.WriteLine(point);
            }
            Console.Out.WriteLine("Le ha puesto narc: ");
            foreach (bool n in this.narc)
            {
                Console.Out.WriteLine(n);
            }
            Console.Out.WriteLine("Le ha puesto inarc: ");
            foreach (bool n in this.inarc)
            {
                Console.Out.WriteLine(n);
            }

             Console.Out.WriteLine("Armas del mech:");
            for (int i = 0; i < this.nWeapons; i++) {
                Console.Out.WriteLine("\t%%%Arma nº "+i+" ");
                this.weapons[i].print();
            }
            
            Console.Out.WriteLine("Munición del mech:");
            for (int i = 0; i < this.nAmmo; i++)
            {
                Console.Out.WriteLine("\t$$$Munición nº " + i + " ");
                this.ammo[i].print();
            }

            Console.Out.WriteLine("Equipos del mech:");
            for (int i = 0; i < this.nEquip; i++)
            {
                Console.Out.WriteLine("\t@@@Equipo nº " + i + " ");
                this.equip[i].print();
            }

            Console.Out.WriteLine("Actuadores del mech:");
            for (int i = 0; i < this.actuators.Count; i++)
            {
                Console.Out.WriteLine("\t&&&Actuador nº " + i + " ");
                this.actuators[i].print();
            }
        }
    }
}
