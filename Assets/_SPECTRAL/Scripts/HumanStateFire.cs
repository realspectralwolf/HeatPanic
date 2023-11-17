using UnityEngine;

[CreateAssetMenu(menuName = "HumanStates/Fire")]
public class HumanStateFire : HumanState
{
    public override void Enter(Human humanOrigin)
    {
        base.Enter(humanOrigin);
    }

    public override void FrameUpdate()
    {
        HumanStateExtensions.HandleHealthManipulations(human, healingType: Location.RoomType.SwimPool);
    }

    public override void Exit()
    {
        AudioMgr.instance.PlayAudioClip("regenerated");
    }
}