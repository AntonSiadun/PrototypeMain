using Photon.Pun;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PlayerElements
{
    public class PlayerTrigger : MonoBehaviourPunCallbacks,IPunObservable
    {
        public int Health { get; private set; }
        
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
            Health = 3;
        }

        private void OnTriggerEnter(Collider other)
        {
            GetDamageIfAttackEnemyPlayer(other);
        }

        private void GetDamageIfAttackEnemyPlayer(Collider enemy)
        {
            if (!IsEnemyWeaponCollideWithPlayer(enemy)) 
                return;
            GetDamageFromEnemyCollider(enemy);
            CheckPlayerHealthAndReactOrKill();
        }

        private void GetDamageFromEnemyCollider(Collider enemy)
        {
            var swordController = enemy.gameObject.GetComponent<SwordController>();
            var swordOwner = swordController.owner;
            Player damageDealer = swordOwner.GetComponent<Player>();
            Debug.Log("Get Damage");
            Health -= damageDealer.GetCurrentAttackSkillDamage();
        }

        private void CheckPlayerHealthAndReactOrKill()
        {
            if (Health <= 0)
            {
                _player.Kill();
            }
            else
            {
                _player.React();
            }
        }
        
        private bool IsEnemyWeaponCollideWithPlayer(Collider sword)
        {
            return sword.gameObject.CompareTag("Sword") && sword.gameObject.GetComponent<SwordController>()
                       .owner.CompareTag("Enemy");
        }

        private bool IsMobWeaponCollideWithPlayer(Collider sword)
        {
            var swordController = sword.gameObject.GetComponent<SwordController>();
            var swordOwner = swordController.owner;
            return sword.gameObject.CompareTag("Sword") && swordOwner.CompareTag("Mob");
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Health);
            }
            else
            {
                Health = (int) stream.ReceiveNext();
            }
        }
    }
}
