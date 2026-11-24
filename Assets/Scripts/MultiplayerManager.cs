using JetBrains.Annotations;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
   [UsedImplicitly]
   public void OnPlayerJoined() {
      Debug.Log("OnPlayerJoined");
   }
   [UsedImplicitly]
   public void OnPlayerLeft() {
      Debug.Log("OnPlayerLeft");
   }
}
