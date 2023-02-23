#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class MarmiteEdiorWindow : OdinMenuEditorWindow
{
    [MenuItem("Toolbox/Marmite")]
    private static void OpenWindow()
    {
        var window = GetWindow<MarmiteEdiorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }


    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree odinMenuTree = new OdinMenuTree()
        {
            {"Home", this, EditorIcons.House},
            {"Player Character", new MarmitePlayerSettings(), EditorIcons.Male}
        };

        return odinMenuTree;
    }
}
#endif