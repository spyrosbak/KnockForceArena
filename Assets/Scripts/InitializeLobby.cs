using UnityEngine;
using UnityEngine.UI;

public class InitializeLobby : MonoBehaviour
{
    [SerializeField] private Image[] characterSelectionIcon;
    [SerializeField] private GameObject starterCharacter;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //PlayerData.Instance.data.playableCharacterId = characterId;
        starterCharacter.SetActive(true);
    }

    public void DeselectAllCharacters()
    {
        foreach(var checkmark in characterSelectionIcon)
        {
            checkmark.gameObject.SetActive(false);
        }
    }
}