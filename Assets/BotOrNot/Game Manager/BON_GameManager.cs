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

    // player reference
    private BON_CCPlayer _player;

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
            newObject.AddComponent<BON_GameManager>();
            _gameManager = newObject.GetComponent<BON_GameManager>();
            _gameManager.InitFields();
        }
        return _gameManager;
    }

    public void InitFields()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _inventory = _player.GetComponent<BON_Inventory>();
        _isPlayingNut = true;
        _collectiblesToWin = 4;

        //init la scene actuelle
        _currentScene = Scenes.Menu;
    }

    public void ChangeScene(Scenes nextScene)
    {
        print("Chnage scene from "+_currentScene +" to "+ nextScene.ToString());
        switch (_currentScene)
        {
            case Scenes.Menu: // n'importe quel niveau possible?
                _currentScene = nextScene;
                SceneManager.LoadScene(nextScene.ToString());
                print("good");
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
                    print("good");
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
                        print("good");
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
        //init la scene au Menu
        _currentScene = Scenes.Menu;

        //init player
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();

        //init la current state
        //_player.AvatarState.InitState(BON_AvatarState.States.Idle); bug  nullref<- parce que le start de l'avatar se fait apres je pense ?

        //Lancer la scene du level1
        //ChangeScene(Scenes.Level1);
    }

    // Update is called once per frame
    void Update()
    {
        //print("state = "+_currentState);
    }
}
