using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 미니맵 UI를 화면에 띄워주는 걸 관리하는 매니져
/// </summary>
public class MiniMapManager : Singleton<MiniMapManager>//싱글톤으로 제작하긴 했는데... 막 안해도 될 것 같기도 하고..?
{
    [SerializeField] Image mapImage;
    Material mapMaterial;

    protected override void Awake()
    {
        base.Awake();

        mapMaterial = mapImage.material;
    }

    /// <summary>
    /// 미니맵 지도 이미지 적용 함수
    /// </summary>
    /// <param name="texture">지도 이미지 텍스쳐</param>
    public void SetMapImage(Texture texture)
    {
        mapMaterial.SetTexture("_MapTex", texture);
    }
}
