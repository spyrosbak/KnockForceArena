using PurrNet;
using PurrNet.Prediction;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private PredictedPlayerSpawner spawner;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI playerNameText;

    //private CharId.ID characterId;

    private void Awake()
    {
        if(networkManager.players.Count <= 1)
            networkManager.StartServer();

        networkManager.StartClient();

        //characterId = PlayerData.Instance.data.playableCharacterId;
        playerNameText.text = PlayerData.Instance.data.playerName;
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
}