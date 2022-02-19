
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private string ActionType;

    private void Start() {
        if (ActionType == null || ActionType == ""){
            ActionType = "Add";
        }    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch(ActionType){
                case "Add":
                    collision.GetComponent<Health>().AddHealth(healthValue);
                    break;
                case "Increase":
                    collision.GetComponent<Health>().AddLife();
                    break;
            }
            gameObject.SetActive(false);

        }
    }
}
