using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPoint : FEvent
{
    [SerializeField] private Phrase[] phrases;
    private Animator characterAnimator; // Аніматор персонажа
    float timePerCharacter = 0.05f;

    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

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
            string[] animations = phrases[i].animations;

            for (int k = 0; k < characterPhrases.Length; k++)
            {
                string phrase = characterPhrases[k];
                string animation = animations.Length > k ? animations[k] : "Idle";
                PlayAnimation(animation);

                dialogManager.phraseField.text = string.Empty;

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
        PlayAnimation("Idle"); // Повернення до idle
    }

    public void PlayAnimation(string animationName)
    {
        if (characterAnimator != null)
        {
            characterAnimator.Play(animationName);
        }
    }

    public override void OnInteract(PlayerDialogManager playerDialogManager)
    {
        startDialogWithPlayer(playerDialogManager);
    }
}

[System.Serializable]
public class Phrase
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string[] phrases;
    [SerializeField]
    public string[] animations; 
}
