using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public Player character;
    public float respawnCooldown;

    [HideInInspector]
    public Player currentCharacter;

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
        return currentCharacter;
    }

    bool CharacterIsDead()
    {
        return !currentCharacter;
    }

    public void Spawn()
    {
        CmdSpawn();
    }

    [Command]
    public void CmdSpawn()
    {
        Player characterInstance = Instantiate(character);
        characterInstance.manager = this;
        NetworkServer.Spawn(characterInstance.gameObject, connectionToClient);
    }
}
