using UnityEngine;

public enum CrtScanLinesSizes { S32 = 32, S64 = 64, S128 = 128, S256 = 256, S512 = 512, S1024 = 1024, S2048 = 2048 };
[ExecuteInEditMode]
public class Crt : MonoBehaviour
{
    #region Variables
    public Shader curShader;
    public float distortion = 0.1f;
    public float gamma = 1.0f;
    public float yExtra = 0.5f;
    public float curvatureSet1 = 0.5f;
    public float curvatureSet2 = 1.0f;
    public float dotWeight = 1.0f;

    private CrtScanLinesSizes ScanSize
    {
        set => scanSizeVector = new Vector2((float)value, (float)value);
    }
    private Vector2 scanSizeVector;

    public Color rgb1 = Color.white;
    public Color rgb2 = Color.white;
    private Material curMaterial;

    private static readonly int Distortion = Shader.PropertyToID("_Distortion");
    private static readonly int Gamma = Shader.PropertyToID("_Gamma");
    private static readonly int CurvatureSet1 = Shader.PropertyToID("_curvatureSet1");
    private static readonly int CurvatureSet2 = Shader.PropertyToID("_curvatureSet2");
    private static readonly int YExtra = Shader.PropertyToID("_YExtra");
    private static readonly int Rgb1R = Shader.PropertyToID("_rgb1R");
    private static readonly int Rgb1G = Shader.PropertyToID("_rgb1G");
    private static readonly int Rgb1B = Shader.PropertyToID("_rgb1B");
    private static readonly int Rgb2R = Shader.PropertyToID("_rgb2R");
    private static readonly int Rgb2G = Shader.PropertyToID("_rgb2G");
    private static readonly int Rgb2B = Shader.PropertyToID("_rgb2B");
    private static readonly int DotWeight = Shader.PropertyToID("_dotWeight");
    private static readonly int TextureSize = Shader.PropertyToID("_TextureSize");

    #endregion

    #region Properties

    private Material Material
    {
        get
        {
            if (curMaterial != null) return curMaterial;
            curMaterial = new Material(curShader) { hideFlags = HideFlags.HideAndDontSave };
            return curMaterial;
        }
    }
    #endregion

    private void Awake()
    {
        ScanSize = CrtScanLinesSizes.S1024;
    }

    private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (curShader != null)
        {
            Material.SetFloat(Distortion, distortion);
            Material.SetFloat(Gamma, gamma);
            Material.SetFloat(CurvatureSet1, curvatureSet1);
            Material.SetFloat(CurvatureSet2, curvatureSet2);
            Material.SetFloat(YExtra, yExtra);
            Material.SetFloat(Rgb1R, rgb1.r);
            Material.SetFloat(Rgb1G, rgb1.g);
            Material.SetFloat(Rgb1B, rgb1.b);
            Material.SetFloat(Rgb2R, rgb2.r);
            Material.SetFloat(Rgb2G, rgb2.g);
            Material.SetFloat(Rgb2B, rgb2.b);
            Material.SetFloat(DotWeight, dotWeight);
            Material.SetVector(TextureSize, scanSizeVector);
            Graphics.Blit(sourceTexture, destTexture, Material);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    private void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }


}