using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/

public class Matrix
{
    public List<char[]> matrix {get;set;}
    public int width=0;
    public int height=0;
    public Matrix(int _width,int _height)
    {
        matrix= new List<char[]>();
        width=_width;
        height=_height;
    }
    public string closestX(int X, int Y)
    {
        if (X<width-1)
        {
            for (int j=X+1;j<width;j++)
            {
                if (matrix[Y][j]=='0')
                {
                    //node found
                    return j.ToString();
                }
            }
        } else
        {
            if (matrix[Y][width-1]=='0')
                {
                    //node found
                    return "-1";
                }
        }
        return "-1";
    }
    public string closestY(int X, int Y)
    {
        if (Y<height-1){
            for (int i=Y+1;i<height;i++)
            {
                if (matrix[i][X]=='0')
                {
                    //node found
                    return i.ToString();
                }
            }
        } else {
            if (matrix[height-1][X]=='0')
            {
                //node found
                return "-1";
            }
        }
        return "-1";
    }
}
public class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        Matrix m=new Matrix(width,height);
        
        for (int i = 0; i < height; i++)
        {
            string nextline = Console.ReadLine(); // width characters, each either 0 or .
            m.matrix.Add(nextline.ToCharArray());
        }
        
        string result="";
        for (int i = 0; i < height; i++)
        {
            for (int j=0;j<width;j++)
            {
                if (m.matrix[i][j]=='0')
                {
                    //node found
                    string current=j+" "+i;
                    string closetX=m.closestX(j,i);
                    string closetY=m.closestY(j,i);
                    string right="-1 -1";
                    if (closetX!="-1")
                    {
                        right=closetX+" "+i;
                    }
                    string bottom="-1 -1";
                    if (closetY!="-1")
                    {
                        bottom=j+" "+closetY;
                    }
                    result=current+" "+right+" "+bottom;
                    Console.WriteLine(result);
                }
            }
        }
    }
}
