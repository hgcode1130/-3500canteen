using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WaterDistortionFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class WaterDistortionSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        [Range(0, 0.1f)]
        public float distortionStrength = 0.02f;

        [Range(0, 5f)]
        public float waveSpeed = 1.0f;

        [Range(0, 1f)]
        public float causticsIntensity = 0.5f;

        [Range(0, 10f)]
        public float causticsScale = 2.0f;

        public Texture2D noiseTexture;
        public Texture2D causticsTexture;
    }

    public WaterDistortionSettings settings = new WaterDistortionSettings();
    private WaterDistortionPass waterDistortionPass;
    private Shader waterDistortionShader;
    private Material waterDistortionMaterial;

    static readonly int DistortionStrength = Shader.PropertyToID("_DistortionStrength");
    static readonly int WaveSpeed = Shader.PropertyToID("_WaveSpeed");
    static readonly int CausticsIntensity = Shader.PropertyToID("_CausticsIntensity");
    static readonly int CausticsScale = Shader.PropertyToID("_CausticsScale");
    static readonly int NoiseTexture = Shader.PropertyToID("_NoiseTexture");
    static readonly int CausticsTexture = Shader.PropertyToID("_CausticsTexture");
    static readonly int TimeFactor = Shader.PropertyToID("_TimeFactor");

    public override void Create()
    {
        waterDistortionShader = Shader.Find("Hidden/URP/WaterDistortion");
        if (waterDistortionShader == null)
        {
            Debug.LogError("�޷��ҵ�ˮ�����Ŷ���ɫ������ȷ����ɫ��·����ȷ");
            return;
        }

        waterDistortionMaterial = CoreUtils.CreateEngineMaterial(waterDistortionShader);
        waterDistortionPass = new WaterDistortionPass(settings, waterDistortionMaterial);
        waterDistortionPass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (waterDistortionMaterial == null)
        {
            Debug.LogWarning("ˮ�����Ŷ�����δ��ʼ�������������Ⱦͨ��");
            return;
        }

        renderer.EnqueuePass(waterDistortionPass);
    }

    public class WaterDistortionPass : ScriptableRenderPass
    {
        private WaterDistortionSettings settings;
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        private string profilerTag = "Water Distortion";

        public WaterDistortionPass(WaterDistortionSettings settings, Material material)
        {
            this.settings = settings;
            this.material = material;
            tempTexture.Init("_TempWaterDistortionTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // �޸���ʹ�� renderingData.cameraData.renderer.cameraColorTarget ��� renderer.cameraColorTarget
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

            // ���ò�������
            material.SetFloat(DistortionStrength, settings.distortionStrength);
            material.SetFloat(WaveSpeed, settings.waveSpeed);
            material.SetFloat(CausticsIntensity, settings.causticsIntensity);
            material.SetFloat(CausticsScale, settings.causticsScale);
            material.SetTexture(NoiseTexture, settings.noiseTexture);
            material.SetTexture(CausticsTexture, settings.causticsTexture);
            material.SetFloat(TimeFactor, Time.time);

            // ִ��Ч��
            cmd.GetTemporaryRT(tempTexture.id, renderingData.cameraData.cameraTargetDescriptor);
            cmd.Blit(source, tempTexture.Identifier(), material);
            cmd.Blit(tempTexture.Identifier(), source);
            cmd.ReleaseTemporaryRT(tempTexture.id);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}