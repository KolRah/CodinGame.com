using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/

class Player
{
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int lightX = int.Parse(inputs[0]); // the X position of the light of power
        int lightY = int.Parse(inputs[1]); // the Y position of the light of power
        int initialTx = int.Parse(inputs[2]); // Thor's starting X position
        int initialTy = int.Parse(inputs[3]); // Thor's starting Y position

        // game loop
        
        while (true)
        {
            int[] v = {lightX-initialTx,lightY-initialTy};
            int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.
            string res="";
            if (v[1]<0)
            {
                res="N";
                initialTy=initialTy-1;
                if (v[0]<0)
                {
                    res=res+"W";
                    initialTx=initialTx-1;
                } else if (v[0]>0)
                {
                    res=res+"E";
                    initialTx=initialTx+1;
                } 
            }
            else if (v[1]>0)
            {
                res="S";
                initialTy=initialTy+1;
                if (v[0]<0)
                {
                    res=res+"W";
                    initialTx=initialTx-1;
                } else if (v[0]>0)
                {
                    initialTx=initialTx+1;
                    res=res+"E";
                }
            }
            else
            {
                if (v[0]<0)
                {
                    res=res+"W";
                    initialTx=initialTx-1;
                } else if (v[0]>0)
                {
                    res=res+"E";
                    initialTx=initialTx+1;
                }
            }
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
          

            // A single line providing the move to be made: N NE E SE S SW W or NW
            Console.WriteLine(res);
        }
    }
}
