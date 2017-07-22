using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private Light target;

        private Quaternion baseRotation;
        private Quaternion flashRotation;
        private float flashIntensity;
        private float flashTimer;
        private float flashAlpha;


        public void Flash()
        {
            this.flashAlpha = Mathf.Clamp01(this.flashAlpha + 1.0f);
            this.flashRotation = Quaternion.Euler(0.15f * Random.rotation.eulerAngles);
            this.flashIntensity = Mathf.Max(this.flashIntensity, Random.Range(1.0f, 3.5f));
        }

        private void Awake()
        {
            this.baseRotation = this.target.transform.rotation;
        }
        private void Update()
        {
            this.flashTimer -= Time.deltaTime;
            if (this.flashTimer <= 0.0f)
            {
                this.Flash();
                this.flashTimer += Mathf.Pow(2.0f, Random.Range(-1.0f, 2.5f));
            }

            if (this.flashAlpha > 0.0f)
            {
                this.flashAlpha = Mathf.MoveTowards(
                    this.flashAlpha,
                    0.0f,
                    Mathf.Max(1.0f, this.flashAlpha * 2.5f) * Time.deltaTime);

                this.target.intensity = this.flashAlpha * this.flashIntensity;
                this.target.transform.rotation = this.flashRotation * this.baseRotation;
                this.target.enabled = true;
            }
            else
            {
                this.target.enabled = false;
                this.flashIntensity = 0.0f;
                this.flashRotation = Quaternion.identity;
            }
        }
    }
}