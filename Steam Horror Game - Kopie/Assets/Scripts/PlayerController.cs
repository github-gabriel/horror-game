using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerController : NetworkBehaviour // NetworkBehaviour -> Sync player across the Server
{
    
    // Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;

    [SyncVar(hook = nameof(PlayerNameUpdate))] // Function will be called when value changes
    public string PlayerName;

    [SyncVar(hook = nameof(PlayerReadyUpdate))]
    public bool Ready;
    
    // Flashlight: https://forum.unity.com/threads/solved-with-example-sync-globallight-and-personal-flashlight-over-the-network.484524/
    
    [SerializeField] private Light FlashLight;
    [SyncVar] private bool LightState;

    private CustomNetworkManager manager;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        GetLightValue ();
        if (Input.GetButtonDown("F"))
        {
               
            if (isLocalPlayer == true)
            {
                bool ChangState = !LightState;                  
                Debug.Log(LightState);
                CmdSendLightValue(ChangState);
            }
        }
    }
    
    private void GetLightValue()
    {
        FlashLight.enabled = LightState;
    }
    
    [Command]
    private void CmdSendLightValue(bool ChangState)
    {
        LightState = ChangState;
        Debug.Log("Switched the FlashLight state.");
    }

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) // Already assigned
            {
                return manager;
            }

            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void PlayerReadyUpdate(bool oldValue, bool mewValue)
    {
        if (isServer)
        {
            this.Ready = mewValue;
        }

        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }
    
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if (isServer) // Host
        {
            this.PlayerName = NewValue;
        }

        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }
    
    // Start Game

    public void CanStartGame(string SceneName)
    {
        if (hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }
    
    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
    
}
