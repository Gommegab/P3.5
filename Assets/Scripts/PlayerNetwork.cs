using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerNetwork : NetworkBehaviour
{
    private float speed = 5f;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CorutineManager cm = new CorutineManager();
            StartCoroutine(cm.ManagerBuff());
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 300f, 0));

        }
        this.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;

    }

    private bool CanJump()
    {
        return transform.position.y <= 1f;
    }
    [ClientRpc]
    public void BuffClientRpc()
    {
        speed += 2f;
        GetComponent<Renderer>().material.color = Color.green;
    }
    [ClientRpc]
    public void DeBuffClientRpc()
    {
        speed -= 2f;
        GetComponent<Renderer>().material.color = Color.red;
    }
    [ClientRpc]
    public void NormalClientRpc()
    {
        speed = 5f;
        GetComponent<Renderer>().material.color = Color.white;
    }
}
