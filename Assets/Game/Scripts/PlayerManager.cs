using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [System.Serializable]
    public class PlayerComponents
    {
        public GameObject gameObject;
        public PlayerUI ui;
    }

    [SerializeField]
    private PlayerInputManager _inputManager;

    [HideInInspector]
    public List<PlayerComponents> Players = new List<PlayerComponents>();

    private void Start()
    {
        if (PlayerManager.instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void PlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Total Players: " + _inputManager.playerCount.ToString());
        Debug.Log("Player Joined: " + playerInput.playerIndex.ToString());
        var playerUI = playerInput.GetComponentInChildren<PlayerUI>();
        playerUI.SetPlayerID(playerInput.playerIndex);
        Players.Insert(playerInput.playerIndex, new PlayerComponents {
            gameObject = playerInput.gameObject,
            ui = playerUI
        });
        PlayerSetup();
    }

    public void PlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Total Players: "  + _inputManager.playerCount.ToString());
        Debug.Log("Player Left: " + playerInput.playerIndex.ToString());
        Players.RemoveAt(playerInput.playerIndex);
        PlayerSetup();
    }

    private void PlayerSetup()
    {
        // Set Player vars
        for (int i = 0; i < Players.Count; i++)
        {
            // Set position
            float radius = 2.5f + 0.5f * Players.Count / 8;
            float angle = i * Mathf.PI * 2f / Players.Count + Mathf.Deg2Rad * 90f;
            Vector3 spawnPos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius) + this.transform.position;
            Players[i].gameObject.transform.position = spawnPos;

            // Set ID
            Players[i].ui.SetPlayerID(i + 1);
        }

        // Get game ready to start if there are enough players
        if (Players.Count > 1)
        {
            GameManager.instance.ResetGame();
            GameManager.instance.ShowSettings();
        }
    }

    public void UpdatePlayerImages()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.UpdateImage();
        }
    }

    public void ResetPlayerOptions()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.SetOption(0);
            Players[i].ui.UpdateImage();
            Players[i].ui.UpdateDamageUI(0);
        }
    }

    public void SetDefaultOption(bool random = false)
    {
        int defaultOption = random ? Random.Range(1, 4) : 1; // use random or default to rock
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i].ui.selectedOption == 0)
            {
                Players[i].ui.SetOption(defaultOption);
            }
        }
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.ResetUI();
        }
    }

    public List<PlayerComponents> GetAlivePlayers()
    {
        List<PlayerComponents> alivePlayers = new List<PlayerComponents>();
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].ui.IsAlive())
            {
                alivePlayers.Add(Players[i]);
            }
        }
        return alivePlayers;
    }

    public void SetRoundWinner(int playerID)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.ShowCrown(Players[i].ui.GetPlayerID() == playerID);
        }
    }
}
