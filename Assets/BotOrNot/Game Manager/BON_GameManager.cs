using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BON_GameManager : MonoBehaviour
{

    /*
    *  FIELDS
    */
  
    private static BON_GameManager _gameManager;
    public static BON_GameManager GameManager
    {
        get { return _gameManager; }
    }

    // inventory script reference
    [SerializeField] private BON_Inventory inventory;


    /*  Level1 requirement */

    private int _collectiblesToWin;


    public enum Scenes
    {
        Menu,
        Level1,
        Level2
    };

    /*
     *  CLASS METHODS
     */

    
    public void ChangeScene(Scenes currentScene, Scenes nextScene)
    {
        switch (currentScene)
        {
            case Scenes.Menu: // n'importe quel niveau possible?
                break;

            case Scenes.Level1: //uniquement level2 ou menu possible
                if (nextScene != Scenes.Level2 || nextScene != Scenes.Menu)
                {
                    //impossible
                }
                else
                {
                    SceneManager.LoadScene((int)nextScene);
                }
                break;
            case Scenes.Level2: //uniquement menu possible (+ futurs levels)
                if (nextScene != Scenes.Menu)
                {
                    //impossible
                }
                else
                {
                    if ( inventory.CountItem() == _collectiblesToWin) //si on stock bien tous les items necessaires
                    {
                        SceneManager.LoadScene((int)nextScene);
                    }
                }
                break;
        }
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = new BON_GameManager();
        DontDestroyOnLoad(_gameManager);

        //init la scene au Menu
        Scenes currentScene = Scenes.Menu;

        //Lancer la scene du level1
        ChangeScene(currentScene, Scenes.Level1);


        _collectiblesToWin = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
