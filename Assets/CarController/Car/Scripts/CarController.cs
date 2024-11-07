using UnityEngine;
using System;
using System.Collections.Generic;


public class CarController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public bool OnePlayer = true; // Determines if it's a one-player game
    

    public bool PlayerOneControl = true;
    public bool PlayerTwoControl = true;


    public int timer = 100;
    public ControlMode control;
    public float maxSpeed = 0f;
    public float maxMotorTorque = 600f;
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;
    public float speed;
    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;
    private bool isHandbrakePressed = false;
    private float handbrakeTimer = 0.0f;
    public float handbrakeDuration = 2.0f;

    public LayerMask groundLayer;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private CarLights carLights;

    public List<ParticleSystem> vihlopParticle;

    public float turboMaxSpeed = 1000f;
    public float turboDuration = 1.0f;
    public float turboGaugeMax = 100f;
    public float turboGauge;
    public float turboDrainRate = 100f;
    public float turboRefillRate = 50f;

    private bool isTurboActive = false;
    private float turboRefillTimer = 0f;

    public AudioSource source;
    public AudioClip clip;
    public float interval = 0.0f;

    private float timerAudio = 0f;

    public UpgradeCar _currentUpgradeCar;


    void Start()
    {
        maxSpeed = _currentUpgradeCar.maxSpeed;

        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        carLights = GetComponent<CarLights>();

        turboGauge = turboGaugeMax;


        turboMaxSpeed = maxSpeed + 10;

        foreach (var wheel in wheels)
        {
            SetWheelFriction(wheel.wheelCollider);
        }
    }

    void SetWheelFriction(WheelCollider wheelCollider)
    {
        WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
        sidewaysFriction.extremumValue = 2.5f;
        sidewaysFriction.asymptoteValue = 2.0f;
        sidewaysFriction.extremumSlip = 0.1f;
        sidewaysFriction.asymptoteSlip = 0.4f;
        wheelCollider.sidewaysFriction = sidewaysFriction;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        FixUpdate();
        GetInputs();
        AnimateWheels();
        ApplyHandbrake();
    }

    void FixUpdate()
    {
        foreach (var particle in vihlopParticle)
        {
            particle.Play();
            Debug.Log("Stop");
        }

        foreach (var wheel in wheels)
        {
            if (!isGrounded)
            {
                if (wheel.wheelEffectObj != null)
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                    wheel.smokeParticle.Emit(0);
                }
            }

            if (isGrounded)
            {
                if (carRb.velocity.magnitude < 0.8f)
                {
                    wheel.smokeParticle.Emit(0);
                }
                else
                {
                    wheel.smokeParticle.Emit(1);
                }
            }
        }
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            if (OnePlayer)
            {
                moveInput = Input.GetAxis("Vertical");
                steerInput = Input.GetAxis("Horizontal");
            }
            else
            {
                // Player 1 controls
                if (PlayerOneControl)
                {
                    moveInput = Input.GetAxis("Vertical");
                    steerInput = Input.GetAxis("Horizontal");
                }
                else
                {
                    moveInput = Input.GetAxis("VerticalP2");
                    steerInput = Input.GetAxis("HorizontalP2");
                }

                // Player 2 controls
                if (PlayerTwoControl)
                {
                    moveInput = Input.GetAxis("VerticalP2");
                    steerInput = Input.GetAxis("HorizontalP2");
                }
                else
                {
                    moveInput = Input.GetAxis("Vertical");
                    steerInput = Input.GetAxis("Horizontal");
                }
            }
        }
    }

    private void Move()
    {
        

        speed = carRb.velocity.magnitude;
        bool isMovingForward = Vector3.Dot(carRb.velocity, transform.forward) > 0;

        float minSpeedThreshold = 1.0f;

        // Handle turbo for Player 1
        if (OnePlayer || PlayerOneControl)
        {
            if (Input.GetKey(KeyCode.F) && turboGauge > 0 && isMovingForward && speed >= minSpeedThreshold && moveInput > 0)
            {
                ActivateTurbo();
            }
            else
            {
                RefillTurbo();
            }
        }

        // Handle turbo for Player 2
        if (!OnePlayer && PlayerTwoControl)
        {
            if (Input.GetKey(KeyCode.Semicolon) && turboGauge > 0 && isMovingForward && speed >= minSpeedThreshold && moveInput > 0)
            {
                ActivateTurbo();
            }
            else
            {
                RefillTurbo();
            }
        }

        float torque = (speed < maxSpeed && !isTurboActive) ? maxMotorTorque * moveInput * maxAcceleration * Time.deltaTime : 0;

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }

    private void ActivateTurbo()
    {
        isTurboActive = true;
        turboGauge -= turboDrainRate * Time.deltaTime;

        if (turboGauge < 0)
        {
            turboGauge = 0;
        }

        float targetSpeed = Mathf.Lerp(speed, turboMaxSpeed, turboDuration * Time.deltaTime);
        carRb.velocity = carRb.velocity.normalized * targetSpeed;

        Debug.Log("Turbo Boost Active: " + speed);

        turboRefillTimer = 0f;
    }

    private void RefillTurbo()
    {
        isTurboActive = false;
        turboRefillTimer += Time.deltaTime;

        if (turboRefillTimer >= 2f)
        {
            turboGauge += 5;

            if (turboGauge > turboGaugeMax)
            {
                turboGauge = turboGaugeMax;
            }

            turboRefillTimer = 0f;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float speedFactor = carRb.velocity.magnitude / 200f;
                float currentMaxSteerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngle / 1.5f, speedFactor);

                var _steerAngle = steerInput * turnSensitivity * currentMaxSteerAngle;

                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.8f);
            }
        }
    }

    void Brake()
    {
        if (OnePlayer)
        {
            if (Input.GetKey(KeyCode.Space) || moveInput == 0)
            {
                ApplyBrakes();
            }
            else
            {
                ReleaseBrakes();
            }
        }
        else
        {
            if (PlayerOneControl && (Input.GetKey(KeyCode.Space) || moveInput == 0))
            {
                ApplyBrakes();
            }
            else if (PlayerTwoControl && (Input.GetKey(KeyCode.Return) || moveInput == 0))
            {
                ApplyBrakes();
            }
            else
            {
                ReleaseBrakes();
            }
        }
    }

    private void ApplyBrakes()
    {
        if (isGrounded)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }

            carLights.isBackLightOn = true;
            carLights.OperateBackLights();
        }
    }

    private void ReleaseBrakes()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 0;
        }

        carLights.isBackLightOn = false;
        carLights.OperateBackLights();
    }

    void ApplyHandbrake()
    {
        if (OnePlayer)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                HandleHandbrake();
            }
            else
            {
                ReleaseHandbrake();
            }
        }
        else
        {
            if (PlayerOneControl && Input.GetKey(KeyCode.Space))
            {
                HandleHandbrake();
            }
            else if (PlayerTwoControl && Input.GetKey(KeyCode.Return))
            {
                HandleHandbrake();
            }
            else
            {
                ReleaseHandbrake();
            }
        }
    }

    private void ReleaseHandbrake()
    {
        if (isHandbrakePressed)
        {
            handbrakeTimer += Time.deltaTime;

            if (handbrakeTimer >= handbrakeDuration)
            {
                isHandbrakePressed = false;
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Rear)
                    {
                        SetWheelFriction(wheel.wheelCollider); // Восстанавливаем сцепление

                        // Выключаем визуальные эффекты заноса
                        if (wheel.wheelEffectObj != null)
                        {
                            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;

                            Debug.Log("AAAAA");

                            interval = 0.0f;
                            timerAudio = 0f;
                            source.Stop();
                        }
                    }
                }
            }
        }
    }

    private void HandleHandbrake()
    {
        if (isGrounded)
        {
            isHandbrakePressed = true;
            handbrakeTimer = 0.0f;

            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Rear)
                {
                    // Снижаем сцепление задних колес для эффекта заноса
                    WheelFrictionCurve sidewaysFriction = wheel.wheelCollider.sidewaysFriction;
                    sidewaysFriction.extremumSlip = 0.5f;
                    sidewaysFriction.asymptoteSlip = 0.8f;
                    wheel.wheelCollider.sidewaysFriction = sidewaysFriction;

                    // Включаем визуальные эффекты заноса
                    if (wheel.wheelEffectObj != null)
                    {

                        wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;


                        if (speed > 10)
                        {
                            timerAudio += Time.deltaTime;

                            if (timerAudio >= interval)
                            {
                                interval = 6f;
                                source.PlayOneShot(clip);
                                timerAudio = 0f;
                                Debug.Log("NA NAHUI");
                            }
                        }

                    }
                }
            }

            // Применяем торможение к задним колесам для снижения скорости
            Vector3 brakingForce = -carRb.velocity.normalized * 1000f * Time.deltaTime;
            carRb.AddForce(brakingForce, ForceMode.Acceleration);
        }
    }


    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;

            wheel.wheelCollider.GetWorldPose(out pos, out rot);

            if (isHandbrakePressed && wheel.axel == Axel.Rear)
            {
                // Устанавливаем положение, но не вращаем колесо, если активирован ручной тормоз
                wheel.wheelModel.transform.position = pos;
                continue; // Пропускаем установку вращения
            }

            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded && carRb.velocity.magnitude >= 10.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);

                
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;

                
            }
        }
    }
}
