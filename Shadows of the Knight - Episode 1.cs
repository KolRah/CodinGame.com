using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int W = int.Parse(inputs[0]); // width of the building.
        int H = int.Parse(inputs[1]); // height of the building.
        int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
        inputs = Console.ReadLine().Split(' ');
        int X0 = int.Parse(inputs[0]);
        int Y0 = int.Parse(inputs[1]);
        int X = X0;
        int Y=Y0;

        int lastPosX=X0;
        int lastPosY=Y0;
        int lastSquareXmax = W-1;  //(int)Math.Round((W-1-X0)*0.8,0)
        int lastSquareXmin = 0;
        int lastSquareYmax = H-1;
        int lastSquareYmin = 0;
        
        while (true)
        {
            string bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
            
            int squareXmin=(bombDir.Contains("L")?lastSquareXmin:0)+(bombDir.Contains("R")?X+1:0);
            int squareXmax=(bombDir.Contains("L")?X-1:0)+(bombDir.Contains("R")?lastSquareXmax:0);
            int squareYmin=(bombDir.Contains("U")?lastSquareYmin:0)+(bombDir.Contains("D")?Y+1:0);
            int squareYmax=(bombDir.Contains("U")?Y-1:0)+(bombDir.Contains("D")?lastSquareYmax:0);
            
            int squaremiddleX = (int)Math.Floor((squareXmax-squareXmin)*0.5);
            int squaremiddleY = (int)Math.Floor((squareYmax-squareYmin)*0.5);
            Console.Error.WriteLine("squaremiddleX:"+squaremiddleX+":squaremiddleY:"+squaremiddleY);

            X+= (bombDir.Contains("R")?+squaremiddleX+1:0) +(bombDir.Contains("L")?-squaremiddleX-1:0);
            Y+= (bombDir.Contains("U")?-squaremiddleY-1:0) + (bombDir.Contains("D")?+squaremiddleY+1:0);

            lastSquareXmin=squareXmin;
            lastSquareXmax=squareXmax;
            lastSquareYmin=squareYmin;
            lastSquareYmax=squareYmax;

            // the location of the next window Batman should jump to.
            Console.WriteLine(X+" "+Y);
        }
    }
}
