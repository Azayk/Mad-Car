using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject muzzle;
    public Transform spawnPoint;

    public UpgradeCar _upgradeCar;

    public float health = 50f;

    private CarController carController; // Reference to CarController
    private TurelController turelController; // Reference to TurelController
    private MeshCollider meshCollider;
    private Rigidbody rigidbody;

    public float shootCooldown = 0.5f;
    private bool canShoot = true;

    public GameObject carLife;
    public GameObject carNoLife;

    public Transform spawnDamagePoint;
    public GameObject damagePrefab1; // Prefab for damage1
    public GameObject damagePrefab2; // Prefab for damage2
    public GameObject damagePrefab3; // Prefab for damage3

    private GameObject currentDamageEffect;
    private float lastHealth;

    void Start()
    {
        // Get references to the CarController and TurelController scripts
        carController = GetComponent<CarController>();
        turelController = GetComponentInChildren<TurelController>();
        meshCollider = GetComponent<MeshCollider>();
        rigidbody = GetComponent<Rigidbody>();

        health = _upgradeCar.health;

        lastHealth = health;

    }

    void Update()
    {
        FixUpdate();
    }

    void FixUpdate()
    {
        if (health != lastHealth)
        {
            lastHealth = health;

            if (health <= 300f && health > 200f)
            {
                if (currentDamageEffect != damagePrefab1)
                {
                    SpawnDamageEffect(damagePrefab1);
                }
            }
            else if (health <= 200f && health > 100f)
            {
                if (currentDamageEffect != damagePrefab2)
                {
                    SpawnDamageEffect(damagePrefab2);
                }
            }
            else if (health <= 100f && health > 0f)
            {
                if (currentDamageEffect != damagePrefab3)
                {
                    SpawnDamageEffect(damagePrefab3);
                }
            }
            else if (health <= 0f)
            {
                if (currentDamageEffect != null)
                {
                    Destroy(currentDamageEffect);
                    currentDamageEffect = null; // Clear reference to ensure no further effects
                }
            }
        }
    }

    void SpawnDamageEffect(GameObject damagePrefab)
    {
        // Destroy the current damage effect if it exists and is different from the new one
        if (currentDamageEffect != null && currentDamageEffect != damagePrefab)
        {
            Destroy(currentDamageEffect);
        }

        // Instantiate the new damage effect only if it's not the current one
        if (currentDamageEffect != damagePrefab)
        {
            currentDamageEffect = Instantiate(damagePrefab, spawnDamagePoint.position, Quaternion.identity, spawnDamagePoint);
            currentDamageEffect.transform.localPosition = Vector3.zero;
            currentDamageEffect.transform.localRotation = Quaternion.identity;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Instantiate muzzle effect
        GameObject muzzleInstance = Instantiate(muzzle, spawnPoint.position, spawnPoint.rotation);

        // Disable the CarController script if it exists
        if (carController != null)
        {
            carController.enabled = false;
        }

        // Disable the TurelController script if it exists
        if (turelController != null)
        {
            turelController.enabled = false;
        }

        // Disable the MeshCollider if it exists
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }

        // Make the Rigidbody kinematic if it exists
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }

        carNoLife.SetActive(true);
        carLife.SetActive(false);

        // Destroy the current damage effect
        if (currentDamageEffect != null)
        {
            Destroy(currentDamageEffect);
        }
    }
}