using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameA : MonoBehaviour
{
    public GameObject guestPrefab;

    public GameObject door;
    public List<WaitingServiceA> toiletServices = new List<WaitingServiceA>();
    public List<WorkEventA> toiletCleanWork = new List<WorkEventA>();

    private void Awake()
    {
        for (int i = 0; i < toiletServices.Count; i++)
        {
            var indexTmp = i;
            toiletServices[i].SetSevice(3, () => NeedCleanToilet(indexTmp), RearrangeToiletLine);
            toiletServices[i].ChargeAvailableCount();
            toiletCleanWork[indexTmp].SetWorkEvent(5f, () =>
            {
                toiletServices[indexTmp].ChargeAvailableCount();
                toiletCleanWork[indexTmp].gameObject.SetActive(false);
            });
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnGuest(2f));
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

    int ChooseToilet()
    {
        var toiletCounter = 0;
        var toiletIndex = 0;
        var minGuests = int.MaxValue;

        foreach (var service in toiletServices)
        {
            if (service.GetWaiterCount() == 0 && service.IsAvailable())
            {
                toiletIndex = toiletCounter;
                break;
            }

            if (service.GetWaiterCount() < minGuests)
            {
                minGuests = service.GetWaiterCount();
                toiletIndex = toiletCounter;
            }

            toiletCounter++;
        }

        return toiletIndex;
    }

    void NeedCleanToilet(int index)
    {
        toiletCleanWork[index].gameObject.SetActive(true);
    }

    void EnterToilet(GuestA guest)
    {
        var chooseToilet = ChooseToilet();
        var toiletService = toiletServices[chooseToilet];

        guest.SetNowUsingService(toiletService);
        toiletService.Enqueue(guest);

        guest.SetMover(5f, 0.1f, 0.01f);

        var waitingPos = toiletService.GetWaitingZone().transform.position;
        waitingPos.y -= toiletService.GetWaiterCount() - 1;
        guest.DoMove(waitingPos, () => MoveToUseToilet(guest.GetNowUsingService()));
    }

    void MoveToUseToilet(WaitingServiceA service)
    {
        if (service.TryUsingService(out var guest))
            guest.DoMove(service.GetServiceZone().transform.position, () => UsingToilet(guest));
    }

    void RearrangeToiletLine(GuestA guest, int waitingNumber)
    {
        var service = guest.GetNowUsingService();
        var waitingPos = service.GetWaitingZone().transform.position;
        waitingPos.y -= waitingNumber;
        guest.DoMove(waitingPos, null);
    }

    void UsingToilet(GuestA guest)
    {
        guest.DoAction(15f, () => FinishUsingToilet(guest));
    }

    void FinishUsingToilet(GuestA guest)
    {
        guest.GetNowUsingService().CompleteToUsingService();
        MoveToWashHand(guest);
        MoveToUseToilet(guest.GetNowUsingService());
    }

    void MoveToWashHand(GuestA guest)
    {
        guest.DoMove(door.transform.position, () => guest.gameObject.SetActive(false));
    }
}
