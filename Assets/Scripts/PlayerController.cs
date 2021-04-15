using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isFiringLaserBeam = false;
    private bool isLaserBeamInstantiated = false;

    private bool isFiringLaserInCooldown = false;
    public float firingLaserCooldown = 2.0f;
    public bool infiniteLaserBeam = false;
    public Color infiniteLaserBeamColor;

    public GameObject LaserBeamBase;
    public GameObject LaserBeamBody;

    public GameObject simpleLaserPrefab;

    public float speed = 5.5f;

    public int laserBodyCount;

    public Transform laserOrigin;

    public Transform[] simpleLaserOriginsLevel1;
    public Transform[] simpleLaserOriginsLevel2;
    public Transform[] simpleLaserOriginsLevel3;
    public Transform[] simpleLaserOriginsLevel4;
    public Transform[] currentSimpleLaserOrigins;

    public Slider laserBeamSlider;
    public int laserBeamAmmo = 0;

    public int playerLevel = 1;
    public int playerEvolutionGauge = 0;
    public Slider playerLevelSlider;
    public Color maxLevelSliderColor;

    public bool canFireSimpleLaser = true;
    public float simpleLaserFireRate = .5f;

    public int life = 100;
    public Slider lifeSlider;

    // Start is called before the first frame update
    void Start()
    {
        laserBodyCount = Display.main.renderingHeight / 8;

        laserBeamSlider.value = laserBeamAmmo;

        currentSimpleLaserOrigins = simpleLaserOriginsLevel1;

        lifeSlider.value = life;

        playerLevelSlider.value = playerEvolutionGauge;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isGameRunning)
        {
            if (Input.GetKey(KeyCode.Mouse0) && canFireSimpleLaser)
            {
                FireSimpleLaser();
            }

            if(playerLevel > 1)
            {
                if (Input.GetKey(KeyCode.Mouse1) && !isFiringLaserInCooldown)
                {
                    StartFiringLaserBeam();
                }
                else if((Input.GetKeyUp(KeyCode.Mouse1) && isFiringLaserBeam))
                {
                    StopFiringLaserBeam();
                }
            }

            HandleMovement();

            if (isFiringLaserBeam)
            {
                CheckToKillEnemies();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameManager.instance.isGameRunning)
                {
                    GameManager.instance.PauseGame();
                }
                else
                {
                    GameManager.instance.UnPauseGame();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var other = collider.gameObject.GetComponent<ICollectible>();
        if (other != null)
        {
            other.Collect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject.GetComponent<Enemy>();
        if(other != null)
        {
            this.Damage(20);

            other.Die();

            StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(.3f, .1f));
        }
    }

    public void HandleMovement()
    {
        var inputVector = new Vector2();

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        if((transform.position.x > 8.5 && inputVector.x > 0) || (transform.position.x < -8.5 && inputVector.x < 0))
        {
            inputVector.x = 0;
        }

        if ((transform.position.y > 5 && inputVector.y > 0) || (transform.position.y < -2.8 && inputVector.y < 0))
        {
            inputVector.y = 0;
        }

        transform.Translate(inputVector * speed * Time.deltaTime);
    }

    public void FireSimpleLaser()
    {
        foreach(var origin in currentSimpleLaserOrigins)
        {
            Instantiate(simpleLaserPrefab, origin.transform.position, Quaternion.Euler(origin.eulerAngles));
        }

        canFireSimpleLaser = false;

        StartCoroutine(SimpleLaserFireRate());
    }

    public IEnumerator SimpleLaserFireRate()
    {
        yield return new WaitForSeconds(simpleLaserFireRate);

        canFireSimpleLaser = true;
    }

    public void StartFiringLaserBeam()
    {
        isFiringLaserBeam = true;

        if (!isLaserBeamInstantiated)
        {
            InstantiateLaserBeams();
        }

        if (!infiniteLaserBeam)
        {
            StartCoroutine(DeductLaserBeamAmmo());
        }
    }

    void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
    {
        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    public void CheckToKillEnemies()
    {
        var colliders = Physics2D.OverlapBoxAll(new Vector2(laserOrigin.position.x, laserOrigin.position.y + 5f), new Vector2(.5f, 10f), 0);

        DebugDrawBox(new Vector2(laserOrigin.position.x, laserOrigin.position.y + 5f), new Vector2(.5f, 10f), 0, Color.yellow, 5f);
        if(colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                var enemy = collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.Die();
                }
            }
        }
    }

    public void InstantiateLaserBeams()
    {
        isLaserBeamInstantiated = true;

        for (int i = 0; i <= laserBodyCount; i++)
        {
            if (i > 0)
            {
                var beamBodyPosition = new Vector2(0, laserOrigin.localPosition.y + (i * 8) - 1);

                var beamBody = Instantiate(LaserBeamBody, transform, false);

                beamBody.transform.localPosition = beamBodyPosition;
            }
            else
            {
                var beamBase = Instantiate(LaserBeamBase, transform, false);

                beamBase.transform.localPosition = new Vector2(laserOrigin.localPosition.x, laserOrigin.localPosition.y);
            }
        }
    }

    public void StopFiringLaserBeam()
    {
        isFiringLaserBeam = false;

        isFiringLaserInCooldown = true;

        StartCoroutine(firingLaserCooldownTimer());
    }

    public IEnumerator firingLaserCooldownTimer()
    {
        yield return new WaitForSeconds(firingLaserCooldown);

        isFiringLaserInCooldown = false;
    }

    public void DestroyInstantiatedLaserBeams()
    {
        var beams = gameObject.GetComponentsInChildren<BeamController>();

        foreach (var beam in beams)
        {
            beam.DestroySelf();
        }

        isLaserBeamInstantiated = false;
    }

    public void LevelUp()
    {
        playerLevel += 1;

        var playerAnim = GetComponent<Animator>();

        playerAnim.SetInteger("Level", playerLevel);

        playerEvolutionGauge = 0;

        switch (playerLevel)
        {
            case 1:
                currentSimpleLaserOrigins = simpleLaserOriginsLevel1;
                break;
            case 2:
                currentSimpleLaserOrigins = simpleLaserOriginsLevel2;
                break;
            case 3:
                currentSimpleLaserOrigins = simpleLaserOriginsLevel3;
                break;
            case 4:
                currentSimpleLaserOrigins = simpleLaserOriginsLevel4;
                infiniteLaserBeam = true;

                RefreshLaserBeamAmmo(100);
                laserBeamSlider.fillRect.gameObject.GetComponent<Image>().color = infiniteLaserBeamColor;
                playerLevelSlider.fillRect.gameObject.GetComponent<Image>().color = maxLevelSliderColor;
                break;
            default:
                GameManager.instance.WinScreen();
                break;
        }

        playerLevelSlider.value = playerEvolutionGauge;
    }

    public void Heal(int healValue)
    {
        life = Mathf.Clamp((life + healValue), 0, 100);

        lifeSlider.value = life;
    }

    public void Damage(int damageValue)
    {
        life = Mathf.Clamp((life - damageValue), 0, 100);

        if(life == 0)
        {
            GameManager.instance.GameOver();

            Destroy(gameObject);
        }

        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(.3f, .1f));

        lifeSlider.value = life;
    }

    public void AddEvolutionValue(int evolutionValue)
    {
        playerEvolutionGauge = Mathf.Clamp(playerEvolutionGauge + evolutionValue, 0, 100);

        playerLevelSlider.value = playerEvolutionGauge;

        if (playerEvolutionGauge == 100)
        {
            LevelUp();
        }
    }

    public void RefreshLaserBeamAmmo(int ammoRecovered)
    {
        laserBeamAmmo = Mathf.Clamp(laserBeamAmmo + ammoRecovered, 0, 100);

        laserBeamSlider.value = laserBeamAmmo;
    }

    public IEnumerator DeductLaserBeamAmmo()
    {
        while (isFiringLaserBeam)
        {
            laserBeamAmmo = Mathf.Clamp(laserBeamAmmo - 1, 0, 100);

            laserBeamSlider.value = laserBeamAmmo;

            yield return new WaitForSeconds(.3f);

            if(!(laserBeamAmmo > 0) && !infiniteLaserBeam)
            {
                StopFiringLaserBeam();

                StopCoroutine(DeductLaserBeamAmmo());
            }
        }

        StopCoroutine(DeductLaserBeamAmmo());
    }
}
