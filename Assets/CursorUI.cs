using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour
{
    [SerializeField] GameObject cursorPoint;
    [SerializeField] GameObject cursorGrab;
    [SerializeField] GameObject cursorHover;

    [SerializeField] Transform circleEffectHolder;
    [SerializeField] GameObject vfx_circleclick;

    public static CursorUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;

        GameObject newCursor = cursorPoint;

        if (Input.GetMouseButton(0))
        {
            transform.localScale = Vector3.one * 0.9f;
        }
        else
        {
            transform.localScale = Vector3.one * 1f;
        }

        if (ClickManager.instance != null)
        {
            if (ClickManager.instance.isMouseOverHuman)
            {
                newCursor = cursorHover;
            }

            if (ClickManager.instance.heldHuman != null)
            {
                newCursor = cursorGrab;
            }
        }

        SetCursorActive(newCursor);
    }

    void SetCursorActive(GameObject target)
    {
        cursorPoint.SetActive(false);
        cursorGrab.SetActive(false);
        cursorHover.SetActive(false);
        target.SetActive(true);
    }

    public void SpawnClickEffect()
    {
        Destroy(Instantiate(vfx_circleclick, transform.position, Quaternion.identity, circleEffectHolder), 1f);
    }
}
