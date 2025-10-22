using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scratch : MonoBehaviour
{
    public Image SpriteMask;
    public RectTransform CanvasRectTransform;

    public float BrushSize;
    [Range(0, 1)] public float BrushPorosity;

    public float SpriteQualityLoss;
    [Range(0, 1)] public float SpritePorosity;

    private Texture2D _maskTexture;
    private Sprite _maskSprite;
    private Rect _maskRect;

    private int _textureHeight;
    private int _textureWidth;

    private int _pixelBrushSize;

    void Start()
    {
        _textureWidth =  Mathf.RoundToInt(Screen.width / SpriteQualityLoss);
        _textureHeight = Mathf.RoundToInt(Screen.height / SpriteQualityLoss);

        _maskRect = new Rect(0, 0, _textureWidth, _textureHeight);

        _maskTexture = CreateNewTexture();
        _maskSprite = Sprite.Create(_maskTexture, _maskRect, new Vector2(0.5f, 0.5f), _textureHeight);

        SpriteMask.sprite = _maskSprite;
    }

    private Texture2D CreateNewTexture()    // Create new texture2D
    {
        Texture2D texture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);

        Color32[] pixels = new Color32[_textureWidth * _textureHeight];   // Array for colors for each pixel

        for (int i = 0; i < pixels.Length; i++)
        {
            if (Random.value < (1f - SpritePorosity))   // Fill pixels with white
                pixels[i] = Color.white; 
        }

        // Set info to texture
        texture.SetPixels32(pixels);
        texture.Apply();

        return texture;
    }

    public void DrawCircle(Vector2 Position, int PixelRadius)
    {
        Position /= SpriteQualityLoss;

        Color32[] pixels = _maskTexture.GetPixels32();

        for (int posY = Mathf.Max(0, (int)(Position.y - PixelRadius));
                posY < Mathf.Min(_textureHeight, (int)(Position.y + PixelRadius));
                posY++)  // Start loop on Y
        {
            for (int posX = Mathf.Max(0, (int)(Position.x - PixelRadius));
            posX < Mathf.Min(_textureWidth, (int)(Position.x + PixelRadius));
            posX++) // Loop on X
            {
                if (Vector2.Distance(Position, new Vector2(posX, posY)) < PixelRadius)
                    if (Random.value < (1f - BrushPorosity))
                        pixels[posY * _textureWidth + posX] = Color.clear;
            }
        }

        _maskTexture.SetPixels32(pixels);

        _maskTexture.Apply();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            _pixelBrushSize = Mathf.RoundToInt(BrushSize / CanvasRectTransform.rect.height * _textureHeight);

            DrawCircle(Input.mousePosition, _pixelBrushSize);
        }
    }
}
