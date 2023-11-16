using Unity.AI.Navigation;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private EnterDoor[] _doorsInScene;
    private NavMeshSurface[] _navMeshSurfaces;
    private GameObject _player;

    private void Awake()
    {
        _navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        if (_navMeshSurfaces.Length == 0)
        {
            Debug.LogWarning("No NavMesh prefab fund in scene, enemies won't work");
        }

        foreach (var navMeshSurface in _navMeshSurfaces)
        {
            if (navMeshSurface.navMeshData == null)
            {
                Debug.LogWarning("NavMesh data not baked. It will be generated at runtime, but bake in editor for optimization");
                navMeshSurface.BuildNavMesh();
            }
        }

        _doorsInScene = FindObjectsOfType<EnterDoor>();
        _player = GameObject.FindGameObjectWithTag("Player");

        foreach (var enterDoor in _doorsInScene)
        {
            if (enterDoor.doorIndex == PlayerRuntimeData.GetData().RoomData.NextDoorIndex)
            {
                var position = enterDoor.spawnPoint.position;
                _player.transform.position = new Vector3(position.x, _player.transform.position.y, position.z);
                return;
            }
        }
        Debug.Log("No door found");
    }
}
