using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameA : MonoBehaviour
{
    public GameObject guestPrefab;

    public GameObject door;
    public GameObject waitingZone;
    public GameObject toilet;

    Queue<GameObject> waitingLine = new Queue<GameObject>();
    bool usingToilet = false;

    private void Start()
    {
        StartCoroutine(SpawnGuest(10f));
    }

    IEnumerator SpawnGuest(float term)
    {
        while (true)
        {
            var guest = Instantiate(guestPrefab);
            guest.transform.position = door.transform.position;
            guest.gameObject.SetActive(true);
            EnterToilet(guest);

            yield return new WaitForSeconds(term);
        }
    }

    void EnterToilet(GameObject guest)
    {
        guest.GetComponent<GuestA>().SetMover(0.01f, 0.1f, 0.0004f);
        waitingLine.Enqueue(guest);

        var waitingPos = waitingZone.transform.position;
        waitingPos.y = waitingZone.transform.position.y - waitingLine.Count + 1;
        guest.GetComponent<GuestA>().DoMove(waitingPos, MoveToUseToilet);
    }

    void MoveToUseToilet()
    {
        if (usingToilet)
            return;

        usingToilet = true;

        var guest = waitingLine.Dequeue();
        guest.GetComponent<GuestA>().DoMove(toilet.transform.position, () => UsingToilet(guest));

        var waitignNumber = 0;
        foreach (var waiter in waitingLine)
        {
            var waitingPos = waitingZone.transform.position;
            waitingPos.y = waitingZone.transform.position.y - waitignNumber;
            waiter.GetComponent<GuestA>().DoMove(waitingPos, null);
            waitignNumber++;
        }
    }

    void UsingToilet(GameObject guest)
    {
        guest.GetComponent<GuestA>().DoAction(15f, () => FinishUsingToilet(guest));
    }

    void FinishUsingToilet(GameObject guest)
    {
        usingToilet = false;
        MoveToWashHand(guest);
        MoveToUseToilet();
    }

    void MoveToWashHand(GameObject guest)
    {
        guest.GetComponent<GuestA>().DoMove(door.transform.position, () => guest.gameObject.SetActive(false));
    }
}
