using UnityEngine;

public class SimpleVRRowBoat : MonoBehaviour
{
    private Vector3 prevPaddlePos;

    // A or B = which bank the boat is currently at
    [SerializeField] private string currentBank = "A";

    [Header("Refs")]
    public Rigidbody boatRb;
    public Transform boatTransform;
    public Transform paddleTransform;
    public Transform xrRigRoot;
    public GameObject paddle;

    [Header("Rowing")]
    public float forceMultiplier = 2f;
    public float minBackwardSpeed = 0.2f;

    [Header("Water")]
    public float waterLevelY = 7f;
    public float minSubmerge = 0.02f;

    [Header("Seat")]
    [Tooltip("Set by code; do not modify in Inspector.")]
    public bool isInBoat = false;
    public Vector3 rigOffSet = new Vector3(0, 0.2f, 0);  // roughly eye height above boat

    [Header("Banks")]
    [SerializeField] private GameObject bankA;
    [SerializeField] private GameObject bankB;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    private bool firstFrame = true;

    private AudioSource audioSource;
    [SerializeField] private AudioClip splashClip;

    private bool soundReady = false;

    [SerializeField] private Transform riverPlane;
    private float amplitude = 0.3f;
    private float frequency = 1f;
    private float baseHeight;
    private float boatWaterOffset;
    

    public void Awake()
    {
        bankA.transform.position = new Vector3(bankA.transform.position.x, boatTransform.position.y, bankA.transform.position.z);
        bankB.transform.position = new Vector3(bankB.transform.position.x, boatTransform.position.y, bankB.transform.position.z);
        audioSource = GetComponent<AudioSource>();
        baseHeight = riverPlane.position.y;
        boatWaterOffset = boatTransform.position.y - riverPlane.position.y;

    }
    public void EnterBoat()
    {
        if (currentBank == "A")
        {
            boatTransform.LookAt(bankB.transform);
        }
        else
        {
            boatTransform.LookAt(bankA.transform);
        }
        boatTransform.Rotate(0, 180, 0, Space.World);
        paddle.SetActive(true);
        isInBoat = true;
    }

    public void ExitBoat()
    {
        paddle.SetActive(false);
        isInBoat = false;
        if (Mathf.Abs((bankA.transform.position - boatTransform.position).magnitude) < Mathf.Abs((bankB.transform.position - boatTransform.position).magnitude))
        {
            xrRigRoot.position = bankA.transform.position;
            currentBank = "A";
        }
        else
        {
            xrRigRoot.position = bankB.transform.position;
            currentBank = "B";
        }
    }

    public void Update()
    {
        if (isInBoat == true)
        {
            xrRigRoot.position = boatTransform.position + rigOffSet;
            
        }

        if (currentBank == "A" && Mathf.Abs((bankB.transform.position - boatTransform.position).magnitude) < 10)
        {
            boatRb.linearVelocity = Vector3.zero;
        }
        if (currentBank == "B" && Mathf.Abs((bankA.transform.position - boatTransform.position).magnitude) < 10)
        {
            boatRb.linearVelocity = Vector3.zero;
        }
    }

    public void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.fixedTime * frequency) * amplitude;
        float currentWaterY = baseHeight + offset;
        boatTransform.position = new Vector3(boatTransform.position.x, currentWaterY + boatWaterOffset, boatTransform.position.z);
        Vector3 pos = riverPlane.position;
        pos.y = baseHeight + offset;
        riverPlane.position = pos;
        if (paddleTransform.position.y < riverPlane.position.y && isInBoat)
        {   
            if (soundReady == true)
            {
                PlaySplash();
            }
            soundReady = false;
            if (firstFrame == false)
            {
                newPosition = paddleTransform.position;
                Vector3 velocity = (newPosition - oldPosition) / Time.deltaTime;
                float magnitude = velocity.magnitude; 
                ApplyForceTowardsCurrentBank(magnitude);
                oldPosition = newPosition;
            }
            else
            {
                oldPosition = paddleTransform.position;
                firstFrame = false;
            }    
        }
        else
        {
            soundReady = true;
        }
    }

    public void ApplyForceTowardsCurrentBank(float magnitude)
    {
        Vector3 direction;
        
        if (currentBank == "A")
        {
            direction = (bankB.transform.position - boatTransform.position).normalized;
        }
        else
        {
            direction = (bankA.transform.position - boatTransform.position).normalized;
        }
        Vector3 force = direction * magnitude;
        boatRb.AddForce(force, ForceMode.Force);
    }

    public void PlaySplash()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(splashClip, Random.Range(0.3f, 0.5f));
    }
}    