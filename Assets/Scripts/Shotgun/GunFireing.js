function Update () {

    if (Input.GetButtonDown("Fire 1")) {
        var gunsound : AudioSource = GetComponent.<AudioSource>();
        gunsound.Play();

        GetComponent.<Animation>().Play("Firing");
    }
    if (Input.GetButtonDown("Sneak")){
        	GetComponent.<Animation>().Play("haha");
    }
    if (Input.GetButtonUp("Sneak")){
        	GetComponent.<Animation>().Play("idle");
    }
    if (Input.GetMouseButtonDown(1)){
        GetComponent.<Animation>().Play("aimin");
    }
    if (Input.GetMouseButtonUp(1)){
        GetComponent.<Animation>().Play("aimout");
    }
}