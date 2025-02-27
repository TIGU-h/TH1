using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;  
    [SerializeField] private float lifetime = 1f;   

    [SerializeField]private TextMeshProUGUI textMesh;
    [SerializeField] private TMP_FontAsset[] fontAssets;

    public void Setup(int damage, TypeOfDamage typeOfDamage)
    {
        print(damage);
        textMesh.text = damage.ToString();
        transform.LookAt(Camera.main.transform);

        textMesh.font = fontAssets[(int)typeOfDamage];
        textMesh.gameObject.SetActive(true);
        Destroy(gameObject, lifetime);
    }


}
public enum TypeOfDamage
{
    Phisical,
    Wather,
    Earth,
    Fire,
    Air
}