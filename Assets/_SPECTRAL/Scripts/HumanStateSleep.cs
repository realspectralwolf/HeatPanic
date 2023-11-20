using UnityEngine;

[CreateAssetMenu(menuName = "HumanStates/Sleep")]
public class HumanStateSleep : HumanState
{
    public override void Enter(Human humanOrigin)
    {
        base.Enter(humanOrigin);
    } 

    public override void FrameUpdate()
    {
        HumanStateExtensions.HandleHealthManipulations(human, healingType: Location.RoomType.Nursery);
    }

    public override void Exit()
    {
        AudioManager.Instance.PlayAudioClip("regenerated");
    }
}