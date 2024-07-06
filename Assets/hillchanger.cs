using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class hillchanger : MonoBehaviour
{


    SpriteShapeController sprtshpchnge;
    public SpriteShape[] spriteshapes;



    private void Awake()
    {
        sprtshpchnge = GetComponent<SpriteShapeController>();

    }

    bool changesMade = false;
  
    public float tangentLength = 1.0f;
    public SpriteShapeController spriteShapeController;

    [HideInInspector]
    public List<GameObject> allMembraneSegments;

    void Start()

    {

        var spriteShapeController = GetComponent<SpriteShapeController>();

        var spline = spriteShapeController.spline;

        for (int i = 0; i < spline.GetPointCount(); ++i)

        {

            Vector3 pos = spline.GetPosition(i);

            pos.x = pos.x + (Random.Range(-0.1f, 0.1f));

            pos.y = pos.y + (Random.Range(-8.3f, 1.6f));

            spline.SetPosition(i, pos);

        }
        spriteShapeController.spriteShape = spriteshapes[Random.Range(0, spriteshapes.Length)];
    /*    if(spriteShapeController.spriteShape == spriteshapes[0])
        {
            spriteShapeController.colliderOffset = -0.3f;
        }else if (spriteShapeController.spriteShape == spriteshapes[1])
        {
            spriteShapeController.colliderOffset = 0.13f;

        }else if(spriteShapeController.spriteShape == spriteshapes[2])
        {
            spriteShapeController.colliderOffset = -0.13f;

        }*/
    }

    public void SetSpline()
    {
        Spline spline = spriteShapeController.spline;
     //   spline.Clear();

        for (int i = 0; i < allMembraneSegments.Count; i++)
        {
            int j = allMembraneSegments.Count - i - 1;
            GameObject membraneSegment = allMembraneSegments[j];
            Vector3 position = membraneSegment.transform.position;
            Quaternion rotation = membraneSegment.transform.rotation;

            spline.InsertPointAt(i, position);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spline.SetRightTangent(i, rotation * Vector3.down * tangentLength);
            spline.SetLeftTangent(i, rotation * Vector3.up * tangentLength);
            print("" + j);
        }

        spriteShapeController.RefreshSpriteShape();
    }
   
}
