using UnityEngine;
using System.Collections;

public class SoundSource : MonoBehaviour {

#region general Sound
    [Header("General Sounds")]
    public AudioClip puzzleComplete;
    public AudioClip questComplete;
    public AudioClip levelUp;
    public AudioClip openingShop;
    public AudioClip buyingSomething;
    public AudioClip ragebarFull;
    public AudioClip buttonClick;
    public AudioClip inventoryOpenClose;
    public AudioClip itemPickUp;
    public AudioClip itemDrop;
    public AudioClip equipingSound;
    #endregion

#region player Sound
    [Header("Player Sounds")]
    public AudioClip playerDamageDealing;
    public AudioClip playerDamageTaking;
    public AudioClip playerTalk;
    public AudioClip playerJump;
    public AudioClip playerCrouch;
    public AudioClip playerWalk;
    public AudioClip playerRun;
    public AudioClip playerThrow;
    public AudioClip playerDied;
    #endregion

#region enemy Sound
    [Header("Enemy Sounds")]
    public AudioClip enemyDamageGiving;
    public AudioClip enemyDamageTaking;
    public AudioClip enemyDied;
    public AudioClip radiusSound;
    #endregion
	

	void Update () {
	
	}
}
