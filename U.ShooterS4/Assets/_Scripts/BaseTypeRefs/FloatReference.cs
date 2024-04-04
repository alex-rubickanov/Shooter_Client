using UnityEngine;

[CreateAssetMenu(menuName = "Base Type Refs/Float Reference", fileName = "New FloatReference")]
public class FloatReference : ScriptableObject
{
    public float Value = 0.5f;
    public float MinValue = 0.1f;
    public float MaxValue = 1.0f;
}
