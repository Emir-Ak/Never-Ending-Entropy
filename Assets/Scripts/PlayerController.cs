using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float minSpeed = 175f;
    public float maxSpeed = 300;
     float currentSpeed;
    public float bulletSpeed;
    public float fireRate;
    float counter;
    public float maxHealth;
    float currentHealth;
    private Rigidbody2D rb;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firingPos;

    Vector2 movementDirection;
    Vector2 mousePosition;

    Coroutine shootingCoroutine;

    bool isShooting = false;
    bool isInvulnerable = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        counter = fireRate + 1;
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        GetInput();
        if (Input.GetMouseButton(0))
        {
            isShooting = true;
            if (counter >= fireRate)
            {
                Shoot();
                counter = 0;
            }
        }

        else
            isShooting = false;
         if(isShooting && currentSpeed != minSpeed)
            currentSpeed = minSpeed;
         else if(!isShooting && currentSpeed != maxSpeed)
            currentSpeed = maxSpeed;

        if (counter < fireRate)
            counter += Time.deltaTime;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
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

    void Shoot()
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
        }
    }

    void ResetVulnerability()
    {
        isInvulnerable = false;
    }
}
