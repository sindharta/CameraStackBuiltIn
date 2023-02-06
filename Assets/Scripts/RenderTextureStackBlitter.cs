using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class RenderTextureStackBlitter : MonoBehaviour {
    
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {

        int numRenderTextures = m_renderTextures.Count; 
        if (numRenderTextures <= 0) {
            Graphics.Blit(src, dest);
            return;
        }

        if (numRenderTextures <= 1) {
            Graphics.Blit(m_renderTextures[0], dest);
            return;
        }
        

        for (int i = 0; i < NUM_SWAP_BUFFERS; ++i) {
            m_swapBuffers[i] = RenderTexture.GetTemporary(src.width, src.height);
            ClearRenderTexture(m_swapBuffers[i]);
        }

        if (null == m_material) {
            Shader shader = Shader.Find("Custom/RenderTextureBlend");
            m_material = new Material(shader);
        }
        
        //Blend
        Graphics.Blit(m_renderTextures[0], m_swapBuffers[0], m_material);        
        for (int i = 1; i < numRenderTextures-1; ++i) {
            m_material.SetTexture(SHADER_PROP_BACKGROUND, m_swapBuffers[(i-1) % NUM_SWAP_BUFFERS]);
            Graphics.Blit(m_renderTextures[i], m_swapBuffers[(i) % NUM_SWAP_BUFFERS], m_material);                        
        }        
        m_material.SetTexture(SHADER_PROP_BACKGROUND, m_swapBuffers[(numRenderTextures-2) % NUM_SWAP_BUFFERS]);
        Graphics.Blit(m_renderTextures[numRenderTextures-1], dest, m_material);
        
        
        for (int i = 0; i < NUM_SWAP_BUFFERS; ++i) {
            RenderTexture.ReleaseTemporary(m_swapBuffers[i]);
        }
        
    }
    
    private static void ClearRenderTexture(RenderTexture rt) {
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = prevRT;            
    }
    
    
    [SerializeField] private List<RenderTexture> m_renderTextures = new List<RenderTexture>();

    
    private          Material        m_material;
    private readonly RenderTexture[] m_swapBuffers = new RenderTexture[NUM_SWAP_BUFFERS];

    private const int NUM_SWAP_BUFFERS = 2;
 
    internal static readonly int SHADER_PROP_BACKGROUND = Shader.PropertyToID("_BackgroundTex");


}
