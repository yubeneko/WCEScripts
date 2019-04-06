using UnityEngine;
using UniRx;
using System.Linq;

public class PlayerRoomEnterSubscriber : MonoBehaviour
{
    [SerializeField]
    private PlayerRoomEnterObserve playerRoomEnterObserve;

    void Start ()
    {
        playerRoomEnterObserve.OnPlayerRoomEnter
            .Subscribe(_ =>
            {
                foreach(Transform childrenTransform in this.transform)
                {
                    childrenTransform.gameObject.SetActive(true);
                }
            });
    }
}
