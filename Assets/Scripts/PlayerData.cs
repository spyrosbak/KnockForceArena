using PurrLobby;
using PurrNet.Prediction;
using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private LobbyManager lobbyManager;
    public static PlayerData Instance;
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    [Serializable] public struct Data
    {
        public CharId.ID playableCharacterId;
        public string playerName;
    }
    public Data data;

    [ContextMenu("Read Data")]
    public void ReadPlayerData()
    {
        lobbyManager = FindFirstObjectByType<LobbyManager>();
        
        if (lobbyManager == null)
            return;

        Lobby room = lobbyManager.CurrentLobby;
        var localUserId = lobbyManager.CurrentProvider.GetLocalUserIdAsync().Result;
        var user = room.Members.Find(member => member.Id == localUserId);
        
        data.playerName = user.DisplayName;

        Debug.Log("Written");
    }
}