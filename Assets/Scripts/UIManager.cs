using UnityEngine;
using PurrNet;
using TMPro;

public class UIManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float counter = 10.0f;

    private void Update()
    {
        counter -= Time.deltaTime;
        
        ShowTimeServer(counter);
    }

    [ServerRpc(requireOwnership:false)]
    private void ShowTimeServer(float data)
    {
        ShowTime(data.ToString("F0"));
    }

    [ObserversRpc]
    private void ShowTime(string time)
    {
        timerText.text = time;
    }
}