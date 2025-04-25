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
    // Input related
    public InputAction _PRMoveInputAction;
    public InputAction _MachineInputAction;
    public Vector2 _directionalInputValue;

    // his instance
    private static BON_GameManager _gameManager;

    // inventory script reference
    [SerializeField] public BON_Inventory Inventory;

    // player reference
    private BON_CCPlayer _player;

    public BON_CCPlayer Player
    {
        get { return _player; }
    }

    private BON_AvatarState _currentState;

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
        MainMenu,
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
            _player.GetComponent<BON_MovePR>(),
            //_player.GetComponent<BON_SwitchPlayerPR>(),
            _player.GetComponent<BON_CablePR>()
        };
        _componentsDR = new ()
        {
            //_player.GetComponent<BON_MoveDR>(),
            //_player.GetComponent<BON_SwitchPlayerDR>()
        };

        //init la scene actuelle
        _currentScene = Scenes.MainMenu;
    }

    public void ChangeScene(Scenes nextScene)
    {
        _currentScene = nextScene;
        SceneManager.LoadScene(nextScene.ToString());    
    }

    public void GiveControl() //donner le controle a une machine
    {
        _lastCharacterPlayed = _currentCharacterPlayed; //save l'id du perso
        _currentCharacterPlayed = -1;
        if (_lastCharacterPlayed != -1)
        {
            DisableCompPlayer(_lastCharacterPlayed);
        }
        Debug.Log("control switch to " + _player.GetComponent<PlayerInput>().currentActionMap);
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
        print("control switch to " + _player.GetComponent<PlayerInput>().currentActionMap);
    }

    public void RecoverControl() //reprendre le controle
    {
        _currentCharacterPlayed = _lastCharacterPlayed;
        EnableCompPlayer(_currentCharacterPlayed);
        print("control switch to " + _player.GetComponent<PlayerInput>().currentActionMap);
    }

    public void DisableCompPlayer(int CharacterStopPlaying)
    {
        if (_componentsAvatar == null) return;

        if (CharacterStopPlaying != 0 && CharacterStopPlaying != 1) return; //pas PR et pas DR

        for (int i = 0; i < _componentsAvatar[CharacterStopPlaying].Count; i++) //disable all comps in list
        {
            if(_componentsAvatar[CharacterStopPlaying][i] != null)
            {
                if (_componentsAvatar[CharacterStopPlaying][i].enabled)
                {
                    Debug.Log("Disabling " + _componentsAvatar[CharacterStopPlaying][i].GetType().ToString());
                    _componentsAvatar[CharacterStopPlaying][i].enabled = false;
                }
            }
        }
    }

    public void EnableCompPlayer(int CharacterWillPlay)
    {
        if (_componentsAvatar == null) return;

        if (CharacterWillPlay != 0 && CharacterWillPlay != 1) return; //pas PR et pas DR

        for (int i = 0; i < _componentsAvatar[CharacterWillPlay].Count; i++) //enable all comps in list
        {
            if (_componentsAvatar[CharacterWillPlay][i] != null)
            {
                if (!_componentsAvatar[CharacterWillPlay][i].enabled)
                {
                    _componentsAvatar[CharacterWillPlay][i].enabled = true;
                }
            }
        }
    }
    
    public IEnumerator CooldownSwitchControl()
    {
        _isSwitching = true;
        yield return new WaitForSeconds(0.5f); //dur�e anim?
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
    {
        // Input setup
        _PRMoveInputAction = InputSystem.actions.FindAction("ActionsMapPR/Move");
        _MachineInputAction = InputSystem.actions.FindAction("MachineControl/Move");

        //init player values
        _currentCharacterPlayed = 0; //PR play
        _lastCharacterPlayed = _currentCharacterPlayed;
        _componentsAvatar = new();
        _componentsAvatar.Add(_componentsPR);
        _componentsAvatar.Add(_componentsDR);

        //start with PR => disable DR
        DisableCompPlayer(1); //disable DR comp
        EnableCompPlayer(0); //enable PR

        //init la scene au Menu
        _currentScene = Scenes.MainMenu;

        //init player
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();

        //Lancer la scene du level1
        //ChangeScene(Scenes.Level1);
    }

    // Used for input pulling
    void Update()
    {
        // Input PC
        _directionalInputValue = _PRMoveInputAction.ReadValue<Vector2>();
        _directionalInputValue = _MachineInputAction.ReadValue<Vector2>();

        // Input Mobile 
        //_moveInputValue = _joystick.InputValues;
    }
}
