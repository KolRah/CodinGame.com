using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


 public class CheckPoint
 {
    public Point Coordinates {get;set;}
    public int ID {get;set;}
    public CheckPoint(int id, Point coords)
    {
        ID=id;
        Coordinates=coords;
    }
    public override string ToString()
    {
        return "CheckPoint :"+ID+" "+Coordinates.ToString();
    }
 }
 public class Point
 {
    public int X {get;set;}
    public int Y {get;set;}
    public Point(int x, int y)
    {
        X=x;
        Y=y;
    }
    public Point()
    {
        X=0;
        Y=0;
    }
    public override string ToString()
    {
        return "Point: ("+X+","+Y+")";
    }
    public static Point operator -(Point A, Point B)
    {
        return new Point(A.X-B.X,A.Y-B.Y);
    } 
    public static Point operator +(Point A, Point B)
    {
        return new Point(A.X+B.X,A.Y+B.Y);
    } 
    public static Point operator /(Point A, int DIV)
    {
        return new Point(A.X/DIV,A.Y/DIV);
    } 
    public static Point operator *(Point A, int MULT)
    {
        return new Point(A.X*MULT,A.Y*MULT);
    } 
 }
 public class Vector
 {
    public int X {get;set;}
    public int Y {get;set;}
    public Vector(Point s, Point t)
    {
        X=t.X-s.X;
        Y=t.Y-s.Y;
    }
    public Vector(int x, int y)
    {
        X=x;
        Y=y;
    }
    public override string ToString()
    {
        return "Vector: ("+(X)+","+(Y)+")";
    }
    public double Norme()
    {
        return Math.Sqrt(X*X+Y*Y);
    }
    public static Vector operator /(Vector A, int DIV)
    {
        return new Vector(A.X/DIV,A.Y/DIV);
    } 
    public static Vector operator /(Vector A, double DIV)
    {
        return new Vector((int)Math.Round(A.X/DIV,0),(int)Math.Round(A.Y/DIV,0));
    } 
    public static Vector operator *(Vector A, int MULT)
    {
        return new Vector(A.X*MULT,A.Y*MULT);
    } 
    public static Vector operator *(Vector A, double MULT)
    {
        return new Vector((int)Math.Round(A.X*MULT,0),(int)Math.Round(A.Y*MULT,0));
    } 
    public static Vector operator +(Vector A, Vector B)
    {
        return new Vector(A.X+B.X,A.Y+B.Y);
    } 
    public static Point operator +(Point A, Vector B)
    {
        return new Point(A.X+B.X,A.Y+B.Y);
    }
    public static Point operator -(Point A, Vector B)
    {
        return new Point(A.X-B.X,A.Y-B.Y);
    }  
    public static Vector operator -(Vector A, Vector B)
    {
        return new Vector(A.X-B.X,A.Y-B.Y);
    } 
    public static Vector operator -(Vector A)
    {
        return new Vector(-A.X,-A.Y);
    }
 }
 public class POD
 {
    public Point Coordinates{get;set;}
    public Point NextCoordinates{get;set;}
    public Point PreviousCoordinates{get;set;}
    public double Speed {get;set;}
    public POD(Point coords)
    {
        Coordinates =coords;
    }
    public void UpdateData(Point target)
    {
        Point Delta = PreviousCoordinates-Coordinates;
    }
 }
class Player
{
    
    static void Main(string[] args)
    {
        string[] inputs;
        Dictionary<int, CheckPoint> CHK = new Dictionary<int, CheckPoint>();
        Point MapCenter=new Point(8000,4500);
        int MAXCHECKPOINTS=0;
        int BoostRun=0;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);

            int nextCheckpointX = int.Parse(inputs[2]); // x position of the next check point
            int nextCheckpointY = int.Parse(inputs[3]); // y position of the next check point
            
            int nextID=1;
            if (CHK.Keys.Count()==0)
            {
                Console.Error.WriteLine("adding CheckPoint:"+nextID);
                // first
                CHK.Add(1, new CheckPoint(1,new Point(nextCheckpointX,nextCheckpointY)));
            } else
            {
                // try to find the same one
                if (CHK.Values.Any(o=>o.Coordinates.X==nextCheckpointX && o.Coordinates.Y==nextCheckpointY ))
                {
                    // we already have it
                    
                    nextID=CHK.Values.Where(o=>o.Coordinates.X==nextCheckpointX && o.Coordinates.Y==nextCheckpointY ).Select(o=>o.ID).First();
                    //Console.Error.WriteLine("exiting CheckPoint:"+nextID);
                } else
                {
                    // add it
                    int maxindex = CHK.Values.Max(o=>o.ID)+1;
                    //Console.Error.WriteLine("adding CheckPoint:"+maxindex);
                    CHK.Add(maxindex, new CheckPoint(maxindex,new Point(nextCheckpointX,nextCheckpointY)));
                }
            }
            Console.Error.WriteLine(CHK.Count());
            MAXCHECKPOINTS = 4;
            int nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
            int nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
            inputs = Console.ReadLine().Split(' ');
            int opponentX = int.Parse(inputs[0]);
            int opponentY = int.Parse(inputs[1]);

            
            Point Barycentre=new Point(0,0);
            Vector AB=null;
            Vector PERPEND=null;
            Vector BC=null;
            Vector CA=null;
            Vector TRAJ=null;
            int Max=0;
            int Min=0;
            
