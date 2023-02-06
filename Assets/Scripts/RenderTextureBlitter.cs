using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class RenderTextureBlitter : MonoBehaviour
{    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (null == m_rt) {
            Graphics.Blit(src, dest);            
        }
        else {
            Graphics.Blit(m_rt, dest);            
        }
    }

    
    
    [SerializeField] private RenderTexture m_rt;

}
