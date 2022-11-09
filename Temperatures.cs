using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
        string inputs = Console.ReadLine();
        Console.Error.WriteLine(inputs);
        if (inputs==String.Empty)
        {
            Console.WriteLine("0");
        } else 
        {
            string[] input=inputs.Split(' ');
            int[] listpos=null;
            int[] listneg=null;
            int? minposvalue=null;
            int? minnegvalue=null;
            listpos=input.Select(o=>int.Parse(o)).Where(p=>p>=0).ToArray();
            if (listpos.Length>0){
                minposvalue=listpos.Min();
                Console.Error.WriteLine("*MINPOS"+minposvalue+"*");
            } else{}
            listneg=input.Select(o=>int.Parse(o)).Where(p=>p<0).Select(o=>-o).ToArray();
            if (listneg.Length>0){
                minnegvalue=listneg.Min();
                Console.Error.WriteLine("*MINNEG"+minnegvalue+"*");
            }
            
            if (listpos.Length<=0){Console.WriteLine(-minnegvalue);}
            else if (listneg.Length<=0){Console.WriteLine(minposvalue);} else {
            Console.WriteLine(minnegvalue>=minposvalue?minposvalue:-minnegvalue);}
        }
    }
}
