using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Collider2D TriggerArea;

    public bool active;
    public bool activated;

    public Transform objective;
    public Transform fightCamPosition;
    public float length;
    public GameObject cam;
    public GameObject door;

    public float fightCamSize;
    public float originalCamSize;

    public GameObject boss;
    public GameObject bossBar;
    void Start()
    {
        bossBar = GameObject.Find("Boss Health Bar");
        originalCamSize = cam.GetComponent<Camera>().orthographicSize;
    }

    void Update()
    {
        if (active)
            MoveCamera();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
            return;

        activated = true;
        StartCoroutine(startCutscene());
    }

    private IEnumerator startCutscene()
    {
        InputHandler.ForbidInput();
        active = true;
        cam.GetComponent<CameraMovement>().follow = false;
        yield return new WaitForSeconds(length);
        //cam.GetComponent<CameraMovement>().follow = true;
        door.SetActive(true);
        InputHandler.AllowInput();
        active = false;
        cam.GetComponent<Camera>().orthographicSize = fightCamSize;
        cam.transform.position = fightCamPosition.position;
        boss.GetComponent<KarpAI>().Inert = false;
        bossBar.GetComponent<BossBar>().Activate();

        //spawn boss
    }

    public void OpenDoors()
    {
        door.SetActive(false);
        originalCamSize = cam.GetComponent<Camera>().orthographicSize = originalCamSize;
        cam.GetComponent<CameraMovement>().follow = true;
    }

    private void MoveCamera()
    {

        Vector3 newPosition = Vector3.Lerp(cam.transform.position, objective.position, Time.deltaTime * 1f);

        cam.transform.position = newPosition;
    }
}
