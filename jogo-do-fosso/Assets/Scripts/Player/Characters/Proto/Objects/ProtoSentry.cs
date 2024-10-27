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
        Behaviour();
    }

    [ServerCallback]
    void Behaviour()
    {
        Vector3 targetPosition = ClosestTarget();

        if(!targetPosition.Equals(Vector3.positiveInfinity)){
            Aim(targetPosition);

            if(canUse){
                Fire();
                StartCoroutine(Cooldown());
            }
        }
    }

    [Server]
    void Aim(Vector3 position)
    {
        projectileOrigin.up = position - transform.position;
    }

    [Server]
    void Fire()
    {
        SentryProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.owner = this;
        team.SpawnTeammate(projectile.GetComponent<TeamBehaviour>());
    }
    
    Vector3 ClosestTarget()
    {
        CharacterHealth[] targets = FindObjectsOfType<CharacterHealth>();

        Vector3 closestCharacterPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        bool ownerHasHealth = false;
        CharacterHealth ownerHealth = null;
        if(owner){
            ownerHasHealth = owner.TryGetComponent<CharacterHealth>(out ownerHealth);
        }

        foreach (CharacterHealth target in targets){
            bool targetIsSelf = target.gameObject == gameObject;
            bool targetIsOwner = owner && ownerHasHealth && ownerHealth == target;
            if(targetIsSelf || targetIsOwner){
                continue;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, aimRange, aimLayers);
            if(!hit){
                continue;
            }
            bool targetIsObscured = hit.collider.gameObject != target.gameObject;
            if(targetIsObscured){
                continue;
            }

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < mininumDistance){
                closestCharacterPosition = target.transform.position;
                mininumDistance = distance;
            }
        }

        return closestCharacterPosition;
    }
}
