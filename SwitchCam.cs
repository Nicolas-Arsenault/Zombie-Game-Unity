using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    [Header("Camera à assigner")]
    public GameObject camVise;
    public GameObject canvaVise;
    public GameObject camTroisemeP;
    public GameObject camTroisemePCanvas;

    [Header("Cam Animator")]
    public Animator animator;


    public Transform playerTransform;
    public float rotationSpeed = 10f;

    private void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W))
        {

            //Désactiver le viseur et la cam
            camTroisemeP.SetActive(false);
            camTroisemePCanvas.SetActive(false);

            //Activer l'autre viseur
            camVise.SetActive(true);
            canvaVise.SetActive(true);
           // RotatePlayerOppositeToCamera();
        }
        else if(Input.GetButton("Fire2"))
        {

            //Désactiver le viseur et la cam
            camTroisemeP.SetActive(false);
            camTroisemePCanvas.SetActive(false);

            //Activer l'autre viseur
            camVise.SetActive(true);
            canvaVise.SetActive(true);
            //RotatePlayerOppositeToCamera();
        }
        else
        {


            //Activer le viseur et la cam
            camTroisemeP.SetActive(true);
            camTroisemePCanvas.SetActive(true);

            //Désactiver l'autre viseur
            camVise.SetActive(false);
            canvaVise.SetActive(false);
        }
    }

	/**private void RotatePlayerOppositeToCamera()
	{
		// Get the camera's forward direction (ignoring the Y axis to prevent tilting)
		Vector3 cameraForward = camVise.transform.forward;
		cameraForward.y = 0; // Ensure the player only rotates horizontally
		cameraForward.Normalize(); // Normalize to ensure consistent rotation speed

		// Calculate the direction the player should face (opposite of the camera's forward)
		Vector3 targetDirection = -cameraForward; // Opposite direction

		// Calculate the target rotation
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

		// Smoothly rotate the player towards the target rotation
		playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}**/
}
