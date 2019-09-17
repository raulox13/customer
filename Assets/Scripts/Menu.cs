using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    [SerializeField] private float minSpeed = 1f, additionSpeed = 1f, maxSpeed = 10f, mouseSensitivity = 5, rotationSmoothTime = .12f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas ending;
    //[SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Camera camera;
    [SerializeField] private Canvas stats;
    [SerializeField] private Image region;
    [SerializeField] private Transform earth;
    [SerializeField] private Transform movement;
    [SerializeField] private Image pollution;
    [SerializeField] private Image nature;
    [SerializeField] private Transform planet;
    [SerializeField] private Image bar;
    [SerializeField] private Text playButton;

    [SerializeField] private Button button;
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private RawImage mouseUi;

    [SerializeField] private Animator animator;


    private CanvasGroup canvasGroup, inGameUi;

    private float yaw, pitch, speed = 1f, moving = 0f;

    private bool returnMenu = false, modify = false, returnPressed = false, buttonPressed = false, showMouseUi = false;

    private Vector3 rotationSmoothVelocity, currentRotation;

    public void GoToMenu()
    {
        Debug.Log("button pressed");
        playButton.text = "Restart";
        returnPressed = true;
    }

    private void Start()
    {
        button.enabled = false;

        Cursor.visible = true;
        animator = canvas.GetComponentInChildren<Animator>();
        animator.GetComponentInParent<Image>().enabled = false;

        canvasGroup = canvas.GetComponentInChildren<CanvasGroup>();
        inGameUi = stats.GetComponentInChildren<CanvasGroup>();
    }

    public void PressHelp()
    {
        showMouseUi = !showMouseUi;
    }

    public void CheckPlanet()
    {
        if (canvasGroup.alpha == 1f)
        {
            canvasGroup.alpha = 0.9f;

            if (playButton.text != "Continue")
            {
                foreach(Transform region in planet.GetComponentsInChildren<Transform>())
                {
                    if(region.tag == "Region" && !region.GetComponent<MeshCollider>().enabled)
                    {
                        region.GetComponent<MeshCollider>().enabled = true;
                    }
                }

                Debug.Log("reset from menu");
                pollution.fillAmount = nature.fillAmount = 0.5f;
            }
        }
    }

    IEnumerator Fade()
    {
        CanvasGroup canvasGroup = animator.GetComponentInParent<Image>().GetComponent<CanvasGroup>();
        while(canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / 1.5f;
            yield return null;
        }

        Application.Quit();
    }

    private void ShowEndingScreen(Image image)
    {
        if (ending.GetComponent<CanvasGroup>().alpha < 1f)
            ending.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
        else button.enabled = true;

        //title.color = description.color = image.color;

        if (inGameUi.alpha > 0f)
            inGameUi.alpha -= 1f * Time.deltaTime;
    }

    public void ReturnToMenu()
    {
        buttonPressed = true;
    }

    public void Quit()
    {
        if (canvasGroup.alpha == 1f)
        {
            showMouseUi = false;
            animator.GetComponentInParent<Image>().enabled = true;
            StartCoroutine(Fade());
        }
    }


    private void Update()
    {
        int contor = 0;

        if (showMouseUi)
        {
            if(mouseUi.GetComponent<CanvasGroup>().alpha < 1f)
            {
                mouseUi.GetComponent<CanvasGroup>().alpha += 1f * Time.deltaTime;
            }
        }

        else
        {
            if (mouseUi.GetComponent<CanvasGroup>().alpha > 0f)
            {
                mouseUi.GetComponent<CanvasGroup>().alpha -= 1f * Time.deltaTime;
            }
        }

        if (canvasGroup.alpha < 1)
        {
            showMouseUi = false;

            if (canvasGroup.alpha == 0 && !returnMenu)
            {
                canvas.enabled = false;

                contor = 0;

                foreach (MeshCollider region in planet.GetComponentsInChildren<MeshCollider>())
                {
                    if (region.enabled == true)
                    {
                        contor++;
                    }
                }

                if (buttonPressed && !returnMenu && ending.GetComponent<CanvasGroup>().alpha == 0f)
                {
                    Debug.Log("Backspace pressed");
                    buttonPressed = false;
                    returnPressed = true;
                    canvas.enabled = true;
                    returnMenu = true;
                    button.enabled = false;
                }
                else if (ending.GetComponent<CanvasGroup>().alpha == 0f && (pollution.fillAmount > 0.99f || contor == 0))
                {
                    /*foreach (MeshRenderer mesh in planet.GetComponentsInChildren<Transform>().Find("Good").GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.enabled = true;
                    }

                    foreach (MeshRenderer mesh in planet.Find("Bad").GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.enabled = true;
                    }*/

                    Debug.Log("Values reseted");
                    returnPressed = false;
                    canvas.enabled = true;
                    returnMenu = true;
                    button.enabled = true;
                }

                else
                {
                    playButton.text = "Continue";
                    float q = Input.GetAxis("Mouse ScrollWheel");

                    if (Input.GetKey(KeyCode.Q) && speed > minSpeed) speed -= additionSpeed;
                    if (Input.GetKey(KeyCode.E) && speed < maxSpeed) speed += additionSpeed;

                    if (q < 0 && moving > 0) { camera.transform.position -= camera.transform.forward.normalized; moving -= 0.1f; }
                    if (q > 0 && moving < 0.8f) { camera.transform.position += camera.transform.forward.normalized; moving += 0.1f; }

                    float xdir = 0, ydir = 0;

                    if (Input.GetMouseButton(1))
                    {
                        Cursor.visible = false;
                        modify = true;
                        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                    }

                    else
                    {
                        Cursor.visible = true;

                        if (Input.GetKey(KeyCode.A)) xdir = -speed;
                        else if (Input.GetKey(KeyCode.D)) xdir = speed;

                        if (Input.GetKey(KeyCode.W)) ydir = -speed;
                        else if (Input.GetKey(KeyCode.S)) ydir = speed;

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            xdir = xdir == 0 ? xdir : (xdir > 0 ? maxSpeed : -maxSpeed);
                            ydir = ydir == 0 ? ydir : (ydir > 0 ? maxSpeed : -maxSpeed);
                        }

                        if (xdir != 0 || ydir != 0) modify = true;
                        yaw += xdir;
                        pitch += ydir;
                    }

                    if (modify)
                    {
                        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
                        movement.eulerAngles = currentRotation;
                        modify = false;
                    }

                    //Debug.Log(moving);
                }
            }
            else if (!returnMenu)
            {
                canvasGroup.alpha -= 1f * Time.deltaTime;
                inGameUi.alpha += 1f * Time.deltaTime;
            }

            else
            {
                Cursor.visible = true;

                if (!returnPressed)
                {
                    if (pollution.fillAmount >= 0.75f)
                    {
                        ShowEndingScreen(pollution);
                    }

                    else if (contor == 0 && playButton.text != "Restart")
                    {
                        bar.GetComponent<CanvasGroup>().alpha = 0.9f;

                        ShowEndingScreen(nature);
                    }
                }

                else
                {
                    if (ending.GetComponent<CanvasGroup>().alpha > 0f)
                    {
                        ending.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
                    }

                    if (canvasGroup.alpha < 1f)
                    {
                        if (ending.GetComponent<CanvasGroup>().alpha == 0f)
                            canvasGroup.alpha += 1f * Time.deltaTime;

                        if (inGameUi.alpha > 0f)
                            inGameUi.alpha -= 1f * Time.deltaTime;

                        if (region.GetComponent<RectTransform>().position.x < 2288.8f)
                        {
                            region.GetComponent<RectTransform>().position = region.GetComponent<RectTransform>().position + new Vector3(20f * Time.deltaTime, 0, 0);
                        }

                        if (moving > 0f)
                        {
                            camera.transform.position -= camera.transform.forward.normalized; moving -= 0.1f;
                        }
                    }

                    if (canvasGroup.alpha >= 1f)
                    {
                        canvasGroup.alpha = 1f;
                        inGameUi.alpha = 0f;
                        returnMenu = false;

                        //Debug.Log(region.GetComponent<RectTransform>().position);
                    }
                }
            }

        }

        else
        {
            yaw -= 0.1f;
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            movement.eulerAngles = currentRotation;
        }
    }
}
