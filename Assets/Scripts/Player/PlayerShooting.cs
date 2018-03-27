using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

    public float cooldown = 0.5f;
    float cooldownRemaining = 0f;
    public float damage = 35f;
    FXManager fx;

    void Start()
    {
        fx = GameObject.FindObjectOfType<FXManager>();
    }

	// Update is called once per frame
	void Update () {
        cooldownRemaining -= Time.deltaTime;

        if (Input.GetButton("Fire 1"))
        {
            if (PauseMenu.isPaused == true)
            {
                Debug.Log("You Are Paused");
            }
            else
            {
                Fire();
            }
        }
	}

    void Fire()
    {

        if(cooldownRemaining > 0)
        {
            return;
        }
        Debug.Log("Fireing");

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Transform hitTransform;
        Vector3 hitPoint;

        hitTransform = FindColsestHitObject(ray, out hitPoint);

        if(hitTransform != null)
        {
            Debug.Log("We Hit " + hitTransform.transform.name);

            Health h = hitTransform.transform.GetComponent<Health>();

            if(h == null && hitTransform.parent)
            {
                hitTransform = hitTransform.parent;
                h = hitTransform.GetComponent<Health>();
            }

            if(h != null)
            {


                h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBufferedViaServer, damage);
                
                
                //h.TakeDamage(damage);
            }
            if(fx != null)
            {
                hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
                fx.GetComponent<PhotonView>().RPC("bulletFX",PhotonTargets.All, Camera.main.transform.position, hitPoint);
            }
        }
        else
        {
            if (fx != null)
            {
                fx.GetComponent<PhotonView>().RPC("bulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
            }
        }

        cooldownRemaining = cooldown;
    }

    Transform FindColsestHitObject(Ray ray, out Vector3 hitPoint)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray);

        Transform closestHit = null;
        float distance = 0;
        hitPoint = Vector3.zero;


        foreach (RaycastHit hit in hits)
        {
            if (hit.transform != this.transform && (closestHit == null || hit.distance < distance))
            {
                closestHit = hit.transform;
                distance = hit.distance;
            } 
        }
        return closestHit;
    }
}
