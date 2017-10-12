using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour {
    public float size = 5;
    public int depth = 2;

    public Transform[] points;


	// Use this for initialization
	void Start () {
        points = new Transform[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        var quadtree = new Quadtree<bool>(this.transform.position, size, depth);
        foreach(var point in points)
        {
            quadtree.Insert(point.position, true);
        }


        DrawNode(quadtree.GetRoot());
    }

    private Color minColor = new Color(1, 1, 1, 1f);
    private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

    private void DrawNode(Quadtree<bool>.QuadtreeNode<bool> node, int nodeDepth = 0)
    {
        if (!node.IsLeaf())
        {
            foreach(var subnode in node.Nodes)
            {
                if(subnode != null)
                {
                    DrawNode(subnode, nodeDepth + 1);
                }
            }
        }

        Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        Gizmos.DrawWireCube(node.Position, new Vector2(1, 1) * node.Size);
    }
}
