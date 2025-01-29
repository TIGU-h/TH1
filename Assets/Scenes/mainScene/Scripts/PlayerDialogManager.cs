using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerDialogManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 5f;
    [SerializeField] private LayerMask npcLayer;

    [Header("UI Settings")]
    [SerializeField] private GameObject npcSelectionCanvas;
    [SerializeField] private Transform npcListParent;
    [SerializeField] private GameObject npcButtonPrefab;

    private List<DialogPoint> nearbyNpcs = new List<DialogPoint>();
    private Dictionary<DialogPoint, GameObject> npcButtons = new Dictionary<DialogPoint, GameObject>();

    private bool inDialogState = false;
    private int selectedButtonIndex = 0;
    private List<Button> npcButtonList = new List<Button>();

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
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateNearbyNpcsCoroutine());
        StartCoroutine(UpdateNpcListUICoroutine());
    }

    private IEnumerator UpdateNearbyNpcsCoroutine()
    {
        while (true)
        {
            DetectNpcs();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator UpdateNpcListUICoroutine()
    {
        while (true)
        {
            UpdateNpcListUI();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Update()
    {
        if (npcButtonList.Count > 0 && !InDialog)
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                npcButtonList[selectedButtonIndex].onClick.Invoke();
            }
        }
        SelectButton(selectedButtonIndex);
    }

    private void DetectNpcs()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, npcLayer);
        List<DialogPoint> detectedNpcs = new List<DialogPoint>();

        foreach (Collider collider in colliders)
        {
            DialogPoint dialogPoint = collider.GetComponent<DialogPoint>();
            if (dialogPoint != null)
            {
                detectedNpcs.Add(dialogPoint);
                if (!nearbyNpcs.Contains(dialogPoint))
                {
                    nearbyNpcs.Add(dialogPoint);
                    AddNpcButton(dialogPoint);
                }
            }
        }

        for (int i = 0; i < nearbyNpcs.Count; i++)
        {
            if (!detectedNpcs.Contains(nearbyNpcs[i]))
            {
                RemoveNpcButton(nearbyNpcs[i]);
                nearbyNpcs.RemoveAt(i);
            }
        }
    }

    private void AddNpcButton(DialogPoint npc)
    {
        if (!npcButtons.ContainsKey(npc))
        {
            GameObject npcButton = Instantiate(npcButtonPrefab, npcListParent);
            Button buttonComponent = npcButton.GetComponent<Button>();
            npcButton.GetComponentInChildren<Text>().text = npc.DialogPointName;
            buttonComponent.onClick.AddListener(() => StartDialogWithNpc(npc));
            npcButtons[npc] = npcButton;
            npcButtonList.Add(buttonComponent);

            if (npcButtonList.Count == 1)
            {
                SelectButton(0);
            }
        }
    }

    private void RemoveNpcButton(DialogPoint npc)
    {
        if (npcButtons.ContainsKey(npc))
        {
            Button buttonToRemove = npcButtons[npc].GetComponent<Button>();
            npcButtonList.Remove(buttonToRemove);
            Destroy(npcButtons[npc]);
            npcButtons.Remove(npc);

            if (npcButtonList.Count > 0)
            {
                SelectButton(0);
            }
        }
    }

    private void UpdateNpcListUI()
    {
        npcSelectionCanvas.SetActive(npcButtons.Count > 0 && !InDialog);

        //if (npcListParent.childCount > 0 && !npcButtons.ContainsValue(EventSystem.current.currentSelectedGameObject))        
        //    SelectButton(0);

    }

    private void StartDialogWithNpc(DialogPoint npc)
    {
        npc.startDialogWithPlayer(GetComponent<PlayerDialogManager>());
        npcSelectionCanvas.SetActive(false);
    }

    private void SelectButton(int index)
    {
        if (npcButtonList.Count == 0) return;

        selectedButtonIndex = Mathf.Clamp(index, 0, npcButtonList.Count - 1);
        EventSystem.current.SetSelectedGameObject(npcButtonList[selectedButtonIndex].gameObject);
    }

    private void SelectNextButton()
    {
        SelectButton((selectedButtonIndex + 1) % npcButtonList.Count);
    }

    private void SelectPreviousButton()
    {
        SelectButton((selectedButtonIndex - 1 + npcButtonList.Count) % npcButtonList.Count);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
