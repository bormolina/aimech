using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaICO
{
    class Test
    {
        
        public static void printMechs(environment e) {
            e.myMech.print();
            for (int i = 0; i < e.theOthers.Count; i++) {
                e.theOthers[i].print();
            }
        }

        public static void testNeigs(environment e) {
            /*utilities.printList(e.land.frontNeigs("0110", 2),"0110");
            utilities.printList(e.land.frontNeigs("0117", 3), "0117");
            utilities.printList(e.land.frontNeigs("0115", 6), "0115");
            utilities.printList(e.land.frontNeigs("0314", 2), "0314");
            utilities.printList(e.land.frontNeigs("0315", 1), "0315");
            utilities.printList(e.land.frontNeigs("0501", 1), "0501");
            utilities.printList(e.land.frontNeigs("0601", 1), "0601");

            utilities.printList(e.land.frontNeigs("0707", 2), "0707");
            utilities.printList(e.land.frontNeigs("0808", 6), "0808");
            utilities.printList(e.land.frontNeigs("1501", 2), "1501");
            utilities.printList(e.land.frontNeigs("1507", 5), "1507");
            utilities.printList(e.land.frontNeigs("1509", 2), "1509");
            utilities.printList(e.land.frontNeigs("1517", 3), "1517");
            utilities.printList(e.land.frontNeigs("1317", 3), "1317");*/
            string node;
            int side;
            //par 1
            node = "1010";
            Console.Out.WriteLine(e.land.isInFrontAngle(node, 1).Contains("0608"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, 1).Contains("0502"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, 1).Contains("1111"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, 1).Contains("0512"));
            Console.Out.WriteLine("----");

            //par 2
            node = "0216";
            side = 2;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0417"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1311"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0117"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0108"));
            Console.Out.WriteLine("----");
            
            //par 3
            Console.Out.WriteLine(e.land.isInFrontAngle("0812", 3).Contains("1509"));
            Console.Out.WriteLine(e.land.isInFrontAngle("0812", 3).Contains("1215"));
            Console.Out.WriteLine(e.land.isInFrontAngle("0812", 3).Contains("0812"));
            Console.Out.WriteLine(e.land.isInFrontAngle("0812", 3).Contains("1508"));
            Console.Out.WriteLine("----");

            //par 4
            node = "0101";
            side = 4;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0102"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1517"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0301"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1004"));
            Console.Out.WriteLine("----");

            //par 5
            node = "0808";
            side = 5;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0814"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0213"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0104"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1205"));
            Console.Out.WriteLine("----");

            //par 6
            node = "0403";
            side = 6;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0401"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0103"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0906"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0106"));
            Console.Out.WriteLine("----");

            //impar 1
            node = "0717";
            side = 1;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1413"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0204"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0817"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0216"));
            Console.Out.WriteLine("----");

            //impar 2
            node = "0911";
            side = 2;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1514"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1004"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0805"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1415"));
            Console.Out.WriteLine("----");

            //impar 3
            node = "1416";
            side = 3;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1516"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1517"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1317"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1017"));
            Console.Out.WriteLine("----");

            //impar 4
            node = "0510";
            side = 4;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0614"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0112"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0508"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1413"));
            Console.Out.WriteLine("----"+node);

            //impar 5
            node = "0304";
            side = 5;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0204"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0110"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0102"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0414"));
            Console.Out.WriteLine("----" + node);

            //impar 6
            node = "1104";
            side = 6;
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0507"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0404"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("1109"));
            Console.Out.WriteLine(e.land.isInFrontAngle(node, side).Contains("0110"));
            Console.Out.WriteLine("----" + node);
        }

        public static void testAngles(environment e){
            weaponsAttack w = new weaponsAttack(e);
            Console.Out.WriteLine("heigth is " + e.land.height);
            Console.Out.WriteLine("w is " + e.land.width);
            int rows = e.land.height;
            int cols = e.land.width;
            for (int r = 1; r <= 15; r++) {
                for (int c = 1; c <= 17; c++) {
                    string node = utilities.numbersToCoordinates(r,c);
                    Console.Out.WriteLine("For node " + node + " angle is: " + w.getAngle(node));
                }
                Console.Out.WriteLine("#####");
            }
        }
    }
}
