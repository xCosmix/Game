using UnityEngine;
using System.Collections;

public class Testpf : MonoBehaviour {
    public Transform from;
    public Transform to;
    Pathfinding path;
	// Use this for initialization
	void Start () {
        StartCoroutine(a());
	}
	
	// Update is called once per frame
	void Update () {
        if (path == null) return;
	    for (int i = 1; i < path.path.Count; i++)
        {
            Debug.DrawLine(path.path[i], path.path[i - 1]);
        }
	}
    private IEnumerator a ()
    {
        yield return null;
            path = new Pathfinding(from.position, to.position, this);
    }
}
