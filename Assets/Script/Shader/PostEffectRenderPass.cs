using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class PostEffectRenderPass  : ScriptableRenderPass
{
    private readonly List<Material> _material;

    public PostEffectRenderPass(List<Material> material, RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing)
    {
        _material = material;
        this.renderPassEvent = renderPassEvent;
    }
    
    //ContextContainerは描画に必要なデータを格納するためのコンテナー、汎用的な設計になっており、ContextItemクラスを継承すれば、どんなデータも入れることができる。
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)//描画設定、描画実行など一連の操作が全部集約された関数
    {
        // リソース関連の情報：カメラカラーバッファ、カメラ深度バッファなど
        var resourceData = frameData.Get<UniversalResourceData>();

        var activeColorTexture = resourceData.activeColorTexture;
        TextureDesc desc = renderGraph.GetTextureDesc(activeColorTexture);
        desc.name = "PostEffectTempTexture";
        TextureHandle tempTexture = renderGraph.CreateTexture(desc);

        int loop;
        for (loop = 0; loop < _material.Count; loop++)
        {
            if (loop % 2 == 0)
            {
                RenderGraphUtils.BlitMaterialParameters param = new(activeColorTexture, tempTexture, _material[loop], 0);
                renderGraph.AddBlitPass(param);
            }
            else
            {
                RenderGraphUtils.BlitMaterialParameters param = new(tempTexture, activeColorTexture, _material[loop], 0);
                renderGraph.AddBlitPass(param);
            }

        }

        // 奇数ならコピーする
        if (loop % 2 == 1)
        {
            renderGraph.AddCopyPass(tempTexture, activeColorTexture);
        }

    }
}

//  private readonly Material material;

//     public GrayScaleRenderPass(Shader shader)
//     {
//         material = CoreUtils.CreateEngineMaterial(shader);
//         renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
//     }

//     public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
//     {
//         if (renderingData.cameraData.isSceneViewCamera) { return; }

//         var cmd = CommandBufferPool.Get("GrayScaleRenderPass");

//         using (new ProfilingScope(cmd, new ProfilingSampler("GrayScaleRenderPass")))
//         {
//             var handle = renderingData.cameraData.renderer.cameraColorTargetHandle;
//             Blitter.BlitCameraTexture(cmd, handle, handle, material, 0);
//         }

//         context.ExecuteCommandBuffer(cmd);
//         CommandBufferPool.Release(cmd);
//     }