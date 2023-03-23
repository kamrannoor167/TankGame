using UnityEngine;
using UnityEngine.UI;
using System;

public class TankHealth : MonoBehaviour
{
    public float StartingHealth = 100f;          
    public Slider Slider;                        
    public Image FillImage;                      
    public Color FullHealthColor = Color.green;  
    public Color ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    public GameObject shieldObject;
    public bool isShield = false;

    public static Action<GameCommands> SentCommand;


    private AudioSource ExplosionAudio;          
    private ParticleSystem ExplosionParticles;   
    private float CurrentHealth;  
    private bool isDead;
    
    private ResourceComponent resource;

  
    private void Awake()
    {
        ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        ExplosionAudio = ExplosionParticles.GetComponent<AudioSource>();
        ExplosionParticles.gameObject.SetActive(false);
        resource = GetComponent<ResourceComponent>();
        CurrentHealth = StartingHealth;
        isShield = false;
    }


    private void OnEnable()
    {
      
        isDead = false;
        SetTankHealthUI();
    }


    public void HealthDamage(float amount)
    {
        if (isShield)
            return;

        CurrentHealth -= amount;
        SetTankHealthUI();
        if(CurrentHealth <=0f && !isDead){
            switch (resource.type) {
                case ResourceType.Cacti:
                    ResourceHolder.Score += 1;
                    if(ResourceHolder.Score == 20)
                    {
                        SentCommand?.Invoke(GameCommands.GameWin);
                    }

                    PlayerDeath();
                    break;
  
            }

            PlayerDeath();
        }
    }

    private void SetTankHealthUI()
    {
        Slider.value = CurrentHealth;
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, CurrentHealth/StartingHealth);
    }


    private void PlayerDeath()
    {

        isDead = true;
        ExplosionParticles.transform.position=transform.position;
        ExplosionParticles.gameObject.SetActive(true);

        ExplosionParticles.Play();
        ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}