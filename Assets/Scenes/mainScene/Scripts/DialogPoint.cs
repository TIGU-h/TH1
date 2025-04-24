using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPoint : FEvent
{
    [SerializeField] private Phrase[] phrases;
    [SerializeField] private AnimationClip startAnimation;
    private Animator characterAnimator; // Аніматор персонажа
    float timePerCharacter = 0.05f;

    private void Start()
    {
        characterAnimator = GetComponent<Animator>();

        PlayAnimation(startAnimation);
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

            for (int k = 0; k < characterPhrases.Length; k++)
            {
                string phrase = characterPhrases[k];
                if (k < phrases[i].animations.Length)
                    PlayAnimation(phrases[i].animations[k]);

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
        PlayAnimation(startAnimation); // Повернення до idle
        yield return new WaitForSeconds(0.3f);
        dialogManager.InDialog = false;
    }

    public void PlayAnimation(AnimationClip animation)
    {
        if (characterAnimator != null && animation != null)
        {
            characterAnimator.Play(animation.name);
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
    public AnimationClip[] animations;
}
