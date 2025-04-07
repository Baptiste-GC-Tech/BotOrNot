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

    // inventory script reference
    [SerializeField] private BON_Inventory inventory;


    /*  Level1 requirement */
    private int _collectiblesToWin;


    private Scenes _currentScene;

    public enum Scenes
    {
        Menu,
        Level1,
        Level2
    };

    /*
     *  CLASS METHODS
     */

    public static BON_GameManager Instance()
    {
        if (_gameManager == null)
        {
            GameObject newObject = new GameObject("BON_GameManager");
            _gameManager = newObject.AddComponent<BON_GameManager>();
        }
        return _gameManager;
    }

    
    public void ChangeScene(Scenes nextScene)
    {
        print("call funct scene");
        print("from "+_currentScene +" to "+ nextScene.ToString());
        switch (_currentScene)
        {
            case Scenes.Menu: // n'importe quel niveau possible?
                break;

            case Scenes.Level1: //uniquement level2 ou menu possible
                if (nextScene != Scenes.Level2 && nextScene != Scenes.Menu)
                {
                    //impossible
                    print("tu ne peux pas faire ca");
                }
                else
                {
                    _currentScene = nextScene;
                    SceneManager.LoadScene(nextScene.ToString());
                }
                break;
            case Scenes.Level2: //uniquement menu possible (+ futurs levels)
                if (nextScene != Scenes.Menu)
                {
                    //impossible
                    print("tu ne peux pas");
                }
                else
                {
                    if ( inventory.CountItem() == _collectiblesToWin) //si on stock bien tous les items necessaires
                    {
                        _currentScene = nextScene;
                        SceneManager.LoadScene(nextScene.ToString());
                    }
                    else
                    {
                        print("condition on respecté");
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
        DontDestroyOnLoad(this.gameObject);

        //init la scene au Menu
        _currentScene = Scenes.Level1;

        //Lancer la scene du level1
        ChangeScene(Scenes.Level2);
        print(_currentScene);

        _collectiblesToWin = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
