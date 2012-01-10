using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSFramework.Utils
{
    /// <summary>
    /// Bag Class based on Artemis' Bag data structure. This is a type of container
    /// </summary>
    /// <typeparam name="E">the class the bag contains</typeparam>
    public class Bag<E> where E : class
    {
        private E[] data;
        private int size = 0;

        
        /// <summary>
        /// Constructs an empty Bag with an initial capacity of 64.
        /// </summary>
        public Bag()
        {
            data = new E[16];
        }

       
        /// <summary>
        /// Constructs an empty Bag with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">the initial capacity of Bag</param>
        public Bag(int capacity)
        {
            data = new E[capacity];
        }

        /// <summary>
        /// Removes the element at the specified position in this Bag. does this by
        /// overwriting it was last element then removing last element
        /// </summary>
        /// <param name="index">the index of element to be removed</param>
        /// <returns>element that was removed from the Bag</returns>
        public E Remove(int index)
        {
            E o = data[index]; // make copy of element to remove so it can be
            // returned
            data[index] = data[--size]; // overwrite item to remove with last
            // element
            data[size] = null; // null last element, so gc can do its work
            return o;
        }

        /// <summary>
        /// Remove and return the last object in the bag.
        /// </summary>
        /// <returns>the last object in the bag, null if empty.</returns>
        public E RemoveLast()
        {
            if (size > 0)
            {
                E o = data[--size];
                data[size] = null;
                return o;
            }
            
            return default(E);
        }

        /// <summary>
        /// Removes the first occurrence of the specified element from this Bag, if
        /// it is present. If the Bag does not contain the element, it is unchanged.
        /// does this by overwriting it was last element then removing last element
        /// </summary>
        /// <param name="o">element to be removed from this list, if present</param>
        /// <returns>true if this list contained the specified element</returns>
        public bool Remove(E o)
        {
            for (int i = 0; i < size; i++)
            {
                Object o1 = data[i];

                if (o.Equals(o1))
                {
                    data[i] = data[--size]; // overwrite item to remove with last
                    // element
                    data[size] = null; // null last element, so gc can do its work
                    return true;
                }
            }

            return false;
        }
                
        /// <summary>
        /// Check if bag contains this element
        /// </summary>
        /// <param name="o">the element to check</param>
        /// <returns>true if it contains the element</returns>
        public bool Contains(E o)
        {
            for (int i = 0; size > i; i++)
            {
                if (o.Equals(data[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  Removes from this Bag all of its elements that are contained in the
        ///  specified Bag.
        /// </summary>
        /// <param name="bag">Bag containint elements to be removed from this Bag</param>
        /// <returns>true if the bag changed as a result of the call</returns>
        public bool RemoveAll(Bag<E> bag)
        {
            bool modified = false;

            for (int i = 0, bagSize = bag.Size(); i < bagSize; i++)
            {
                Object o1 = bag.Get(i);

                for (int j = 0; j < size; j++)
                {
                    Object o2 = data[j];

                    if (o1 == o2)
                    {
                        Remove(j);
                        j--;
                        modified = true;
                        break;
                    }
                }
            }

            return modified;
        }

        /// <summary>
        /// Returns the element at the specified positon in the bag
        /// </summary>
        /// <param name="index">index of the element to return</param>
        /// <returns>the element at the specified position in the bag</returns>
        public E Get(int index)
        {
            return data[index];
        }

        /// <summary>
        /// returns the number of elements in the bag
        /// </summary>
        /// <returns>number of elements in the bag</returns>
        public int Size()
        {
            return size;
        }

        /// <summary>
        /// returns the number of elements the bag can hold without having to grow
        /// </summary>
        /// <returns>the number of element slots in the bag</returns>
        public int GetCapacity()
        {
            return data.Length;
        }

        /// <summary>
        /// is the bag empty?
        /// </summary>
        /// <returns>true if bag contains no elements</returns>
        public bool IsEmpty()
        {
            return size == 0;
        }

        /// <summary>
        /// adds the specified element to the end of the bag. if needed also increases the capacity of the bag
        /// </summary>
        /// <param name="o">element to be added to this bag</param>
        public void Add(E o)
        {
            // is size greater than capacity increase capacity
            if (size == data.Length)
            {
                Grow();
            }

            data[size++] = o;
        }

        /// <summary>
        /// set element at the specified index in the bag
        /// </summary>
        /// <param name="index">index to be set</param>
        /// <param name="o">element to be set</param>
        public void Set(int index, E o)
        {
            if (index >= data.Length)
            {
                Grow(index * 2);
                size = index + 1;
            }
            else if (index >= size)
            {
                size = index + 1;
            }
            data[index] = o;
        }

        /// <summary>
        /// auto grows the bag
        /// </summary>
        private void Grow()
        {
            int newCapacity = (data.Length * 3) / 2 + 1;
            Grow(newCapacity);
        }

        /// <summary>
        /// grows the bag to the specified capacity
        /// </summary>
        /// <param name="newCapacity">new capacity</param>
        private void Grow(int newCapacity)
        {
            E[] oldData = data;
            data = new E[newCapacity];
            Array.Copy(oldData, 0, data, 0, oldData.Length);
        }

        /// <summary>
        /// remove all the elements from the bag. bag is empty after return.
        /// </summary>
        public void Clear()
        {
            // null all elements so gc can clean up
            for (int i = 0; i < size; i++)
            {
                data[i] = null;
            }

            size = 0;
        }

        /// <summary>
        /// adds all of another bag's items to this bag
        /// </summary>
        /// <param name="items">the other bag</param>
        public void AddAll(Bag<E> items)
        {
            for (int i = 0, j = items.Size(); j > i; i++)
            {
                Add(items.Get(i));
            }
        }

    }
}
