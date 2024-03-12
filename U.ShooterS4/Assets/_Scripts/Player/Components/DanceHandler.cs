using System;
using System.Collections;
using ShooterNetwork;
using UnityEngine;
using Random = UnityEngine.Random;

public class DanceHandler : NetworkBehaviour
{
    [SerializeField] private PlayerAnimatorController playerAnimatorController;
    [SerializeField] private InputReader inputReader;

    private void Start()
    {
        playerAnimatorController.OnDanceStart += OnDanceStart;
        playerAnimatorController.OnDanceEnd += OnDanceEnd;
    }

    private void OnDanceStart(int danceIndex)
    {
        inputReader.DisableGameplayInput();
        SendDancePacket(danceIndex);
    }

    private void OnDanceEnd()
    {
        inputReader.EnableGameplayInput();
    }

    public void Dance()
    {
        playerAnimatorController.PlayDance(GetRandomDanceIndex());
    }

    public int GetRandomDanceIndex()
    {
        return Random.Range(1, 5);
    }
}
