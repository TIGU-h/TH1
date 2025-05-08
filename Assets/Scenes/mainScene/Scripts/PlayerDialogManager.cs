using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerDialogManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 5f;
    [SerializeField] private LayerMask FEventLayer;

    [Header("UI Settings")]
    [SerializeField] private GameObject FEventSelectionCanvas;
    [SerializeField] private Transform FEventListParent;
    [SerializeField] private GameObject FEventButtonPrefab;

    public Text nameField;
    public Text phraseField;
    public GameObject dialogOnCanvas;

    private List<FEvent> nearbyFEvents = new List<FEvent>();
    private Dictionary<FEvent, GameObject> FEventButtons = new Dictionary<FEvent, GameObject>();

    private bool inDialogState = false;
    private int selectedButtonIndex = 0;
    private List<Button> FEventButtonList = new List<Button>();

    public bool InDialog
    {
        get => inDialogState;
        set
        {
            if (value)
            {
                var animator = GetComponent<Animator>();
                foreach (var parameter in animator.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Trigger)
                        animator.ResetTrigger(parameter.name);
                    else if (parameter.type == AnimatorControllerParameterType.Bool)
                        animator.SetBool(parameter.name, false);
                }

            }
            GetComponent<PlayerMovementController>().enabled = !value;
            GetComponent<PlayerAttackAndSpellController>().enabled = !value;
            inDialogState = value;
            UpdateFEventListUI();
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateNearbyFEventsCoroutine());
        StartCoroutine(UpdateFEventListUICoroutine());
    }

    private IEnumerator UpdateNearbyFEventsCoroutine()
    {
        while (true)
        {
            DetectFEvents();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator UpdateFEventListUICoroutine()
    {
        while (true)
        {
            UpdateFEventListUI();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Update()
    {
        if (FEventButtonList.Count > 0 && !InDialog)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll < 0)
            {
                SelectNextButton();
            }
            else if (scroll > 0)
            {
                SelectPreviousButton();
            }
            if (Input.GetButton("Submit"))
            {
                FEventButtonList[selectedButtonIndex].onClick.Invoke();
            }
        }
        SelectButton(selectedButtonIndex);
    }

    private void DetectFEvents()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, FEventLayer);
        List<FEvent> detectedFEvents = new List<FEvent>();

        foreach (Collider collider in colliders)
        {
            FEvent dialogPoint = collider.GetComponent<FEvent>();
            if (dialogPoint != null && dialogPoint.DialogPointName != string.Empty)
            {
                detectedFEvents.Add(dialogPoint);
                if (!nearbyFEvents.Contains(dialogPoint))
                {
                    nearbyFEvents.Add(dialogPoint);
                    AddFEventButton(dialogPoint);
                }
            }
        }

        for (int i = 0; i < nearbyFEvents.Count; i++)
        {
            if (!detectedFEvents.Contains(nearbyFEvents[i]))
            {
                RemoveFEventButton(nearbyFEvents[i]);
                nearbyFEvents.RemoveAt(i);
            }
        }
    }

    private void AddFEventButton(FEvent FEvent)
    {
        if (!FEventButtons.ContainsKey(FEvent))
        {
            GameObject FEventButton = Instantiate(FEventButtonPrefab, FEventListParent);
            Button buttonComponent = FEventButton.GetComponent<Button>();
            FEventButton.GetComponentInChildren<Text>().text = FEvent.DialogPointName;
            buttonComponent.onClick.AddListener(() => FEvent.OnInteract(this));
            FEventButtons[FEvent] = FEventButton;
            FEventButtonList.Add(buttonComponent);

            if (FEventButtonList.Count == 1)
            {
                SelectButton(0);
            }
        }
    }

    private void RemoveFEventButton(FEvent FEvent)
    {
        if (FEventButtons.ContainsKey(FEvent))
        {
            Button buttonToRemove = FEventButtons[FEvent].GetComponent<Button>();
            FEventButtonList.Remove(buttonToRemove);
            Destroy(FEventButtons[FEvent]);
            FEventButtons.Remove(FEvent);

            if (FEventButtonList.Count > 0)
            {
                SelectButton(0);
            }
        }
    }

    private void UpdateFEventListUI()
    {
        FEventSelectionCanvas.SetActive(FEventButtons.Count > 0 && !InDialog);

        //if (FEventListParent.childCount > 0 && !FEventButtons.ContainsValue(EventSystem.current.currentSelectedGameObject))        
        //    SelectButton(0);

    }

    private void StartDialogWithFEvent(DialogPoint FEvent)
    {
        FEvent.startDialogWithPlayer(GetComponent<PlayerDialogManager>());
        //FEventSelectionCanvas.SetActive(false);
    }

    private void SelectButton(int index)
    {
        if (FEventButtonList.Count == 0) return;

        selectedButtonIndex = Mathf.Clamp(index, 0, FEventButtonList.Count - 1);
        EventSystem.current.SetSelectedGameObject(FEventButtonList[selectedButtonIndex].gameObject);
    }

    private void SelectNextButton()
    {
        SelectButton((selectedButtonIndex + 1) % FEventButtonList.Count);
    }

    private void SelectPreviousButton()
    {
        SelectButton((selectedButtonIndex - 1 + FEventButtonList.Count) % FEventButtonList.Count);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
