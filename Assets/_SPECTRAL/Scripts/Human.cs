using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public static System.Action OnDied;
    public static System.Action<Location> OnDroppedOn;
    public System.Action OnOverheatedSymptom;
    public System.Action OnRegenerated;

    public HumanData humanData;
    public GameObject currentVisual;
    public bool isDragged = false;
    public float health = 100;

    public HumanState currentState;
    public HumanState initialStateOverride;
    public Location currentLocation;

    public Slider hpBar;

    public Transform spritesHolderTransform;

    public bool isTutorialOnly = false;
    [HideInInspector]public int previousSymptomID = 0;

    private void Start()
    {
        if (isTutorialOnly)
        {
            health = humanData.maxHp;
            SetState(initialStateOverride);
        }
    }

    public void Init()
    {
        health = humanData.maxHp;
    }

    private void Update()
    {
        if (!isDragged)
        {
            if (currentState != null)
                currentState.FrameUpdate();

            HandleHpBarVisibility();
        } 

        if (health == 0)
        {
            Kill();
        }
    }

    void HandleHpBarVisibility()
    {
        if (health < humanData.maxHp)
        {
            hpBar.gameObject.SetActive(true);
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void SetState(HumanState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = Instantiate(newState);
        currentState.Enter(this);
    }

    public void PickedUp()
    {
        isDragged = true;
    }

    public void DroppedOn(Location newLocation)
    {
        if (newLocation == null) return;

        if (currentLocation != null)
            currentLocation.RemoveHuman(this);

        transform.parent = newLocation.transform;
        currentLocation = newLocation;
        newLocation.AddHuman(this);

        isDragged = false;
        OnDroppedOn?.Invoke(newLocation);
    }

    public void Kill()
    {
        if (currentLocation != null)
        {
            currentLocation.RemoveHuman(this);
        }
        Destroy(gameObject);
        Destroy(Instantiate(humanData.poofEffectPrefab, transform.position, Quaternion.identity), 1.5f);
        OnDied?.Invoke();
        AudioMgr.instance.PlayAudioClip("death");
    }

    public void SetVisual(GameObject spriteObject)
    {
        if (currentVisual != null)
            Destroy(currentVisual);

        currentVisual = Instantiate(spriteObject, transform.position, Quaternion.identity, spritesHolderTransform);
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
        hpBar.value = health / humanData.maxHp;
    }
}
