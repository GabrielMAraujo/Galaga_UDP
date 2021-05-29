using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CameraData cameraData;
    private Camera mainCamera;
    private GameManager gameManager;

    private float lerpTimer;
    private Vector3 initialPos;
    private bool gameOver;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        gameManager = GameManager.instance;
        gameManager.OnGameOver += OnGameOver;
    }

    private void Start()
    {
        initialPos = transform.position;
    }

    private void OnDestroy()
    {
        gameManager.OnGameOver -= OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (lerpTimer > 0f)
            {
                lerpTimer -= Time.deltaTime;

                float perc = (cameraData.animationTime - lerpTimer) / cameraData.animationTime;
                //Lerping camera position
                transform.position = Vector3.Lerp(initialPos, cameraData.animationTarget, perc);
                //Lerping camera size
                mainCamera.orthographicSize = 1 + (perc * (cameraData.animationFinalScale - 1));
            }
            else
            {
                transform.position = cameraData.animationTarget;
                mainCamera.orthographicSize = cameraData.animationFinalScale;
            }
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            OnGameOver(true);
        }
    }

    private void OnGameOver(bool winner)
    {
        if (winner)
        {
            gameOver = true;
            lerpTimer = cameraData.animationTime;
            //Change background color to gray
            mainCamera.backgroundColor = Color.gray;
            //Destroy side restrainers
            GameObject restrainers = GameObject.Find("Background");
            if (restrainers != null)
            {
                Destroy(restrainers);
            }
        }
    }


}
