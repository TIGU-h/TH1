using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogPoint : MonoBehaviour
{
    public string DialogPointName;
    [SerializeField] private Phrase[] phrases;
    [SerializeField] private Text nameField;
    [SerializeField] private Text phraseField;
    [SerializeField] private float timePerCharacter = 0.05f;
    [SerializeField] private GameObject dialogOnCanvas;


    public void startDialogWithPlayer(PlayerDialogManager dialogManager)
    {
        StartCoroutine(DisplayDialog(dialogManager));
    }



    public IEnumerator DisplayDialog(PlayerDialogManager dialogManager)
    {
        dialogManager.InDialog = true;
        dialogOnCanvas.SetActive(true);
        for (int i = 0; i < phrases.Length; i++)
        {
            nameField.text = phrases[i].name;
            phraseField.text = string.Empty;

            string[] characterPhrases = phrases[i].phrases;

            foreach (string phrase in characterPhrases)
            {

                for (int j = 0; j < phrase.Length; j++)
                {
                    this.phraseField.text += phrase[j];
                    yield return new WaitForSeconds(timePerCharacter);
                }

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                this.phraseField.text = string.Empty;
            }
        }
        dialogOnCanvas.SetActive(false);
        dialogManager.InDialog = false;

        Debug.Log("Діалог завершено");
    }
}

[System.Serializable]
public class Phrase
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string[] phrases;
}