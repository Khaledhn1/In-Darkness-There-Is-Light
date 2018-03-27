using UnityEngine;
using System.Collections;

public class FXManager : Photon.MonoBehaviour {

    public GameObject FireingFX;
	[PunRPC]
    void bulletFX(Vector3 startPos, Vector3 endPos)
    {
        Debug.Log("FX SHIT IS WORKING FINALLY ON TRY 100!!!");
        GameObject FX = (GameObject)Instantiate(FireingFX, startPos, Quaternion.LookRotation(endPos - startPos));

        /*LineRenderer lr = FX.transform.Find("Shit I AINT GONNA USE").GetComponent<LineRenderer>();
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);*/
    }
}
