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
    [SerializeField] private BON_Inventory _inventory;

    // stateMachine script reference
    [SerializeField] private BON_AvatarState _AvatarState;

    private BON_AvatarState _currentState;
    public BON_AvatarState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

    /*  Level1 requirement */
    private int _collectiblesToWin;

    private bool _isPlayingNut; ////true if nut, false if Dame robot
    public bool IsPlayingNut
    {
        get { return _isPlayingNut; }
        set { _isPlayingNut = value; }
    }

    private Scenes _currentScene;

    public enum Scenes
    {
        //modify for futur with trues scenes names
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
        print("Chnage scene from "+_currentScene +" to "+ nextScene.ToString());
        switch (_currentScene)
        {
            case Scenes.Menu: // n'importe quel niveau possible?
                break;

            case Scenes.Level1: //uniquement level2 ou menu possible
                if (nextScene != Scenes.Level2 && nextScene != Scenes.Menu)
                {
                    //impossible
                    print("tu ne peux pas");
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
                    if ( _inventory.CountItem() == _collectiblesToWin) //si on stock bien tous les items necessaires
                    {
                        _currentScene = nextScene;
                        SceneManager.LoadScene(nextScene.ToString());
                    }
                    else
                    {
                        print("condition non respecté");
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
        _currentScene = Scenes.Menu;

        //init la current state
        _AvatarState.InitState(BON_AvatarState.States.Idle);

        //Lancer la scene du level1
        //(Scenes.Level1);

        //init bool
        _isPlayingNut = true;
        _collectiblesToWin = 4;
    }

    // Update is called once per frame
    void Update()
    {
        print("state = "+_currentState);
    }
}
