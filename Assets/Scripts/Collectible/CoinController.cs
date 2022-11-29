using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")){
            EventManager.TriggerEvent("OnCoin"); 
            Destroy(gameObject);
        }
    }

}
