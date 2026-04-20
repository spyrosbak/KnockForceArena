using PurrNet;
using UnityEngine;

public class RPCSender : NetworkBehaviour 
{
    private void Start()
    {
        //Message("Game Start");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetColor(Color.red);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetColor(Color.green);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetColor(Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetColor(Color.black);
    }

    [ServerRpc(requireOwnership:false)]
    private void SetColor(Color color, RPCInfo info = default)
    {
        //SetColorFromServer(color);
        
        SetColorToTarget(info.sender, color);
        Message("Game Start");
    }

    [ObserversRpc]
    private void SetColorFromServer(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    [TargetRpc]
    private void SetColorToTarget(PlayerID target, Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    [ObserversRpc]
    private void Message(string msg)
    {
        Debug.Log(msg);
    }
}