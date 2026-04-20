using PurrLobby;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PurrLobby
{
    public class ConnectionManager : MonoBehaviour
    {
        [Header("Network")]
        [SerializeField] private LobbyManager lobbyManager;
        
        [Header("Lobby Screen")]
        [SerializeField] private LobbyMemberList lobbyMemeberList;
        [SerializeField] private FriendsList friendList;
        [SerializeField] private TextMeshProUGUI activeLobbyName;

        [Header("Browse Screen")]
        [SerializeField] private GameObject browseScreen;
        [SerializeField] private TextMeshProUGUI lobbyIdCode;

        [Header("Inbetween Screens")]
        [SerializeField] private GameObject connectingScreen;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject JoinningScreen;
        [SerializeField] private TextMeshProUGUI joinScreenText;

        private void Start()
        {
            CreateLobby();
        }

        private void OnDisable()
        {
            lobbyManager.LeaveLobby(lobbyManager.CurrentLobby.LobbyId);
        }

        private void Update()
        {
            if (lobbyManager.CurrentLobby.IsValid)
            {
                StartCoroutine(LoadLobby(2.0f));

                lobbyMemeberList.LobbyDataUpdate(lobbyManager.CurrentLobby);
                lobbyManager.PullFriends(friendList.filter);
            }
        }

        private void CreateLobby()
        {
            connectingScreen.SetActive(true);
            feedbackText.text = "Connecting";

            lobbyManager.CreateRoom();
        }

        private IEnumerator LoadLobby(float seconds)
        {
            feedbackText.text = "Connected";
            activeLobbyName.text = $"You are in <color=blue>{lobbyManager.CurrentLobby.Name}</color>";
            lobbyIdCode.text = $"Your room's ID: <br>{lobbyManager.CurrentLobby.LobbyId}";

            yield return new WaitForSeconds(seconds);

            connectingScreen.SetActive(false);
        }

        public void Join(TextMeshProUGUI roomId)
        {
            if(lobbyManager.CurrentLobby.IsValid)
                lobbyManager.LeaveLobby(lobbyManager.CurrentLobby.LobbyId);

            lobbyManager.JoinLobby(roomId.text);

            OnJoinningRoom(roomId.text);
        }

        public void OnJoinningRoom(Lobby room)
        {
            if (room.LobbyId != lobbyManager.CurrentLobby.LobbyId)
            {
                JoinningScreen.SetActive(true);
                joinScreenText.text = $"Joinning {room.Name}";
            }

            StartCoroutine(WaitToConnect(room.LobbyId));
        }

        public void OnJoinningRoom(string roomId)
        {
            if (roomId != lobbyManager.CurrentLobby.LobbyId)
            {
                JoinningScreen.SetActive(true);
                joinScreenText.text = $"Joinning...";
            }

            StartCoroutine(WaitToConnect(roomId));
        }

        private IEnumerator WaitToConnect(string roomId)
        {
            yield return new WaitUntil(() => lobbyManager.CurrentLobby.LobbyId == roomId);

            joinScreenText.text = "Joined!";
            JoinningScreen.SetActive(false);
            browseScreen.SetActive(false);
        }
    }
}