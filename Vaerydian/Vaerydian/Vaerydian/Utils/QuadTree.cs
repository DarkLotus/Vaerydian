using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public class QuadTree<E> where E:class
    {

        private QuadNode<E> rootNode;

        public QuadTree(Vector2 ul, Vector2 lr)
        {
            rootNode = new QuadNode<E>(ul, lr);
            rootNode.Parent = null;
        }

        public void buildQuadTree(int tiers)
        {
            rootNode.subdivide();
            growTree(rootNode, tiers);
        }

        /// <summary>
        /// grows a tree of n-tiers from current node
        /// </summary>
        /// <param name="node">node to grow from</param>
        /// <param name="tiers">number of tiers to grow</param>
        private void growTree(QuadNode<E> node, int tiers)
        {
            if (tiers == 0)
            {
                return;
            }
            else
            {
                node.Q1.subdivide();
                growTree(node.Q1, tiers - 1);
                node.Q2.subdivide();
                growTree(node.Q2, tiers - 1);
                node.Q3.subdivide();
                growTree(node.Q3, tiers - 1);
                node.Q4.subdivide();
                growTree(node.Q4, tiers - 1);
            }
        }

        /// <summary>
        /// retrieves the contents at a given leaf node containing the given point
        /// </summary>
        /// <param name="location">point the leaf node contains</param>
        /// <returns>successful or not</returns>
        public List<E> retrieveContentsAtLocation(Vector2 location)
        {
            return retrieveContentsAtLocation(rootNode,location);
        }

        private List<E> retrieveContentsAtLocation(QuadNode<E> node, Vector2 location)
        {
            
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                return node.Contents;
            }
            else
            {
                //List<E> temp = new List<E>();
                if (node.Q1.contains(location))
                    return retrieveContentsAtLocation(node.Q1, location);
                if (node.Q2.contains(location))
                    return retrieveContentsAtLocation(node.Q2, location);
                if (node.Q3.contains(location))
                    return retrieveContentsAtLocation(node.Q3, location);
                if (node.Q4.contains(location))
                    return retrieveContentsAtLocation(node.Q4, location);

                return null;
            }
        }

        public List<E> retrieveAllContents(Vector2 location)
        {
            List<E> contents = new List<E>();
            contents = retrieveAllContents(rootNode, location, contents);
            return contents;
        }

        private List<E> retrieveAllContents(QuadNode<E> node, Vector2 location, List<E> contents)
        {
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                return node.Contents;
            }
            else
            {
                contents.AddRange(retrieveAllContents(node.Q1, location, contents));
                contents.AddRange(retrieveAllContents(node.Q2, location, contents));
                contents.AddRange(retrieveAllContents(node.Q3, location, contents));
                contents.AddRange(retrieveAllContents(node.Q4, location, contents));
                return contents;
            }
        }

        /// <summary>
        /// sets a piece of content to the leaf node containing the given point
        /// </summary>
        /// <param name="content">content to place</param>
        /// <param name="location">location contained by the leaf node</param>
        /// <returns>true if success</returns>
        public bool setContentAtLocation(E content, Vector2 location)
        {
            return setContentAtLocation(rootNode,content,location);
        }

        private bool setContentAtLocation(QuadNode<E> node, E val, Vector2 point)
        {
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                if (node.contains(point))
                {
                    node.Contents.Add(val);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                if (node.Q1.contains(point))
                    return setContentAtLocation(node.Q1, val, point);
                if (node.Q2.contains(point))
                    return setContentAtLocation(node.Q2, val, point);
                if (node.Q3.contains(point))
                    return setContentAtLocation(node.Q3, val, point);
                if (node.Q4.contains(point))
                    return setContentAtLocation(node.Q4, val, point);
                return false;
            }
        }

        /// <summary>
        /// locate the leaf node at the given point
        /// </summary>
        /// <param name="point">point to check</param>
        /// <returns>leaf node containing point</returns>
        public QuadNode<E> locateNode(Vector2 point)
        {
            return locateNode(rootNode, point);
        }
         
        private QuadNode<E> locateNode(QuadNode<E> node, Vector2 point)
        {
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                return node;
            }
            else
            {
                if (node.Q1.contains(point))
                    return locateNode(node.Q1, point);
                if (node.Q2.contains(point))
                    return locateNode(node.Q2, point);
                if (node.Q3.contains(point))
                    return locateNode(node.Q3, point);
                if (node.Q4.contains(point))
                    return locateNode(node.Q4, point);
                return null;
            }
        }

        

    }
}
