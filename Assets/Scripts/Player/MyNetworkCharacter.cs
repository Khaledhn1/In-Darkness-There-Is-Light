using UnityEngine;
using System.Collections;

public class MyNetworkCharacter : Photon.MonoBehaviour
{

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;

    public bool Updated = false;

    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("ZOMG, you forgot to put an Animator component on this character prefab!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.23f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.23f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // This is OUR player. We need to send our actual position to the network.

            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetFloat("Speed"));
            stream.SendNext(anim.GetBool("Jumping"));
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            anim.SetFloat("Speed", (float)stream.ReceiveNext());
            anim.SetBool("Jumping", (bool)stream.ReceiveNext());

            if (Updated == false)
            {
                transform.position = realPosition;
                /*
                WHO THE FUCK wANTS TO DANCE?
                Why?Who's asking ?<3<3 (as Alena does a jerking motion with her hands)
                WHOA I...I eh fine LETS FUCK!
                (And they proceded to fuck...)
                THE END...OR IS IT?????????????????????????
                yes... yes it is so.. fuck off
                */
                transform.rotation = realRotation;
                Updated = true;
            }
        }

    }
}