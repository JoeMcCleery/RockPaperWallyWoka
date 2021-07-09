using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [System.Serializable]
    public class PlayerPositions
    {
        public List<Transform> positions;

        public Transform this[int key]
        {
            get
            {
                return positions[key];
            }
            set
            {
                positions[key] = value;
            }
        }
    }

    [System.Serializable]
    public class PlayerComponents
    {
        public GameObject gameObject;
        public PlayerUI ui;
    }

    [SerializeField]
    private PlayerInputManager _inputManager;
    [SerializeField]
    private List<PlayerPositions> _playerPositions = new List<PlayerPositions>();

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
            Players[i].gameObject.transform.position = _playerPositions[Players.Count - 1][i].position;
        }

        // Start the game if there are enough players
        if (Players.Count > 1)
        {
            GameManager.instance.StartGame();
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
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].ui.SetOption(1); // default to rock
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
}
