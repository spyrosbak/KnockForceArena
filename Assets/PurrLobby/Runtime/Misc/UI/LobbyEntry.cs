using TMPro;
using UnityEngine;

namespace PurrLobby
{
    public class LobbyEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text lobbyNameText;
        [SerializeField] private TMP_Text playersText;

        private Lobby _room;
        private LobbyManager _lobbyManager;
        private ConnectionManager _connectionManagaer;

        private void Awake()
        {
            _connectionManagaer = FindFirstObjectByType<ConnectionManager>();
        }

        public void Init(Lobby room, LobbyManager lobbyManager)
        {
            lobbyNameText.text = room.Name.Length > 0 ? room.Name : room.LobbyId;
            playersText.text = $"{room.Members.Count}/{room.MaxPlayers}";
            _room = room;
            _lobbyManager = lobbyManager;
        }

        public void OnClick()
        {
            if (_room.LobbyId != _lobbyManager.CurrentLobby.LobbyId)
            {
                if(_lobbyManager.CurrentLobby.IsValid)
                    _lobbyManager.LeaveLobby(_lobbyManager.CurrentLobby.LobbyId);

                _lobbyManager.JoinLobby(_room.LobbyId);
                _connectionManagaer.OnJoinningRoom(_room);
            }
            else
            {
                Debug.Log("You are already in this lobby");
            }
        }
    }
}
