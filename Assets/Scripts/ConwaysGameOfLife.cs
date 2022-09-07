using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConwaysGameOfLife : MonoBehaviour
{
    public int width = 512;
    public int height = 512;

    public ComputeShader compute;

    [HideInInspector]
    public RenderTexture renderTexPing;
    [HideInInspector]
    public RenderTexture renderTexPong;

    public Material mat;

    private int kernel;
    private bool pingPong;

    // Use this for initialization
    void Start()
    {
        if (height < 1 || width < 1) return;

        kernel = compute.FindKernel("GameOfLife");

        renderTexPing = new RenderTexture(width, height, 24);
        renderTexPing.wrapMode = TextureWrapMode.Repeat;
        renderTexPing.enableRandomWrite = true;
        renderTexPing.filterMode = FilterMode.Point;
        renderTexPing.useMipMap = false;
        renderTexPing.Create();

        renderTexPong = new RenderTexture(width, height, 0);
        renderTexPong.wrapMode = TextureWrapMode.Repeat;
        renderTexPong.enableRandomWrite = true;
        renderTexPong.filterMode = FilterMode.Point;
        renderTexPong.useMipMap = false;
        renderTexPong.Create();

        Texture2D input = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                if(Random.Range(0,10) == 0)
                    input.SetPixel(x, y, new Color(1f, 1f, 1f));
                else
                    input.SetPixel(x, y, new Color(0f, 0f, 0f));
            }
        }

        input.Apply();

        Graphics.Blit(input, renderTexPing);

        pingPong = true;

        compute.SetFloat("Width", width);
        compute.SetFloat("Height", height);
    }

    // Update is called once per frame
    void Update()
    {
        if (height < 1 || width < 1) return;

        compute.SetTexture(kernel, "Input", pingPong ? renderTexPing : renderTexPong);
        compute.SetTexture(kernel, "Result", pingPong ? renderTexPong : renderTexPing);
        compute.Dispatch(kernel, width / 8, height / 8, 1);

        mat.mainTexture = pingPong ? renderTexPong : renderTexPing;
        pingPong = !pingPong;
    }
}