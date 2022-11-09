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
 public class Clone
 {
    public int ID {get;set;}
    public int Floor {get;set;}
    public int State {get;set;}     // Wait 0, block 1
    public int Position {get;set;}
    public int Direction {get;set;}  // 0 left, 1 right
    public bool isLead {get;set;}

    public Clone(int id,int floor, string direction, int position)
    {
        ID=id;
        Floor=floor;
        Position=position;
        Direction = direction=="LEFT"?-1:1;
        isLead=false;
    }
    public  override string ToString()
    {
        return "ID:"+ID+" -Floor:"+Floor+" -State:"+State+" -Position:"+Position
            +" -Direction:"+Direction+" -isLead:"+isLead;
    }
 }
 public class Elevator
 {
    public int ID {get;set;}
    public int Floor {get;set;}
    public int Position {get;set;}

    public Elevator(int id,int floor, int position)
    {
        ID=id;
        Floor=floor;
        Position=position;
    }
 }
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int nbFloors = int.Parse(inputs[0]); // number of floors
        int width = int.Parse(inputs[1]); // width of the area
        int nbRounds = int.Parse(inputs[2]); // maximum number of rounds
        int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        int exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        int nbTotalClones = int.Parse(inputs[5]); // number of generated clones
        int nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
        int nbElevators = int.Parse(inputs[7]); // number of elevators
        List<Elevator> ELEVATORS = new List<Elevator>();
        for (int i = 0; i < nbElevators; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            int elevatorPos = int.Parse(inputs[1]); // position of the elevator on its floor
            Elevator E = new Elevator(i,elevatorFloor,elevatorPos);
            ELEVATORS.Add(E);
        }
        Elevator finalE = new Elevator(nbElevators,exitFloor,exitPos);
        ELEVATORS.Add(finalE);
        int CurrentTurn=-1;
        int StartFloor=-1;
        int StartPosition=-1;
        int StartDirection=-1;
        int lastCloneID=-1;
        // game loop
        List<Clone> CLONES = new List<Clone>();
        while (true)
        {
            CurrentTurn++;
            inputs = Console.ReadLine().Split(' ');
            int cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
            int clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
            string direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT
            
            if (CurrentTurn==0)
            {
                lastCloneID=-1;
                StartFloor=cloneFloor;
                StartPosition=clonePos;
                StartDirection=direction=="LEFT"?-1:1;
            }
            if (CurrentTurn%3==0)
            {
                lastCloneID++;
                Clone NewClone = new Clone(lastCloneID,StartFloor,direction,StartPosition);
                if (CurrentTurn==0)
                {
                    NewClone.isLead=true;
                    NewClone.State=0;
                } else
                {
                    bool IsThereALead = CLONES.Any(o=>o.isLead==true);
                    if (!IsThereALead)
                    {
                        NewClone.isLead=true;
                        NewClone.State=0;
                    }
                }
                CLONES.Add(NewClone);
            }
            if (!CLONES.Any(o=>o.isLead))
            {   
                // case when the lead is blocked and no other clone is yet on the map
                Console.WriteLine("WAIT"); 
            } else 
            {
                Clone LEAD = CLONES.Where(o=>o.isLead).First();
                LEAD.Position=clonePos;
                LEAD.Direction=(direction=="LEFT"?-1:1);
                LEAD.Floor=cloneFloor;
                Elevator FLOOR_ELEVATOR = ELEVATORS.Where(o=>o.Floor==LEAD.Floor).FirstOrDefault();
                
                if (FLOOR_ELEVATOR.Position-LEAD.Position<0) {
                    // should go left
                    if (LEAD.Direction<0)
                    {
                        Console.WriteLine("WAIT"); 
                    }
                    else
                    {
                        if (LEAD.Position>=StartPosition && LEAD.Position<width-1 && LEAD.Position>0 )
                        {
                            LEAD.State=1;
                            LEAD.isLead=false;
                            Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
                        } else 
                        {
                            Console.WriteLine("WAIT"); 
                        }
                    }
                } else if (FLOOR_ELEVATOR.Position-LEAD.Position>0)
                {
                    // should go right
                    if (LEAD.Direction>0)
                    {
                        Console.WriteLine("WAIT"); 
                    }
                    else
                    {
                        if (LEAD.Position!=StartPosition && LEAD.Position<width-1 && LEAD.Position>0 )
                        {
                            LEAD.State=1;
                            LEAD.isLead=false;
                            Console.WriteLine("BLOCK"); // action: WAIT or BLOCK
                        } else 
                        {
                            Console.WriteLine("WAIT"); 
                        }
                    }
                } else
                {
                    // should wait
                    Console.WriteLine("WAIT"); 
                    
                }
            }
        }
    }
}