            if (CHK.Count()==MAXCHECKPOINTS)
            {
                //this is to find the vector tangent to the circle from Barycentre
                int PointB=nextID%MAXCHECKPOINTS==0?1:nextID+1;
                int PointC=PointB%MAXCHECKPOINTS==0?1:PointB+1;
                Point OppositeMiddle = (CHK[PointB].Coordinates+CHK[PointC].Coordinates)/2;
                Barycentre=(CHK[nextID].Coordinates+OppositeMiddle)*2/3;
                Vector GA=new Vector(CHK[nextID].Coordinates,Barycentre);
                PERPEND = new Vector(-GA.Y,GA.X);
                PERPEND = PERPEND*Min/(PERPEND.Norme());
                TRAJ=new Vector(new Point(x,y),CHK[nextID].Coordinates);
                
                // current targeted checkpoint A to next checkpoint B (C is the one after B)
                // Min = shortest distance in the triangle ABC
                // Max = largest distance in the triangle ABC
                 AB=new Vector(CHK[nextID].Coordinates,CHK[PointB].Coordinates);
                 Max=(int)Math.Round(AB.Norme(),0);
                 Min=Max;
                 BC=new Vector(CHK[PointB].Coordinates,CHK[PointC].Coordinates);
                 Max=BC.Norme()>Max?(int)Math.Round(BC.Norme(),0):Max;
                 Min=BC.Norme()<Min?(int)Math.Round(BC.Norme(),0):Min;
                 CA=new Vector(CHK[PointC].Coordinates,CHK[nextID].Coordinates);
                 Max=CA.Norme()>Max?(int)Math.Round(CA.Norme()):Max;
                 Min=CA.Norme()<Min?(int)Math.Round(CA.Norme(),0):Min;

                 //Console.Error.WriteLine("CURRENT POSITION: "+new Point(x,y).ToString());
                 //Console.Error.WriteLine("NEXT CHECKPOINT"+CHK[nextID].ToString());
                 //Console.Error.WriteLine("TRAJ "+TRAJ.ToString());
                 //Console.Error.WriteLine("PERPEND "+PERPEND.ToString());
            }
            
            int SoftThrottle = 50;
            int SoftDistance = 1200;
            int StopDistance = 600;
            int BoostAngle = 4;
            int DistanceForBoost=7000;
            if (Barycentre.X!=0)
            {
                // all checkpoints known, optimizing trajectory
                Point NextTarget = new Point();
                int Throttle=100;
                
                if (nextCheckpointDist<SoftDistance )
                {
                    // close to the checkpoint, target it
                    NextTarget=CHK[nextID].Coordinates;
                    Throttle=SoftThrottle;
                } else if (nextCheckpointDist<StopDistance )
                {
                    // very close to the checkpoint, target it
                    NextTarget=CHK[nextID].Coordinates;
                    Throttle=0;
                }  else
                {
                    // far from the checkpoint, target the soft turning location
                    NextTarget=CHK[nextID].Coordinates+(PERPEND/Min-(TRAJ/TRAJ.Norme())*StopDistance/2);
                }
                string Outtt="";
                //Console.Error.WriteLine("nextCheckpointAngle "+nextCheckpointAngle.ToString());
                if (BoostRun==0 && 
                    nextCheckpointAngle<BoostAngle && 
                    nextCheckpointAngle>-BoostAngle && 
                    TRAJ.Norme()>DistanceForBoost)
                {
                    Outtt="BOOST";
                    BoostRun=1;
                } else
                {
                    Outtt=Throttle.ToString();
                }
                Console.WriteLine((Outtt=="BOOST"?nextCheckpointX:NextTarget.X)+ " " 
                    + (Outtt=="BOOST"?nextCheckpointY:NextTarget.Y) + " " 
                    + Outtt) ;
            } else
            {
                // all checkpoints not yet known, "normal" behaviour
                int Throttle = 100;
                Throttle = 
                    (CHK.Count()>1 && (nextCheckpointAngle<=90 && nextCheckpointAngle<-90) 
                    ?0:100);
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + Throttle );
            }
        }
    }
}
