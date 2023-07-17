using UnityEngine;

public class TestButton : MonoBehaviour
{
   [SerializeField] private Resource _resource;

   public void TakeDamage()
   {
      _resource.TakeDamage(1,0);
   }
}
