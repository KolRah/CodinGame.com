using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class NODE
{
    public bool isPasserelle {get;set;}
    public int NodeID {get;set;}
    public List<NODE> Nodes{get;set;}
    public NODE(int ID)
    {
        NodeID=ID;
        isPasserelle=false;
        Nodes=new List<NODE>();
    }
    public void CreateLink(NODE node)
    {
        Nodes.Add(node);
        node.Nodes.Add(this);
    }
    public void DeleteLink(NODE node)
    {
        Nodes.Remove(node);
        node.Nodes.Remove(this);
    }
    public void SetPasserelle()
    {
        isPasserelle=true;
    }
    public NODE ClosestNodeToPasserelle(ref List<NODE> sourcenode)
    {
        if (this.Nodes.Count()==0)
        {
            return null;
        } else {
            int LEN = sourcenode.Count();
            int excludenode=sourcenode[LEN-1].NodeID;
            List<NODE> myPasserelleNodes= this.Nodes.Where(o=>o.isPasserelle && o.NodeID!=excludenode).ToList();
            if (myPasserelleNodes.Count()==0)
            {
                List<int> tmpList = sourcenode.Select(o=>o.NodeID).ToList();
                foreach (NODE n in Nodes.Where(o=>!tmpList.Contains(o.NodeID)))
                {
                    Console.Error.WriteLine("deep:"+n.NodeID);
                    sourcenode.Add(n);
                    NODE res=n.ClosestNodeToPasserelle(ref sourcenode);
                    if (res==null)
                    {
                        sourcenode.Remove(n);
                    } else
                    {
                        return n;
                    }
                }
            } else
            {
                NODE nx=myPasserelleNodes[0];
                sourcenode.Add(nx);
                return nx;
            }
            
        }
        return null;
    }
}

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways
        
        Dictionary<int,NODE> Map=new Dictionary<int, NODE>();
        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            int N2 = int.Parse(inputs[1]);
            if (!Map.ContainsKey(N1))
            {
                NODE tmp = new NODE(N1);
                Map.Add(N1,tmp);
            }
            if (!Map.ContainsKey(N2))
            {
                NODE tmp = new NODE(N2);
                Map.Add(N2,tmp);
            }
            NODE A=Map[N1];
            NODE B=Map[N2];
            A.CreateLink(B);
        }
        for (int i = 0; i < E; i++)
        {
            int passid=int.Parse(Console.ReadLine());
            Map[passid].SetPasserelle(); // the index of a gateway node
        }
        
        List<NODE> vector=new List<NODE>();
        while (true)
        {
            NODE SINODE=Map[int.Parse(Console.ReadLine())];
            vector.Add(SINODE);
            NODE nn=SINODE.ClosestNodeToPasserelle(ref vector);
            SINODE.DeleteLink(vector[1]);
            Console.WriteLine(SINODE.NodeID+" "+vector[1].NodeID);
            vector.Clear();    
        }
    }
}
