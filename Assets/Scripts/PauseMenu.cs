using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void HasAppeared()
    {
        animator.SetTrigger("hasAppeared");
    }
    public void Continue()
    {
        Time.timeScale = 1;
        FindObjectOfType<UIManager>().pauseMenuActive = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
