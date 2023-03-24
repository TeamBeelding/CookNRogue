using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MarmiteEnemyManager
{

    private EnemyManager _enemyManager;

    [InfoBox("Can't load levels when not playing.", InfoMessageType.Warning, "IsInEditor")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Medium)]
    private void KillAllEnemies()
    {
        _enemyManager.DestroyAll();
    } 

    public MarmiteEnemyManager()
    {
        _enemyManager = EnemyManager.Instance;
    }

    private bool IsInEditor()
    {
        return !Application.isPlaying;
    }
}
