using UnityEngine;

public class BON_SIdle : BON_State
{
    public override void Enter()
    {
        Debug.Log("Entrée en idle");
    }

    public override void Exit()
    {
        Debug.Log("Sortie de idle");
    }

    public override void Update()
    {
        //nothing
    }
}
