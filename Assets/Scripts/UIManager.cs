using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    public bool pauseMenuActive = false;
    GameObject pauseMenuInstance;
    [SerializeField]
    Transform toEnable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenuActive)
            {
                Time.timeScale = 0;
                pauseMenuInstance = Instantiate(pauseMenu, toEnable);
                pauseMenuActive = true;
            }
            else
            {
                pauseMenuInstance.GetComponent<PauseMenu>().Continue();
                pauseMenuActive = false;
                Destroy(pauseMenuInstance);
            }

        }
    }
}

