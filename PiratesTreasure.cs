using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Solution
{
    static void Main(string[] args)
    {
        int W = int.Parse(Console.ReadLine());
        int H = int.Parse(Console.ReadLine());
        int[,] Map = new int[H,W];
        for (int y = 0; y < H; y++)
        {
            int[] splitt=Console.ReadLine().Split(' ').AsQueryable().Select(o=>int.Parse(o.ToString())).ToArray();
            for (int x=0;x<W;x++)
            {
                Map[y,x]=splitt[x];
            }
        }
        for (int y=0; y < H; y++)
        {
            for (int x=0;x<W;x++)
            {
                //Console.Error.Write(Map[y,x]+" ");
                if (Map[y,x]==0 && SearchAround(ref Map, y,x))
                {
                    Console.WriteLine(x+" "+y);
                }
                //if (x==W-1)
                //    Console.Error.WriteLine(System.Environment.NewLine);
            }
        }
    }
    public static bool SearchAround(ref int[,] Map, int y,int x)
    {
        int sum=0;
        int excount=0;
        //Console.Error.WriteLine("searching y="+y+" x="+x);
        try{ sum=sum+Map[y-1,x-1];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y-1,x];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y-1,x+1];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y,x-1];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y,x+1];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y+1,x-1];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y+1,x];} catch (Exception e){excount++;};
        try{ sum=sum+Map[y+1,x+1];} catch (Exception e){excount++;};
        
        if (sum==3 && excount==5)
                    return true;
        else if (sum==5 && excount==3)
                    return true;
        else if (sum==8 && excount==0)
                    return true;
        else
                    return false;
    }
}
