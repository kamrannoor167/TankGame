using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public int numMiisionToWin = 3;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public float pickUpsTime = 15f;
    public CameraControl cameraControl;
    public Text messageText;
    public GameObject tankPrefab;
    public TankManager[] tanks;
    public UIManager uiManager;


    private int roundNumber;
    private float gamePlayTimer;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private TankManager roundWinner;
    private TankManager gameWinner;
    [SerializeField]
    private TankHealth tankHealth;

    private  ResourceComponent resource;
    [SerializeField]
    private RectTransform timerPanel;



    [SerializeField]
    private GameObject[] singleModeObjects;

    [Header("Configuration")]
    [SerializeField] private GameCommands startedCommand;

    private CommandsHandler _commandsHandler;




    private void Awake()
    {
        _commandsHandler = new CommandsHandler();
    
    }


    private void OnEnable()
    {
        ButtonCommandSender.SentCommand += HandleState;
        _commandsHandler.ModeChanged += ModesHandlers;
        SpawnAllTanks();
        SetCameraTargets();
        TankHealth.SentCommand += HandleState;
        SubscribeResourceType();

    }


    private void Start()
    {

        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        HandleState(startedCommand);

    }
    private void OnDisable()
    {
        ButtonCommandSender.SentCommand -= HandleState;
        _commandsHandler.ModeChanged -= ModesHandlers;
        TankHealth.SentCommand -= HandleState;
        UnSubscribeResourceType();
    }

    protected void ResourcesValue(ResourceComponent _resource)
    {

        foreach (TankManager manager in tanks)
        {
            resource = _resource;
            if (resource.type == ResourceType.Ammo)
            {

                manager.Instance.GetComponent<TankShooting>().currentAmmoCount = resource.Count;
                ResourceHolder.CurrentAmmo = resource.Count;
                Debug.Log("ammo resource value");
            }
            else if (resource.type == ResourceType.Shield)
            {
                pickUpsTime = 15f;

                manager.Instance.GetComponent<TankHealth>().shieldObject.SetActive(true);
                manager.Instance.GetComponent<TankHealth>().isShield = true;
                timerPanel.gameObject.SetActive(true);
                StartCoroutine(PickUpTimerLoop(resource));
           
            }
            else if (resource.type == ResourceType.Turbo)
            {
                pickUpsTime = 15f;
                StartCoroutine(PickUpTimerLoop(resource));
                timerPanel.gameObject.SetActive(true);
                manager.Instance.GetComponent<TankMovement>().speed = 20f ;
            }
        }
    }

   

    private void ModesHandlers(object sender, bool state)
    {

        if (state)
        {
            for (int i = 0; i < singleModeObjects.Length; i++)
            {

                singleModeObjects[i].gameObject.SetActive(true);
            }
        }
        else
        {
            messageText.gameObject.SetActive(true);
            StartCoroutine(GamePlayLoop());
            Debug.Log("Game loop control");
        }

      
    }

    void SubscribeResourceType()
    {
        foreach (TankManager manager in tanks)
        {

            manager.Instance.GetComponent<BarrierCheckers>().SendResourceType += ResourcesValue;
            resource = manager.Instance.GetComponent<ResourceComponent>();
        }
    }
    void UnSubscribeResourceType()
    {
        foreach (TankManager manager in tanks)
        {

            manager.Instance.GetComponent<BarrierCheckers>().SendResourceType -= ResourcesValue;
            resource = manager.Instance.GetComponent<ResourceComponent>();
        }
    }
    IEnumerator PickUpTimerLoop(ResourceComponent resource)
    {

        while (pickUpsTime > 0)
        {
            pickUpsTime -= 1f;
            ResourceHolder.Timer = (int)pickUpsTime;
            Debug.Log("current timer :" + pickUpsTime);

            yield return new WaitForSeconds(1f);
        }

        if (resource.type == ResourceType.Turbo)
        {
            foreach (TankManager manager in tanks)
            {
                manager.Instance.GetComponent<TankMovement>().speed = 12f;

            }
        }

        else if (resource.type == ResourceType.Shield)
        {
            foreach (TankManager manager in tanks)
            {
                manager.Instance.GetComponent<TankHealth>().shieldObject.SetActive(false);
                manager.Instance.GetComponent<TankHealth>().isShield = false;

            }
        }
        timerPanel.gameObject.SetActive(false);
    }

    private void SpawnAllTanks()
    {

        for (int i = 0; i <= 1; i++)
        {
            tanks[i].Instance =
                Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].TankSetup();
        }
    }


    private void SetCameraTargets()
    {

        Transform[] targets = new Transform[tanks.Length];


        for (int i = 0; i < targets.Length; i++)
        {

            targets[i] = tanks[i].Instance.transform;
        }

        cameraControl.targets = targets;
    }

    private IEnumerator GamePlayLoop()
    {
        yield return StartCoroutine(MissionStarting());

        yield return StartCoroutine(MissionPlaying());

        yield return StartCoroutine(MissionEnding());

        if (gameWinner != null)
        {
            Application.LoadLevel(0);

        }
        else
        {
            StartCoroutine(GamePlayLoop());

        }
    }


    private IEnumerator MissionStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        cameraControl.SetCameraStartPositionAndSize();

        roundNumber++;
        messageText.text = "ROUND " + roundNumber;

        yield return startWait;
    }


    private IEnumerator MissionPlaying()
    {
        EnableTankControl();

        messageText.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }
    }


    private IEnumerator MissionEnding()
    {
        DisableTankControl();

        roundWinner = null;

        roundWinner = GetMissionWinner();

        if (roundWinner != null)
            roundWinner.Wins++;

        gameWinner = GetWinner();

        string message = FinalMessage();
        messageText.text = message;

        yield return endWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetMissionWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].Instance.activeSelf)
                return tanks[i];
        }

        return null;
    }


    private TankManager GetWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].Wins == numMiisionToWin)
                return tanks[i];
        }

        return null;
    }


    private string FinalMessage()
    {

        string message = "DRAW!";

        if (roundWinner != null)
            message = roundWinner.ColoredPlayerText + " WINS THE MISSION!";

        message += "\n\n\n\n";

        for (int i = 0; i < tanks.Length; i++)
        {
            message += tanks[i].ColoredPlayerText + ": " + tanks[i].Wins + " WINS\n";
        }

        if (gameWinner != null)
            message = gameWinner.ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].ResetValue();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableTankControl();
        }
    }

  
  
    private void DisableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableTnankControl();
        }
    }

    private void HandleState(GameCommands gameCommands)
    {

        Debug.Log("Hanlde State : " + gameCommands);
        _commandsHandler.HandleState(gameCommands);
        uiManager.SwitchScreen(gameCommands);
    }
}