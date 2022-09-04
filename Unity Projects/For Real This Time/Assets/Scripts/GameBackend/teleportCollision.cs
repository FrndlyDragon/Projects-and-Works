using UnityEngine;

public class teleportCollision : Collision
{
    public string stageName;
    public Sprite activeTeleport;
    protected override void OnCollision(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player") {
            GetComponent<SpriteRenderer>().sprite = activeTeleport;
            Debug.Log("Teleporting");
            CharacterManager.instance.SaveState();
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageName);
        }
    }
}
