using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnLocation : MonoBehaviour
{
    [SerializeField] Transform currentSpawnLocation;
    [SerializeField] UnityEvent hasRespawned;

    public void OnPlayerDeath()
    {
        Invoke("MovePlayer", 1f);
    }

    void MovePlayer()
    {
        Transform playerposition = GetComponent<Transform>();
        playerposition.position = currentSpawnLocation.position;
        hasRespawned.Invoke();
    }

    public void OnNewSpawnLocation(Vector3 location)
    {
        currentSpawnLocation.position = location;
    }
}
