using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase que representa el tablero de juego de BattleTech
     * 
     * */
    class map
    {
        public int height = 0;
        public int width = 0;
        public Dictionary<string, node> nodes = new Dictionary<string, node>();
        /**
         * Constructor de la clase, a partir de un array que contiene los datos del mapa generamos el mapa
         * */
        public map(string[] data) {
            this.height = Convert.ToInt32(data[1]);
            this.width = Convert.ToInt32(data[2]);
            int index = 3;
            for (int c = 1; c <= this.width ; c++) {
                //Leemos los datos correspondiente a un nodo
                for (int r = 1; r <= this.height ; r++) { 
                    string coordinates = utilities.numbersToCoordinates(c,r);
                    this.nodes.Add(coordinates,new node(coordinates));
                    string[] nodeData=new String[20];
                    Array.Copy(data,index,nodeData,0,20);
                    this.nodes[coordinates].setData(nodeData) ;
                    index += 20;
                }
                
            }
        }

        /**
         * Devuelve los vecinos de un nodo dado 
         **/
        public List<string> neigs(string node) {
            int[] coordinates = new node(node).getC();
            int x=coordinates[0];
            int y=coordinates[1];
            List<String> neigs = new List<string>();
            List<int[]> neigsCoordinates = new List<int[]>();

            if (x > this.width || y > this.height || x < 1 || y < 1) { return neigs; }
            
            if (y - 1 > 0) {
                int[] up = { x, y - 1 };
                neigsCoordinates.Add(up);
            }
            
            if (y+1<= this.height){
                int[] down = { x, y + 1 };
                neigsCoordinates.Add(down);
            }

            if (x % 2 == 0)
            {
                if (x + 1 <= this.width)
                {
                    int[] ne = { x + 1, y };
                    neigsCoordinates.Add(ne);
                }

                if (x + 1 <= this.width && y + 1 <= this.height)
                {
                    int[] se = { x + 1, y + 1 };
                    neigsCoordinates.Add(se);
                }

                if (x - 1 > 0 && y + 1 <= this.width)
                {
                    int[] sw = { x - 1, y + 1 };
                    neigsCoordinates.Add(sw);
                }

                if (x - 1 > 0)
                {
                    int[] nw = { x - 1, y };
                    neigsCoordinates.Add(nw);
                }
            }

            else {
                if (x + 1 <= this.width && y-1 > 0)
                {
                    int[] ne = { x + 1, y-1 };
                    neigsCoordinates.Add(ne);
                }

                if (x + 1 <= this.width)
                {
                    int[] se = { x + 1, y};
                    neigsCoordinates.Add(se);
                }

                if (x - 1 > 0)
                {
                    int[] sw = { x - 1, y };
                    neigsCoordinates.Add(sw);
                }

                if (y - 1 > 0 && x-1>0)
                {
                    int[] nw = { x - 1, y-1 };
                    neigsCoordinates.Add(nw);
                }
            }

            foreach (int[] coordinate in neigsCoordinates) {
                neigs.Add(utilities.numbersToCoordinates(coordinate[0],coordinate[1]));
            }

            return neigs;
        }

        public string neig(string node, int side){
            int[] coords = new node(node).getC();
            int x = coords[0];
            int y = coords[1];
            if(side==1){
                y--;
            }
            else if(side==4){
                y++;
            }
            else{
                if(x%2==0){
                    if(side==2){
                        x++;

                    }
                    if(side==3){
                        x++;
                        y++;
                    }
                    if(side==5){
                        x--;
                        y++;
                    }
                    if(side==6){
                        x--;

                    }
                }
                else{
                    if(side==2){
                        x++;
                        y--;
                    }
                    if(side==3){
                        x++;
                    }
                    if(side==5){
                        x--;
                    }
                    if(side==6){
                        x--;
                        y--;
                    }
                }
            }
            if (x <= 0 || y <= 0 || x > this.width || y > this.height) {
                return utilities.numbersToCoordinates(0, 0);
            }
            return utilities.numbersToCoordinates(x,y);
        }

        public List<string> frontNeigs(string node, int side) {
            int side1 = side - 1;
            int side2 = (side1 + 1) % 6;
            int side3 = -1;
            if (side1 - 1 == -1)
            {
                side3 = 5;
            }
            else {
                side3 = (side1 - 1) % 6;
            }
            List<string> theFrontNeigs = new List<string>();
            string neig1 = this.neig(node, side1+1);
            string neig2 = this.neig(node, side2+1);
            string neig3 = this.neig(node, side3+1);
            if (String.Compare(neig3, "0000") != 0)
            {
                theFrontNeigs.Add(neig3);
            }
            if (String.Compare(neig1, "0000") != 0) {
                theFrontNeigs.Add(neig1);
            }
            if (String.Compare(neig2, "0000") != 0)
            {
                theFrontNeigs.Add(neig2);
            }
            return theFrontNeigs;
        }

        /**
         * Nos dice si dos nodos son vecinos
         * **/
        public bool areNeigs(string n1, string n2) {
            List<string> neigsN1 = this.neigs(n1);
            if (neigsN1.Exists(item => item==n2))
            {
                return true;
            }
            else {
                return false;
            }
        }

        /**
         * Nos dice si un nodo se puede acceder en un solo paso -andando o corriendo-desde otro
         * Es decir nos dice si es vecino y ademas la diferencia de nivel no es superior a 2
         * **/
        public bool reachable (string n1, string  n2){
        //queda por implementar el de si la casilla es muy profunda??
            if (!this.areNeigs(n1, n2)) {
                return false;
            }
            //Calculamso el desnivel
            int ramp = Math.Abs(this.nodes[n1].nivel - this.nodes[n2].nivel);

            if (ramp > 2) {
                return false;
            }
            
            return true;

        }

        /**
         * Lista de nodos accesibles desde uno dado
         * */
        public List<string> listOfReachables(string n1) { 
            List<string> canGo = new List<string>();
            List<string> neigs = this.neigs(n1);
            foreach(string neig in neigs){
                if(this.reachable(n1,neig)){
                    canGo.Add(neig);
                }
            }
            return canGo;
        }

        public List<string> isInFrontAngle(string start, int side) {
            List<string> abiertos = new List<string>();
            List<string> solucion = new List<string>();
            abiertos.Add(start);
            while(abiertos.Count>0){
                List<string> candidatos = new List<string>();
                foreach (string item in abiertos.ToArray()) {
                    foreach (string item2 in this.frontNeigs(item, side)) {
                        if (!solucion.Contains(item2)) {
                            solucion.Add(item2);
                            candidatos.Add(item2);
                        }
                    }
                }
                abiertos = candidatos;
            }
            return solucion;
        }

        /**
         * Imprime en pantalla el mapa, método para testear la clase
         * */
        public void print() {
            Console.Out.WriteLine("alto: " + this.height);
            Console.Out.WriteLine("ancho: " + this.width);
            for (int r = 1; r <= this.height ; r++) {
                for (int c = 1; c <= this.width; c++) {
                    //Console.Out.WriteLine("coordinates: ");
                    string coordinates = utilities.numbersToCoordinates(c, r);
                    node n=this.nodes[coordinates];
                    n.print();
                }
            }
        }

        /**
         * Devuelve la diferencia de nivel entre dos nodos del mapa
         * */
        public int diffLevel(string node1, string node2) {
            return this.nodes[node1].nivel-this.nodes[node2].nivel;
        }

        /**
         * Dada una lista de candidatos y una meta, devuelve los nodos de dicha lista
         * que tenga una determina diferencia de altura con la meta
         * @diff : diferencia de altura
         * */
        public List<string> onlyThisDiffLevel(int diff, List<string> candidates, string goal) {
            List<string> theNodes = new List<string>();
            //Console.Out.WriteLine("entro--->"+candidates.Count);
            for (int i = 0; i < candidates.Count; i++) {
                string currentNode = candidates[i];
                int currentDiff = this.diffLevel(currentNode,goal);
                //Console.Out.WriteLine(currentNode + " " + goal + " " + currentDiff);
                if (currentDiff == diff) {
                    theNodes.Add(currentNode);
                }
            }
            return theNodes;
        } 

        /**
         * Dada una lista de nodos devuelve el de mayor altura
         * */
        public List<string> highest(List<string> theNodes) {
            string highestNode = theNodes[0];
            int level = this.nodes[highestNode].nivel;
            for (int i = 1; i < theNodes.Count; i++) { 
                string CurrentNode = theNodes[i];
                int curentLevel = this.nodes[CurrentNode].nivel;
                if (curentLevel > level) {
                    highestNode = CurrentNode;
                    level = curentLevel;
                }
            }
            List<string> highestNodes = new List<string>();
            for (int i = 0; i < theNodes.Count; i++) {
                if (this.nodes[theNodes[i]].nivel == level) {
                    highestNodes.Add(theNodes[i]);
                }
            }
            return highestNodes;
        }

        /**
         * Dada una lista de nodos devuelve cual de ellos está mas cercano a la meta
         * */
        public List<string> nearest(List<string> theNodes, string goal) { 
            string nearestNode = theNodes[0];
            int minDistance = movement.h(nearestNode,goal);
            for (int i = 1; i < theNodes.Count; i++)
            {
                string currentNode = theNodes[i];
                int currentDistance = movement.h(currentNode, goal);
                if (currentDistance < minDistance)
                {
                    nearestNode = currentNode;
                    minDistance = currentDistance;
                }
            }
            List<string> nearestNodes = new List<string>();
            for (int i = 0; i < theNodes.Count; i++)
            {
                if (movement.h(theNodes[i],goal) == minDistance)
                {
                    nearestNodes.Add(theNodes[i]);
                }
            }
            return nearestNodes;
        }

        /*
         * Dada una lista de nodos nos devuelve aquellos nodos que están mas cerca de la meta
         * que una determinada cantidad definida por minDistance
         **/
        public List<string> areNearer(List<string> theNodes, string goal, int minDistance) {
            List<string> newNodes = new List<string>();
            for (int i = 0; i < theNodes.Count; i++) {
                if (movement.h(theNodes[i], goal) < minDistance) {
                    newNodes.Add(theNodes[i]);
                }
            }
            return newNodes;
        }

        /**
         * Halla el area circular de un determinado radio a un nodo centro dado
         * */
        public List<string> circularArea(string centerNode, int radix) {
            
            List<string> nodesInArea = new List<string>();
            nodesInArea.Add(centerNode);
            for (int i = 1; i <=radix; i++) {
                List<string> newNodes = new List<string>();
                foreach (string node in nodesInArea) {
                    foreach (string candidate in this.neigs(node)) {
                        if (!nodesInArea.Contains(candidate) && !newNodes.Contains(candidate)) {
                            newNodes.Add(candidate);
                        }
                    }  
                }
                nodesInArea.AddRange(newNodes);
                List<string> toRemove = new List<string>();
            }
            return nodesInArea;
        }

        /**
         * Devuelve una lista con todos los nodos del mapa
         * */
        public List<string> allNodes() {
            string [] nodes = new string[this.height*this.width];
            this.nodes.Keys.CopyTo(nodes,0);
            return new List<string>(nodes);
        }

        /*public List<string> line(string node) {
            int[] coords = new node(node).getC();
            List<string> theLine = new List<string>();
            int x = coords[0];
            int y = coords[1];
            while (x <= this.width && y <= this.height) {
                theLine.Add(utilities.numbersToCoordinates(x,y));
                x++;
                y++;
            }
        }*/
    }
}
