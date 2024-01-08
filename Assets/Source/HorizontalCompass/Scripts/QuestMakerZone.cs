using UnityEngine;

public class QuestMakerZone : MonoBehaviour
{
    [SerializeField] private QuestMaker[] _hiddenQuestMakers;
    [SerializeField] private QuestMaker[] _openQuestMakers;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Compass compass))
        {
            if(_hiddenQuestMakers.Length > 0)
            {
                foreach (var maker in _hiddenQuestMakers)
                {
                    compass.RemoveQuestMarket(maker);
                }
            }

            if(_openQuestMakers.Length > 0)
            {
                foreach (var maker in _openQuestMakers)
                {
                    compass.AddQuestMarket(maker);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
