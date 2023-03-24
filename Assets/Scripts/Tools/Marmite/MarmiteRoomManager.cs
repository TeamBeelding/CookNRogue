#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEngine;

public class MarmiteRoomManager
{

    private RoomManager _roomManager;

    [InfoBox("Can't load levels when not playing.", InfoMessageType.Warning, "IsInEditor")]
    [PropertySpace(10, 10)]
    [BoxGroup("LoadSpecificLevel", true,true)]
    [DisableInEditorMode]
    [ValueDropdown("GetLoadableRooms")]
    public int roomToLoad = 0;

    [BoxGroup("LoadSpecificLevel")]
    [DisableInEditorMode]
    [Button]
    private void LoadLevel()
    {
        _roomManager.LoadPecifiedLevel(roomToLoad);
    }
    [PropertySpace(10)]
    [DisableInEditorMode]
    [Button(ButtonSizes.Medium)]
    private void LoadRandomLevel()
    {
        _roomManager.LoadRandomLevel();
    } 

    public MarmiteRoomManager()
    {
        _roomManager = RoomManager.instance;
    }

    private ValueDropdownList<int> GetLoadableRooms()
    {
        if (_roomManager == null)
            return new ValueDropdownList<int> { { "None", 0 } };

        var dropdown = new ValueDropdownList<int>();
        for (var i = 0; i < _roomManager.LevelNames.Length; i++)
        {
            dropdown.Add( _roomManager.LevelNames[i], i );
        }
        
        return dropdown;
    }
    
    private bool IsInEditor()
    {
        return !Application.isPlaying;
    }
}

#endif