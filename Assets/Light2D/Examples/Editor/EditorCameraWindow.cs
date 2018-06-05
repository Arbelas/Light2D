using UnityEngine;
using UnityEditor;

public class EditorCameraWindow : EditorWindow
{
    string _myString = "Hello World";
    bool _groupEnabled;
    bool _myBool = true;
    float _myFloat = 1.23f;

    [MenuItem("Window/Editor Camera Settings")]
    static void Init()
    {
        EditorCameraWindow window = GetWindow<EditorCameraWindow>();
    }

    void OnGui()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        _myString = EditorGUILayout.TextField("Text Field", _myString);

        _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
        _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
        _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}