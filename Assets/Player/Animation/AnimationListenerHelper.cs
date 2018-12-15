using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class AnimationListenerHelper
{
    const string currentFileName = "AnimationListenerHelper.cs";

    public static void GenerateConstFile(string fileName, Animator animator)
    {
        string path = GetPath(fileName);
        string content = GetFileContent(fileName, animator);
        WriteToFile(path, content);
    }

    static void WriteToFile(string path, string content)
    {
        using (System.IO.FileStream fs = System.IO.File.Create(path))
        {
            char[] fileContentByte = content.ToCharArray();
            for (int i = 0; i < fileContentByte.Length; i++)
            {
                fs.WriteByte((byte)fileContentByte[i]);
            }
            fs.Close();
        }
        UnityEditor.AssetDatabase.Refresh();
    }

    static string GetFileContent(string fileName, Animator animator)
    {
        AnimatorControllerParameter[] animatorParameters = animator.parameters;

        StringBuilder fileContent = new StringBuilder();
        fileContent.Append("\npublic static class " + fileName + "Const" + " \n{ \n");
        foreach (AnimatorControllerParameter param in animatorParameters)
        {
            fileContent.Append("    "); //for indentation
            fileContent.Append("public const int ");
            fileContent.Append(param.name);
            fileContent.Append(" = " + param.nameHash + "; \n"); // = poolObject;
        }
        fileContent.Append("}");

        return fileContent.ToString();
    }

    static string GetPath(string fileName)
    {
        string[] paths = System.IO.Directory.GetFiles(Application.dataPath, currentFileName, System.IO.SearchOption.AllDirectories);
        string path = paths[0].Replace(currentFileName, "").Replace("\\", "/");
        return path + fileName + "Const.cs";
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