using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private GameObject gameOverPanel;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        gameOverPanel.SetActive(false);

    }

    private void Update()
    {
        if (isDead)
        {
            //If R is hit, restart the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                QuitGame();
            }
        }
    }
    
     public void QuitGame()
    {
        // save any game data here
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!isDead)
            {
                playDead();
            }
        }
    }
    private void playDead(){

        anim.SetTrigger("die");
        //Deactivate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = false;

        isDead = true;
        gameOverPanel.SetActive(true);
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    public void Kill(){
        currentHealth = 0;
        isDead = true;
        playDead();
    }
    public void AddLife()
    {
        startingHealth += 1;
        currentHealth = startingHealth;
    }
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
