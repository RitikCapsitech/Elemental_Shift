using UnityEngine;

public enum ElementType { Water, Fire, Earth, Space, Air }

public class PlayerController : MonoBehaviour
{
    private float Startspeed = 3f;
    private float maxspeed = 10f;
    private float accelerationRate = 0.2f;
    public ElementType currentElement;

    public GameObject waterForm;
    public GameObject fireForm;
    public GameObject earthForm;
    public GameObject airForm;
    public GameObject spaceForm;

    private Rigidbody2D rb;
    private bool isDead = false;
    private float currentSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = Startspeed;
        SwitchElement(ElementType.Water);
    }

    void Update()
    {

        if (GameManager.Instance.IsGameStarted && !isDead)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, Startspeed, maxspeed);

            transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
        }
    }

    public void SwitchElement(ElementType newElement)
    {

        if (!GameManager.Instance.IsGameStarted) return;

        currentElement = newElement;

        waterForm.SetActive(false);
        fireForm.SetActive(false);
        earthForm.SetActive(false);
        airForm.SetActive(false);
        spaceForm.SetActive(false);

        switch (newElement)
        {
            case ElementType.Water: waterForm.SetActive(true); break;
            case ElementType.Fire: fireForm.SetActive(true); break;
            case ElementType.Earth: earthForm.SetActive(true); break;
            case ElementType.Air: airForm.SetActive(true); break;
            case ElementType.Space: spaceForm.SetActive(true); break;
        }
    }

    public void ResetPlayer()
    {
        isDead = false;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
        currentSpeed = Startspeed;
        SwitchElement(ElementType.Water);
    }

    public void KillPlayer()
    {
        isDead = true;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
        enabled = false;
    }

    public bool IsDead => isDead;
}
