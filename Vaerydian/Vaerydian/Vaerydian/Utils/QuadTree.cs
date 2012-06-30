using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public class QuadTree<E> where E:class
    {

        private QuadNode<E> q_RootNode;

        public QuadTree(Vector2 ul, Vector2 lr)
        {
            q_RootNode = new QuadNode<E>(ul, lr);
            q_RootNode.Parent = null;
        }

        public void buildQuadTree(int tiers)
        {
            q_RootNode.subdivide();
            growTree(q_RootNode, tiers);
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
            return retrieveContentsAtLocation(q_RootNode,location);
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

        /// <summary>
        /// retrieves all contents at the location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<E> retrieveAllContents(Vector2 location)
        {
            List<E> contents = new List<E>();
            contents = retrieveAllContents(q_RootNode, location, contents);
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
        /// <returns>node it was set to if successful, otherwise null</returns>
        public QuadNode<E> setContentAtLocation(E content, Vector2 location)
        {
            return setContentAtLocation(q_RootNode,content,location);
        }

        private QuadNode<E> setContentAtLocation(QuadNode<E> node, E val, Vector2 point)
        {
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                if (node.contains(point))
                {
                    if(!node.Contents.Contains(val))
                        node.Contents.Add(val);
                    return node;
                }
                else
                {
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// locate the leaf node at the given point
        /// </summary>
        /// <param name="point">point to check</param>
        /// <returns>leaf node containing point</returns>
        public QuadNode<E> locateNode(Vector2 point)
        {
            return locateNode(q_RootNode, point);
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

        /// <summary>
        /// remove
        /// </summary>
        /// <param name="node"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool removeValAtNode(QuadNode<E> node, E val)
        {
            return removeVal(q_RootNode, node, val); ;
        }

        private bool removeVal(QuadNode<E> node, QuadNode<E> target, E val)
        {
            if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
            {
                if (node == target)
                    return node.Contents.Remove(val);
                else
                    return false;
            }
            else
            {
                if (node.Q1.contains(target.ULCorner))
                    return removeVal(node.Q1, target, val);
                if (node.Q2.contains(target.ULCorner))
                    return removeVal(node.Q2, target, val);
                if (node.Q3.contains(target.ULCorner))
                    return removeVal(node.Q3, target, val);
                if (node.Q4.contains(target.ULCorner))
                    return removeVal(node.Q4, target, val);
                return false;
            }
        }


        private void remove(QuadNode<E> node, E val)
        {
            node.Contents.Remove(val);
        }

        //TODO: add range/distance query

    }
}
