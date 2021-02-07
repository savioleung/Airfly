using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
  public void backToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
