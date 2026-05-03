using PurrNet.Prediction;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private GameObject[] characterModels;

    public void EnableCharacter(int charid)
    {
        foreach(var go in characterModels)
            go.SetActive(false);

        characterModels[charid - 1].SetActive(true);
    }
}