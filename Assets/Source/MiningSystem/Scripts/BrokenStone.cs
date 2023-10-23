using System.Collections;
using UnityEngine;

public class BrokenStone : MonoBehaviour
{
  [SerializeField] private float _timeDisappearance = 10f;
  private float _timeDestroy = 2f;

  private void Start()
  {
    StartCoroutine(BeDestroy());
  }


  IEnumerator BeDestroy()
  {
    yield return new WaitForSeconds(_timeDisappearance);
    
    foreach (var child in GetComponentsInChildren<Collider>())
    {
      child.enabled = false;
    }
    yield return new WaitForSeconds(_timeDestroy);
    Debug.Log("tut");
    Destroy(this.gameObject);
  }
}
