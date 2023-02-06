using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class amogus : EditorWindow
{
    public Texture AmogusTexture;
    public AudioClip AmogAudioclip;
    [MenuItem("Toolbox/Toolbox Amogus")]

    static void InitWindow()
    {

        amogus sus = GetWindow<amogus>();
        
        sus.titleContent = new GUIContent("Amogus Box");

        sus.Show();

    }
    private void OnGUI()
    {
        //EditorGUILayout.TextArea("AMOGUS", GUILayout.Width(400), GUILayout.Height(400));
        GUILayout.Box(AmogusTexture);
        if (GUILayout.Button("AMOGUS"))
        {
            
            PlayClip((AmogAudioclip));
        }
    }
    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );

        Debug.Log(method);
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }
}
