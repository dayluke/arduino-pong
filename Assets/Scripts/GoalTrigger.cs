using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public int whoShouldGetAPoint;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            gameManager.PlayerScored(whoShouldGetAPoint);
        }
    }
}
