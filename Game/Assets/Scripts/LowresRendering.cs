using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class LowresRendering : MonoBehaviour
    {
        [SerializeField] private Material mat;

        private Camera cam;
        private RenderTexture tex;

        private int TargetWidth
        {
            get { return Screen.width / 16; }
        }
        private int TargetHeight
        {
            get { return Screen.height / 16; }
        }

        private void DestroyRenderTarget()
        {
            if (this.tex == null) return;
            Destroy(this.tex);
            this.tex = null;
        }
        private void SetupRenderTarget()
        {
            if (this.tex != null)
                this.DestroyRenderTarget();

            this.tex = new RenderTexture(
                this.TargetWidth, 
                this.TargetHeight, 
                24, 
                RenderTextureFormat.ARGBFloat, 
                RenderTextureReadWrite.sRGB);
            this.tex.filterMode = FilterMode.Point;
            this.tex.useMipMap = false;
            this.tex.autoGenerateMips = false;
        }
        private void EnsureRenderTarget()
        {
            if (this.tex == null || 
                this.tex.width != this.TargetWidth || 
                this.tex.height != this.TargetHeight)
            {
                this.SetupRenderTarget();
            }
        }

        private void Awake()
        {
            this.cam = GetComponent<Camera>();
        }
        private void OnPreRender()
        {
            this.EnsureRenderTarget();
            this.cam.targetTexture = this.tex;
        }
        private void OnPostRender()
        {
            this.cam.targetTexture = null;
            Graphics.Blit(this.tex, null as RenderTexture, this.mat);
        }
    }
}