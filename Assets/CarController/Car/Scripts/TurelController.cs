using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurelController : MonoBehaviour
{
    public float mouseSensitivity = 600f;
    public float xRotation = 0f;

    public GameObject muzzle;
    public GameObject impact;
    public Transform spawnPoint;

    public float baseRotationSpeed;
    public float RotationSpeed;
    private float currentRotationSpeed;
    public Transform GameObject;
    public float distance = 15f;

    public float damage = 10f;
    public GameObject cam;

    private bool canShoot = true;
    private int shotsFired = 0;
    public float shootCooldown = 0.5f;

    private float lastInputTime = 0f;
    public float idleTimeBeforeAutoRotate = 3f;

    public bool oneGame = true;

    public CarController _carController;

    public AudioSource source;
    public AudioClip clip;

    public float interval = 0.0f;


    private float timerAudio = 0f;

    public AudioSource source2;
    public AudioClip clipRotating;
    public float intervalRotating = 0.0f;

    private bool inputReceived = false;

    private float rotationTime = 0f; // Время вращения для ускорения

    public float x;
    private float timer = 0f; // Timer for tracking time

    //public float mouseSensitivity = 0.1f; // Уменьшите значение чувствительности
    public float mouseSpeedMultiplier = 1f; // Коэффициент для управления скоростью

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentRotationSpeed = baseRotationSpeed;
    }

    void Update()
    {
        if (oneGame)
        {
            HandleMouseRotation();
            FireOnePlayer();
        }
        else if (!oneGame)
        {
            HandleTurretRotation();
            Fire();

            InputReceived();
            HandleAutoRotation();
        }

        
    }

    void HandleMouseRotation()
    {

        float mouseX = Input.GetAxis("Mouse X") * (-mouseSensitivity) * mouseSpeedMultiplier;

        if (mouseX != 0)
        {
            lastInputTime = Time.time; // Обновляем время последнего ввода
        }

        xRotation -= mouseX;
        transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);

        // Воспроизведение звука при вращении мышкой
        bool isRotating = mouseX != 0;

        if (isRotating && !source2.isPlaying)
        {
            source2.loop = true; // Устанавливаем звук в режим повторения
            source2.clip = clipRotating;
            source2.Play(); // Запускаем звук
        }
        else if (!isRotating && source2.isPlaying)
        {
            source2.Stop(); // Останавливаем звук, если вращение прекратилось
        }
    }


    void InputReceived()
    {
        if (inputReceived)
        {
            lastInputTime = Time.time; // Обновляем время последнего ввода

            // Увеличиваем время вращения при каждом нажатии, начиная с 0
            if (rotationTime < 100)
            {
                rotationTime += Time.deltaTime + x; // Увеличиваем время вращения
            }
        }
        else
        {
            rotationTime = 0f; // Сбрасываем время вращения сразу при отпускании клавиши
        }
    }
    void HandleTurretRotation()
    {
        inputReceived = false;

        // Увеличиваем скорость вращения с течением времени для эффекта ускорения
        currentRotationSpeed = RotationSpeed + rotationTime;

        bool isRotating = false; // Флаг, чтобы отслеживать, происходит ли вращение

        // Проверяем ввод для игрока 1
        if (_carController.PlayerOneControl)
        {
            if (Input.GetKey(KeyCode.U))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * currentRotationSpeed);
                inputReceived = true;
                isRotating = true; // Вращение происходит
            }

            if (Input.GetKey(KeyCode.Y))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * (-currentRotationSpeed));
                inputReceived = true;
                isRotating = true; // Вращение происходит
            }
        }

        // Проверяем ввод для игрока 2
        if (_carController.PlayerTwoControl)
        {
            if (Input.GetKey(KeyCode.O))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * currentRotationSpeed);
                inputReceived = true;
                isRotating = true; // Вращение происходит
            }

            if (Input.GetKey(KeyCode.U))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * (-currentRotationSpeed));
                inputReceived = true;
                isRotating = true; // Вращение происходит
            }
        }

        // Воспроизведение звука, если происходит вращение
        if (isRotating && !source2.isPlaying)
        {

            timerAudio += Time.deltaTime;

            if (timerAudio >= intervalRotating)
            {
                
                intervalRotating = 0f;
                source2.PlayOneShot(clipRotating);
                timerAudio = 1f;
                Debug.Log("NA NAHUI");
            }
        }
        else if (!isRotating)
        {


            intervalRotating = 0.0f;
            timerAudio = 1f;
            source2.Stop(); // Останавливаем звук, если вращение прекратилось
        }
    

        // Проверяем ввод для игрока 2
        if (_carController.PlayerTwoControl)
        {
            if (Input.GetKey(KeyCode.O))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * currentRotationSpeed);
                inputReceived = true;
            }

            if (Input.GetKey(KeyCode.U))
            {
                GameObject.Rotate(Vector3.up, Time.deltaTime * (-currentRotationSpeed));
                inputReceived = true;
            }

            else
            {
                inputReceived = false;
            }
        }
    }

    void HandleAutoRotation()
    {
        if (!inputReceived && Time.time - lastInputTime > idleTimeBeforeAutoRotate)
        {
            // Если прошло idleTimeBeforeAutoRotate секунд с последнего ввода, начинаем вращение к нулю по оси Y
            float currentYRotation = transform.localEulerAngles.y;
            Debug.Log("RR");
            // Плавно вращаем угол по Y к 0 со скоростью baseRotationSpeed (без ускорения)
            float targetYRotation = Mathf.MoveTowardsAngle(currentYRotation, 0f, baseRotationSpeed * Time.deltaTime);

            // Обновляем угол только по Y, остальные остаются неизменными
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetYRotation, transform.localEulerAngles.z);
        }
    }

    void FireOnePlayer()
    {
        if ((Input.GetButton("Fire1")) && canShoot)
        {
            spawnPoint.gameObject.SetActive(true);
            Shoot(); // Звук будет вызван в этом методе

            shotsFired++;
            if (shotsFired >= 1)
            {
                canShoot = false;
                shotsFired = 0;
                StartCoroutine(ShootCooldown());
            }
        }
    }

    void Fire()
    {
        if (Input.GetKey(KeyCode.T) && canShoot && _carController.PlayerOneControl)
        {
            spawnPoint.gameObject.SetActive(true);
            Shoot(); // Звук будет вызван в этом методе

            shotsFired++;
            if (shotsFired >= 1)
            {
                canShoot = false;
                shotsFired = 0;
                StartCoroutine(ShootCooldown());
            }

            inputReceived = true;
        }

        if (Input.GetKey(KeyCode.P) && canShoot && _carController.PlayerTwoControl)
        {
            spawnPoint.gameObject.SetActive(true);
            Shoot(); // Звук будет вызван в этом методе

            shotsFired++;
            if (shotsFired >= 1)
            {
                canShoot = false;
                shotsFired = 0;
                StartCoroutine(ShootCooldown());
            }

            inputReceived = true;
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void Shoot()
    {
        GameObject muzzleInstance = Instantiate(muzzle, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Fire");

        // Проигрываем звук выстрела
        source.PlayOneShot(clip);

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            Vector3 impactPoint;

            if ((hit.point - cam.transform.position).magnitude > 1.5f)
            {
                impactPoint = hit.point;
            }
            else
            {
                impactPoint = cam.transform.position + cam.transform.forward * 1.5f;
            }

            GameObject impactGO = Instantiate(impact, impactPoint, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void AudioFire()
    {
        Debug.Log("sssss");

        // Increase timer
        timer += Time.deltaTime;

        // If enough time has passed for the next shot
        if (timer >= interval)
        {
            interval = 2f;
            // Play shooting sound
            source.PlayOneShot(clip);

            // Reset timer
            timer = 0f;
        }
    }

    private void BreakeAudioFire()
    {
        interval = 0.0f;
        timer = 0f;
        source.Stop();  
    }
}
