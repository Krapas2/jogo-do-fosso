using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public Character character;
    public float respawnCooldown;

    [HideInInspector]
    public Character currentCharacter;

    void Start()
    {
        if(!isLocalPlayer) {
            this.enabled = false;
            return;
        }
        
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() 
    {
        Spawn();
        yield return new WaitUntil(CharacterIsAlive);
        yield return new WaitUntil(CharacterIsDead);
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
        Character characterInstance = Instantiate(character);
        characterInstance.manager = this;
        NetworkServer.Spawn(characterInstance.gameObject, connectionToClient);
    }
}
