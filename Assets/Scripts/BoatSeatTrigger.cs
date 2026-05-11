using UnityEngine;

public class BoatSeatTrigger : MonoBehaviour
{
    [Header("Boat Operation")]
    public SimpleVRRowBoat rowBoat;
    public string playerTag = "Player";

    [Header("Debug Management")]
    public GameObject boat;
    public GameObject boatsurfacecollider;
    public GameObject leftsphere;
    public GameObject rightsphere;

    [Header("Animal Movement")]
    public GameStateMachine gamestate;
    public GameObject chickens;
    public GameObject foxes;
    

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;

            
        }
    }

    void Update()
    {
        if (!playerInside || rowBoat == null) return;

        // A button (OVRInput.Button.One)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (!rowBoat.isInBoat)
            {
                rowBoat.EnterBoat();

                Rigidbody rb = boat.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;

                boatsurfacecollider.SetActive(false);
                
                leftsphere.SetActive(false);
                rightsphere.SetActive(false);

                foreach (GameObject occupant in gamestate.occupants)
                {
                    if (occupant != null)
                    {
                        occupant.transform.SetParent(boat.transform);
                    }
                }
            }
                
            else
            {
                rowBoat.ExitBoat();

                Rigidbody rb = boat.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeAll;

                boatsurfacecollider.SetActive(true);

                leftsphere.SetActive(true);
                rightsphere.SetActive(true);

                foreach (GameObject occupant in gamestate.occupants)
                {
                    if (occupant != null && occupant.CompareTag("chicken")) 
                    {
                        occupant.transform.SetParent(chickens.transform);
                    } else if (occupant != null && occupant.CompareTag("fox"))
                    {
                        occupant.transform.SetParent(foxes.transform);
                    }
                }
            }
            
        }
    }
}

