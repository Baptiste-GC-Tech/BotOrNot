using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BON_DebugTool : MonoBehaviour
{
    /*
     *  FIELDS
     */

    //other vars
    private BON_MovePR _CompMov;

    [Header("Player")]

    public BON_CCPlayer Player;
    public BON_MovePR PRMoveComp;
    public string CurrentPlayerPlayed;
    private BON_Controllable _machinePossessed;

    public BON_AvatarState AvatarState;

    public BON_Inventory Inventory;
    public List<int> InventoryList;

    [Space]
    [Header("States")]

    [Tooltip("Current State")]
    public BON_AvatarState.State CurrentState;
    public string CurrentActionMap;
    public bool IsGrounded;
    public bool IsAgainstWallLeft;
    public bool IsAgainstWallRight;
    public bool HasCableOut;
    public bool IsConstrollingMachine;
    public bool IsNearIOMInteractible;
    public bool IsSwitching;
    public bool IsNearElevator;

    [Space]
    [Header("MOUVEMENTS")]
    [Space(5)]
    [Header("Main mouvements")]

    public Vector2 InputValues;
    public Vector2 MoveMachineValue;
    public Vector2 MoveValue;
    public float CurSpeed;
    public bool UseGravity;
    public Vector3 GroundNormalVect;
    public Vector3 WrldSpcMoveDir;
    public Vector3 MoveDir;
    public Vector3 PRRBVelocity;

    [Space(5)]
    [Header("Drift")]
    public bool IsDrifting;
    public float TimeSinceLastMove;
    public float DriftDuration;
    public float DriftAcceleration;

    [Space(5)]
    [Header("Bounce")]
    public bool IsBouncing;
    public Vector3 FallHeight;
    public int BounceCount;

    [Space]
    [Header("ENVIRONMENT INFO")]
    public string GameObjectLayer;
    public string GameObjecTag;
    public string GameObjecName;

    /*
     *  UNITY METHODS
     */

    void Start()
    {
        Player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        AvatarState = Player.AvatarState;
        Inventory = BON_GameManager.Instance().Inventory;
        CurrentState = AvatarState.CurrentState;
        PRMoveComp = Player.GetComponent<BON_MovePR>();

        _CompMov = Player.GetComponent<BON_MovePR>();
    }

    private void Update()
    {
        // Player 
        if (BON_GameManager.Instance().CurrentCharacterPlayed == 0)
        {
            CurrentPlayerPlayed = "petit Robot";
        }
        else if (BON_GameManager.Instance().CurrentCharacterPlayed == -1)
        {
            _machinePossessed = Player.GetComponent<BON_MachineControllerPR>().MachinePossessed;
            CurrentPlayerPlayed = _machinePossessed.name;
        }
        InventoryList = Inventory.Items;

        // States
        CurrentState = AvatarState.CurrentState;
        CurrentActionMap = Player.GetComponent<PlayerInput>().currentActionMap.name;
        IsGrounded = AvatarState.IsGrounded;
        IsAgainstWallLeft = AvatarState.IsAgainstWallLeft;
        IsAgainstWallRight = AvatarState.IsAgainstWallRight;
        HasCableOut = AvatarState.HasCableOut;
        
        IsConstrollingMachine = AvatarState.IsConstrollingMachine;
        IsNearIOMInteractible = AvatarState.IsNearIOMInteractible;
        IsSwitching = BON_GameManager.Instance().IsSwitching;
        IsNearElevator = AvatarState.IsNearElevator;

        // main Mouvements

        InputValues = BON_GameManager.Instance().DirectionalInputValue;
        MoveMachineValue = Player.GetComponent<BON_MachineControllerPR>().MoveMachineValue;
        MoveValue = _CompMov.MoveInputValue;
        CurSpeed = _CompMov.CurSpeed;

        UseGravity = Player.GetComponent<Rigidbody>().useGravity;

        //drift
        IsDrifting = Player.AvatarState.IsDrifting;
        //////TimeSinceLastMove = _CompMov.TimeSinceLastMove;
        //////DriftDuration = _CompMov.DriftDuration;
        //////DriftAcceleration = _CompMov.DriftAcceleration;

        ////////bounce
        //////IsBouncing = _CompMov.IsBouncing;
        //////BounceCount = _CompMov.BounceCount;
        //////FallHeight = _CompMov.FallHeight;


        // Environment Info
        GameObjectLayer = _CompMov.Layer;
        GameObjecTag = _CompMov.tag;
        GameObjecName = _CompMov.name;
    }
}
