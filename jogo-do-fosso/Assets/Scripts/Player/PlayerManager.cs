using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public Player character;
    public float respawnCooldown;

    private Player characterInstance;

    void Start()
    {
        if(!isLocalPlayer) {
            this.enabled = false;
        }
        
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() 
    {
        Spawn();
        Debug.Log("waiting spawn");
        yield return new WaitUntil(CharacterIsAlive);
        Debug.Log("waiting death");
        yield return new WaitUntil(CharacterIsDead);
        Debug.Log("waiting cooldown");
        yield return new WaitForSeconds(respawnCooldown);
        StartCoroutine(SpawnRoutine());
    }

    bool CharacterIsAlive()
    {
        return characterInstance;
    }

    bool CharacterIsDead()
    {
        return !characterInstance;
    }

    public void Spawn()
    {
        CmdSpawn();
    }

    [Command]
    public void CmdSpawn()
    {
        characterInstance = Instantiate(character);
        NetworkServer.Spawn(characterInstance.gameObject, connectionToClient);
    }
}
