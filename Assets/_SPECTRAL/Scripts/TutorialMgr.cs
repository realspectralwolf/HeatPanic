using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMgr : MonoBehaviour
{
    [SerializeField] Human tutorialHuman;
    [SerializeField] float finalDelay;
    [SerializeField] GameObject[] uiStepPanels;

    int tutorialProgress = 0;

    private void Start()
    {
        tutorialProgress = 0;
        uiStepPanels[0].SetActive(true);

        Human.OnDroppedOn += HumanDroppedToRoomHandler;
        DragDropManager.Instance.PickedUpHuman += PickedUpHumanHandler;
        DragDropManager.Instance.CanceledHumanMove += DroppedHumanHandler;
        tutorialHuman.OnRegenerated += HumanRegeratedHandler;
        tutorialHuman.OnOverheatedSymptom += OnHumanOverheated;
    }

    private void OnHumanOverheated()
    {
        if (tutorialProgress == 0)
        {
            SetTutorialProgressTo(1);
            uiStepPanels[1].transform.position = Camera.main.WorldToScreenPoint(tutorialHuman.transform.position);
        }

        else if (tutorialProgress == 4)
        {
            SetTutorialProgressTo(5);
            uiStepPanels[5].transform.position = Camera.main.WorldToScreenPoint(tutorialHuman.transform.position);
        }
    }

    private void HumanDroppedToRoomHandler(Location space)
    {
        if (tutorialProgress == 2)
        {
            SetTutorialProgressTo(3);
        }

        else if (tutorialProgress == 6)
        {
            SetTutorialProgressTo(7);
        }
    }

    private void PickedUpHumanHandler()
    {
        if (tutorialProgress == 1)
        {
            SetTutorialProgressTo(2);
        }

        else if (tutorialProgress == 5)
        {
            SetTutorialProgressTo(6);
        }
    }

    private void HumanRegeratedHandler()
    {
        if (tutorialProgress == 3)
        {
            SetTutorialProgressTo(4);
        }

        else if (tutorialProgress == 7)
        {
            SetTutorialProgressTo(8);
            StartCoroutine(TutorialCompletedRoutine());
        }
    }

    private void DroppedHumanHandler()
    {
        if (tutorialProgress == 2)
        {
            SetTutorialProgressTo(1);
        }

        else if (tutorialProgress == 6)
        {
            SetTutorialProgressTo(5);
        }
    }

    private void SetTutorialProgressTo(int index)
    {
        tutorialProgress = index;
        SetUIPanelTo(uiStepPanels[tutorialProgress]);
    }

    private void SetUIPanelTo(GameObject panel)
    {
        for (int i = 0; i < uiStepPanels.Length; i++)
        {
            if (uiStepPanels[i] == panel)
                panel.SetActive(true);
            else
                uiStepPanels[i].SetActive(false);
        }
    }

    IEnumerator TutorialCompletedRoutine()
    {
        yield return new WaitForSeconds(finalDelay);
        SceneChanger.Instance.LoadGameplayScene();
    }
}
