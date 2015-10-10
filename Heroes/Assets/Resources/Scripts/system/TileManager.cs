using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileManager: MonoBehaviour {
    public static float tileSize;
    void OnDrawGizmos()
    {
        if (tileSize == 0.0f) return;
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 pos = sceneCamera.transform.position;
        pos.x = Mathf.Round(pos.x / tileSize) * tileSize;
        pos.y = Mathf.Round(pos.y / tileSize) * tileSize;
        Vector2 camRect = new Vector2(sceneCamera.pixelWidth, sceneCamera.pixelHeight);
        camRect.x = camRect.x / 100.0f;
        camRect.y = camRect.y / 100.0f;//world units
        //camRect *= sceneCamera.orthographicSize;
        //
        Vector2 max = (Vector2)pos + camRect * 0.5f;
        Vector2 min = (Vector2)pos - camRect * 0.5f;
        //
        Vector2 delta = new Vector2(camRect.x / tileSize, camRect.y / tileSize);
        delta.x = Mathf.Round(delta.x);
        delta.y = Mathf.Round(delta.y);
        Vector2 valueOnI = camRect;
        valueOnI.x = valueOnI.x / delta.x;
        valueOnI.y = valueOnI.y / delta.y;
        //Gizmos.DrawLine(max, min);
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        for (int x = 0; x < delta.x; x++)
        {
            Vector3 posA = max;
            posA.y += tileSize*0.5f;
            Vector3 posB = new Vector3(min.x, max.y, 0.0f);
            posB.y += tileSize * 0.5f;
            posA.y -= (valueOnI.y * x);
            posB.y -= (valueOnI.y * x);
            Gizmos.DrawLine(posA, posB);
        }
        for (int y = 0; y < delta.y*2; y++)
        {
            Vector3 posA = max;
            Vector3 posB = new Vector3(max.x, min.y, 0.0f);
            posA.x -= (valueOnI.x * y);
            posB.x -= (valueOnI.x * y);
            Gizmos.DrawLine(posA, posB);
        }
    }
    public void Delete ()
    {
        Destroy(gameObject);
    }
    /*
    Camera sceneCamera = SceneView.lastActiveSceneView.camera;
    Vector3 pos = sceneCamera.transform.position;
    Vector2 camRect = new Vector2(sceneCamera.pixelWidth, sceneCamera.pixelHeight);
    camRect.x = camRect.x / 100.0f;
        camRect.y = camRect.y / 100.0f;//world units
        camRect *= sceneCamera.orthographicSize;
        Vector2 delta = new Vector2(camRect.x / tileSize, camRect.y / tileSize);
    delta.x = Mathf.Round(delta.x);
        delta.y = Mathf.Round(delta.y);
        Vector2 lineMount = delta;
    Vector2 max = (Vector2)pos + camRect * 0.5f;
    Vector2 min = (Vector2)pos - camRect * 0.5f;
    Vector2 valueOnI = camRect;
    valueOnI.x = valueOnI.x / lineMount.x;
        valueOnI.y = valueOnI.y / lineMount.y;
        //Gizmos.DrawLine(max, min);
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        for (int x = 0; x<delta.x; x++)
        {
            Vector3 posA = max;
    posA.x = Mathf.Round(posA.x / tileSize) * tileSize;
            posA.y = Mathf.Round(posA.y / tileSize) * tileSize;
            Vector3 posB = new Vector3(min.x, max.y, 0.0f);
    posB.x = Mathf.Round(posB.x / tileSize) * tileSize;
            posB.y = Mathf.Round(posB.y / tileSize) * tileSize;
            //
            posA.y -= (valueOnI.y* x);
    posB.y -= (valueOnI.y* x);
    Gizmos.DrawLine(posA, posB);
        }
        for (int y = 0; y<delta.y*2; y++)
        {
            Vector3 posA = max;
posA.x = Mathf.Round(posA.x / tileSize) * tileSize;
            posA.y = Mathf.Round(posA.y / tileSize) * tileSize;
            Vector3 posB = new Vector3(max.x, min.y, 0.0f);
posB.x = Mathf.Round(posB.x / tileSize) * tileSize;
            posB.y = Mathf.Round(posB.y / tileSize) * tileSize;
            posA.x -= (valueOnI.x* y);
            posB.x -= (valueOnI.x* y);
            Gizmos.DrawLine(posA, posB);
        }
        */
}
