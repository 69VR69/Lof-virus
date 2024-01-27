using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    //TODO add audiomanager instance (Singleton)
    
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
        //TODO add Audio Effect here!!!
        menu_ui.gameObject.SetActive( true );
    }
}
