using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update

    public int index;

    public bool keyDown;

    public int maxIndex = 2;

    //audiosource

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MenuUserSelection();

        NewGame();
    }

    private void MenuUserSelection()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }

    private void NewGame()
    {
        if (Input.GetKeyDown(KeyCode.Return) && index == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
