using UnityEngine;
using PurrNet;
using TMPro;

public class PlayerBehaiviour : NetworkBehaviour
{
    [SerializeField] private SyncVar<int> playerHealth = new SyncVar<int>(100, 0, true);
    [SerializeField] private TextMeshProUGUI healtText;

    [SerializeField] private string speach;

    private struct DataStruct
    {
        public string dialogString;
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;
    }

    private void Awake()
    {
        healtText.text = "100";
        playerHealth.onChanged += OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(int value)
    {
        healtText.text = value.ToString();
    }

    protected override void OnDespawned()
    {
        playerHealth.onChanged -= OnPlayerHealthChanged;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            playerHealth.value -= 10;

        if (Input.GetKeyDown(KeyCode.V))
        {
            var myData = new DataStruct()
            {
                dialogString = speach
            };

            Dialog(myData);
        }
    }

    [ObserversRpc]
    private void Dialog(DataStruct data)
    {
        Debug.Log($"{gameObject.name} says: {data.dialogString}");
    }
}