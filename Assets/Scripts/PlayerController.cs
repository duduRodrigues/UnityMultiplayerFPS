using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    
    [SerializeField]
    private float thrusterForce = 1300f;


    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;
    
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    void Update()
    {
        // ==========================================
        // Calculate movement velocity as a 3D vector
        // ==========================================
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        // Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        // Apply movement
        motor.Move(_velocity);

        // ==================================================
        // Calculate rotation as a 3D vector (turning around)
        // ==================================================
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        // Apply rotation
        motor.Rotate(_rotation);

        // ========================================
        // Calculate camera rotation as a 3D vector
        // ========================================
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotation = _xRot * lookSensitivity;

        // Apply rotation
        motor.RotateCamera(-_cameraRotation);

        // ==================================================
        // Calculate the thurster force based on player input
        // ==================================================
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };


    }
}
