using UnityEngine;
using AdminLIB;

public class Example()
{
    void Start()
    {
        Admin.RegisterAdmins(new string[]
        {
            "YOUR_PHOTON_ID",
            "FRIEND_PHOTON_ID"
        });

        if (Admin.IsAdmin)
        {
            Debug.Log("You are an admin!");
        }
        else
        {
            Debug.Log("You are NOT an admin!");
        }
    }
}
