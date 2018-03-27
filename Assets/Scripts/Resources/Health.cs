using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public float health = 100f;
    float currentHealth;
    void Start()
    {
        currentHealth = health;
    }
    [PunRPC]
	public void TakeDamage(float amt)
    {
        currentHealth -= amt;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (GetComponent<PhotonView>().instantiationId == 0)
        {
            Destroy(gameObject);
        }else
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
                //GUI.Label(GuiRect,PhotonNetwork.player.name + " died " + deaths + " times");
            }
        }
    }
}
