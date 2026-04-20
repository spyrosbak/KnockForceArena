using PurrNet;
using PurrNet.Packing;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private Data data;

    [Serializable]
    private struct Data : IPackedAuto
    {
        public int testInt;
    }

    private void OnEnable()
    {
        InstanceHandler.NetworkManager.Subscribe<Data>(OnDataReceived);
    }

    private void OnDisable()
    {
        if(InstanceHandler.NetworkManager)
            InstanceHandler.NetworkManager.Unsubscribe<Data>(OnDataReceived);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (InstanceHandler.NetworkManager.isServer)
            {
                InstanceHandler.NetworkManager.SendToAll(data);
            }
            else
            {
                InstanceHandler.NetworkManager.SendToServer(data);
            }

            data.testInt++;
        }
    }

    private void OnDataReceived(PlayerID player, Data data, bool asServer)
    {
        Debug.Log($"Received data: {data.testInt.ToString()}");
    }
}