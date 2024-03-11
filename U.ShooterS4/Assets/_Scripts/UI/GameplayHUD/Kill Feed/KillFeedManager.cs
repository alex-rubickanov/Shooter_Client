using ShooterNetwork;
using UnityEngine;

public class KillFeedManager : MonoBehaviour
{
    [SerializeField] private KillFeedMessage killFeedMessgaePrefab;
    [SerializeField] private float messageLifeTime = 3f;

    public static KillFeedManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clear();
        Client.Instance.OnDeathPacketReceived += ReadDeathPacket;
    }

    private void ReadDeathPacket(DeathPacket packet)
    {
        string killerName = Client.Instance.GetPlayerNameByID(packet.KillerID.ToString());
        AddKill(killerName, packet.DataHolder.Name);
    }

    public void AddKill(string killer, string killed)
    {
        KillFeedMessage kfm = Instantiate(killFeedMessgaePrefab, transform);
        kfm.SetNames(killer, killed);
        Destroy(kfm.gameObject, messageLifeTime);
    }

    public void Clear()
    {
        KillFeedMessage[] messages = GetComponentsInChildren<KillFeedMessage>();
        for (int i = 0; i < messages.Length; i++)
        {
            Destroy(messages[i].gameObject);
        }
    }

    private void OnDestroy()
    {
        Client.Instance.OnDeathPacketReceived -= ReadDeathPacket;
    }
}