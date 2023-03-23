using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;             
    public Rigidbody shellRigidbody;                   
    public Transform fireTransform;          
    public Slider aimSlider;                  
    public AudioSource shootingAudio;       
    public AudioClip chargingClip;            
    public AudioClip fireClip;                
    public float minLaunchForce = 15f;        
    public float maxLaunchForce = 30f;        
    public float maxChargeTime = 0.75f;

    private string fireButton;                
    private float currentLaunchForce;        
    private float chargeSpeed;              
    private bool isFired;
   
    public int currentAmmoCount = 10;

 

 
    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        ResourceHolder.CurrentAmmo = currentAmmoCount;
 
    }


    private void Start ()
    {
        fireButton = "Fire" + playerNumber;

        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }




    private void Update ()
    {
        aimSlider.value = minLaunchForce;

        if (currentLaunchForce >= maxLaunchForce && !isFired)
        {
            currentLaunchForce = maxLaunchForce;
            Shooting ();
        }
        else if (Input.GetButtonDown (fireButton))
        {
            isFired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play ();
        }
        else if (Input.GetButton (fireButton) && !isFired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp (fireButton) && !isFired )
        {
            Shooting ();
        }

    }


 
    private void Shooting()
    {
        if (currentAmmoCount >= 1)
        {

            isFired = true;

            currentAmmoCount -= 1;
            ResourceHolder.CurrentAmmo = currentAmmoCount;
            Rigidbody shellInstance =
              Instantiate(shellRigidbody, fireTransform.position, fireTransform.rotation) as Rigidbody;

            shellInstance.velocity = currentLaunchForce * fireTransform.forward; ;

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;
        }
    }

}