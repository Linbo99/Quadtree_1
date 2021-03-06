﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuadtreeIndex
{
    TofLeft = 0,     //00
    TopRight = 1,    //01
    BottomLeft = 2,  //10
    BottomRight = 3  //11
}


public class Quadtree<TType>
{
    private QuadtreeNode<TType> node;
    private int depth;

    public Quadtree(Vector2 position, float size, int depth)
    {
        node = new QuadtreeNode<TType>(position, size);
        this.depth = depth;
        //node.Subdivide(depth);
    }

    public void Insert(Vector2 position, TType value)
    {
        node.Subdivide(position, value, depth);
    }

    public class QuadtreeNode<TType>
    {
        Vector2 position;
        float size;
        QuadtreeNode<TType>[] subNodes;
        TType value;

        public QuadtreeNode(Vector2 pos, float size)
        {
            position = pos;
            this.size = size;
        }

        public IEnumerable<QuadtreeNode<TType>> Nodes
        {
            get { return subNodes; }
        }
        
        public Vector2 Position
        {
            get { return position; }
        }

        public float Size
        {
            get { return size; }
        }

        public void Subdivide(Vector2 targetPosition, TType type, int depth = 0)
        {
            var subdivIndex = GetIndexOfPosition(targetPosition, position);
            if (subNodes == null)
            {
                subNodes = new QuadtreeNode<TType>[4];
                for (int i = 0; i < subNodes.Length; i++)
                {
                    Vector2 newPos = position;
                    if ((i & 2) == 2)
                    {
                        newPos.y -= size * 0.25f;
                    }
                    else
                    {
                        newPos.y += size * 0.25f;
                    }

                    if ((i & 1) == 1)
                    {
                        newPos.x += size * 0.25f;
                    }
                    else
                    {
                        newPos.x -= size * 0.25f;
                    }

                    subNodes[i] = new QuadtreeNode<TType>(newPos, size * 0.5f);
                }
            }

            if (depth > 0)
            {
                Debug.Log("Subdivide the element");
                subNodes[subdivIndex].Subdivide(targetPosition, value, depth - 1);
            }
        }

        public bool IsLeaf()
        {
            return subNodes == null;
        }
    }


    private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y < nodePosition.y ? 2 : 0;
        index |= lookupPosition.x > nodePosition.x ? 1 : 0;

        return index;
    }

    public QuadtreeNode<TType> GetRoot()
    {
        return node;
    }
}
