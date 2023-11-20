using UnityEngine;

[CreateAssetMenu(menuName = "HumanStates/Walk")]
public class HumanStateWalk : HumanState
{
    float timeToNextMoney = 0;
    float timeToNextSymptom = 0;
    bool isWalkingRight = false;

    public override void Enter(Human humanOrigin)
    {
        base.Enter(humanOrigin);
        timeToNextMoney = Random.Range(0, DataHolder.Instance.GameData.moneyMakingSpeed);
        isWalkingRight = Random.Range(0, 2) == 1;

        if (human.IsTutorialOnly)
        {
            timeToNextSymptom = human.HumanData.eventsDelayMin;
        }
        else
        {
            timeToNextSymptom = Random.Range(human.HumanData.eventsDelayMin, human.HumanData.eventsDelayMax);
        }
        
    }

    public override void FrameUpdate()
    {
        HandleMoneyMaking();
        HandleMovement();
        HandleOverheatingSymptom();
    }

    private void HandleMoneyMaking()
    {
        timeToNextMoney -= Time.deltaTime;
        if (timeToNextMoney < 0)
        {
            timeToNextMoney = CalculateTimeToNextMoney();
            ResourceManager.Instance.AddMoney();
            Destroy(Instantiate(human.HumanData.OnePlusEffectPrefab, human.transform.position, Quaternion.identity), 1.5f);
        }
    }

    private void HandleMovement()
    {
        if (isWalkingRight)
        {
            human.SetFlipX(false);
            human.transform.position += Vector3.right * human.HumanData.walkSpeed * Time.deltaTime;
            if (human.transform.position.x >= human.CurrentLocation.walkRange.max)
            {
                isWalkingRight = !isWalkingRight;
            }
        }
        else
        {
            human.SetFlipX(true);
            human.transform.position -= Vector3.right * human.HumanData.walkSpeed * Time.deltaTime;
            if (human.transform.position.x <= human.CurrentLocation.walkRange.min)
            {
                isWalkingRight = !isWalkingRight;
            }
        }
    }

    private void HandleOverheatingSymptom()
    {
        timeToNextSymptom -= Time.deltaTime;
        if (timeToNextSymptom <= 0)
        {
            int newStateID = Random.Range(0, 2);

            if (human.IsTutorialOnly)
            {
                newStateID = human.PreviousSymptomID + 1;
            }

            if (newStateID == 1)
            {
                human.SetState(human.HumanData.fireState);
            }
            else
            {
                human.SetState(human.HumanData.sleepState);
            }

            human.PreviousSymptomID = newStateID;

            human.OnOverheatedSymptom?.Invoke();

            float a = Mathf.Lerp(1, 4.5f, ResourceManager.Instance.GetNormalizedTemp());
            timeToNextSymptom = Random.Range(human.HumanData.eventsDelayMin, human.HumanData.eventsDelayMax) / a;
        }
    }

    private float CalculateTimeToNextMoney()
    {
        float moneyFreq = DataHolder.Instance.GameData.moneyMakingSpeed;
        float value = !human.IsTutorialOnly ? ResourceManager.Instance.GetNormalizedTemp() : 0;
        return Mathf.Lerp(moneyFreq, moneyFreq / 6, value);
    }

    public override void Exit()
    {

    }
}