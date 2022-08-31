using UnityEngine;
using UnityEngine.UI;

public class PlayerMotion : MonoBehaviour
{
    private float speed, walk, run;
    private float angularSpeed;
    private CharacterController controller;
    private float rotationAboutX = 0, rotationAboutY = 0;

    public Slider HPBar;
    public Slider StaminaBar;
    public GameObject PlayerCamera;
    private AudioSource [] audioSources;
    private AudioSource footStepSound;
    private AudioSource runningSound;
    public static bool dead;
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        HPBar.maxValue = 1000;
        HPBar.value = 1000;
        StaminaBar.maxValue = 1000f;
        StaminaBar.value = StaminaBar.maxValue;
        walk = 10;
        run = 20;
        speed = walk;
        Cursor.lockState = CursorLockMode.Locked;
        angularSpeed = 20;
        controller = GetComponent<CharacterController>();
        audioSources = GetComponents<AudioSource>();
        footStepSound = audioSources[0];
        runningSound = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {//Time.deltaTime is time that has passed from frame to frame
        if (!PlayerOperations.inGameOverUI)
        {
            float dx, dy = -1, dz; // dy=-1 is a gravity
                                   // player rotation
            if (HPBar.value <= 0)
            {
                dead = true;
            }
            rotationAboutY += Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, rotationAboutY, 0);
            // camera rotation
            rotationAboutX -= Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
            rotationAboutX = Mathf.Clamp(rotationAboutX, -90f, 90f);
            PlayerCamera.transform.localEulerAngles = new Vector3(rotationAboutX, 0, 0);
            if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.value >= 5)
            {
                speed = run;
                StaminaBar.value -= 5;
            }
            else
            {
                StaminaBar.value += 1;
                speed = walk;
            }
            // motion after rotation
            dz = Input.GetAxis("Vertical"); //can be one of: -1, 0 , 1
            dz *= speed * Time.deltaTime;
            dx = Input.GetAxis("Horizontal");
            dx *= speed * Time.deltaTime;


            // motion using CharacterController
            Vector3 motion = new Vector3(dx, dy, dz); // motion is defined in Local coordinates
            motion = transform.TransformDirection(motion);//Now motion is in Global coordinates
            controller.Move(motion);// must recieve Vector3 in Global coordinates
                                    // add footstep sound effect
            if (dz < -0.1 || dz > 0.1 || dx < -0.1 || dx > 0.1)
            {
                if (speed == walk)
                {
                    runningSound.Stop();
                    if (!footStepSound.isPlaying)
                        footStepSound.Play();
                }
                else
                {
                    footStepSound.Stop();
                    if (!runningSound.isPlaying)
                        runningSound.Play();
                }
            }
        }
    }
}