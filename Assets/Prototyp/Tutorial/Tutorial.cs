using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject scareCrowParticle;
    public float distanceThreshold = 3f; // Adjust this threshold as needed

    public GameObject moveUI;
    public GameObject combatUI;

    // Move UI dismiss
    private bool wPressed;
    private bool aPressed;
    private bool sPressed;
    private bool dPressed;
    private bool spaceBarPressed;

    
    public UiScaleUpAndDown wUI;
    public UiScaleUpAndDown aUI;
    public UiScaleUpAndDown sUI;
    public UiScaleUpAndDown dUI;
    public UiScaleUpAndDown spaceBarUI;


    // Combat UI dismiss
    private bool leftMousePressed;
    private bool rightMousePressed;
    private bool shiftPressed;
    private bool rPressed;
    
    public UiScaleUpAndDown lMouse;
    public UiScaleUpAndDown RMouse;
    public UiScaleUpAndDown shift;
    public UiScaleUpAndDown r;



    public void Start()
    {
        moveUI.SetActive(true);
        scareCrowParticle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (scareCrowParticle.activeSelf)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, scareCrowParticle.transform.position);
            if (distanceToPlayer <= distanceThreshold)
            {
                // Player is close to the scarecrow particle
                scareCrowParticle.SetActive(false);
            }
        }
        
        float distanceToPlayer1 = Vector3.Distance(player.transform.position, scareCrowParticle.transform.position);
        if (distanceToPlayer1 <= distanceThreshold)
        {
            // Player is close to the scarecrow particle
            combatUI.SetActive(true);
        }

        // Move UI dismiss
        if (moveUI.activeSelf)
        {
            if (Input.GetKey(KeyCode.W))
            {
                wPressed = true;
                wUI.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.A))
            {
                aPressed = true;
                aUI.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.S))
            {
                sPressed = true;
                sUI.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.D))
            {
                dPressed = true;
                dUI.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                spaceBarPressed = true;
                spaceBarUI.PlayAnimation();
            }

            if (wPressed && aPressed && sPressed && dPressed && spaceBarPressed)
            {
                moveUI.SetActive(false);
            }
        }

        // Combat UI dismiss
        if (combatUI.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                leftMousePressed = true;
                lMouse.PlayAnimation();
            }

            if (Input.GetMouseButtonDown(1))
            {
                rightMousePressed = true;
                RMouse.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                shiftPressed = true;
                shift.PlayAnimation();
            }

            if (Input.GetKey(KeyCode.R))
            {
                rPressed = true;
                r.PlayAnimation();
            }

            if (leftMousePressed && rightMousePressed && shiftPressed && rPressed)
            {
                combatUI.SetActive(false);
            }
        }
    }

    public void ScareCrowNotificationOn()
    {
        scareCrowParticle.SetActive(true);
    }

    public void ScareCrowNotificationOff()
    {
        scareCrowParticle.SetActive(false);
    }

    public void CombatUIOff()
    {
        combatUI.SetActive(false);
    }

    public void MoveUIOff()
    {
        moveUI.SetActive(false);
    }
}
