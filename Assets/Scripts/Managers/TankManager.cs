using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TankManager
{
    public Color playerColor;                            
    public Transform spawnPoint;                       
    [HideInInspector] public int playerNumber;            
    [HideInInspector] public string ColoredPlayerText; 
    [HideInInspector] public GameObject Instance;         
    [HideInInspector] public int Wins;

    public TankMovement movement;                        
    public TankShooting shooting;
    private GameObject canvasGameObject;

 

    
    public void TankSetup ()
    {
        movement = Instance.GetComponent<TankMovement> ();
        shooting = Instance.GetComponent<TankShooting> ();
        canvasGameObject = Instance.GetComponentInChildren<Canvas> ().gameObject;

        movement.playerNumber = playerNumber;
        shooting.playerNumber = playerNumber;

        ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        MeshRenderer[] renderers = Instance.GetComponentsInChildren<MeshRenderer> ();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = playerColor;
        }

    }

    public void DisableTnankControl ()
    {
        movement.enabled = false;
        shooting.enabled = false;

        canvasGameObject.SetActive (false);
    }


    public void EnableTankControl ()
    {
        movement.enabled = true;
        shooting.enabled = true;

        canvasGameObject.SetActive (true);
    }



    public void ResetValue ()
    {
        Instance.transform.position = spawnPoint.position;
        Instance.transform.rotation = spawnPoint.rotation;
        Instance.SetActive (false);
        Instance.SetActive (true);
    }
}