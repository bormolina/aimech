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
