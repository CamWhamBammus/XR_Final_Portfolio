using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameStateMachine : MonoBehaviour
{
    [Header("Components")]
    public GameObject[] chickens;
    private int chickensNum;

    public GameObject[] foxes;
    private int foxesNum;

    public GameObject boat;
    public int boatmax = 2;
    public int turnmax = 20;
    private int turns = 0;

    private bool loss = false;
    private bool win = false;

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();


    [Header("Shores")]
    private Dictionary<string, int> start = new Dictionary<string, int>()
    {
        {"foxes", 0},
        {"chickens", 0},
        {"boat", 1}
    };
    private Dictionary<string, int> goal = new Dictionary<string, int>()
    {
        {"foxes", 0},
        {"chickens", 0},
        {"boat", 0}
    };

    [Header("Boat")]
    public GameObject[] occupants = new GameObject[2];
    public GameObject[] seats = new GameObject[2];

    void Start()
    {
        chickensNum = chickens.Length;
        foxesNum = foxes.Length;

        start["foxes"] = foxesNum; start["chickens"] = chickensNum;

        RecordPositions();
    }

    private void RecordPositions()
    {
        foreach (var c in chickens)
        {
            if (c == null) continue;
            originalPositions[c] = c.transform.position;
            originalRotations[c] = c.transform.rotation;
        }

        foreach (var f in foxes)
        {
            if (f == null) continue;
            originalPositions[f] = f.transform.position;
            originalRotations[f] = f.transform.rotation;
        }
    }

    private IEnumerator ResetAnimalRoutine(GameObject animal)
    {
        AnimalGrabbedAnim animaltracker = animal.GetComponent<AnimalGrabbedAnim>();

        yield return new WaitWhile(() => animaltracker.pickedup);

        if (originalPositions.TryGetValue(animal, out var pos))
        {   
            animal.transform.position = pos;
            if (originalRotations.TryGetValue(animal, out var rot))
            {
                animal.transform.rotation = rot;
            }

        }
    }

    void Update()
    {
        LossOrWinCheck();
        if (win)
        {
            Debug.Log("[SATURN]: WIN REGISTERED");
            SceneManager.LoadScene("StartingScreen");
        } 

        if (loss)
        {
            Debug.Log("[SATURN]: LOSS REGISTERED");
            SwitchSceneScript.Instance.LoadGameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool shore = DetermineShore(); //true = start, false = goal
        GameObject otherGO = other.gameObject;

        if (other.gameObject.CompareTag("fox"))
        {
            if (!AddToBoat(otherGO))
            {
                StartCoroutine(ResetAnimalRoutine(otherGO));
                Debug.Log("[SATURN]: TRIED TO RESET FOX.");

                return;
            }

            if (shore)
            {
                start["foxes"] = start["foxes"] - 1;
                StartCoroutine(PlaceAnimal(otherGO));

                Debug.Log("[SATURN]: FOX REMOVED FROM START SHORE.");
            } else
            {
                goal["foxes"] = goal["foxes"] - 1;
                StartCoroutine(PlaceAnimal(otherGO));

                Debug.Log("[SATURN]: FOX REMOVED FROM GOAL SHORE.");
            }
        }

        if (other.gameObject.CompareTag("chicken"))
        {
            if (!AddToBoat(otherGO))
            {
                StartCoroutine(ResetAnimalRoutine(otherGO));
                Debug.Log("[SATURN]: TRIED TO RESET CHICKEN.");

                return;
            }

            if (shore)
            {
                start["chickens"] = start["chickens"] - 1;
                StartCoroutine(PlaceAnimal(otherGO));

                Debug.Log("[SATURN]: CHICKEN REMOVED FROM START SHORE");
            } else
            {
                goal["chickens"] = goal["chickens"] - 1;
                StartCoroutine(PlaceAnimal(otherGO));

                Debug.Log("[SATURN]: CHICKEN REMOVED FROM GOAL SHORE");
            }
        }

        if (other.gameObject.CompareTag("start"))
        {
            if (goal["boat"] == 1) turns++;

            start["boat"] = 1;
            goal["boat"] = 0;
            Debug.Log("[SATURN]: turn counted, moved (goal > start)");
        } 

        if (other.gameObject.CompareTag("goal"))
        {
            if (start["boat"] == 1) turns++;

            start["boat"] = 0;
            goal["boat"] = 1;
            Debug.Log("[SATURN]: turn counted, moved (start > goal)");
        }
    }

    private IEnumerator PlaceAnimal(GameObject animal)
    {
        foreach (GameObject seat in seats)
        {
            SeatTracker tracker = seat.GetComponent<SeatTracker>();
            AnimalGrabbedAnim animaltracker = animal.GetComponent<AnimalGrabbedAnim>();

            if (!tracker.SeatFilled)
            {
                yield return new WaitWhile(() => animaltracker.pickedup);

                animal.transform.position = seat.transform.position;
                
                tracker.SeatFilled = true;
                tracker.occupant = animal;

                Debug.Log("[SATURN]: seat enter triggered, correctly identified empties");
            }
        }
    }

    private void RemoveAnimal(GameObject animal)
    {
        foreach (GameObject seat in seats)
        {
            SeatTracker tracker = seat.GetComponent<SeatTracker>();
            if (tracker.occupant == animal)
            {
                tracker.SeatFilled = false;
                tracker.occupant = null;

                Debug.Log("[SATURN]: seat exit triggered, correctly identified idk");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool shore = DetermineShore(); //true = start, false = goal
        GameObject otherGO = other.gameObject;
        
        if (other.gameObject.CompareTag("fox"))
        {
            if (!RemoveFromBoat(otherGO)) return;
            if (shore)
            {
                start["foxes"] = start["foxes"] + 1;
                RemoveAnimal(otherGO);

                Debug.Log("[SATURN]: FOX ADDED TO START SHORE");
            } else
            {
                goal["foxes"] = goal["foxes"] + 1;
                RemoveAnimal(otherGO);

                Debug.Log("[SATURN]: FOX ADDED TO GOAL SHORE");
            }
        } 

        if (other.gameObject.CompareTag("chicken"))
        {
            if (!RemoveFromBoat(otherGO)) return;
            if (shore)
            {
                start["chickens"] = start["chickens"] + 1;
                Debug.Log("[SATURN]: CHICKEN ADDED TO START SHORE");
            } else
            {
                goal["chickens"] = goal["chickens"] + 1;
                Debug.Log("[SATURN]: CHICKEN ADDED TO GOAL SHORE");
            }
        }

        Debug.Log("START: " + start);
        Debug.Log("GOAL: " + goal);
    }

    private bool DetermineShore()
    {
        switch(true)
        {
            case true when start["boat"] == 1:
                return true;
            
            case true when goal["boat"] == 1:
                return false;
        }
        
        return true;
    }
    
    private bool AddToBoat(GameObject obj)
    {
        for (int i = 0; i < occupants.Length; i++)
        {
            if (occupants[i] == null)
            {
                occupants[i] = obj;
                return true;
            }
        }
        return false;
    }

    private bool RemoveFromBoat(GameObject obj)
    {
        for (int i = 0; i < occupants.Length; i++)
        {
            if (occupants[i] == obj)
            {
                occupants[i] = null;
                return true;
            }
        }

        return false;
    }

    private void LossOrWinCheck()
    {
        if ( (start["foxes"] > start["chickens"] && start["chickens"] != 0 && start["boat"] == 0) 
            || (goal["foxes"] > goal["chickens"] && goal["chickens"] != 0 && goal["boat"] == 0)
            || turns > turnmax)
        {
            loss = true;
            boat.SetActive(false);
        }

        if (start["foxes"] == 0 && start["chickens"] == 0 && start["boat"] == 0)
        {
            win = true;
        }
    }

}
