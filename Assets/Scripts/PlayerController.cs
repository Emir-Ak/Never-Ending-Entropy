using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float minSpeed = 175f;
    public float maxSpeed = 300;
    float currentSpeed;

    public float bulletSpeed;
    public float fireRate;

    float shootingCounter;

    public float maxHealth;
    float currentHealth;

    public float regenAmount;
    private Rigidbody2D rb;

    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject blackHolePrefab;
    [SerializeField] 
    Transform firingPos;
    
    [SerializeField]
    Stat barCurrentHealth;

    [SerializeField]
    private TMP_Text barValueText;

    Vector2 movementDirection;
    Vector2 mousePosition;

    [SerializeField]
    GameObject light;
    [SerializeField]
    GameObject triggerField;

    bool isShooting = false;
    bool isInvulnerable = false;
    bool isRegenerating = false;
    public bool hasWeapon = false;
    public bool hasLight = false;
    float blackHoleCounter;
    public float blackHoleRate = 8f;
    public float blackHoleSpeed = 2.5f;
    public bool hasEntropy = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shootingCounter = fireRate + 1;
        blackHoleCounter = blackHoleRate + 1;
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
    }
    private void Start()
    {

        barCurrentHealth.Initialize();
        barCurrentHealth.CurrentVal = currentHealth;
        barValueText.text = ((int)currentHealth).ToString();
    }

    private void Update()
    {
        GetInput();
        if (Input.GetMouseButton(0) && hasWeapon)
        {
            isShooting = true;
            if (shootingCounter >= fireRate)
            {
                Shoot(bulletPrefab, bulletSpeed);
                shootingCounter = 0;
            }
        }
        else
            isShooting = false;
        if (isShooting && currentSpeed != minSpeed)
            currentSpeed = minSpeed;
        else if (!isShooting && currentSpeed != maxSpeed)
            currentSpeed = maxSpeed;

        if (shootingCounter < fireRate)
            shootingCounter += Time.deltaTime;


        if (Input.GetMouseButtonUp(1) && hasEntropy)
        {
            if(blackHoleCounter >= blackHoleRate)
            {
                Shoot(blackHolePrefab, blackHoleSpeed);
                blackHoleCounter = 0;
            }
        }
        if (blackHoleCounter < blackHoleRate)
            blackHoleCounter += Time.deltaTime;

        if (currentHealth <= 0)
        {
            barValueText.text = "DEAD";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        if (!isRegenerating && currentHealth <= maxHealth)
        {
            StartCoroutine(Regenerate(1f, regenAmount));
            isRegenerating = true;
        }

        if (hasLight)
        {
            light.SetActive(true);
            triggerField.SetActive(false);
            hasLight = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rayDistance"></param>
    /// <param name="direction"></param>
    /// <param name="layerMaskToCheckAgainst"> Layer you want to detect</param>
    /// <returns></returns>
    bool CheckShit(float rayDistance,Vector2 direction,int layerMaskToCheckAgainst)
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position,direction,rayDistance,layerMaskToCheckAgainst);
        if (ray)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Shoot(GameObject prefab, float force)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPos.position, firingPos.rotation);
        bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
    }
    void GetInput()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Move()
    {
        movementDirection.Normalize();
        rb.velocity = movementDirection * Time.fixedDeltaTime * currentSpeed;
    }

    void Rotate()
    {
        Vector2 lookingDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public void ReceiveDamage(float damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            isInvulnerable = true;
            Invoke("ResetVulnerability", 0.35f);
            Debug.Log(currentHealth);
            barCurrentHealth.CurrentVal = currentHealth;
            barValueText.text = ((int)currentHealth).ToString();
        }
    }

    void ResetVulnerability()
    {
        isInvulnerable = false;
    }

    private IEnumerator Regenerate(float delay, float regenAmount)
    {

        yield return new WaitForSeconds(delay);
        bool hasFinishedRegenerating = false;
        while (!hasFinishedRegenerating)
        {
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
                hasFinishedRegenerating = true;;
                break;
            }

            currentHealth += regenAmount;
            barCurrentHealth.CurrentVal = currentHealth;
            barValueText.text = ((int)currentHealth).ToString();
            yield return new WaitForSeconds(0.2f);

        }

        isRegenerating = false;
    }
}
