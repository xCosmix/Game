using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
public class TileEditor : Editor {
    private static SpriteRenderer[] sceneObjects;
    public static TileManager tileManager;
    public static SystemManager systemManager;
    static TileEditor()
    {
        if (systemManager==null)
        {
            systemManager = GameObject.FindObjectOfType<SystemManager>();
            if (systemManager == null)
                systemManager= new GameObject("systemManager").AddComponent<SystemManager>();
            systemManager.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        if (tileManager == null)
        {
            tileManager = GameObject.FindObjectOfType<TileManager>();
            if (tileManager == null)
                tileManager = new GameObject("tileManager").AddComponent<TileManager>();
            tileManager.gameObject.transform.SetParent(systemManager.transform);
        }
        EditorApplication.update += Update;
    }
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	static void Update () {
        if (!Application.isPlaying)
        {
            sceneObjects = GameObject.FindObjectsOfType<SpriteRenderer>();
            foreach (SpriteRenderer go in sceneObjects)
            {
                if (TileManager.tileSize == 0.0f)
                {
                    if (go.tag == "Terrain")
                    {
                        SpriteRenderer terr = go.GetComponent<SpriteRenderer>();
                        Rect rect = terr.sprite.rect;
                        float size = rect.width / 100;
                        TileManager.tileSize = size;
                    }
                    continue;
                }
                Vector3 pos = go.transform.position;
                float deltaY = pos.y / TileManager.tileSize;
                float deltaX = pos.x / TileManager.tileSize;
                float valueY = Mathf.Round(deltaY) * TileManager.tileSize;
                float valueX = Mathf.Round(deltaX) * TileManager.tileSize;
                pos.x = valueX; pos.y = valueY;
                go.transform.position = pos;
                if (go.tag=="Terrain")
                    go.name = "Terrain(" + Mathf.Round(deltaX) + "," + Mathf.Round(deltaY) + ")";
            }
        }
        else
        {
            //
        }
	}
}
