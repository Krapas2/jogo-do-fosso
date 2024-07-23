using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoMelee : PlayerSkill
{
    public ProtoPunch punch;
    public Transform punchPivot;
    [SyncVar]
    public float punchLength;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();
        
        cameraData = FindObjectOfType<CameraData>();

        punch.owner = GetComponent<Player>();

        punch.gameObject.SetActive(false);
    }

    void Update()
    {
        if(canUse){
            Aim();

            if(Input.GetButtonDown("Fire1")){
                StartCoroutine(Punch());
                StartCoroutine(Cooldown());
            }
        }
    }

    void Aim()
    {
        punchPivot.up = cameraData.worldMousePosition - transform.position.Vector2();
    }

    IEnumerator Punch()
    {
        punch.gameObject.SetActive(true);
        yield return new WaitForSeconds(punchLength);
        punch.gameObject.SetActive(false);
    }
}
