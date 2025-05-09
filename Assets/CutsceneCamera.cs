using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutsceneCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraPathGroup
    {
        public string groupName;
        public List<Transform> pathPoints; // позиція і ротація
        public float distanceBeforeExit = 1;
        public float waitBeforeExit = 0;
        public float moveSpeed = 5f;
        public float rotateSpeed = 2f;
    }

    [Header("Cutscene Settings")]
    public List<CameraPathGroup> cameraPathGroups;
    public GameObject playerCamera;
    public UnityEvent onCutsceneStart;
    public UnityEvent onCutsceneEnd;
    public GameObject CutsceneCanvas;

    private int currentGroupIndex = 0;
    private int currentPointIndex = 0;
    private bool isMoving = false;
    public bool playOnStart = false;


    private void Start()
    {
        if (playOnStart)
        {
            StartCatScene();
        }
    }

    [Header("Налаштування")]
    public Image targetImage;
    public float fadeDuration = 2f;

    private float elapsed = 0f;
    private Color originalColor;
    private bool isFading = false;

    public void StartFade()
    {
        elapsed = 0f;
        isFading = true;
    }

    void Update()
    {
        if (!isFading && targetImage == null) return;

        elapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);

        targetImage.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            alpha
        );

        if (elapsed >= fadeDuration)
        {
            isFading = false;
            targetImage.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                0f
            );
        }
    }

    public void StartCatScene()
    {

        if (targetImage != null)
        {
            originalColor = targetImage.color;
            StartFade();

        }


        currentGroupIndex = 0;
        currentPointIndex = 0;
        if (cameraPathGroups.Count == 0) return;

        if (playerCamera != null)
            playerCamera.SetActive(false);
        CutsceneCanvas.SetActive(true);
        onCutsceneStart?.Invoke();
        StartCoroutine(PlayGroup(currentGroupIndex));
    }

    private IEnumerator PlayGroup(int groupIndex)
    {
        var group = cameraPathGroups[groupIndex];
        if (group.pathPoints.Count == 0) yield break;

        // Телепортація до першої точки
        transform.position = group.pathPoints[0].position;
        transform.rotation = group.pathPoints[0].rotation;
        currentPointIndex = 1;
        isMoving = true;

        while (currentPointIndex < group.pathPoints.Count)
        {
            Transform target = group.pathPoints[currentPointIndex];
            float speed = group.moveSpeed;
            float rotateSpeed = group.rotateSpeed;

            while (Vector3.Distance(transform.position, target.position) > group.distanceBeforeExit)
            {
                // Плавний рух
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                // Плавне обертання до заданої ротації
                transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);

                yield return null;
            }

            currentPointIndex++;
        }

        isMoving = false;

        currentGroupIndex++;
        if (currentGroupIndex < cameraPathGroups.Count)
        {
            yield return new WaitForSeconds(group.waitBeforeExit);
            StartCoroutine(PlayGroup(currentGroupIndex));
        }
        else
        {
            EndCutscene();
        }
    }

    private void EndCutscene()
    {
        if (playerCamera != null)
            playerCamera.SetActive(true);

        CutsceneCanvas.SetActive(false);

        onCutsceneEnd?.Invoke();
        targetImage.color = Color.black;
        gameObject.SetActive(false);
    }
}
