using UnityEngine;


[ CreateAssetMenu(fileName = "GameplayAction", menuName = "Arcade2D/GameplayAction")]
public class GameplayAction : ScriptableObject
{
    public AnimatorOverrideController animatorOverrideController;
    public AnimationClip Clip;
    
    public int Damage;
    public float Speed;

}