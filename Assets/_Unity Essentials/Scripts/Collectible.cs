using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // Destroy the collectible when the player collects it
            Destroy(gameObject);
            // Instantiate the onCollectEffect prefab
            Instantiate(onCollectEffect, transform.position, transform.rotation);
        }

    }

}
