using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public class AnimationListenerGenerator : MonoBehaviour
{
    [SerializeField] protected string fileName;
    public void GenerateConstFile()
    {
        List<AnimatorControllerParameter> parameters = GetComponent<Animator>().parameters.ToList();

        string[] names = parameters.Select(x => x.name).ToArray();
        int[] hashes = parameters.Select(x => x.nameHash).ToArray();

        ConstFileWriter.GenerateConstFile(this, fileName, names, hashes);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimationListenerGenerator))]
public class AnimationListenerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationListenerGenerator animationListener = (AnimationListenerGenerator)target;
        if (GUILayout.Button("Generate Const File"))
        {
            animationListener.GenerateConstFile();
        }
    }
}
#endif
