using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchMenu : MonoBehaviour
{
    
    /**
     * Function for switching menu
     * 
     * @params GameObject menu_ui
     * @return void
     */
    public void SwitchMenuTo(GameObject menu_ui)
    {
        for( int i = 0; i < transform.childCount; i++ )
        {
            transform.GetChild(i).gameObject.SetActive( false );
        }
        AudioSystem.instance.PlaySFX(0);
        menu_ui.gameObject.SetActive( true );
    }

    /**
     * Function for switching to main game scene
     * 
     * @return void
     */
    public void SwitchToGame()
    {
        AudioSystem.instance.PlaySFX(0);
        SceneManager.LoadScene("Main");
    }

    /**
     * Function for switching to menu game scene
     * 
     * @return void
     */
    public void SwitchToMainMenu()
    {
        AudioSystem.instance.PlaySFX(0);
        SceneManager.LoadScene("SceneMenuUi");
    }
}
