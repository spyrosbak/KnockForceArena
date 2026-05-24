using PurrNet;
using PurrNet.Prediction;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private PlayerSpawner spawner;

    //[Header("UI")]
    //[SerializeField] private TextMeshProUGUI playerNameText;

    //private CharId.ID characterId;

    private void Awake()
    {
        //if(networkManager.players.Count <= 1)
        //networkManager.StartServer();

        //networkManager.StartClient();

        //characterId = PlayerData.Instance.data.playableCharacterId;
        //playerNameText.text = PlayerData.Instance.data.playerName;

        
    }

    [ObserversRpc]
    public void SetCharacter()
    {
        spawner.playerPrefab = PlayerData.Instance.data.character;
    }

    //private void Start()
    //{
    //    networkManager.StartServer();

    //    var player = spawner._playerPrefab;

    //    switch (characterId)
    //    {
    //        case CharId.ID.PC1:
    //            player.GetComponent<PlayerCore>().EnableCharacter(1);
    //            break;
    //        case CharId.ID.PC2:
    //            player.GetComponent<PlayerCore>().EnableCharacter(2);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void Start()
    {
        SetCharacter();
    }
}