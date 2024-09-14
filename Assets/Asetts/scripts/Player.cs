using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private PlayerMovement playerMovement;
    private AimAndTeleport aimAndTeleport;

    private void Start()
    {
        playerInputs = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
        aimAndTeleport = GetComponent<AimAndTeleport>();
    }
}
