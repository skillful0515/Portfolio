using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public CharacterController myCharacterController;

    public float speed = 5.0f;
    public float jumpPower = 3.0f;
    public float gravity = 9.8f;

    public GameObject fpsCamera;
    public GameObject tpsCamera;

    public GameObject targetMarkerFps;
    public GameObject hitMarkerFps;
    public GameObject targetMarkerTps;
    public GameObject hitMarkerTps;

    public GameObject damageUi;

    public ParticleSystem shotParticle;
    public ParticleSystem hitParticle;

    public float attackTimeSpan = 0.5f;
    public Slider attackBar;
    public Image attackBarFillImage;
    public Color attackBarFullColor;

    public float slowPowerMax = 30.0f;
    public float slowPowerRiseSpeed = 1.0f;
    public float slowPowerDescentSpeed = 1.0f;
    public Slider slowBar;

    public GameObject clearText;

    public Animator anim;

    public float resultLoadTime = 3.0f;

    public GameSystem gameSystem;

    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip reloadClip;
    public AudioClip shotClip;

    public AudioClip falseClip;
    public AudioClip bodyClip;
    public AudioClip finClip;
    public AudioClip mouseClip;

    private float moveX, moveY, moveZ;

    private Camera fpsCameraComponent;
    private Camera tpsCameraComponent;
    private FpsCameraMove fpsCameraMove;
    private TpsCameraMove tpsCameraMove;

    private RaycastHit hit;
    private bool isFire = false;

    private float attackTime = 0;
    private Color attackBarDefaultColor;
    private float slowPower;

    private Vector3 rayPos;

    // Use this for initialization
    void Start()
    {
        fpsCameraComponent = fpsCamera.GetComponent<Camera>();
        tpsCameraComponent = tpsCamera.GetComponent<Camera>();
        fpsCameraMove = fpsCamera.GetComponent<FpsCameraMove>();
        tpsCameraMove = tpsCamera.GetComponent<TpsCameraMove>();

        hitParticle.transform.parent = null;

        attackBar.maxValue = attackTimeSpan;
        attackBarDefaultColor = attackBarFillImage.GetComponent<Image>().color;
        attackTime = attackTimeSpan;
        slowBar.maxValue = slowPowerMax;
        slowPower = slowPowerMax / 4.0f * 3.0f;

        clearText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSystem.gameOverText.activeSelf == false && gameSystem.countDownText.gameObject.activeSelf == false)
        {
            ChangeCamera();

            if (tpsCameraComponent.isActiveAndEnabled)
            {
                ActTpsMove();
            }
            else if (fpsCameraComponent.isActiveAndEnabled)
            {
                ActFpsMove();
            }

            ActAnim();

            attackTime += Time.deltaTime;
            if (attackTime >= attackTimeSpan)
            {
                if (attackBarFillImage.color == attackBarDefaultColor)
                {
                    attackBarFillImage.color = attackBarFullColor;
                    audioSource.PlayOneShot(reloadClip);
                }
            }
            else
            {
                attackBarFillImage.color = attackBarDefaultColor;
            }

            attackBar.value = attackTime;

            if (Input.GetButtonDown("Fire1") && attackTime >= attackTimeSpan)
            {
                isFire = true;
                shotParticle.Play();
                audioSource.PlayOneShot(shotClip);
                attackTime = 0;
            }

            ReturnStart();
        }

        if (gameSystem.gameOverText.gameObject.activeSelf == true)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        ActAttack();

        isFire = false;
    }

    private void ActAnim()
    {
        anim.SetFloat("Blend", new Vector2(moveX, moveZ).magnitude);
    }

    private void ActTpsMove()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(tpsCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        Vector3 moveForward = cameraForward * moveZ + tpsCamera.transform.right * moveX;

        if (Input.GetButtonDown("Jump") && myCharacterController.isGrounded)
        {
            moveY = jumpPower;

            anim.Play("Jump");
            audioSource.PlayOneShot(jumpClip);
        }

        if (!myCharacterController.isGrounded)
        {
            moveY -= gravity * Time.deltaTime;
        }

        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(moveForward.x, 0f, moveForward.z));
        }

        moveForward.y = moveY;

        myCharacterController.Move(moveForward * speed * Time.deltaTime);
    }

    private void ActFpsMove()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(fpsCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        Vector3 moveForward = cameraForward * moveZ + fpsCamera.transform.right * moveX;

        if (Input.GetButtonDown("Jump") && myCharacterController.isGrounded)
        {
            moveY = jumpPower;

            anim.Play("Jump");
            audioSource.PlayOneShot(jumpClip);
        }

        if (!myCharacterController.isGrounded)
        {
            moveY -= gravity * Time.deltaTime;
        }

        if (cameraForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(cameraForward.x, 0f, cameraForward.z));
        }

        moveForward.y = moveY;

        myCharacterController.Move(moveForward * speed * Time.deltaTime);
    }

    private void ChangeCamera()
    {
        if (fpsCameraMove.isActiveAndEnabled)
        {
            if (slowPower > 0)
            {
                slowPower -= slowPowerDescentSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (slowPower < slowPowerMax)
            {
                slowPower += slowPowerRiseSpeed * Time.deltaTime;
            }
        }

        slowBar.value = slowPower;

        if (Input.GetButtonDown("Focus") && slowPower > 0f)
        {
            fpsCameraMove.Head = tpsCameraMove.Head;
            fpsCameraMove.Pitch = tpsCameraMove.Pitch;

            tpsCameraComponent.enabled = false;
            tpsCameraMove.enabled = false;
            targetMarkerTps.SetActive(false);
            hitMarkerTps.SetActive(false);
            fpsCameraComponent.enabled = true;
            fpsCameraMove.enabled = true;
            targetMarkerFps.SetActive(true);
            //hitMarkerFps.SetActive(true);
        }
        if ((Input.GetButtonUp("Focus") || slowPower <= 0) && fpsCameraComponent.enabled)
        {
            tpsCameraMove.Head = fpsCameraMove.Head;
            tpsCameraMove.Pitch = fpsCameraMove.Pitch;

            fpsCameraComponent.enabled = false;
            fpsCameraMove.enabled = false;
            targetMarkerFps.SetActive(false);
            hitMarkerFps.SetActive(false);
            tpsCameraComponent.enabled = true;
            tpsCameraMove.enabled = true;
            targetMarkerTps.SetActive(true);
            //hitMarkerTps.SetActive(true);
        }
    }

    private void ActAttack()
    {
        if (tpsCameraComponent.isActiveAndEnabled)
        {
            MarkerActives(tpsCamera, hitMarkerTps);
        }
        else
        {
            MarkerActives(fpsCamera, hitMarkerFps);
        }
    }

    void MarkerActives(GameObject _camera, GameObject _marker)
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out hit, 15))
        {
            if (hit.collider.tag == "Enemy_Body")
            {
                if (_marker.activeSelf == false)
                {
                    _marker.SetActive(true);
                }

                if (isFire)
                {
                    ActDamage(200);
                    audioSource.PlayOneShot(bodyClip);
                }
            }
            else if (hit.collider.tag == "Enemy_Mouse")
            {
                if (_marker.activeSelf == false)
                {
                    _marker.SetActive(true);
                }

                if (isFire)
                {
                    ActDamage(1000);
                    audioSource.PlayOneShot(mouseClip);
                }
            }
            else if (hit.collider.tag == "Enemy_Fin")
            {
                if (_marker.activeSelf == false)
                {
                    _marker.SetActive(true);
                }

                if (isFire)
                {
                    ActDamage(500);
                    audioSource.PlayOneShot(finClip);
                }
            }
            else if(hit.collider.tag == "SlingShotMan")
            {
                if (_marker.activeSelf == false)
                {
                    _marker.SetActive(true);
                }

                if (isFire)
                {
                    gameSystem.AddScore(10000);
                    hit.transform.gameObject.SetActive(false);
                    audioSource.PlayOneShot(bodyClip);
                }
            }
            else
            {
                if (_marker.activeSelf == true)
                {
                    _marker.SetActive(false);
                }
            }

            if (isFire)
            {
                hitParticle.Stop();
                hitParticle.transform.position = hit.point;
                hitParticle.Play();

                audioSource.PlayOneShot(falseClip);
            }

            rayPos = hit.point;
        }
        else
        {
            if (_marker.activeSelf == true)
            {
                _marker.SetActive(false);
            }
        }
    }

    private void ActDamage(float _damage)
    {
        // パーティクル起動
        //shotParticle.Play();

        // ダメージ処理
        ManbouMove manbouMove = hit.collider.gameObject.GetComponentInParent<ManbouMove>();
        manbouMove.TakeDamage(_damage);
        DamageUIMove damageObejct = Instantiate(damageUi).GetComponent<DamageUIMove>();
        damageObejct.transform.position = hit.point + Vector3.up * 2f;
        damageObejct.GetComponentInChildren<Text>().text = "" + _damage;
        damageObejct.SetNowCamera = GetNowCamera;

        // スコア加算
        gameSystem.AddScore((int)_damage);
    }

    public Transform GetNowCamera
    {
        get
        {
            if (tpsCameraComponent.isActiveAndEnabled)
            {
                return tpsCamera.transform;
            }
            else
            {
                return fpsCamera.transform;
            }
        }
    }

    // 万が一、奈落へ落下した場合の処理
    private void ReturnStart()
    {
        if (transform.position.y < -100f)
        {
            moveY = 0;
            transform.position = new Vector3(0, 10f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            if (clearText.activeSelf == false)
            {
                StartCoroutine("ResultLoad");
                clearText.SetActive(true);
                gameSystem.isClear = true;
            }
        }
    }

    private IEnumerator ResultLoad()
    {
        yield return new WaitForSeconds(resultLoadTime);

        SceneManager.LoadScene("InputFormScene");
    }
}
