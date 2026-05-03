using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private CharId.ID characterId;
    [SerializeField] private Image checkmarkIcon;
    private InitializeLobby lobbyInitializer;

    private void Awake()
    {
        lobbyInitializer = FindFirstObjectByType<InitializeLobby>();
    }

    public void LockCharacter()
    {
        RefreshCharacterList();

        PlayerData.Instance.data.playableCharacterId = characterId;
        checkmarkIcon.gameObject.SetActive(true);
    }

    private void RefreshCharacterList()
    {
        lobbyInitializer.DeselectAllCharacters();
    }
}