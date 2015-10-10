using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : System.Object
{
    private Environment.Node startNode;
    private Environment.Node targetNode;
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closedList = new List<PathNode>();
    private bool foundTarget = false;

    public List<Vector2> path = new List<Vector2>(); //output

    public class PathNode : System.Object
    {
        public Environment.Node node;
        public PathNode parent;
        public int hValue;
        public int gValue;
        public int fValue()
        {
            return hValue + gValue;
        }
        public int movementCost (Coords from, Coords to)
        {
            return from.x == to.x || from.y == to.y ? 10 : 14;
        }
        public PathNode (Environment.Node node, PathNode parent, Coords target)
        {
            this.node = node;
            this.parent = parent;
            if (parent != null)
                this.gValue = movementCost(node.coords, parent.node.coords);
            else
                gValue = 0;
            hValue = Mathf.Abs(target.x - node.coords.x)+Mathf.Abs(target.y - node.coords.y);
        }
    }
    public Pathfinding (Vector2 from, Vector2 to, MonoBehaviour a)
    {
        startNode = Environment.Position2Node(from);
        targetNode = Environment.Position2Node(to);
        Start();
    }
    private void Start ()
    {
        
        PathNode startNodePath = new PathNode(startNode, null, targetNode.coords);
        openList.Add(startNodePath);
        while (!foundTarget)
        {
            PathNode picked = Pick();
            Set(picked);
        }
        Path(openList[openList.Count - 1]);
    }
    private void Set (PathNode node)
    {
        openList.Remove(node);
        closedList.Add(node);
        PathNode newNode;
        foreach (Coords node_nb in node.node.neighbours)
        {
            Environment.Node neighbour = Environment.instance[node_nb.x, node_nb.y];
            if (closedListContains(node_nb)!=-1) continue;
            int index = openListContains(node_nb); //add neighbours to open list
            if (index==-1)
            {
                newNode = new PathNode(neighbour, node, targetNode.coords);
                openList.Add(newNode);
                if (Equals(neighbour, targetNode)) { foundTarget = true; break; } //check if it is destination
            }
            else //repartent if needed
            {
                if (openList[index].gValue > node.gValue + node.movementCost(node.node.coords, openList[index].node.coords)) {
                    newNode = new PathNode(openList[index].node, node, targetNode.coords);
                    openList[index] = newNode;
                }
            }
        }
    }
    private PathNode Pick ()
    {
        int lowestFCost = 1000;
        int value=0;
        for(int i = 0; i < openList.Count; i++)
        {
            int fValue = openList[i].fValue();
            if (fValue < lowestFCost)
            {
                lowestFCost = fValue;
                value = i;
            }
        }
        return openList[value];
    }
    private void Path (PathNode targetNode)
    {
        openList.Remove(targetNode);
        closedList.Add(targetNode);
        PathNode current = targetNode;
        while (current!=null)
        {
            path.Add(current.node.worldLocation);
            current = current.parent;
        }
    }
    //TOOLS______________________________________
    private int openListContains (Coords cords)
    {
        int i = 0;
        foreach (PathNode node in openList)
        {
            if (node.node.coords.x == cords.x && node.node.coords.y == cords.y) return i;
            i++;
        }
        return -1;
    }
    private int closedListContains(Coords cords)
    {
        int i = 0;
        foreach (PathNode node in closedList)
        {
            if (node.node.coords.x == cords.x && node.node.coords.y == cords.y) return i;
            i++;
        }
        return -1;
    }
    private bool Equals (Environment.Node a, Environment.Node b)
    {
        return a.coords.x == b.coords.x && a.coords.y == b.coords.y;
    }
    //TOOLS______________________________________
}