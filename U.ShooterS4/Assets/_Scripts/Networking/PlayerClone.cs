using System;
using ShooterNetwork;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private PlayerData cloneData;
    private CloneMovement cloneMovement;

    private void Awake()
    {
        cloneMovement = GetComponent<CloneMovement>();
    }

    private void OnEnable()
    {
        Client.Instance.OnMovePacketReceived += MoveClone;
    }

    private void OnDisable()
    {
        Client.Instance.OnMovePacketReceived -= MoveClone;
    }

    private void MoveClone(MovePacket packet)
    {
        if (packet.DataHolder.ID != cloneData.ID) return;
        cloneMovement.Move(new UnityEngine.Vector2(packet.Position.X, packet.Position.Y));
        cloneMovement.Rotate(packet.RotationY);
    }

    public void SetCloneData(PlayerData data)
    {
        cloneData = data;
    }
}