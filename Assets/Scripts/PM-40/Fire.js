function Update () {
    
    if (Input.GetButtonDown("Fire 1")) {
        //var gunsound : AudioSource = GetComponent.<AudioSource>();
       // gunsound.Play();

        GetComponent.<Animation>().Play("Fire");
    }
}