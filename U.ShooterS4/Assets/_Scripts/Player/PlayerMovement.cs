using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed;


    private Vector2 moveInputVector;
    private void Start()
    {
        inputReader.OnMoveEvent += ReadMoveInputVector;
    }

    private void Update()
    {
        HandleMove();
    }

    private void ReadMoveInputVector(Vector2 moveInputVector)
    {
        this.moveInputVector = moveInputVector;
    }

    private void HandleMove()
    {
        Vector3 moveDirection = new Vector3(moveInputVector.x, 0.0f, moveInputVector.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Debug.Log(moveDirection);
    }
}