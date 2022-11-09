using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Save humans, destroy zombies!
 **/
 public class point
 {
    public int X {get;set;}
    public int Y {get;set;}
    public  point(){X=0;Y=0;}
    public  point(int x, int y){X=x;Y=y;}
 }
 public class vector
 {
    public point Source{get;set;}
    public point Target{get;set;}

    public int X{get;set;}
    public int Y{get;set;}
    public double Length{get;set;}

    public  vector(point source, point target)
    {
        Source=source;
        Target=target;
        X=Target.X-Source.X;
        Y=Target.Y-Source.Y;
        Length = Math.Sqrt(Math.Pow(Target.Y-Source.Y,2)+Math.Pow(Target.X-Source.X,2));
    }

    public bool IsColinearTo(vector V2)
    {
        double crossProduct = X*V2.Y-Y*V2.X;
        if(crossProduct==0){return true;}else{return false;}

    }
 }
 public class Zombie
 {
    public string Name {get;set;}
    public point Self {get;set;}
    public point SelfNext {get;set;}
    public int Speed {get;set;}
    public Zombie(string name,point coordinates, point next_coordinates)
    {
        Name=name;
        Self=coordinates;
        SelfNext=next_coordinates;
        Speed=400;
    }
 }
 public class Human
 {
    public string Name {get;set;}
    public point Self {get;set;}
    public point Ash {get;set;}
    public int Speed {get;set;}

    public double EatableIn {get;set;}
    public double ReachableIn {get;set;}
    public bool Helpable{get;set;}

    public double ThreatWeight{get;set;}
    private List<Zombie> ZombieList {get;set;}
    public Zombie ClosestZombie {get;set;}

    public Human(string name, point coordinates, point ash_coordinates, List<Zombie> zombielist)
    {
        Name=name;
        Self=coordinates;
        Ash=ash_coordinates;
        Speed=0;
        Helpable=false;
        ZombieList=TargetingZombies(zombielist);
        //Console.Error.WriteLine("ThreatWeight:"+ ThreatWeight);
        Dictionary<Zombie,double> ThreatList = new Dictionary<Zombie,double>();
        double minvalue=100000;
        ClosestZombie=null;
        ThreatWeight=0;
        foreach (Zombie z in ZombieList)
        {
            vector tmpZtoH=new vector(z.Self,Self);
            vector tmpZtoHNext=new vector(z.SelfNext,Self);
            double currentDistance=tmpZtoH.Length;
            double nextTurnDistance=tmpZtoHNext.Length;
            double val=Math.Round(nextTurnDistance/z.Speed,0)+1;
            if (val<minvalue){ minvalue=val;ClosestZombie=z;}
            ThreatList.Add(z,val);
            ThreatWeight=ThreatWeight+val;
            //Console.Error.WriteLine(Name+"<--"+ z.Name+"/"+val);
        }
        if (ThreatList.Count==0)
        {
            foreach (Zombie z in zombielist)
            {
                vector tmpZtoH=new vector(z.Self,Self);
                vector tmpZtoHNext=new vector(z.SelfNext,Self);
                double currentDistance=tmpZtoH.Length;
                double nextTurnDistance=tmpZtoHNext.Length;
                double val=Math.Round(nextTurnDistance/z.Speed,0)+1;
                ThreatWeight=ThreatWeight+val;
                if (val<minvalue){ minvalue=val;ClosestZombie=z;}
                ThreatList.Add(z,val);
                //Console.Error.WriteLine(Name+"<--"+ z.Name+"/"+val);
            }
        }
        EatableIn=minvalue;
        //Console.Error.WriteLine("EatableIn:"+ minvalue);
        ReachableIn=(new vector(Ash,ClosestZombie.Self).Length-1000)/1000-1;
        //Console.Error.WriteLine("ReachableIn:"+ ReachableIn);
        if (ReachableIn<=EatableIn){Helpable = true;}
        //Console.Error.WriteLine("Helpable:"+ Helpable);
    }
     
    private List<Zombie> TargetingZombies(List<Zombie> zombielist)
    {
        List<Zombie> VList = new List<Zombie>();
        foreach (Zombie z in zombielist)
        {
            vector tmpZtoH=new vector(z.Self,Self);
            vector tmpZombieTrajectory = new vector(z.SelfNext, z.Self);
            if (tmpZtoH.IsColinearTo(tmpZombieTrajectory))
            {
                VList.Add(z);
            }
        }
        return VList;
    }
 }
 
 
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);
            int humanCount = int.Parse(Console.ReadLine());
            point Ash=new point(x,y);

            List<point> HumanCoordinates = new  List<point>();
            List<Human> Humans = new List<Human>();
            for (int i = 0; i < humanCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int humanId = int.Parse(inputs[0]);
                int humanX = int.Parse(inputs[1]);
                int humanY = int.Parse(inputs[2]);
                point HumanLoc=new point(humanX,humanY);
                HumanCoordinates.Add(HumanLoc);
            }
            
            int zombieCount = int.Parse(Console.ReadLine());
            List<Zombie> ZombiesList = new  List<Zombie>();
            for (int i = 0; i < zombieCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int zombieId = int.Parse(inputs[0]);
                int zombieX = int.Parse(inputs[1]);
                int zombieY = int.Parse(inputs[2]);
                point Zombie=new point(zombieX,zombieY);
                
                int zombieXNext = int.Parse(inputs[3]);
                int zombieYNext = int.Parse(inputs[4]);
                point ZombieNext=new point(zombieXNext,zombieYNext);
                
                Zombie z= new Zombie(zombieId.ToString(),Zombie,ZombieNext);
                ZombiesList.Add(z);
            }
            for (int j = 0; j < humanCount; j++)
            {
                Human h=new Human("H"+j,HumanCoordinates[j],Ash,ZombiesList);
                Humans.Add(h);
            }
            List<Human> HelpableHumans = new List<Human>();
            HelpableHumans = Humans.Where(o=>o.Helpable).ToList();
            //Console.Error.WriteLine("HelpableHumans:"+ HelpableHumans.Count());
            double MaxThreat = HelpableHumans.Max(o=>o.ThreatWeight);
            Human Target = HelpableHumans.Where(o=>o.ThreatWeight==MaxThreat).FirstOrDefault();
            point MoveTarget=new point(Target.ClosestZombie.SelfNext.X,Target.ClosestZombie.SelfNext.Y);
            Console.WriteLine(MoveTarget.X+" "+MoveTarget.Y); // Your destination coordinates
        }
    }
}
