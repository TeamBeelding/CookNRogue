using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private EnterDoor[] _doorsInScene;
    private GameObject _player;

    private void Start()
    {
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
