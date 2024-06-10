using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Transform spritesHolderTransform;

    public static System.Action OnDied;
    public static System.Action<Location> OnDroppedOn;
    public System.Action OnOverheatedSymptom;
    public System.Action OnRegenerated;

    public HumanData HumanData;
    public float Health = 100 { get; private set; }

    public GameObject CurrentVisual;
    public HumanState CurrentState;
    public HumanState InitialStateOverride;
    public Location CurrentLocation;

    public int PreviousSymptomID = 0;

    public bool IsDragged = false { get; private set; }
    public bool IsTutorialOnly = false { get; private set; }

    private void Start()
    {
        if (IsTutorialOnly)
        {
            Health = HumanData.maxHp;
            SetState(InitialStateOverride);
        }
    }

    public void Init()
    {
        Health = HumanData.maxHp;
    }

    private void Update()
    {
        if (!IsDragged)
        {
            if (CurrentState != null)
                CurrentState.FrameUpdate();

            HandleHpBarVisibility();
        } 

        if (Health == 0)
        {
            Kill();
        }
    }

    void HandleHpBarVisibility()
    {
        if (Health < HumanData.maxHp)
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
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = Instantiate(newState);
        CurrentState.Enter(this);
    }

    public void PickedUp()
    {
        IsDragged = true;
    }

    public void DroppedOn(Location newLocation)
    {
        if (newLocation == null) return;

        if (CurrentLocation != null)
            CurrentLocation.RemoveHuman(this);

        transform.parent = newLocation.transform;
        CurrentLocation = newLocation;
        newLocation.AddHuman(this);

        IsDragged = false;
        OnDroppedOn?.Invoke(newLocation);
    }

    public void Kill()
    {
        if (CurrentLocation != null)
        {
            CurrentLocation.RemoveHuman(this);
        }
        Destroy(gameObject);
        Destroy(Instantiate(HumanData.poofEffectPrefab, transform.position, Quaternion.identity), 1.5f);
        OnDied?.Invoke();
        AudioManager.Instance.PlayAudioClip("death");
    }

    public void SetVisual(GameObject spriteObject)
    {
        if (CurrentVisual != null)
            Destroy(CurrentVisual);

        CurrentVisual = Instantiate(spriteObject, transform.position, Quaternion.identity, spritesHolderTransform);
    }

    public void SetHealth(float newHealth)
    {
        Health = newHealth;
        hpBar.value = Health / HumanData.maxHp;
    }

    public void SetFlipX(bool flipState)
    {
        float x = flipState ? -1f : 1f;
        spritesHolderTransform.localScale = new Vector3(x, 1, 1);
    }
}
