using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileBuilder : EditorWindow {
    public SpriteSet[] sets;
    [System.Serializable]
    public class SpriteSet : System.Object
    {
        public string Name;
        public Sprite[] set;
        [HideInInspector]
        public int[] layer;
        [HideInInspector]
        public byte[] value;
    }
    [MenuItem("Tiles/Builder")]
    public static void ShowWindow ()
    {
        EditorWindow.GetWindow(typeof(TileBuilder));
    }
	void OnGUI ()
    {
        // "target" can be any class derrived from ScriptableObject 
        // (could be EditorWindow, MonoBehaviour, etc)
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("sets");

        EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

    }
}
