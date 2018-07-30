using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    Rigidbody _rigidbody;
    Transform _transform;
    Vector3[] wheelCenters;
    RaycastHit hit;
    WheelCollider[] wheelColls;
    Transform centerOfMassTransform;
    Transform[] wheelMeshes;

    float v;
    float h;
    bool brakeButton;

    [TextArea(4, 5)]
    public string information = "ORDER:" 
        + "\n" + "1-4.   Four wheel meshes." 
        + "\n" + "5.      Center of mass." 
        + "\n" + "6.      Car body. " 
        + "\n" + "7-10.  Four wheel colliders.";

    [Header("Speed / acceleration")]
    public float carTorque = 300f;
    [Header("Angle / Wheel rotation speed")]
    public float carAngle = 30;
    [Header("Stop / brake speed")]
    public float brakeSpeed = 10000f;
    [Header("Front (2/3) or all (4) drive")]
    [Range(2, 4)]
    public int carDrive = 2;

    [Header("Suspension Settings")]
    [Range(0, 20)]
    public float naturalFrequency = 10;
    [Range(0, 3)]
    public float dampingRatio = 0.8f;
    [Range(-1, 1)]
    public float forceShift = 0.03f;
    public bool setSuspensionDistance = true;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        centerOfMassTransform = _transform.GetChild(4);
        _rigidbody.centerOfMass = centerOfMassTransform.localPosition;
        wheelColls = GetComponentsInChildren<WheelCollider>();
        wheelCenters = new Vector3[wheelColls.Length];
        GetMeshes();
    }

    void Update()
    {
        Inputs();
        ConfigureCenterOfMass();
        Move(v, brakeButton);
        Steer(h);
        UpdateWheels();
        Suspension();
    }

    void Inputs()
    {
        v = carTorque * Input.GetAxisRaw("Vertical");
        h = carAngle * Input.GetAxisRaw("Horizontal");
        brakeButton = Input.GetButton("Jump");
    }

    void Move(float torque, bool brake)
    {
        for (int i = 0; i < carDrive; i++)
        {
            wheelColls[i].motorTorque = torque;

            if (brake)
            {
                wheelColls[i].brakeTorque = brakeSpeed;
            }
            else
            {
                wheelColls[i].brakeTorque = 0;
            }
        }
    }

    void Suspension()
    {
        foreach (WheelCollider wc in GetComponentsInChildren<WheelCollider>())
        {
            JointSpring spring = wc.suspensionSpring;

            spring.spring = Mathf.Pow(Mathf.Sqrt(wc.sprungMass) * naturalFrequency, 2);
            spring.damper = 2 * dampingRatio * Mathf.Sqrt(spring.spring * wc.sprungMass);

            wc.suspensionSpring = spring;

            Vector3 wheelRelativeBody = transform.InverseTransformPoint(wc.transform.position);
            float distance = GetComponent<Rigidbody>().centerOfMass.y - wheelRelativeBody.y + wc.radius;

            wc.forceAppPointDistance = distance - forceShift;

            if (spring.targetPosition > 0 && setSuspensionDistance)
                wc.suspensionDistance = wc.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
        }
    }

    void Steer(float angle)
    {
        wheelColls[0].steerAngle = angle;
        wheelColls[1].steerAngle = angle;

        wheelMeshes[0].localEulerAngles = new Vector3(wheelMeshes[0].localEulerAngles.x, angle, wheelMeshes[0].localEulerAngles.z);
        wheelMeshes[1].localEulerAngles = new Vector3(wheelMeshes[1].localEulerAngles.x, angle, wheelMeshes[1].localEulerAngles.z);
    }

    void UpdateWheels()
    {
        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            wheelCenters[i] = wheelColls[i].transform.TransformPoint(wheelColls[i].center);

            if (Physics.Raycast(wheelCenters[i], -wheelColls[i].transform.up, out hit, wheelColls[i].suspensionDistance + wheelColls[i].radius))
            {
                wheelMeshes[i].position = hit.point + (wheelColls[i].transform.up * wheelColls[i].radius);
            }
            else
            {
                wheelMeshes[i].position = wheelCenters[i] - (wheelColls[i].transform.up * wheelColls[i].suspensionDistance);
            }

            wheelMeshes[i].Rotate(0, -wheelColls[i].rpm * carAngle / 5 * Time.deltaTime, 0);
        }
    }

    void GetMeshes()
    {
        wheelMeshes = new Transform[wheelColls.Length];
        for (int i = 0; i < wheelColls.Length; i++)
        {
            wheelMeshes[i] = _transform.GetChild(i);
        }
    }

    void ConfigureCenterOfMass()
    {
        _rigidbody.centerOfMass = centerOfMassTransform.localPosition;
    }

    void Skid(WheelCollider[] wheels, int w)
    {
        WheelHit hit;
        if (wheels[w].GetGroundHit(out hit))
        {
            //float vol = (Mathf.Abs(hit.sidewaysSlip) > 0.25f) ? hit.sidewaysSlip / 5 : 0;
        }
    }
}
