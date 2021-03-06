using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float jumpForce;
    public float hitStrenghtMultiplier = 2.0f; //Le multiplicateur de force que le joueur possède
    public float climbForce;
    public GameObject npc_prefab;

    public Rigidbody2D bulletPrefab;
    
    private bool GunInPocket { get; set; }
    

    protected bool isJumping;
    public bool IsJumping
    {
        get { return isJumping; }
        set { isJumping = value; }
    }

    protected bool isOnGround;
    public bool IsOnGround
    {
        get { return isOnGround; }
        set { isOnGround = value; }
    }

    
    protected bool isCrouching;
    public bool IsCrouching
    {
        get { return isCrouching; }
        set { isCrouching = value; }
    }

    protected bool isRunning;
    public bool IsRunning
    {
        get { return isRunning; }
        set { isRunning = value; }
    }

    protected bool isAttacking;
    public bool IsAttacking
    {
        get { return IsAttacking; }
        set { isAttacking = value; }
    }

    protected bool isClimbing;
    public bool IsClimbing
    {
        get { return isClimbing; }
        set { isClimbing = value; }
    }

    protected bool goUp;
    public bool GoUp
    {
        get { return goUp; }
        set { goUp = value; }
    }

    protected bool gunArmed;
    public bool GunArmed
    {
        get { return gunArmed; }
        set { gunArmed = value; }
    }
    
    protected bool hasGun;
    public bool HasGun
    {
        get { return hasGun; }
        set { hasGun = value; }
    }

    

    /* Notre réference vers notre manager d'animation : BIEN SEPARER LE PLAYER CONTROLLER DE LANIMATION CONTROLLER !*/
    private AnimationManager animationManager;
    private PlayerHealth playerHealth;
    private Collider2D innerCollider;
    private float colorDelta;
    private Rigidbody2D rigidbody2D;
    private Transform transform;
    private AudioSource audioSource;
    private float sprintMultiplier;
    private float crouchMultiplier;
    private float gravityScale;
    private PlayerAttackEnum playerAttackEnum;
    private Vector2 mouseWorld;
    private Vector2 mousePosScreen;
    private float direction;
    public float Direction
    {
        get { return direction; }
    }


    private Vector2 positionBeforeClimb;

    public AudioClip[] audioClips;

    public GameObject weapon { get; set; }



    // Start is called before the first frame update
    void Start()
    {

        if (GetComponent<Rigidbody2D>() != null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            gravityScale = rigidbody2D.gravityScale;

        }

        if (GetComponent<Transform>() != null)
        {
            transform = GetComponent<Transform>();
        }

        if (GetComponent<AnimationManager>() != null)
        {
            this.animationManager = GetComponent<AnimationManager>();
        }

        if (GetComponent<PlayerHealth>() != null)
        {
            this.playerHealth = GetComponent<PlayerHealth>();
        }
        if (GetComponent<AudioSource>() != null)
        {
            this.audioSource = GetComponent<AudioSource>();
        }

        if (GetComponent<PlayerAttackEnum>() != null)
        {
            this.playerAttackEnum = GetComponent<PlayerAttackEnum>();
        }

        InitStickManColor();

        

    }

    private void InitStickManColor()
    {
        Component[] SpriteMesh = GetComponentsInChildren<Anima2D.SpriteMeshInstance>();

        foreach (Anima2D.SpriteMeshInstance spritemesh in SpriteMesh)
        {
            spritemesh.color = Color.red;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "floor" || col.gameObject.tag == "crate") //Doit être remplacé par un tag platform 
        {
            isOnGround = true;
            animationManager.getOnGroundAnimation();
        }

    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Climbable")
            isClimbing = true;

        //On récupere le collider qui rentre en collision avec un tel objet : Utilisé pour gérer les collider des membres, attributs du joueur.
        innerCollider = col;

        

        if (col.gameObject.tag != "floor" && (col.gameObject.tag == "NPC" || col.gameObject.tag == "Arena"))
        {
            

            if (animationManager.getIsAttacking())
            {

                col.gameObject.GetComponent<NPCHealthBar>().takeDamage(playerAttackEnum.PlayerAttackType, transform.forward.z);
                audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
                audioSource.Play();
                Debug.Log("col : " + innerCollider.ToString());
            }
        }
    }



    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Climbable")
        {
            isClimbing = false;
            rigidbody2D.gravityScale = gravityScale;
        }
    }
    

    private void handleMoletDroit(Transform childOfRightLeg)
    {

    }

    private void handleCuisseDroite(Transform childOfRightLeg)
    {

    }

    private void handleMoletGauche(Transform childOfLeftLeg)
    {
        Debug.Log("Handle handleMoletGauche !");
    }

    private void handleCuisseGauche(Transform childOfLeftLeg)
    {
        Debug.Log("Handle handleCuisseGauche !");
    }

    private void handleAvantBrasDroit(Transform child4)
    {
        Debug.Log("Handle handleAvantBrasDroit !");
    }

    private void handleBrasDroit(Transform child4)
    {
        Debug.Log("Handle handleBrasDroit !");

    }

    private void handleAvantBrasGauche(Transform child4)
    {
        Debug.Log("Handle handleAvantBrasGauche !");
    }

    private void handleBrasGauche(Transform child4)
    {
        Debug.Log("Handle handleBrasGauche !");
    }

    private void handleHead(Transform child3)
    {
        Debug.Log("Handle handleHead !");
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        isAttacking = animationManager.getIsAttacking();


        if (direction > 0 && !gunArmed)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

            if (!isCrouching)
                animationManager.startWalking();


            else
                animationManager.startCrouchWalking();


            GetComponent<Transform>().Translate(GetComponent<Transform>().right * 10.05f * Time.deltaTime * sprintMultiplier * crouchMultiplier);
        }

        else if (direction < 0 && !gunArmed)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

            if (!isCrouching)
                animationManager.startWalking();

            else
                animationManager.startCrouchWalking();


            GetComponent<Transform>().Translate(GetComponent<Transform>().right * (-10.05f) * Time.deltaTime * sprintMultiplier * crouchMultiplier);
        }


        if (direction == 0)
        {
            if (isCrouching)
                animationManager.stopCrouchWalking();

            else
                animationManager.stopWalking();
        }



        if (Input.GetAxis("Vertical") < 0)
        {
            animationManager.setCrounching();
            isCrouching = true;

        }

        else if (Input.GetAxis("Vertical") > 0)
        {
            goUp = true;
        }

        else
        {
            goUp = false;

            if (!isClimbing)
            {
                animationManager.standUp();
                animationManager.stopCrouchWalking();
                isCrouching = false;
            }
        }
        
        if(!TutoPanelHandler.isInTutorial)
        {
            HandleInput();
        }

        if (isRunning)
            sprintMultiplier = 2.0f;

        else
            sprintMultiplier = 1.0f;


        if (isCrouching)
            crouchMultiplier = 0.5f;

        else
            crouchMultiplier = 1.0f;

        

        if (goUp && isClimbing)
        {
            animationManager.startClimbing();
            positionBeforeClimb = transform.position;

            if ((GunInPocket && hasGun) || !hasGun)
            {
                transform.position = new Vector3(positionBeforeClimb.x, transform.position.y + climbForce, 0);
                
                rigidbody2D.gravityScale = 0.0f;
            }
        }
        else if (isClimbing && !goUp)
        {
            animationManager.pauseClimbing();
            

        }

        else
        {
            animationManager.stopClimbing();
            replaceWeaponInHand();
        }
    }



    void FixedUpdate()
    {
        if (isJumping)
        {
            animationManager.JumpAnimation();
            this.rigidbody2D.AddForce(new Vector2(0.0f, jumpForce));

            isOnGround = false;
            isJumping = !isJumping;

        }

    }



    void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || isClimbing))
        {
            animationManager.TriggerTakeOff();
            isJumping = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && isOnGround)
        {
            animationManager.TriggerCrouching();
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouching = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isOnGround)
        {
            animationManager.startRunning();
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isOnGround)
        {
            isRunning = false;
            animationManager.stopRunning();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isAttacking = true;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("j'ai frapper au poingts en bas");
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.Log("j'ai frapper au poingts en haut");
                animationManager.uppercutAnimation();
                playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.uppercut;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("j'ai frapper au poingts a droite");
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("j'ai frapper au poingts a gauche");
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            isAttacking = true;
            Debug.Log("j'ai frapper au poingts normal");
            animationManager.punchAnimation();
            playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.punch;

        }


        if (Input.GetKeyDown(KeyCode.U))
        {
            animationManager.uppercutAnimation();
            playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.uppercut;
        }
            

        if (Input.GetKeyDown(KeyCode.L))
        {
            animationManager.lowKickAnimation();
            playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.lowkick;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {

            if(isOnGround)
            {
                animationManager.kickAnimation();
            }
            else
            {
                animationManager.startFlyingKick();
                playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.flyingKick;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("j'ai frapper en bas");
                animationManager.lowKickAnimation();
                playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.lowkick;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.Log("j'ai frapper en haut");
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("j'ai frapper a droite");
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("j'ai frapper a gauche");
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && hasGun && !gunArmed)
        {
            animationManager.armToFire();
            gunArmed = true;
            weapon.GetComponent<WeaponManager>().haveToLaunchBullets = true;
        }
        
        else if(Input.GetKeyUp(KeyCode.F))
        {
            animationManager.stopFire();
            gunArmed = false;
            weapon.GetComponent<WeaponManager>().stopSound();
            weapon.GetComponent<WeaponManager>().haveToLaunchBullets = false;
        }


        if (Input.GetKeyDown(KeyCode.K) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("j'ai frapper normal");
            //playerAttackEnum.PlayerAttackType = PlayerAttackEnum.PlayerAttack.kick;
            animationManager.kickAnimation();

        }


        if(Input.GetMouseButtonDown(0) && !TutoPanelHandler.isInTutorial)
        {
             mousePosScreen = Input.mousePosition;
             mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePosScreen.x, mousePosScreen.y, 0));

            GameObject npc = Instantiate(npc_prefab) as GameObject;
            npc.transform.position = new Vector3(mouseWorld.x, mouseWorld.y, 0);

            Component[] SpriteMesh = npc.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
            Color randColor = UnityEngine.Random.ColorHSV();


            foreach (Anima2D.SpriteMeshInstance spritemesh in SpriteMesh)
            {
                /*TODO changer le sprite de couleur (blanc)*/
                spritemesh.color = Color.yellow;
            }
        }
    }

   
    public void takeDamage(PlayerAttackEnum.PlayerAttack npcAttackType, float dir)
    {
        int ammountDamage = 0;

        if (GetComponent<AudioSource>() != null)
        {
            audioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }

        switch (npcAttackType)
        {
            case PlayerAttackEnum.PlayerAttack.punch:
                ammountDamage = 1;
                break;
            case PlayerAttackEnum.PlayerAttack.uppercut:
                rigidbody2D.AddForce(new Vector2(100f * dir, 5000f));
                ammountDamage = 4;
                break;
            case PlayerAttackEnum.PlayerAttack.kick:
                rigidbody2D.AddForce(new Vector2(1000.0f * dir, 2200f));
                ammountDamage = 5;
                break;
            case PlayerAttackEnum.PlayerAttack.lowkick: ammountDamage = 4; break;
            case PlayerAttackEnum.PlayerAttack.flyingKick: ammountDamage = 10; break;
        }

        this.playerHealth.TakeDamage(ammountDamage);
    }

    /*private void OnGUI()
    {

        GUILayout.BeginArea(new Rect(20, 20, 250, 150));
        GUILayout.Label("Player world pos : " + transform.position.ToString());
        GUILayout.Label("Player is climbing : " + isClimbing);
        GUILayout.Label("Player is jumping : " + isJumping);
        GUILayout.Label("Player is running : " + isRunning);
        GUILayout.Label("Player is onGround : " + isOnGround);
        GUILayout.Label("direction player transform : " + transform.forward.z);
        GUILayout.EndArea();
    }*/


    public void playSoundForWeapon()
    {
        weapon.GetComponent<WeaponManager>().playSound();
        shotRaycast();
    }

    private void shotRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.GetChild(0).position, weapon.transform.right);

        if (hit)
        {
            if (hit.transform.tag == "Destroyable")
                hit.transform.GetComponent<DestroyableController>().takeDamage(100);
        }
    }

    public void playReloadSound()
    {
        weapon.GetComponent<WeaponManager>().playReloadSound();
    }

    public void placeHandGunInPocket()
    {
        GunInPocket = true;
        weapon.transform.SetParent(GameObject.Find("HandGunPocket").transform);
        weapon.transform.localPosition = new Vector2(0, 0);
    }

    public void placeLoudWeaponInBack()
    {
        GunInPocket = true;
        weapon.transform.SetParent(GameObject.Find("LoudWeaponBack").transform);
        weapon.transform.localPosition = new Vector2(0, 0);
    }

    private void replaceWeaponInHand()
    {
        if (weapon)
        {
            weapon.transform.SetParent(GameObject.Find("MainDroite").transform);
            weapon.transform.localPosition = new Vector2(0, 0);
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            GunInPocket = false;
        }
    }
}

