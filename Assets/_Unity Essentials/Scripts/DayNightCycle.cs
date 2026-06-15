using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day/Night Settings")]
    [Tooltip("How many real-world seconds it takes to complete one full day.")]
    [Min(1f)]
    public float dayLengthInSeconds = 300f;

    private float degreesPerSecond;

    private void Start()
    {
        degreesPerSecond = 360f / dayLengthInSeconds;
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, degreesPerSecond * Time.deltaTime, Space.Self);
    }

    private void OnValidate()
    {
        dayLengthInSeconds = Mathf.Max(1f, dayLengthInSeconds);
        degreesPerSecond = 360f / dayLengthInSeconds;
    }
}