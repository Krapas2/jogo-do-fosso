using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoSentry : CharacterSkill
{
    public SentryProjectile projectilePrefab;
    public Transform projectileOrigin;
    public float aimRange;
    public LayerMask aimLayers;

    [SyncVar]
    [HideInInspector]
    public ProtoSentrySpawner owner;

    [SyncVar]
    [HideInInspector]
    public Vector3 targetPosition;

    protected override void Start(){
        team = GetComponent<TeamBehaviour>();

        if(isOwned){
            StartCoroutine(WaitForOwnerDeath());
        }
    }
    
    [Client]
    IEnumerator WaitForOwnerDeath()
    {
        yield return new WaitUntil(CheckOwner);
        CmdDestroySelf(); 
    }

    bool CheckOwner()
    {
        return !owner || !owner.enabled;
    }

    [Command]
    public void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    void Update()
    {
        SetTarget();

        //need to check for aimRange in case of no possible targets in range
        if(targetPosition != transform.position){
            Aim();

            if(canUse){
                Fire();
                StartCoroutine(Cooldown());
            }
        }
    }

    [Server]
    void SetTarget()
    {
        targetPosition = ClosestEligibleCharacter();
    }

    [Server]
    void Aim()
    {
        projectileOrigin.up = targetPosition - transform.position;
    }

    [Server]
    void Fire()
    {
        SentryProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.owner = this;
        team.SpawnTeammate(projectile.GetComponent<TeamBehaviour>());
    }
    
    Vector3 ClosestEligibleCharacter()
    {
        Character[] characters = FindObjectsOfType<Character>();

        Vector3 closestCharacterPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        bool ownerHasCharacter = false;
        Character ownerCharacter = null;
        if(owner){
            ownerHasCharacter = owner.TryGetComponent<Character>(out ownerCharacter);
        }

        foreach (Character character in characters){
            if(owner && ownerHasCharacter && ownerCharacter == character){
                continue;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, character.transform.position - transform.position, aimRange, aimLayers);
            if(!hit || !(hit.collider.gameObject == character.gameObject)){
                continue;
            }

            float distance = Vector3.Distance(transform.position, character.transform.position);

            if (distance < mininumDistance){
                closestCharacterPosition = character.transform.position;
                mininumDistance = distance;
            }
        }

        if(closestCharacterPosition.Equals(Vector3.positiveInfinity)){
            return transform.position;
        }

        return closestCharacterPosition;
    }
}
