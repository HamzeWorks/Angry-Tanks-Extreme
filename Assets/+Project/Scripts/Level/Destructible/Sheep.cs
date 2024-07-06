using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Amulay.Utility;

namespace TankStars.Level {
    public class Sheep : DestructibleObject
    {
        [SerializeField] Animator animator;
        [SerializeField] Collider2D[] colliders;
        [SerializeField] SpriteRenderer[] spriteRenderers;
        [SerializeField] ParticleSystem particle;
        [SerializeField] AudioClip[] clips;

        private bool destroyed = false;
        private int groundLayer;

        private void Start()
        {
            groundLayer = 1 << LayerMask.NameToLayer("Ground");
            //GameplayManager.instance.onStartNewRound += (a) => Jump();
            Alignment();

            StartCoroutine(Movement());
        }

        IEnumerator Movement()
        {
            yield return null;

            do
            {
                float moveDutaion = Random.Range(1,4);
                float moveInput = Random.Range(.6f, 1f) * ((Random.value > .5f) ? -1 : 1);

                Debug.Log("Move Input = " + moveInput);
                Invoker.DoUpdate(moveDutaion, () => Move(moveInput));
                yield return new WaitForSeconds(moveDutaion + 1);

                yield return new WaitForSeconds(Random.Range(1, 4f));

            } while (true);
        }

        public void Move(float moveInput)
        {
            if (destroyed) return;
            if (moveInput < 0 && gameObject.transform.localScale.x < 0 || moveInput > 0 && gameObject.transform.localScale.x > 0) 
                gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            
            transform.position += new Vector3(moveInput * 3 * Time.deltaTime, 0, 0);
            //Alignment();
        }

        private void FixedUpdate()
        {
            Alignment();
        }

        private void Alignment()
        {
            var hit = Physics2D.Raycast(transform.position + transform.up, -transform.up, 50f, groundLayer);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                transform.up = Vector2.Lerp(transform.up, hit.normal, Time.fixedDeltaTime * 3f); //TODO: don't use lerp;
            }
        }



        public void Jump()
        {
            var hit = Physics2D.Raycast(transform.position + new Vector3(5, 10), Vector3.down, 50f, groundLayer);
            transform.DOJump(hit.point, 3, 1, 1).onComplete += () => Alignment();
        }

        internal override void DestroyObject(Vector3 position, float radius, ItemData data)
        {
            if (destroyed)
                return;
            destroyed = true;

            AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length)], Camera.main.transform.position);

            if (particle != null)
                Instantiate(particle, transform.position, Quaternion.identity);

            Destroy(GetComponent<Rigidbody2D>());

            foreach (var item in colliders)
            {
                item.enabled = true;
                var rig = item.gameObject.AddComponent<Rigidbody2D>();
                Vector2 vec = (item.transform.position - position);
                rig.AddForce(vec.normalized * radius * 2, ForceMode2D.Impulse);
            }

            foreach (var item in spriteRenderers)
            {
                item.DOFade(0, 1.4f).SetDelay(.5f);
            }

            Destroy(gameObject, 2.01f);
        }
    }
}