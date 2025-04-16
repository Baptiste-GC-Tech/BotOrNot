using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BON_GameManager : MonoBehaviour
{
    /*
    *  FIELDS
    */
  
    //his instance
    private static BON_GameManager _gameManager;

    // inventory script reference
    [SerializeField] public BON_Inventory Inventory;

    // player reference
    private BON_CCPlayer _player;

    private BON_AvatarState _currentState;
    public BON_AvatarState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

    private bool _isPlayingNut; ////true if nut, false if Dame robot
    public bool IsPlayingNut
    {
        get { return _isPlayingNut; }
        set { _isPlayingNut = value; }
    }

    private Scenes _currentScene;

    /* player gestion related */
    private List<List<MonoBehaviour>> _componentsAvatar;

    [SerializeField] protected List<MonoBehaviour> _componentsPR;
    [SerializeField] protected List<MonoBehaviour> _componentsDR;

    private int _currentCharacterPlayed; //0 = PR, 1= DR, other = machine
    public int CurrentCharacterPlayed
    {
        get { return _currentCharacterPlayed; }
    }
    private int _lastCharacterPlayed; //var tampon pour recup le controle

    private bool _isSwitching = false;  //bool for switch player/machines
    public bool IsSwitching
    {
        get { return _isSwitching; }
        set { _isSwitching = value; }
    }

    public enum Scenes
    {
        //modify for future with trues scenes names
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
        Inventory = _player.GetComponent<BON_Inventory>();
        _isPlayingNut = true;

        _componentsPR = new ()
        {
            _player.GetComponent<BON_Move>(),
            _player.GetComponent<BON_Interact>(),
            _player.GetComponent<BON_Cable>()
        };
        _componentsDR = new ()
        {
            _player.GetComponent<BON_MoveDR>(),
            _player.GetComponent<BON_InteractDR>()
        };

        //init la scene actuelle
        _currentScene = Scenes.Menu;
    }

    public void ChangeScene(Scenes nextScene)
    {
        _currentScene = nextScene;
        SceneManager.LoadScene(nextScene.ToString());    

        /*
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
        */
    }

    public void GiveControl() //donner le controle a une machine
    {
        _lastCharacterPlayed = _currentCharacterPlayed; //save l'id du perso
        _currentCharacterPlayed = -1;
        _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
        DisableCompPlayer(_lastCharacterPlayed);
        _componentsAvatar[_lastCharacterPlayed][2].enabled = true;
        print("control switch to " + _player.GetComponent<PlayerInput>().currentActionMap);
    }

    public void SwitchPlayer()
    {
        //switch PR to DR
        if (_currentCharacterPlayed == 0) //switch PR to DR
        {
            EnableCompPlayer(1);
            DisableCompPlayer(0);
            _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapDR");
            _isPlayingNut = false;
            _currentCharacterPlayed = 1;
        }
        else if (_currentCharacterPlayed == 1)//switch DR to PR
        {
            EnableCompPlayer(0);
            DisableCompPlayer(1);
            _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
            _isPlayingNut = true;
            _currentCharacterPlayed = 0;
        }
        //_avatarState = _listAvatarsStates[_currentCharacterPlayed];
        print("control switch to " + _player.GetComponent<PlayerInput>().currentActionMap);
    }

    public void RecoverControl() //reprendre le controle
    {
        _currentCharacterPlayed = _lastCharacterPlayed;
        EnableCompPlayer(_lastCharacterPlayed);
        if (_currentCharacterPlayed == 0)
        {
            _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
        }
        else
        {
            _player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapDR");
        }
        print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
    }

    public void DisableCompPlayer(int CharacterStopPlaying)
    {
        for (int i = 0; i < _componentsAvatar[CharacterStopPlaying].Count; i++) //disable all comps in list
        {
            if (_componentsAvatar[CharacterStopPlaying][i].enabled)
            {
                _componentsAvatar[CharacterStopPlaying][i].enabled = false;
            }
        }
    }

    public void EnableCompPlayer(int CharacterWillPlay)
    {
        for (int i = 0; i < _componentsAvatar[CharacterWillPlay].Count; i++) //enable all comps in list
        {
            if (!_componentsAvatar[CharacterWillPlay][i].enabled)
            {
                _componentsAvatar[CharacterWillPlay][i].enabled = true;
            }
        }
    }
    
    public IEnumerator CooldownSwitchControl()
    {
        _isSwitching = true;
        yield return new WaitForSeconds(0.5f); //durée anim?
        _isSwitching = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {    //init player values
        _currentCharacterPlayed = 0; //PR play
        _lastCharacterPlayed = _currentCharacterPlayed;
        _componentsAvatar = new();
        _componentsAvatar.Add(_componentsPR);
        _componentsAvatar.Add(_componentsDR);

        //start with PR => disable DR
        DisableCompPlayer(1); //disable DR comp
        EnableCompPlayer(0); //enable PR

        //init la scene au Menu
        _currentScene = Scenes.Menu;

        //init player
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();

        //Lancer la scene du level1
        //ChangeScene(Scenes.Level1);
    }

    // Update is called once per frame
    void Update()
    {
        //print("state = "+_currentState);
    }
}
