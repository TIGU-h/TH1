using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool InDialog
    {
        get => inDialogState;
        
        set
        {
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
            yield return new WaitForSeconds(0.2f); // Adjust delay as needed
        }
    }

    private IEnumerator UpdateNpcListUICoroutine()
    {
        while (true)
        {
            UpdateNpcListUI();
            yield return new WaitForSeconds(0.2f); // Adjust delay as needed
        }
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

        // Remove NPCs that are no longer in range
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
            npcButton.GetComponentInChildren<Text>().text = npc.DialogPointName;
            npcButton.GetComponent<Button>().onClick.AddListener(() => StartDialogWithNpc(npc));
            npcButtons[npc] = npcButton;
        }
    }

    private void RemoveNpcButton(DialogPoint npc)
    {
        if (npcButtons.ContainsKey(npc))
        {
            Destroy(npcButtons[npc]);
            npcButtons.Remove(npc);
        }
    }

    private void UpdateNpcListUI()
    {
        npcSelectionCanvas.SetActive(npcButtons.Count > 0 && !InDialog);
    }

    private void StartDialogWithNpc(DialogPoint npc)
    {
        //InDialog = true;
        npc.startDialogWithPlayer(GetComponent<PlayerDialogManager>());
        npcSelectionCanvas.SetActive(false); // Optionally hide the UI during dialog
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}