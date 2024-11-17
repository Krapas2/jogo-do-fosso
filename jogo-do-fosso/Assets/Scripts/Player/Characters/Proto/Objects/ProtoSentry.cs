using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoSentry : CharacterSkill
{
    public SentryProjectile projectilePrefab;
    public Transform projectileOrigin;
    public float aimRange;
    public LayerMask ignore;

    [SyncVar]
    [HideInInspector]
    public Transform overrideTarget;

    [SyncVar]
    [HideInInspector]
    public ProtoSentrySpawner owner;

    protected override void Start(){
        base.Start();
    }

    void Update()
    {
        if(!CheckOwner()){
            CmdDestroySelf(); 
        }
        Behaviour();
    }

    [ClientCallback]
    void Behaviour()
    {
        Vector3 targetPosition = overrideTarget != null ? overrideTarget.position : ClosestTarget();

        if(!targetPosition.Equals(Vector3.positiveInfinity)){
            Aim(targetPosition);

            if(canUse){
                Fire();
                StartCoroutine(Cooldown());
            }
        }
    }

    [Command]
    void Aim(Vector3 position)
    {
        projectileOrigin.up = position - transform.position;
    }

    [Command]
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

        foreach (CharacterHealth target in targets){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, aimRange, ignore.Inverse());
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

    bool CheckOwner()
    {
        return owner && owner.enabled;
    }

    [Client]
    public void OverrideTarget(Transform target, float length)
    {
        StartCoroutine(OverrideTargetRoutine(target, length));
    }

    [Client]
    public IEnumerator OverrideTargetRoutine(Transform target, float length)
    {
        overrideTarget = target;
        yield return new WaitForSeconds(length);
        overrideTarget = null;
    }

    [Command]
    public void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
