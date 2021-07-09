using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
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

    [SerializeField]
    private PlayerInputManager _inputManager;
    [SerializeField]
    private List<PlayerPositions> _playerPositions = new List<PlayerPositions>();

    public List<GameObject> Players = new List<GameObject>();

    public void PlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Total Players: " + _inputManager.playerCount.ToString());
        Debug.Log("Player Joined: " + playerInput.playerIndex.ToString());
        Players.Insert(playerInput.playerIndex, playerInput.gameObject);
        SetPlayerPositions();
    }

    public void PlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Total Players: "  + _inputManager.playerCount.ToString());
        Debug.Log("Player Left: " + playerInput.playerIndex.ToString());
        Players.RemoveAt(playerInput.playerIndex);
        SetPlayerPositions();
    }

    private void SetPlayerPositions()
    {
        for(int i = 0; i < Players.Count; i++)
        {
            Players[i].transform.position = _playerPositions[Players.Count][i].position;
        }
    }
}
