using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TeamBehaviour : NetworkBehaviour
{
    public enum Team
    {
        undefined,
        Red,
        Grin,
        Blu,
        Spectate
    }

    [SyncVar]
    public Team team = Team.undefined;

    //TODO: both should be dropdown menus for layer selection
    public int teammate;
    public int enemy;

    private bool isPlayerManager;

    void Start()
    {
        isPlayerManager = GetComponent<PlayerManager>();

        //temporary random team assignment
        //change this to always assign undefined when team selection exists
        // remmove system from imports too btw
        if (isPlayerManager && isOwned){
            var values = Enum.GetValues(typeof(Team));
            int playerCount = FindObjectsOfType<PlayerManager>().Length;
            team = (Team)values.GetValue((playerCount % 3) + 1);
        }

        if (team != Team.undefined){
            SetupTeam();
        }
    }

    public void AssignTeam(Team teamBeingAssigned)
    {
        team = teamBeingAssigned;
        SetupTeam();

        if (!isPlayerManager){
            return;
        }

        TeamBehaviour[] teamBehaviours = FindObjectsOfType<TeamBehaviour>();

        foreach (TeamBehaviour teamBehaviour in teamBehaviours){
            teamBehaviour.SetupTeam();
        }
    }

    [Client]
    public void SetupTeam()
    {
        SetTeamLayer();
        SetTeamColor();
    }

    void SetTeamColor()
    {
        if(TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite)){
            Color color;
            switch(team) 
            {
                case Team.Red:
                    color = Color.red;
                    break;
                case Team.Grin:
                    color = Color.green;
                    break;
                case Team.Blu:
                    color = Color.blue;
                    break;
                default:
                    color = Color.gray;
                    break;
            }
            sprite.color = color;
        }
    }

    void SetTeamLayer()
    {
        GameObject localPlayer = NetworkClient.localPlayer.gameObject;
        Team localPlayerTeam = localPlayer.GetComponent<TeamBehaviour>().team;

        int layerToSet;
        if (localPlayerTeam == team){
            layerToSet = teammate;
        } else {
            layerToSet = enemy;
        }
        gameObject.layer = layerToSet;
    }

    public void SpawnTeammate(TeamBehaviour objectToSpawn)
    {
        NetworkServer.Spawn(objectToSpawn.gameObject, connectionToClient);
        TargetSetup(objectToSpawn.gameObject);
    }

    [ClientRpc]
    void TargetSetup(GameObject objectToSpawn)
    {
        objectToSpawn.GetComponent<TeamBehaviour>().AssignTeam(team);
    }
}
