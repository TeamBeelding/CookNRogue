#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class MarmiteEdiorWindow : OdinMenuEditorWindow
{
    private OdinMenuTree _odinMenuTree;
    
    [MenuItem("Toolbox/Marmite")]
    private static void OpenWindow()
    {
        var window = GetWindow<MarmiteEdiorWindow>();
        window.titleContent.text = "Marmite Editor";
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }

    private void OnFocus()
    {
        ForceMenuTreeRebuild();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        _odinMenuTree = new OdinMenuTree
        {
            {"Home", this, EditorIcons.House},
            {"Player Character", new MarmitePlayerSettings(), EditorIcons.Male},
            {"Camera System", new MarmiteCameraSystem(), EditorIcons.MagnifyingGlass},
            {"Room Manager", new MarmiteRoomManager(), EditorIcons.Marker},
            {"Enemy Manager", new MarmiteEnemyManager(), EditorIcons.Crosshair},
        };
        
        
        return _odinMenuTree;
    }
}
#endif