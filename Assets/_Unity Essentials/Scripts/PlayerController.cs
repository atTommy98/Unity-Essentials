using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves forward/backward and rotates with WASD/Arrow keys.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Variables for the movement and rotation
    private float currentSpeed = 0.0f;
    private Vector2 moveInput;
    [Tooltip("Forward/back speed (units/sec).")]
    public float speed = 5.0f;
    [Tooltip("Turn speed (degrees/sec).")]
    public float rotationSpeed = 120.0f;
    [Tooltip("Jump force (units/sec).")]
    public float jumpForce = 5.0f;

    // Reference to the AudioSource component
    [Tooltip("Control UFO volume")]
    private AudioSource ufoAudio;
    public float maxVolume = 1.0f;
    public float volumeChangeRate = 0.1f;
    public float moveThreshHold = 0.1f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOriginalPosition;
    [SerializeField] private float cameraBobAmplitude = 0.05f;
    [SerializeField] private float cameraBobFrequency = 1.5f;
    [SerializeField] private float cameraSmooth = 6f;


    private Rigidbody rb;

    private void Start()
    {
        // Remember the original position of the camera
        cameraOriginalPosition = cameraTransform.localPosition;
        // Initialize the AudioSource component
        if (ufoAudio == null)
        {
            ufoAudio = GetComponent<AudioSource>();
        }
        ufoAudio.loop = true;
        ufoAudio.Play();
        ufoAudio.volume = 0.0f;


        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("PlayerController needs a Rigidbody.");
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        currentSpeed = moveInput.magnitude;
        float targetVolume = currentSpeed > moveThreshHold ? maxVolume : 0.0f;

        ufoAudio.volume = Mathf.Lerp(ufoAudio.volume, targetVolume, volumeChangeRate * Time.deltaTime);

        // Bobbing motion
        float moveFactor = Mathf.Clamp01(currentSpeed);

        // Camera bobbing motion
        float cameraBobOffset = Mathf.Sin(Time.time * cameraBobFrequency)
                                * cameraBobAmplitude
                                * moveFactor;

        Vector3 targetLocalPos = cameraOriginalPosition;
        targetLocalPos.y += cameraBobOffset;

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetLocalPos,
            Time.deltaTime * cameraSmooth
        );
    }

    private void FixedUpdate()
    {
        moveInput = Vector2.zero;

        // Forward/backward
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1f;

        // Left/right (rotation)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1f;

        // Move in facing direction 
        Vector3 movement = transform.forward * moveInput.y * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Y-axis rotation (invert when going backwards)
        float turnDirection = moveInput.x;
        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        float turn = turnDirection * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
