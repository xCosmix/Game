using UnityEngine;
using System.Collections.Generic;
using System;

public class GridBuilder : MonoBehaviour {
    public Sprite[] terrainSprites;
    private List<GameObject> terrain = new List<GameObject>();
    public static float tileSize;
    public bool debug;
	// Use this for initialization
	void Start () {
        SetUp();
        if (debug) show();
	}
    private void SetUp ()
    {
        Rect rect = terrainSprites[0].rect;
        float size = rect.width / 100.0f;
        tileSize = size;
        SpriteRenderer[] sceneSprites = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer spr in sceneSprites)
        {
            int value = containsAt(terrainSprites, spr.sprite);
            if (value == -1) continue;
            terrain.Add(spr.gameObject);
        }
        Environment.SetUp(terrain);
    }
    private int containsAt (Sprite[] list, Sprite sprite)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == sprite) return i;
        }
        return -1;
    }
    private void show ()
    {
        foreach (Environment.Node nd in Environment.instance)
        {
            GameObject d;
            try {
                d = new GameObject("node: " + nd.coords.x + "," + nd.coords.y);
            } catch (NullReferenceException e) { continue; }
            d.transform.position = nd.worldLocation;
            byte x = 0;
            foreach (Coords nbCoords in nd.neighbours)
            {
                Environment.Node nb = Environment.instance[nbCoords.x, nbCoords.y];
                var c = new GameObject("nb: " + nb.coords.x+","+nb.coords.y +" dir: "+ nd.direction[x]);
                c.transform.position = nb.worldLocation;
                c.transform.SetParent(d.transform);
                x++;
            }
        }
    }
}
public static class Environment : System.Object {

    public static Node[,] instance;
    public static Vector2 min;
    public static Vector2 max;
    public static float nodeSize;

    public class Node : System.Object
    {
        public Vector2 worldLocation;
        public Coords coords;
        public Coords[] neighbours;
        public byte[] direction; //0 straight 1 diagonal
        public Node(Coords coords, Vector2 worldLocation)
        {
            this.coords = coords;
            this.worldLocation = worldLocation;
        }
    }
    public static void SetUp(List<GameObject> terrain)
    {
        nodeSize = GridBuilder.tileSize;
        //CALCULATE MIN & MAX :::::::::::::::::::::::::::::::::::::::::::::
        int lowerX = 100;
        int lowerY = 100;
        int biggerX = -100;
        int biggerY = -100;

        foreach (GameObject raw_node in terrain)
        {
            Vector3 pos = raw_node.transform.position;
            float deltaY = pos.y / nodeSize;
            float deltaX = pos.x / nodeSize;
            int x = Mathf.RoundToInt(deltaX);
            int y = Mathf.RoundToInt(deltaY);
            if (x < lowerX) { lowerX = x; min.x = pos.x; }
            if (y < lowerY) { lowerY = y; min.y = pos.y; }
            if (x > biggerX) { biggerX = x; max.x = pos.x; }
            if (y > biggerY) { biggerY = y; max.y = pos.y; }
        }

        int width = Mathf.Abs(lowerX - biggerX) + 1;
        int height = Mathf.Abs(lowerY - biggerY) + 1;
        instance = new Node[width, height];
        Debug.Log(width + " " + height);
        //CALCULATE MIN & MAX :::::::::::::::::::::::::::::::::::::::::::::
        //Set Nodes INSTANCES::::::::::::::::::::::::::::::::::::::::::::::::::::
        Coords coords;
  
        foreach (GameObject raw_node in terrain)
        {
            Vector3 pos = raw_node.transform.position;
            float deltaY = (pos.y - min.y) / nodeSize;
            float deltaX = (pos.x - min.x) / nodeSize;
            int x = Mathf.RoundToInt(deltaX);
            int y = Mathf.RoundToInt(deltaY);
            coords = new Coords(x, y);
            Node node = new Node(coords, pos);
            instance[x, y] = node;
        }
        //Set Nodes INSTANCES::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Set Nodes Nerighbours:::::::::::::::::::::::::::::::
        Coords[] neighbours;
        byte[] dirs;
        List<Coords> neighbours_Comp = new List<Coords>();
        List<byte> dir_Comp = new List<byte>();
        Coords[] neighboursNormalized =
        {
            new Coords(1, 0),
            new Coords(0, 1),
            new Coords(-1, 0),
            new Coords(0, -1),
            new Coords(-1, -1),
            new Coords(1, 1),
            new Coords(-1, 1),
            new Coords(1, -1)
        };

        for (int xi = 0; xi < instance.GetLength(0); xi++)
        {
            for (int yi = 0; yi < instance.GetLength(1); yi++)
            {
                if (instance[xi, yi] == null) continue;
                dir_Comp = new List<byte>();
                neighbours_Comp = new List<Coords>();
                for (int ni = 0; ni < neighboursNormalized.Length; ni++)
                {
                    byte value = ni <= 3 ? (byte)0 : (byte)1;
                    Coords neighbourCords = new Coords(xi + neighboursNormalized[ni].x, yi + neighboursNormalized[ni].y);
                    try {
                        if (instance[neighbourCords.x, neighbourCords.y] == null) continue;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        continue;
                    }
                    dir_Comp.Add(value);
                    neighbours_Comp.Add(new Coords(neighbourCords.x, neighbourCords.y));
                }
                neighbours = new Coords[dir_Comp.Count];
                dirs = new byte[dir_Comp.Count];
                for (int ai = 0; ai < dir_Comp.Count; ai++)
                {
                    neighbours[ai] = neighbours_Comp[ai];
                    dirs[ai] = dir_Comp[ai];
                }
                instance[xi, yi].neighbours = neighbours;
                instance[xi, yi].direction = dirs;
            }
        }
        //Set Nodes Nerighbours:::::::::::::::::::::::::::::::
    }
    public static Node Position2Node (Vector2 pos)
    {
        float deltaY = (pos.y - min.y) / nodeSize;
        float deltaX = (pos.x - min.x) / nodeSize;
        int x = Mathf.RoundToInt(deltaX);
        int y = Mathf.RoundToInt(deltaY);
        return instance[x, y];
    }
}
public class Coords
{
    public int x, y;
    public Coords (int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
