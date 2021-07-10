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
        Players.Insert(playerInput.playerIndex, new PlayerComponents {
            gameObject = playerInput.gameObject,
            ui = playerInput.GetComponentInChildren<PlayerUI>()
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
        // Set Player Postions
        for(int i = 0; i < Players.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / Players.Count + Mathf.Deg2Rad * 90f;
            Vector3 spawnPos = new Vector3(Mathf.Cos(angle) * 3f, 0f, Mathf.Sin(angle) * 3f) + this.transform.position;
            Players[i].gameObject.transform.position = spawnPos;
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
        }
    }

    public void SeRandomPlayerOptionsWithoutShowing()
    {
        int randOption = Random.Range(1, 4);
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.SetOption(randOption);
            Players[i].ui.UpdateImage();
        }
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.ResetUI();
            Players[i].ui.SetPlayerID(i + 1);
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
}
