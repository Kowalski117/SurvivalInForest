using UnityEngine;

public class TestButton : MonoBehaviour
{
   [SerializeField] private Animals _animals;

   public void TakeDamage()
   {
      _animals.TakeDamage(15,5);
   }
}
