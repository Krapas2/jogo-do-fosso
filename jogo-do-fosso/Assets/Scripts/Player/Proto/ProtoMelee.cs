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
    [SyncVar]
    public float cooldown;

    [SyncVar]
    private bool canPunch;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();

        cameraData = FindObjectOfType<CameraData>();

        punch.owner = GetComponent<Player>();

        canPunch = true;
        punch.gameObject.SetActive(false);
    }

    void Update()
    {
        Aim();

        if(Input.GetButtonDown("Fire1") && canPunch){
            StartCoroutine(Punch());
            StartCoroutine(CoolDown());
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

    IEnumerator CoolDown()
    {
        canPunch = false;
        yield return new WaitForSeconds(cooldown);
        canPunch = true;
    }
}
