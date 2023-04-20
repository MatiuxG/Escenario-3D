using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTPSController : MonoBehaviour
{
    public Camera cam;
    public UnityEvent onInteractionInput;
    private InputData input;
    private CharacterAnimBasedMovement characterMovement;

    public bool OnInteractionZone { get; set; }
    void Start()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
    }

    void Update()
    {
        // Get input from player
        input.getInput();

  

        if (OnInteractionZone)
        {
            onInteractionInput.Invoke();
        }
        else
        {
            // Apply input to character
            characterMovement.moveCharacter(input.hMovement, input.vMovement, cam, input.jump, input.dash);
        }
    }
}