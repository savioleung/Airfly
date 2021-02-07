using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private Light spotLight;
    float lightTime = 0;
    bool starting = false;
    private void Update()
    {
        if (starting)
        {
            movingScene();
        }
    }

    public void StartGameButton()
    {
        starting = true;
    }
    public void EndGameButton()
    {
        Application.Quit();
    }

    void movingScene()
    {
        lightTime += 1 * Time.deltaTime * 50;
        spotLight.spotAngle = lightTime;
        if (lightTime >= 180)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
