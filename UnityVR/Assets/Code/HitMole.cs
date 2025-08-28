using System;
using UnityEngine;
using UnityEngine.Events;

public class HitMole : MonoBehaviour
{
   public UnityEvent OnHit;
   public bool isUp = false;
   
   private void OnCollisionEnter(Collision other)
   {
      if (other.collider.TryGetComponent(out MoleHammer hammer))
      {
         print("Mole Hit!");
         if(isUp)
            OnHit?.Invoke();
      }
   }
}
