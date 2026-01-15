using UnityEngine;

public class CartDestroy : MonoBehaviour
{
    public float destroyYLimit = -10f;
    private MinecartController cart;

    private void Awake() => cart = GetComponent<MinecartController>();

    private void Update()
    {
        if (transform.position.y < destroyYLimit)
        {
            SafeDestroyCart();
        }
    }

    private void SafeDestroyCart()
    {
        Destroy(gameObject);
        Debug.Log("[CartDestroy] La bagoneta destruida de manera segura.");
    }

}




