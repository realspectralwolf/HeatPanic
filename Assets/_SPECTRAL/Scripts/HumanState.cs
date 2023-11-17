using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class HumanState : ScriptableObject
{
    [HideInInspector] public Human human;
    [SerializeField] public GameObject visual;

    public virtual void Enter(Human humanOrigin)
    {
        human = humanOrigin;
        human.SetVisual(visual);
    }

    public virtual void FrameUpdate()
    {
        Debug.Log("updating test state");
    }

    public virtual void Exit()
    {
        Debug.Log("exiting test state");
    }
}

public static class HumanStateExtensions
{
    public static void HandleHealthManipulations(Human human, Location.RoomType healingType)
    {
        if (human.currentLocation.roomType == healingType)
        {
            var newHp = Mathf.Clamp(human.health + human.humanData.hpregspeed * Time.deltaTime, 0, human.humanData.maxHp);
            human.SetHealth(newHp);

            if (human.health == human.humanData.maxHp)
            {
                human.OnRegenerated?.Invoke();
                DragDropManager.Instance.SendBackToRandomFloor(human);
            }
        }
        else if (human.currentLocation.roomType == Location.RoomType.WaitingRoom)
        {
            var newHp = Mathf.Clamp(human.health - human.humanData.hpfallspeed / 2.5f * Time.deltaTime, 0, human.humanData.maxHp);
            human.SetHealth(newHp);
        }
        else if (human.isTutorialOnly)
        {
            human.SetHealth(40);
        }
        else
        {
            var newHp = Mathf.Clamp(human.health - human.humanData.hpfallspeed * Time.deltaTime, 0, human.humanData.maxHp);
            human.SetHealth(newHp);
        }
    }
}
