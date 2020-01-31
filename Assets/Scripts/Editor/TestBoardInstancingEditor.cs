using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestBoardInstantiating))]
public class TestBoardInstantiatingEditor : Editor
{
    private TestBoardInstantiating instance;

    private void OnEnable()
    {
        instance = target as TestBoardInstantiating;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Instantiate Default Board"))
            instance.InstantiateDefaultBoard();
    }
}