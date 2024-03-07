using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] private float moveSmoothTime = 0.1f;
    
    public void Move(float x, float y)
    {
        transform.position = new Vector3(x, 0, y);
    }
}
