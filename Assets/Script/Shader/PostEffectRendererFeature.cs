using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostEffectRendererFeature  : ScriptableRendererFeature
{
    [SerializeField]
    private List<Material> material;    //ポストエフェクトシェーダーを適応したマテリアル
    private PostEffectRenderPass  postEffectRenderPass;
    
    public override void Create()
    {
        postEffectRenderPass  = new PostEffectRenderPass (material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(postEffectRenderPass);
    }
}

//  [SerializeField] private Shader _shader;

//     private GrayScaleRenderPass grayscalePass;

//     public override void Create()
//     {
//         grayscalePass = new GrayScaleRenderPass(_shader);
//     }

//     public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//     {
//         renderer.EnqueuePass(grayscalePass);
//     }