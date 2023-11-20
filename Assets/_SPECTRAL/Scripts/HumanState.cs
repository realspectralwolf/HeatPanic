using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (human.CurrentLocation.roomType == healingType)
        {
            var newHp = Mathf.Clamp(human.Health + human.HumanData.hpregspeed * Time.deltaTime, 0, human.HumanData.maxHp);
            human.SetHealth(newHp);

            if (human.Health == human.HumanData.maxHp)
            {
                human.OnRegenerated?.Invoke();
                DragDropManager.Instance.SendBackToRandomFloor(human);
            }
        }
        else if (human.CurrentLocation.roomType == Location.RoomType.WaitingRoom)
        {
            var newHp = Mathf.Clamp(human.Health - human.HumanData.hpfallspeed / 2.5f * Time.deltaTime, 0, human.HumanData.maxHp);
            human.SetHealth(newHp);
        }
        else if (human.IsTutorialOnly)
        {
            human.SetHealth(40);
        }
        else
        {
            var newHp = Mathf.Clamp(human.Health - human.HumanData.hpfallspeed * Time.deltaTime, 0, human.HumanData.maxHp);
            human.SetHealth(newHp);
        }
    }
}
