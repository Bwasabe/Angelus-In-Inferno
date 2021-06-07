using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    
    [SerializeField]
    private float speed = 0.5f;

    private MeshRenderer meshRenderer = null;
    private Vector2 offset = Vector2.zero;
    void Start()
    {
        offset.y = PlayerPrefs.GetFloat("OffsetY" , 0);
        if(meshRenderer == null)meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        offset.y+=speed * Time.deltaTime;
        meshRenderer.material.SetTextureOffset("_MainTex",offset);
    }
}
