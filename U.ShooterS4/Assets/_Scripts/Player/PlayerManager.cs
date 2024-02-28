using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    public InputReader GetPlayerInputReader()
    {
        return inputReader;
    }
}
