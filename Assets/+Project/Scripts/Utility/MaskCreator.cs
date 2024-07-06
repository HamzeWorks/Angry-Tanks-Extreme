using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MaskCreator : MonoBehaviour
{
    [SerializeField] SpriteMask mask;
    [SerializeField] SpriteShape spriteShape;
    [SerializeField] SpriteShapeRenderer spriteShapeRenderer;
    [SerializeField] SpriteShapeController controller;
    [SerializeField] Sprite sprite;
    [SerializeField] Texture texture;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
      //var t =  spriteShapeRenderer.material.GetTexture("_Main") as Texture2D;
        var list = new  List<Material>();
       spriteShapeRenderer.GetMaterials(list);
        texture =spriteShapeRenderer.material.mainTexture;
        print(texture.width);
        sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, 1000, 1000), Vector3.zero);
        mask.sprite = sprite;
    }


}
