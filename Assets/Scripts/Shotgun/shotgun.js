function Update () {
    
    if (Input.GetButtonDown("Reload")) {
        var gunsound : AudioSource = GetComponent.<AudioSource>();
        gunsound.Play();

        GetComponent.<Animation>().Play("trlolol");

    }
}