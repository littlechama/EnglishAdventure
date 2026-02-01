using UnityEngine;

public class AnsZone : MonoBehaviour
{
    public int zoneIndex;
    public bool isPlayerInside = false;

    private SpriteRenderer zoneRenderer;

    private void Start()
    {
        zoneRenderer = GetComponent<SpriteRenderer>();
    }

    // ★重要：メソッド名に "2D" を付け、引数も "Collider2D" にします
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2D衝突検知: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered Zone " + zoneIndex);
            isPlayerInside = true;
            if (zoneRenderer != null)
            {
                zoneRenderer.color = Color.black; // SpriteRendererなら .color で直接変えられます
            }
        }
    }

    // ★重要：こちらも "2D" です
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (zoneRenderer != null)
            {
                zoneRenderer.color = Color.white;
            }
        }
    }
}