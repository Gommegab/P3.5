using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CorutineManager : NetworkBehaviour
{
    public IEnumerator ManagerBuff()
    {

        while (true)
        {
            yield return new WaitForSecondsRealtime(10f);
            if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsIds.Count > 0)
            {
                List<NetworkClient> clients = new List<NetworkClient>();

                foreach (NetworkClient uid in NetworkManager.Singleton.ConnectedClientsList)
                {
                    clients.Add(uid);
                }
                int tempnum = Random.Range(0, clients.Count);
                clients[tempnum].PlayerObject.GetComponent<PlayerNetwork>().BuffClientRpc();
                clients.RemoveAt(tempnum);
                if (clients.Count > 0)
                {
                    tempnum = Random.Range(0, clients.Count);
                    clients[tempnum].PlayerObject.GetComponent<PlayerNetwork>().DeBuffClientRpc();
                }
                yield return new WaitForSecondsRealtime(10f);
                foreach (NetworkClient uid in NetworkManager.Singleton.ConnectedClientsList)
                {
                    uid.PlayerObject.GetComponent<PlayerNetwork>().NormalClientRpc();
                }
            }
            else
            {
                  yield return new WaitForSecondsRealtime(10f);
            }
        }
    }
}
