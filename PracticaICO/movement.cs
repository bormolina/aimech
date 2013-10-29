using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    /**
     * Clase para la toma de deciones de la fase de movimiento
     * */
    class movement
    {
        //Contiene todos los datos referentes al juego: mapa, mechs, opciones...
        public environment world;
        //Camino total que se ha decidido para la acción de andar
        public List<string> path = new List<string>();
        //Giros que se han de realizar para poder seguir el camino determinado en la variable path
        public List<int> turns = new List<int>();
        string[] allowedMovements = new string[] {"Inmovil","Andar","Correr","Saltar"};
        int myHeat;
        //Como no tenemos implementada la fase de ataque con armas siempre suponemos que no tenemos armas
        int nWeapons = 0;
        public bool jump = false;
        string nodeToJump = "";
        int sideAtJump = -1;

        public movement(environment m) {
            this.world = m;
            this.myHeat = this.world.myMech.heat;
        }

        /**
         * Funcion h(x) para a*
         * Distancia manhatan para tablero hexagonal
         * Se hace static porq es independiente del mapa
         * **/
        public static int h(string n1, string n2) {
            int[] coordinates1 = new node(n1).getC();
            int[] coordinates2 = new node(n2).getC();
            int x1 = coordinates1[0];
            int y1 = coordinates1[1];
            int x2 = coordinates2[0];
            int y2 = coordinates2[1];
            int deltaX = Math.Abs(x2 - x1);
            int deltaY = Math.Abs(y2 - y1);

            if (x1 == x2) {
                return deltaY;
            }

            if (y1 == y2) {
                return deltaX;
            }

            //Calculamos el factor
            int factor;
            if (deltaY % 2 != 0) { factor = 0; }
            if (y1 < y2) { factor = (x1 - 1) % 2; }
            else { factor = (x2 - 1) % 2; }

            return deltaX + Math.Max(0, deltaY - (deltaX / 2) - factor);
        }

        /**
         * Mide la distancia entre dos lados de un hexagono
         * */
        public static int sideDist(int side1, int side2) {
            int dist = Math.Abs(side1 - side2);
            if (dist > 3) {
                dist -= 6;
                dist *= -1;
            }
            return dist;
        }

        /**
          * Nos dice a que lado nos tenemos que encarar cuando queremos ir de start a goal
          * **/
        public static int whereFace(string start, string goal) { 
            int[] coordinates1 = new node(start).getC();
            int[] coordinates2 = new node(goal).getC();
            int x1 = coordinates1[0];
            int y1 = coordinates1[1];
            int x2 = coordinates2[0];
            int y2 = coordinates2[1];

           int side=-1;

             if (x1 == x2 && y1 > y2) {
               side=1;
            }

            if (x1 == x2 && y1 < y2) {
                side= 4;
            }

            if (x1 % 2 == 0)
            {
                if (x2 > x1 && y2 <= y1)
                {
                    side = 2;
                }

                if (x2 > x1 && y2 > y1)
                {
                    side = 3;
                }

                if (x2 < x1 && y2 > y1)
                {
                    side = 5;
                }

                if (x2 < x1 && y2 <= y1)
                {
                    side = 6;
                }
            }

            else {
                if (x2 > x1 && y2 < y1)
                {
                    side = 2;
                }

                if (x2 > x1 && y2 >= y1)
                {
                    side = 3;
                }

                if (x2 < x1 && y2 >= y1)
                {
                    side = 5;
                }

                if (x2 < x1 && y2 < y1)
                {
                    side = 6;
                }
            }
            return side;
        }

        /**
         * Coste de girar para encararse de 'start' a 'goal' estando mirando a 'side'
         * */
        public static int costToTurn(string start, string goal, int side) {
            int newSide = movement.whereFace(start,goal);
            return movement.sideDist(side,newSide);
        }

        /**
         * Nos dice el coste parcial de movernos de 'start' a 'goal', no tenemos en cuenta el coste de los giros
         * */
        public int rawCost(string start, string goal) {
            node startNode = this.world.land.nodes[start];
            node goalNode = this.world.land.nodes[goal];

            //Si el nodo partida es el mismo que el nodo destino
            if (start == goal) { return 0; }

            int elevationCost = 0;
            int objectCost = 0;
            int kindCost = 0;

            //Coste por elevación
            int diffElevation = Math.Abs(this.world.land.nodes[start].nivel-this.world.land.nodes[goal].nivel);
            if (diffElevation < 3)
            {
                elevationCost = diffElevation;
            }
            else {
                return -1;
            }

            //Coste por objeto
            //Si hay escombros o bosque disperso
            int objectInNode = this.world.land.nodes[start].objeto;
            if(objectInNode==0 || objectInNode==1){
                objectCost=2;
            }
            //Si hay bosque denso
            if (objectInNode == 2)
            {
                objectCost=3;
            }

            //Coste por terreno
            //Si hay carretera el coste es 1 independientemente del tipo de terreno
            int side = movement.whereFace(start,goal);
            if (this.world.land.nodes[start].carreteras[side - 1]){
                kindCost = 1;
            } 
            //Si no hay carretera calculamos el coste según el tipo de suelo
            else { 
                //Si es normal o asfaltado
                int kindNode=this.world.land.nodes[start].tipo;
                if (kindNode == 0 || kindNode == 1) {
                    kindCost = 1;
                }
                //Si es agua
                else if (kindNode == 2) {
                    int elevation = this.world.land.nodes[start].nivel;
                    if (elevation > -1) {
                        kindCost = 1;
                    }
                    else if (elevation == -1) {
                        kindCost = 2;
                    }
                    else if (elevation < -1) {
                        kindCost = 4;
                    }
                }
                //Si es pantano
                else if (kindNode == 3) {
                    kindCost = 2;
                }
            }   

            int total = elevationCost + objectCost + kindCost;
            return total;

            //Console.Out.WriteLine("$"+start + " ---> " + goal + " = " + total + "\n\t*elevation: " + elevationCost + "\n\t*object: " + objectCost + "\n\t*kind: " + kindCost);
        }

        /**
         * TotalCost, dado un nodo de partida, uno de llegada y el encaramiento actual; nos dice cual es el coste total
         * de realizar el movimiento, teniendo en cuenta el coste de los giros necesarios y el de desplazarse por el terreno
         * */
        public int totalCost(string start, string goal, int side) {
            return this.rawCost(start, goal) + movement.costToTurn(start,goal,side);
        }

        /**
         * Devuelve true si se puede saltar de start a goal
         * */
        public bool canIJump(string start, string goal){
            int myJumpPoints = this.world.myMech.jump;
            int myTypeOfTerrain = this.world.land.nodes[start].tipo;
            int myLevel = this.world.land.nodes[start].nivel;
            int goalLevel = this.world.land.nodes[goal].nivel;
            int goalType = this.world.land.nodes[goal].tipo;
            int typeOfRival = this.world.land.nodes[this.world.theOthers[0].position].tipo;

            if (myJumpPoints < goalLevel - myLevel || myJumpPoints==0 ||  goal == this.world.theOthers[0].position)
            {
                return false;
            }
            else if ( typeOfRival!= 3 && goalType == 3) {
                return false;
            }
            else
            {
                List<string> linePath = new List<string>();
                string args = "mapaJ" + this.world.player + ".sbt " + start + " 1 " + goal + " 1";
                utilities.executeLDV(args, ref linePath);
                for (int i = 0; i < linePath.Count; i++)
                {
                    string node = linePath[i];
                    if (node != "")
                    {
                        int levelOfNode = this.world.land.nodes[node].nivel;

                        if (levelOfNode - myLevel > myJumpPoints)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        }

        /**
         * Escoge el mejor nodo entre dos nodos dados
         * */
        public string chooseNode(string node1, string node2) {
            string rivalMechPosition = this.world.theOthers[0].position;
            bool rivalMechOnGround = this.world.theOthers[0].onGround;
            int position = 1;
            if (node2 == "-1") {
                return node1;
            }
            if (rivalMechOnGround)
            {
                position = 0;
            }
            int dist1 = movement.h(node1, rivalMechPosition);
            int dist2 = movement.h(node2, rivalMechPosition);
            if (dist1 < dist2)
            {
                return node1;
            }
            else {
                return node2;
            }
            //Parte para cuando tengamos implementada la parte de ataque con armas
            /*string args1 = "mapaJ" + this.world.player + ".sbt " + node1 + " 1 " + rivalMechPosition + " " + position;
            string args2 = "mapaJ" + this.world.player + ".sbt " + node2 + " 1 " + rivalMechPosition + " " + position;
            List<string> line = new List<string>();
            int score1 = utilities.executeLDV(args1,ref line);
            int score2 = utilities.executeLDV(args2, ref line);
            if (score1 >= score2)
            {
                return node1;
            }
            else {
                return node2;
            }*/
        }

        /**
         * Método para decidir si es mejor saltar que andar/correr
         * */
        public void decideIfJump() {
            string rivalPosition = this.world.theOthers[0].position;
            string candidateToJump = this.betterToJump(this.jumpAbles(),rivalPosition);
            string candidateToWalk;
            
            if(this.path.Count>0){candidateToWalk = this.path[this.path.Count - 1];}
            else{
                candidateToWalk = this.world.myMech.position;
            }
            string better = this.chooseNode(candidateToWalk,candidateToJump);
            if (better == candidateToJump) {
                this.jump = true;
                this.nodeToJump = candidateToJump;
                this.sideAtJump = movement.whereFace(nodeToJump,rivalPosition);
            }
        }

        /**
         * Devuelve una lista de nodos a los que se puede saltar en el turno actual
         * */
        public List<string> jumpAbles()
        {
            string myPosition = this.world.myMech.position;
            int myJumpPoints = this.world.myMech.jump;
            int myTypeOfTerrain = this.world.land.nodes[myPosition].tipo;
            int myLevel = this.world.land.nodes[myPosition].nivel;
            List<string> nodesToJump = new List<string>();
            if (myJumpPoints == 0 || (myTypeOfTerrain==2 && myLevel<0)){
                return nodesToJump;
            }
            else {
                List<string> candidates = this.world.land.circularArea(myPosition,myJumpPoints);
                candidates.RemoveAt(0);
                int currentMinDist = int.MaxValue;
                for (int i = 0; i < candidates.Count; i++) {
                    int currentDist = movement.h(candidates[i],this.world.theOthers[0].position);
                    if (currentDist <= currentMinDist && currentDist>0)
                    {
                        currentMinDist = currentDist;
                        //Si el destino no es pantanoso y ademas no es tan alto como para no saltarlo
                        if (this.canIJump(myPosition, candidates[i]))
                        {
                            nodesToJump.Add(candidates[i]);
                        }
                    }
                }
            }
           return nodesToJump;
        }

        /**
         * Dada una lista de nodos candidatos se elige el mejor nodo para saltar teniendo en cuenta 
         * cual es la meta final
         * */
        public string betterToJump(List<string> candidates, string goal)
        {
            if (candidates.Count == 0)
            {
                return "-1";
            }
            List<string> betterNodes = this.world.land.nearest(candidates, goal);
            if (this.nWeapons == 0)
            {
                int index = betterNodes.FindIndex(node => node == this.bestNodeForPhysicAttack());
                if (index != -1)
                {
                    return betterNodes[index];
                }
                else {
                    betterNodes = this.world.land.highest(betterNodes);
                }
            }
            else
            {
                betterNodes = this.world.land.highest(betterNodes);
            }
            if (betterNodes.Count > 0)
            {
                return betterNodes[0];
            }
            else
            {
                return "-1";
            }
        }

        /**
         * Algoritmo A*, dada una casilla inicial y una final calcula el mejor camino para llegar hasta ella
         * */
        public List<string> A_Start(string start, string goal){
            List<string> closedSet = new List<string>();
            List<string> openSet = new List<string>();
            //Nos dice para cada nodo de que vecino se llega a él
            Dictionary<string,string> cameFrom = new Dictionary<string,string>();
            //Coste de ir desde el inicio a un nodo dado
            Dictionary<string,int> gScore = this.mapOfNodes();
            //Suma de Gscore y de la función heurística
            Dictionary<string,int> fScore = new Dictionary<string,int>();

            openSet.Add(start);
            gScore[start]=0;
            fScore[start]=gScore[start]+movement.h(start,goal);
            //Mientras halla nodos en el conjunto abierto         
            while(openSet.Count!=0){
                //El nodo a evaluar será aquel del conjunto abierto con f() mínima
                string current= this.min(openSet,fScore);
                //Si el nodo a evaluar es el fin, ya hemos acabado y reconstruimos el camino
                if(current == goal){
                    return this.reBuildPath(start,goal,cameFrom);
                }
                openSet.Remove(current);
                closedSet.Add(current);
                //Para cada uno de los vecino vemos si hay que actualizar su g()
                foreach(string neig in this.world.listOfReachables(current)){
                    int tentative = gScore[current]+this.rawCost(current, neig);
                    if(closedSet.Contains(neig) && tentative>=gScore[neig]){
                        continue;
                    }

                    if(!closedSet.Contains(neig) || tentative<gScore[neig]){
                        if(tentative<gScore[neig]){
                            cameFrom[neig] = current;
                            gScore[neig] = tentative;
                            fScore[neig] = gScore[neig] + movement.h(neig, goal);
                        }
                        if(!openSet.Contains(neig)){
                            openSet.Add(neig);
                        }
                    }
                }
            }
            return new List<string>();
        }

        /**
         * Genera un diccionario cuyas entradas son todos los nodos de los mapas
         * */
        public Dictionary<string, int> mapOfNodes() {
            Dictionary<string, int> values = new Dictionary<string, int>();
            int maxX = this.world.land.width;
            int mayY = this.world.land.height;
            for (int i = 1; i <= maxX; i++) {
                for (int j = 1; j <= mayY; j++) {
                    string coordinates = utilities.numbersToCoordinates(i,j);
                    values[coordinates] = int.MaxValue;
                }
            }
            return values;
        }

        /**
         * Reconstruye el camino seguido el algoritmo A_start
         * Se trata de una función auxiliar a mi algoritmo A* que permite reconstruir el camino seguido
         * */
        private List<string> reBuildPath(string start, string goal, Dictionary<string,string> cameFrom) {
            string current = goal;
            List<string> path = new List<string>();
            while(current!=start){
                path.Insert(0,current);
                current = cameFrom[current];
            }
            path.Insert(0, current);
            return path;
        }

        /**
         * Dada una lista de nodos y un diccionario que contiene
         * para todos los vecinos de los nodos el coste de moverse desde ese nodo
         * hasta su vecino no devuelve aquel vecino cuyo coste sea menor
         * */
        private string min(List<string> nodes, Dictionary<string, int> scores) {
            string min = nodes[0];
            foreach (string node in nodes) {
                if (scores[node] < scores[min]) {
                    min = node;
                }
            }
            return min;
        }

        /**
         * Encuentra los giros necesarios que se han de realizar para poder seguir el camino definido en la variable de clase path
         * */
        public List<int> whereToTurn(List<string> thePath) {
            List<int> theTurns = new List<int>();
            int sizePath = thePath.Count;
            theTurns.Add(this.world.myMech.faceSide);
            int newTurn=-1;
            for (int i = 0; i < sizePath - 1; i++) {
                newTurn = movement.whereFace(thePath[i], thePath[i + 1]);
                theTurns.Add(newTurn);
            }
            if (this.nWeapons == 0 && this.path.Count>0) { theTurns.Add(movement.whereFace(this.path[this.path.Count-1], this.world.theOthers[0].position)); }
            return theTurns;
        }

        /**
         * Una vez encontramos el camino a seguir, nos quedamos solo con la parte del mismo que pueda ser recorrida de una sola vez
         * */
        public void prune() {
            int movPoints = this.world.myMech.walk;
            List<string> newMoves = new List<string>();
            List<int> newTurns = new List<int>();
            if (this.path.Count == 0) { return; }
            newMoves.Add(this.path[0]);
            newTurns.Add(this.turns[0]);

            /*string lastNode = this.path[this.path.Count - 1];
            string rivalPosition = this.world.theOthers[0].position;
            // Si el último nodo es la posición de nuestro enemigo, la borramos
            if (lastNode == rivalPosition)
            {
                this.path.RemoveAt(this.path.Count - 1);
            } */

            int usedPoints = 0;
            int i = 1;
            
            while (usedPoints < movPoints && (i < this.turns.Count || i < this.path.Count))
            {
                //Console.Out.WriteLine("valor de i :" + i + " tam" + this.turns.Count);
                if (i < this.turns.Count)
                {
                    int turnCost = movement.sideDist(this.turns[i - 1], this.turns[i]);
                    if (usedPoints + turnCost <= movPoints)
                    {
                        usedPoints += turnCost;
                        //Console.Out.WriteLine("Inserto el giro " + i + "que vale " + turnCost + " y he gastao ya " + usedPoints+" de "+movPoints+" disponibles");
                        newTurns.Add(this.turns[i]);
                    }

                    else
                    {
                        break;
                    }
                }
                //Console.Out.WriteLine("valor de i :" + i + " tam" + this.path.Count);
                if (i < this.path.Count)
                {
                    int movCost = this.rawCost(this.path[i - 1], this.path[i]);
                    if(usedPoints + movCost <= movPoints)
                    {
                        usedPoints += movCost;
                        //Console.Out.WriteLine("Inserto el nodo " + i + "que vale " + movCost + " y he gastao ya " + usedPoints);
                        newMoves.Add(this.path[i]);
                    }
                    else
                    {
                        if (i > 1)
                        {
                            break;
                        }
                        else {
                            newMoves.Add(this.path[i]);
                            break;
                        }
                    }
                }
                i++;
            }

            

            this.path = newMoves;
            this.turns = newTurns;
        }
       
        /**
         * Encuentra el coste total de un camino
         * */
        public int pathCost(List<string> thePath, List<int> theTurns){
            //Si no hay camino, el coste es infinito, es decir no se puede ir
            if (thePath.Count == 0) {
                return int.MaxValue;
            }
            int movsCost = 0;
            int turnsCost = 0;
            for (int i = 1; i < thePath.Count; i++) {
                movsCost += this.rawCost(thePath[i - 1], thePath[i]);
            }
            for (int i = 1; i < theTurns.Count; i++)
            {
                movsCost += movement.sideDist(theTurns[i - 1], theTurns[i]);
            }
            return movsCost + turnsCost;
        }

        /**
         * True si se puede ir andando de start a goal
         * */
        public bool walkable(string start, string goal) {
            List<string> thePath = this.A_Start(start,goal);
            if (thePath.Count > 0)
            {
                return true;
            }

            else {
                return false;
            }
        }

        /**
         * Dada una lista de nodos nos devuelve aquellos a los que se pueda ir andando desde start
         * */
        public List<string> areWalkables(string start, List<string> candidates) {
            List<string> walkables = new List<string>();
            for (int i = 0; i < candidates.Count; i++) {
                if (this.walkable(start, candidates[i])) {
                    walkables.Add(candidates[i]);
                }
            }
            return walkables;
        }

        /**
         * Elige el mejor nodo cuando lo que queremos es atacar físicamente
         * Será aquel nodo adyacente al mech rival, que sea accesible y que este al mismo nivel que el mech rival
         * Si no puede ser al mismo nivel que solo haya uno de desnivel.
         * Si algo de lo anterior no se cumple, entonces no se puede atacar físicamente al rival cuando se encuentre en dicho nodo
         * 
         * */
        public string bestNodeForPhysicAttack() {
            string bestNode = this.world.myMech.position;
            string myPosition = this.world.myMech.position;
            string rivalPos = this.world.theOthers[0].position;
            int rivalLevel = this.world.land.nodes[rivalPos].nivel;
            List<string> candidates = this.world.land.neigs(rivalPos);
            
            if (this.world.myMech.jump == 0)
            {
                candidates = this.areWalkables(myPosition, candidates);
            }
            List<string> candidatesOfFirstLevel = this.world.land.onlyThisDiffLevel(0,candidates,rivalPos);
            List<string> candidatesOfSecondLevel = this.world.land.onlyThisDiffLevel(1, candidates, rivalPos);
            List<string> candidatesOfThirdLevel = this.world.land.onlyThisDiffLevel(-1, candidates, rivalPos);
            List<string> sortedCandidates = new List<string>();
            sortedCandidates.AddRange(candidatesOfFirstLevel);
            sortedCandidates.AddRange(candidatesOfSecondLevel);
            sortedCandidates.AddRange(candidatesOfThirdLevel);
            if (sortedCandidates.Count > 0)
            {
                return sortedCandidates[0];
            }
            else {
                return this.world.myMech.position;
            }
        }

        /**
         * Encuentra el nodo al que ir si se decide ir andando/corriendo
         * */
        public string whereWeGo() {
            //Cogemos la posición del rival, suponenemos que solo tenemos uno, en el caso de haber varios el que este primero será nuestro target
            string rivalPosition = this.world.theOthers[0].position;
            //Si no tenemos armas el mejor nodo para ir sera aquel que nos beneficie más la hora de inchar a ostias al otro mech
            if (this.nWeapons == 0) {
                return this.bestNodeForPhysicAttack();
            }
            bool found = false;
            string start = this.world.myMech.position;
            string goal = "";
            //Si hay camino directo
            if (this.A_Start(start, rivalPosition).Count > 0) {
                return rivalPosition;
            }
            int iter = 0;
            //Número hallado empíricamente
            int maxIter = 10;
            List<string> openList = new List<string>();
            openList.Add(rivalPosition);
            //Buscamos el nodo más cercano al enemigo que se pueda llegar a el andando/corriendo y se de menor coste
            while(!found && iter<maxIter){
                List<string> candidates = new List<string>();
                foreach (string node in openList) {
                    foreach (string neig in this.world.land.neigs(node)) {
                        if (!openList.Contains(neig)) {
                            candidates.Add(neig);
                        }
                    }
                }
               
                int min = int.MaxValue;
                foreach (string node in candidates) {
                    List<string> thePath = this.A_Start(start,node);
                    if (thePath.Count > 0) {
                        int cost = this.pathCost(thePath,this.whereToTurn(thePath));
                        if (cost < min) {
                            goal = node;
                            min = cost;
                        }
                    }
                }
                if (goal != "")
                {
                    found = true;
                    break;
                }
                else {
                    openList.AddRange(candidates);
                }
                iter++;
            }
            return goal;
        }

        /**
         * Decidimo el destino final
         * Calculamos el mejor nodo al cual se llega andando/corriendo 
         * y luego llamamos a una función que evalua qúe nodo es mejor, si el mejor de los que se llega andando
         * o el mejor de los que se llega saltando
         * */
        public void go() {
            string goal = this.whereWeGo();
            this.path = this.A_Start(this.world.myMech.position, goal);
            this.turns = this.whereToTurn(this.path);
            this.prune();
            this.decideIfJump();
        }

        public void printPath() {
            List<string> log = new List<string>();
            int max = Math.Max(this.turns.Count,this.path.Count);
            for (int i = 0; i < this.turns.Count; i++) {
                if(i<this.turns.Count)
                Console.Out.WriteLine("Lado: "+this.turns[i]);
                if (i < this.path.Count)
                    Console.Out.WriteLine("nodo: " + this.path[i]);
            }
        }

        /**
         * Escribe en el fichero accion.sbt los movimientos que hemos decidido realizar
         * */
        public void writeMovement() {
            this.printPath();
            string file = "accionJ" + this.world.player + ".sbt";
            int sizeMove = this.path.Count;
            int sizeTurns = this.turns.Count;
            //Lista que contiene los movimiento a realizar en 'preformato', 
            //es decir un formato intermedio entre el usado para representar internamente el camino
            //y el requerido por el fichero accionJx.sbt
            System.IO.StreamWriter actionFile = new System.IO.StreamWriter(file);
            if (this.world.myMech.stacked == true || this.myHeat >9 || !this.world.myMech.IHaveLegs()) {
                actionFile.WriteLine(this.allowedMovements[0]);
            }
            //Si estamos en el suelo nos intentamos levantar
            if (this.world.myMech.onGround == true)
            {

                actionFile.WriteLine(this.allowedMovements[1]);
                actionFile.WriteLine(this.world.myMech.position);
                actionFile.WriteLine(this.world.myMech.faceSide);
                actionFile.WriteLine("False");
                actionFile.WriteLine("1");
                actionFile.WriteLine("Levantarse");
                actionFile.WriteLine(this.world.myMech.faceSide);
                actionFile.Close();
                return;

            }
            //si hemos decido saltar escribimos el salto
            if (jump == true) {
               actionFile.WriteLine(this.allowedMovements[3]);
                actionFile.WriteLine(this.nodeToJump);
                actionFile.WriteLine(this.sideAtJump);
                actionFile.Close();
                return;
            }
            //Si nos movemos andando o corriendo
            if (jump == false && this.path.Count>0 && this.turns.Count>0)
            {
                if (this.path[sizeMove - 1] == this.world.myMech.position && this.turns[sizeTurns - 1] == this.world.myMech.faceSide)
                {
                    actionFile.WriteLine(this.allowedMovements[0]);
                    actionFile.Close();
                    return;
                }
                List<int> preFormatedMovement = new List<int>();
                for (int i = 0; i < this.turns.Count - 1; i++)
                {

                    preFormatedMovement.Add(utilities.circularDist(this.turns[i], this.turns[i + 1]));
                    if (this.turns[i] != this.turns[i + 1])
                    {
                        if (i != this.turns.Count - 2)
                        {
                            preFormatedMovement.Add(0);
                        }
                        else if (this.path.Count == this.turns.Count)
                        {
                            preFormatedMovement.Add(0);
                        }
                    }
                }

                List<KeyValuePair<string, int>> formatedMovement = new List<KeyValuePair<string, int>>();
                int index = 0;
                while (index < preFormatedMovement.Count)
                {
                    int item = preFormatedMovement[index];
                    if (item < 0)
                    {
                        formatedMovement.Add(new KeyValuePair<string, int>("Izquierda", Math.Abs(item)));
                        index++;
                    }
                    else if (item > 0)
                    {
                        formatedMovement.Add(new KeyValuePair<string, int>("Derecha", Math.Abs(item)));
                        index++;
                    }
                    else
                    {
                        int n = utilities.howManyElements(preFormatedMovement, 0, index);
                        formatedMovement.Add(new KeyValuePair<string, int>("Adelante", n));
                        index += n;
                    }

                }
                int size = formatedMovement.Count - 1;
                if (this.path[this.path.Count - 1] == this.world.theOthers[0].position && formatedMovement[size].Key == "Adelante")
                {
                    int value = formatedMovement[size].Value;
                    formatedMovement.RemoveAt(size);
                    value--;
                    if (value > 0)
                    {
                        formatedMovement.Add(new KeyValuePair<string, int>("Adelante", value));
                    }

                }
                actionFile.WriteLine(this.allowedMovements[1]);
                string goal = this.path[this.path.Count - 1];
                if (goal == this.world.theOthers[0].position)
                {
                    goal = this.path[this.path.Count - 2];
                }
                actionFile.WriteLine(goal);
                actionFile.WriteLine(this.turns[this.turns.Count - 1]);
                actionFile.WriteLine("False");
                int nSteps = formatedMovement.Count;
                actionFile.WriteLine(nSteps);
                for (int i = 0; i < nSteps; i++)
                {
                    actionFile.WriteLine(formatedMovement[i].Key);
                    actionFile.WriteLine(formatedMovement[i].Value);
                }
                actionFile.Close();
            }
        }
    }
}
