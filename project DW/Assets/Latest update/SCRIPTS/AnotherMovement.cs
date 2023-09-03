using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherMovement : MonoBehaviour
{
    [Header("Setup du character")]

    public CharacterController controller;  // character controller (composant a mettre sur objet unity)
    public Transform groundCheck; // Positions des pieds du Character
    public float groundDistance = 0.4f; // taille de la spherecast du groundcheck
    public LayerMask groundMask;  // Layer sur lequel on peut marcher et sauter
    public Animator headAnimator;
    public GameObject weaponHolder; // pour la taille des armes quand on saccroupis
    public Animator animFlingues;
    public LayerMask wallMask;
    public bool walling;
    public Transform head;
    public TMPro.TextMeshProUGUI speedText;
    public AudioSource walksound;
    public AudioSource Sprintsound;

    [Header("Movement")]

    public float speed;
    public float acceleration = 7f;
    public float deceleration = 20f;
    public float WalkSpeed = 6f;
    public float Sprintspeed = 9f;
    public float CrouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jump = 1f;
    public float Stamina = 4f;
    public float StaminaCounter;
    public float TempDeRecup = 5f;
    public float Recuperation;
    public float TrueGroundDelay = 1f;
    public float glideForce = 10f;
    public float slidingTimeLimit = 5f;
    public float knockbackSpeed = 5f;
    public bool IsKnockback = false;
    public float currentKnockbackTime = 0f;
    public float knockbackDuration = 2f;
    public Vector3 knockbackDirection;
    public bool wallJumped = false;
    public float wallDistance = 1f;
    public float walljumpKnockbackForce = 5f;
    public float walljumpForce = 10f;
    public float wjKnockbackTimer = 0f;
    public float wjKnockbackDuration = 0.5f;

    [Header("Actions")]

    public Vector3 Zvelocity; // vélocité z Du Character (juste pour voir le mouveent en l'air selon velocité z)
    public bool isGrounded;  // boule quand on est au sol
    public bool jumping;  // boule pour bunnyjump
    public bool moving; // savoir si on bouge avec wasd pour ne pas build de la vitesse avec bunny sans bouger
    public bool sprinting;  // lorsque que la touche sprint est utilisée
    public bool SprintUsed; // lorsque le character est fatigué
    public bool crouching; // lorsquon saccroupis
    public bool sliding; // is it sliding
    public bool TrueGrounded; // called after being on ground more then x times 

    //privata data

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);  // scale du joueur accroupis
    private Vector3 playerScale; // scale avant accroupissement
    private float StopSpeed = 0f; // vitesse à l'arret
    private float TrueGroundedTimer;
    private float slidingTime;
    private Vector3 move;

    private void Start()  // update is called at beginning blahblah
    {
        TrueGroundedTimer = 0;
        slidingTime = 0;
        StaminaCounter = Stamina;
        playerScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = speed.ToString();
        headAnimator.SetBool("Grounded", isGrounded);
        headAnimator.SetBool("Moving", moving);
        headAnimator.SetFloat("Speed", speed);
        animFlingues.SetFloat("speed", speed);
        animFlingues.SetBool("Moving", moving);
        animFlingues.SetBool("Grounded", isGrounded);
        InputsManager();
        Movement();
    }

    private void InputsManager() // Collecteur de données      
    {
        moving = Input.GetButton("Vertical") || Input.GetButton("Horizontal"); // si on se déplace, le bool déplacement s'active.
        jumping = Input.GetButton("Jump");  // quand jump est ou reste appuyé jumping = true
        sprinting = Input.GetKey(KeyCode.LeftShift); // quand shit est utilisé sprint activé
        crouching = Input.GetKey(KeyCode.LeftControl); // left ctrl pour s'accroupir
    }
    
    private void Movement()
    {

        // On verifie d'abord la position du character

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);  // on vérifie si on est sur le sol avec un spherecast depuis les pieds

        // Check for wall jump 

        walling = Physics.Raycast(head.position, transform.forward, wallDistance, wallMask);

        if (!isGrounded && jumping && !IsKnockback && !wallJumped && walling)
        {
            RaycastHit wallHit;
            if (Physics.Raycast(transform.position, transform.forward, out wallHit, wallDistance, wallMask))
            {
                wallJumped = true;
                Zvelocity.y = Mathf.Sqrt(walljumpForce * -2f * gravity);
            }
        }

        if (isGrounded)     // on verifie quon est bien au sol et pas en train de bunny
        {
            headAnimator.SetBool("AlreadyLanded", true);
            TrueGroundedTimer += Time.deltaTime;
            if (TrueGroundedTimer >= TrueGroundDelay)
            {
                TrueGrounded = true;
            }
        }

        if (!isGrounded)
        {
            headAnimator.SetBool("AlreadyLanded", false);
            TrueGrounded = false;
            TrueGroundedTimer = 0;
        }

        // Mouvement basic (marche)

        float x = Input.GetAxis("Horizontal");  // tranformation du input wasd en données
        float z = Input.GetAxis("Vertical");    // tranformation du input wasd en données

        Vector3 inputVector = new Vector3(x, 0f, z).normalized; // Normalize the input vector
        Vector3 move = transform.right * inputVector.x + transform.forward * inputVector.z; // Use the normalized input vector to determine movement direction
        controller.Move(move * speed * Time.deltaTime);  // Bouge le character sur le plan horizontal selon input

        if (wallJumped)
        {
            if (wjKnockbackTimer < wjKnockbackDuration)
            {
                controller.Move(-transform.forward * walljumpKnockbackForce * Time.deltaTime);
                wjKnockbackTimer += Time.deltaTime;
                if (wjKnockbackTimer >= wjKnockbackDuration)
                {
                    wallJumped = false;
                    wjKnockbackTimer = 0f;
                }
            }
        }

        if (IsKnockback)
        {
            speed = knockbackSpeed;
            currentKnockbackTime += Time.deltaTime;
            controller.Move(knockbackDirection * knockbackSpeed * Time.deltaTime);
            if (isGrounded && currentKnockbackTime < 0.2f || TrueGrounded && currentKnockbackTime < 0.2f)
            {
                Zvelocity.y = Mathf.Sqrt(jump * -2f * gravity);
            }

            if (currentKnockbackTime >= knockbackDuration)
            {
                Zvelocity.y = 0f;
                IsKnockback = false;
                currentKnockbackTime = 0f;  // The knockback has ended, so do any necessary cleanup or reset.
            }
        }

        // Saut et bunny jumping

        if (!isGrounded)
        {
            Zvelocity.y += gravity * Time.deltaTime; // accumulation de la gravité quand on est pas au sol
        }

        controller.Move(Zvelocity * Time.deltaTime);  // on bouge le character sur l'axe z selon la vélocité z


        if (jumping && isGrounded)  // si jumping = true && au sol, on saute (bunny)

        {
            Zvelocity.y = Mathf.Sqrt(jump * -2f * gravity);  //  vélocité y (altitude) = racine de la puissance saut fois la gravité
        }

        if (!isGrounded && moving)  // augmentation de la vitesse quand on fait du bunny

        {
            speed += 1 * Time.deltaTime;  // plus on est en l'air (bunny) plus on va vite
        }

        // Sprint, Fatigue et récupération

        if (sprinting && isGrounded && speed >= Sprintspeed && !SprintUsed)  // Sprint
        {
            StaminaCounter -= Time.deltaTime; // on diminue la stamina
            if (StaminaCounter <= 0)    // point de fatigue
            {
                Debug.Log("Plus de jus, récupération!");
                SprintUsed = true;  // on utilise sprintused
                Recuperation = 0; // reset du timer de récupération a 0
            }

        }

        if (SprintUsed) // Récupération
        {
            Recuperation += Time.deltaTime; // augmentation du délais de récupération
            if (Recuperation >= TempDeRecup) // lorsqu'on a récupéré,
            {
                SprintUsed = false;     // on déclare la fatigue terminée   
                StaminaCounter = Stamina;   // on remet la stamina au maximum
            }
        }

        // Crouching

        if (crouching)
        {
            weaponHolder.transform.localScale = new Vector3(1, 2f, 1);
            transform.localScale = crouchScale;
            if (speed > WalkSpeed + 1 && TrueGrounded && !sliding)      //super glide
            {
                speed += glideForce;
                sliding = true;
            }
            if (sliding)            // timer super glide quand on glisse sur le sol ( pas de spam superglide )
            {
                slidingTime += Time.deltaTime;
                if (slidingTime >= slidingTimeLimit)
                {
                    sliding = false;
                    slidingTime = 0;
                }
            }
        }

        if (TrueGrounded && speed > Sprintspeed)
        {
            speed -= deceleration * Time.deltaTime;
        }

        if (!crouching)
        {
            weaponHolder.transform.localScale = new Vector3(1, 1f, 1);
            transform.localScale = playerScale;

            sliding = false;
        }

        if (isGrounded && moving)
            {
            if (speed < 7)
            {
                if (!walksound.isPlaying)
                    walksound.Play();

                if (Sprintsound.isPlaying)
                    Sprintsound.Stop();
            }
            else
            {
                if (!Sprintsound.isPlaying)
                    Sprintsound.Play();

                if (walksound.isPlaying)
                    walksound.Stop();
            }
        }
        else
        {
            if (walksound.isPlaying)
                walksound.Stop();

            if (Sprintsound.isPlaying)
                Sprintsound.Stop();
        }

        if (isGrounded && Zvelocity.y < 0)
        {
            Zvelocity.y = -2f;  // on reset la gravité

            if (moving)
            {
                if (speed > WalkSpeed && !sprinting ||
                    SprintUsed && speed > WalkSpeed ||
                    speed > Sprintspeed && sprinting ||
                    crouching && speed > CrouchSpeed) // ralentissements en cas de vitesse supérieure 
                {
                    speed -= deceleration * Time.deltaTime;
                }
                else
                {
                    speed += acceleration * Time.deltaTime;
                }
            }
            if (!moving && speed > StopSpeed)
            {
                speed -= deceleration * 2 * Time.deltaTime;
            }
        }
    }
}