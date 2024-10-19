using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoSentry : NetworkBehaviour
{
    [SyncVar]
    public float aimRange;
    [SyncVar]
    public LayerMask aimLayers;

    [SyncVar]
    [HideInInspector]
    public ProtoSentrySpawner owner;

    void Start()
    {
        
    }

    void Update()
    {
        Aim();
    }

    [Server]
    void Aim()
    {
        Vector3 targetPosition = ClosestPlayerPositionInSight();
        //need to check for aimRange in case of no possible targets in range
        if(Vector3.Distance(transform.position, targetPosition) < aimRange){
            Debug.Log(targetPosition);
            transform.up = targetPosition;
        }
    }
    
    Vector3 ClosestPlayerPositionInSight()
    {
        Character[] characters = FindObjectsOfType<Character>();

        Vector3 closestCharacterPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        bool ownerHasCharacter = false;
        Character ownerCharacter = new Character();
        if(owner){
            ownerHasCharacter = owner.TryGetComponent<Character>(out ownerCharacter);
        }

        foreach (Character character in characters){
            if(owner && ownerHasCharacter && ownerCharacter == character){
                continue;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, character.transform.position, aimRange, aimLayers);
            Debug.DrawLine(transform.position, character.transform.position);
            if(!hit){
                Debug.Log("didntmakeit");
                continue;
            }else if(!(hit.collider.gameObject == character.gameObject)){
                Debug.Log("madeit");
                continue;
            }

            float distance = Vector3.Distance(transform.position, character.transform.position);

            if (distance < mininumDistance){
                closestCharacterPosition = character.transform.position;
                mininumDistance = distance;
            }
        }

        return closestCharacterPosition;
    }
}
