using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogPoint : MonoBehaviour
{
    public string DialogPointName;
    [SerializeField] private Phrase[] phrases;
    float timePerCharacter = 0.05f;



    public void startDialogWithPlayer(PlayerDialogManager dialogManager)
    {
        StartCoroutine(DisplayDialog(dialogManager));
    }



    public IEnumerator DisplayDialog(PlayerDialogManager dialogManager)
    {
        dialogManager.InDialog = true;
        dialogManager.dialogOnCanvas.SetActive(true);
        for (int i = 0; i < phrases.Length; i++)
        {
            dialogManager.nameField.text = phrases[i].name;
            dialogManager.phraseField.text = string.Empty;

            string[] characterPhrases = phrases[i].phrases;

            foreach (string phrase in characterPhrases)
            {

                for (int j = 0; j < phrase.Length; j++)
                {
                    dialogManager.phraseField.text += phrase[j];
                    yield return new WaitForSeconds(timePerCharacter);
                }

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F));
                dialogManager.phraseField.text = string.Empty;
            }
        }
        dialogManager.dialogOnCanvas.SetActive(false);
        dialogManager.InDialog = false;

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