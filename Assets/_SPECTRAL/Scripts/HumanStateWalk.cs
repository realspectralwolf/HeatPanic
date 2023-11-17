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
        timeToNextMoney = Random.Range(0, DataMgr.Instance.GameData.moneyMakingSpeed);
        isWalkingRight = Random.Range(0, 2) == 1;

        if (human.isTutorialOnly)
        {
            timeToNextSymptom = human.humanData.eventsDelayMin;
        }
        else
        {
            timeToNextSymptom = Random.Range(human.humanData.eventsDelayMin, human.humanData.eventsDelayMax);
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
            Destroy(Instantiate(human.humanData.OnePlusEffectPrefab, human.transform.position, Quaternion.identity), 1.5f);
        }
    }

    private void HandleMovement()
    {
        if (isWalkingRight)
        {
            human.spritesHolderTransform.localScale = new Vector3(1, 1, 1);
            human.transform.position += Vector3.right * human.humanData.walkSpeed * Time.deltaTime;
            if (human.transform.position.x >= human.currentLocation.walkRange.max)
            {
                isWalkingRight = !isWalkingRight;
            }
        }
        else
        {
            human.spritesHolderTransform.localScale = new Vector3(-1, 1, 1);
            human.transform.position -= Vector3.right * human.humanData.walkSpeed * Time.deltaTime;
            if (human.transform.position.x <= human.currentLocation.walkRange.min)
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

            if (human.isTutorialOnly)
            {
                newStateID = human.previousSymptomID + 1;
            }

            if (newStateID == 1)
            {
                human.SetState(human.humanData.fireState);
            }
            else
            {
                human.SetState(human.humanData.sleepState);
            }

            human.previousSymptomID = newStateID;

            human.OnOverheatedSymptom?.Invoke();

            float a = Mathf.Lerp(1, 4.5f, ResourceManager.Instance.GetNormalizedTemp());
            timeToNextSymptom = Random.Range(human.humanData.eventsDelayMin, human.humanData.eventsDelayMax) / a;
        }
    }

    private float CalculateTimeToNextMoney()
    {
        float moneyFreq = DataMgr.Instance.GameData.moneyMakingSpeed;
        float value = !human.isTutorialOnly ? ResourceManager.Instance.GetNormalizedTemp() : 0;
        return Mathf.Lerp(moneyFreq, moneyFreq / 6, value);
    }

    public override void Exit()
    {

    }
}