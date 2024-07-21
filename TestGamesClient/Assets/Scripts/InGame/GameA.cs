using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameA : MonoBehaviour
{
    public GameObject guestPrefab;

    public GameObject door;
    public GameObject waitingZone;
    public GameObject toilet;

    ServiceA<GuestA> toiletService = new ServiceA<GuestA>();

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
            EnterToilet(guest.GetComponent<GuestA>());

            yield return new WaitForSeconds(term);
        }
    }

    void EnterToilet(GuestA guest)
    {
        guest.SetMover(5f, 0.1f, 0.01f);
        toiletService.Enqueue(guest);

        var waitingPos = waitingZone.transform.position;
        waitingPos.y = waitingZone.transform.position.y - toiletService.GetWaiterCount() + 1;
        guest.DoMove(waitingPos, MoveToUseToilet);
    }

    void MoveToUseToilet()
    {
        if (toiletService.IsUsingService())
            return;

        if (toiletService.TryUsingService(out var guest, RearrangeToiletLine))
            guest.GetComponent<GuestA>().DoMove(toilet.transform.position, () => UsingToilet(guest));
    }

    void RearrangeToiletLine(GuestA guest, int waitingNumber)
    {
        var waitingPos = waitingZone.transform.position;
        waitingPos.y = waitingZone.transform.position.y - waitingNumber;
        guest.DoMove(waitingPos, null);
    }

    void UsingToilet(GuestA guest)
    {
        guest.DoAction(15f, () => FinishUsingToilet(guest));
    }

    void FinishUsingToilet(GuestA guest)
    {
        toiletService.CompleteToUsingService();
        MoveToWashHand(guest);
        MoveToUseToilet();
    }

    void MoveToWashHand(GuestA guest)
    {
        guest.DoMove(door.transform.position, () => guest.gameObject.SetActive(false));
    }
}
