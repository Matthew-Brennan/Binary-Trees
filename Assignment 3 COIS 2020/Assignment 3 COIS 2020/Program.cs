/*
 * COIS 2020 Assignment 3
 * Virya Shields - 0598290
 * Adrian Lim Zheng Ting - 0707216
 * Matthew Brennan - 0713137
 * 2021-12-08
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BinaryTree
{
    public class Node<T>
    {
        public T Item { get; set; }     // Item stored in the Node
        public int Order { get; set; }     // Used to record the order that nodes are visited
        public Node<T> Left { get; set; }     // Left subtree
        public Node<T> Right { get; set; }     // Right subtree


        // Constructor Node

        public Node(T item, int order, Node<T> L, Node<T> R)
        {
            Item = item;
            Order = order;
            Left = L;
            Right = R;
        }
    }

    //----------------------------------------------------------

    public class BinaryTree<T>
    {
        private Node<T> root;        // Reference to the root of the binary tree
        private int n;               // Number of nodes in the binary tree

        private Random r;            // For building the random binary tree

        // BinaryTree
        // Builds a random binary tree on n nodes

        public BinaryTree(int n)
        {
            r = new Random();        // Creates a random variable r

            this.n = n;
            root = RandomBuild(n);
        }

        // RandomBuild (Preorder)
        // Creates and returns a randomly-built binary tree on n nodes
        // Devroye and Kruszewski, Botanical beauty of random binary trees, Springer, 1996

        public Node<T> RandomBuild(int n)
        {
            int left;

            if (n == 0)
                return null;
            else
            {
                left = (int)(n * r.NextDouble());
                return new Node<T>(default(T), 0, RandomBuild(left), RandomBuild(n - left - 1));
            }
        }

        //------------------------------------------------------

        // Preorder
        // Traverses the binary tree in preorder

        public void Preorder()
        {
            int i = 0;
            Preorder(root, ref i);
        }

        // Private Preorder
        // Recursively implements the preorder traversal

        private void Preorder(Node<T> root, ref int i)
        {
            if (root != null)
            {
                root.Order = ++i;
                Preorder(root.Left, ref i);
                Preorder(root.Right, ref i);
            }
        }

        //------------------------------------------------------

        // Public Inorder
        // Traverses the binary tree inorder

        public void Inorder()
        {
            int i = 0;
            Inorder(root, ref i);
        }

        // Private Inorder
        // Recursively implements the inorder traversal

        private void Inorder(Node<T> root, ref int i)
        {
            if (root != null)
            {
                Inorder(root.Left, ref i);
                root.Order = ++i;
                Inorder(root.Right, ref i);
            }
        }

        //------------------------------------------------------

        // Public Postorder
        // Traverses the binary tree in postorder

        public void Postorder()
        {
            int i = 0;
            Postorder(root, ref i);
        }

        // Private Postorder
        // Recursively implements the postorder traversal

        private void Postorder(Node<T> root, ref int i)
        {
            if (root != null)
            {
                Postorder(root.Left, ref i);
                Postorder(root.Right, ref i);
                root.Order = ++i;
            }
        }

        //------------------------------------------------------

        // Size (Postorder)
        // Returns the number of nodes in a binary tree

        public int Size()
        {
            return Size(root);
        }

        // Size
        // Recursively implements public Size

        private int Size(Node<T> root)
        {
            if (root == null)
                return 0;
            else
                return Size(root.Left) + Size(root.Right) + 1;
        }

        //------------------------------------------------------

        // Public Print (Inorder)
        // Outputs the binary tree in a 2-D format without edges and rotated 90 degrees

        public void Print()
        {
            Print(root, 0);
        }

        // Private Print
        // Recursively implements the public Print

        private void Print(Node<T> root, int indent)
        {
            if (root != null)
            {
                Print(root.Right, indent + 3);
                Console.WriteLine(new String(' ', indent) + root.Order);
                Print(root.Left, indent + 3);
            }
        }

        //Public Height
        //Outputs the max height of the tree
        public int Height()
        {
            return Height(root);
        }

        //Private Height
        //Outputs the max height of the tree
        private int Height(Node<T> root)
        {
            if (root == null)
            {
                return -1;
            }
            else
            {
                int leftHeight = Height(root.Left);
                int rightHeight = Height(root.Right);

                return Math.Max(leftHeight, rightHeight) + 1;
            }
        }

        //public isAVL
        //returns true if the tree is balanced, and false if it is not
        public bool isAVL()
        {
            return isAVL(root);
        }

        //private isAVL
        //returns true if the tree is balanced, and false if it is not
        private bool isAVL(Node<T> root)
        {
            if (root == null)
            {
                return false;
            }

            else
            {
                int leftHeight = Height(root.Left);
                int rightHeight = Height(root.Right);
                return (leftHeight - rightHeight <= 1);
            }
        }
    }
}


namespace BinarySearchTree
{
    // Interfaces used for a Binary Search Tree

    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ISearchable<T> : IContainer<T>
    {
        //void Insert(T item);
        //not used for this assignment
        void Remove(T item);
        bool Contains(T item);
    }

    //-------------------------------------------------------------------------

    // Common generic node class for a binary search tree

    public class Node<T> where T : IComparable
    {
        // Read/write properties

        public T Item { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public int height { get; set; }  // Height of a node

        public Node(T item)
        {
            Item = item;
            Left = Right = null;
            height = 1;
        }

        public Node(T item, Node<T> left, Node<T> right)
        {
            Item = item;
            Left = left;
            Right = right;
        }
    }

    //-------------------------------------------------------------------------

    // Implementation:  Binary Search Tree (BST)

    class BinarySearchTree<T> : ISearchable<T> where T : IComparable
    {
        private Node<T> root;     // Reference to the root of a BST
        private List<T> SortedList;
        
        // Constructor
        // Initializes an empty BST
        // Time complexity:  O(1)

        public BinarySearchTree(List<T> L, int start, int end)
        {
            root = null;    // Empty BST
            SortedList = L;
            root = Build(L, start, end);
            
        }



        public void Remove(T item)
        {
            Node<T> curr, prev;
            bool deleted = false;

            prev = null;
            curr = root;

            while (curr != null && !deleted)
            {
                if (item.CompareTo(curr.Item) < 0)        // Move left
                {
                    prev = curr;
                    curr = curr.Left;
                }
                else
                    if (item.CompareTo(curr.Item) > 0)    // Move right
                {
                    prev = curr;
                    curr = curr.Right;
                }
                else
                {
                    // Item found
                    deleted = true;

                    // Cases 1 and 2: curr has no children or one child

                    if (curr.Left == null)
                    {
                        if (prev == null)               // Item found at root
                            root = curr.Right;
                        else
                            if (prev.Left == curr)      // Item not found at root
                            prev.Left = curr.Right;
                        else
                            prev.Right = curr.Right;
                    }
                    else
                        if (curr.Right == null)
                    {
                        if (prev == null)           // Item found at root
                            root = curr.Left;
                        else
                            if (prev.Left == curr)  // Item not found at root
                            prev.Left = curr.Left;
                        else
                            prev.Right = curr.Left;
                    }

                    // Case 3: curr has two children

                    else
                    {
                        prev = curr;              // curr remains at item to be deleted 
                                                    // prev will follow min
                        Node<T> min = curr.Right; // min will be node with the minimum
                                                    // value in the right subtree

                        // Find the leftmost node (min) in the right subtree of curr
                        while (min.Left != null)
                        {
                            prev = min;
                            min = min.Left;
                        }

                        // Assign the value of min to curr
                        curr.Item = min.Item;

                        // Remove min
                        if (prev.Left == min)
                            prev.Left = min.Right;
                        else
                            // min is directly to the right of prev (and curr)
                            prev.Right = min.Right;
                    }
                }
            }
        }

        // Contains (Iterative Version)
        // Returns true if the item is found in a BST; false otherwise
        // Time complexity:  O(n)

        public bool Contains(T item)
        {
            Node<T> curr = root;

            while (curr != null)
            {
                if (item.CompareTo(curr.Item) == 0)      // Found
                    return true;
                else
                    if (item.CompareTo(curr.Item) < 0)
                    curr = curr.Left;                // Move left
                else
                    curr = curr.Right;               // Move right
            }
            return false;                                // Not found
        }

        // Public Contains (Recursive Version)
        // Returns true if the item is found in a BST; false otherwise
        // Time complexity:  O(n)

        public bool RecursiveContains(T item)
        {
            return Contains(item, root);
        }

        // Private Contains
        // Time complexity:  O(n)

        private bool Contains(T item, Node<T> curr)
        {
            if (curr == null)
                return false;                               // Not found
            else
                if (item.CompareTo(curr.Item) == 0)         // Found
                return true;
            else
                    if (item.CompareTo(curr.Item) < 0)      // Move left
                return Contains(item, curr.Left);
            else
                return Contains(item, curr.Right);  // Move right
        }

        // MakeEmpty
        // Resets the BST to empty
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            root = null;
        }

        // Empty
        // Returns true if the BST is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return root == null;
        }

        // Public Size
        // Returns the number of items in a BST
        // Time complexity:  O(n)

        public int Size()
        {
            return Size(root);          // Calls the private, recursive Size
        }

        // Size
        // Calculates the size using a postorder traversal
        // Time complexity:  O(n)

        private int Size(Node<T> node)
        {
            if (node == null)
                return 0;
            else
                // Postorder processing
                return 1 + Size(node.Left) + Size(node.Right);
        }

        // Public Print
        // Outputs the items of a BST in sorted order
        // Time complexity:  O(n)

        //public void Print()
        //{
        //    Print(root);                // Calls private, recursive Print
        //    Console.WriteLine();
        //}

        //// Private Print
        //// Outputs items using an inorder traversal
        //// Time complexity:  O(n)

        //private void Print(Node<T> node)
        //{
        //    if (node != null)
        //    {
        //        Print(node.Left);
        //        Console.Write(node.Item.ToString() + " ");
        //        Print(node.Right);
        //    }
        //}

        // Public Print
        // Outputs the items of an AVL Tree in sorted order
        // Time complexity:  O(n)

        public void Print()
        {
            int indent = 0;

            Print(root, indent);                // Calls private, recursive Print
            Console.WriteLine();
        }

        // Private Print
        // Outputs items using an inorder traversal
        // Time complexity:  O(n)

        private void Print(Node<T> node, int indent)
        {
            if (node != null)
            {
                Print(node.Right, indent + 3);
                Console.WriteLine(new String(' ', indent) + node.Item.ToString() + " " + node.height);
                Print(node.Left, indent + 3);
            }
        }

        //public Build
        //Builds a Binary Search Tree from a List
        /*
        public Build()
        {
            Build(SortedList, 0, SortedList.Count());
        }
        */

//public Build
//Builds a Binary Search Tree from a List
//Will be called from the constructor
private Node<T> Build(List<T> L, int start, int end)
        {
            //termination condition for this recursive method
            //i.e. if there is no more items in the list to insert
            if (start > end)
            {
                return null;

            }
         
            //we insert the mid point first, so the tree would be balanced on both sides.
            int mid = Convert.ToInt32(Math.Round((start + end) / 2d,MidpointRounding.AwayFromZero));
            Node<T> node = new Node<T>(L[mid]);
            node.Left = Build(L, start, mid - 1);
            
            node.Right = Build(L, mid + 1, end);
            node.height = Height(node) + 1;
            return node;

            

        }

        // Public Inorder
        // Traverses the binary tree inorder

       


        //Public Height
        //Outputs the max height of the tree
        //We are using the Height and isAVL methods to check if this Binary Search Tree is balanced
        public int Height()
        {
            return Height(root);
        }

        //Private Height
        //Recursively checks the height of each subtree
        private int Height(Node<T> root)
        {
            if (root == null)
            {
                return -1;
            }
            else
            {
                int leftHeight = Height(root.Left);
                int rightHeight = Height(root.Right);

                return Math.Max(leftHeight, rightHeight) + 1;
            }
        }
        //public isAVL
        //returns true if the tree is balanced, and false if it is not
        public bool isAVL()
        {
            return isAVL(root);
        }

        //private isAVL
        //recursively checks the heights of each subtree
        private bool isAVL(Node<T> root)
        {
            if (root == null)
            {
                return false;
            }

            else
            {
                int leftHeight = Height(root.Left);
                int rightHeight = Height(root.Right);
                return (leftHeight - rightHeight <= 1);
            }
        }
}

    //-----------------------------------------------------------------------------


}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Part A:");
        for (int f = 100; f <= 1000; f += 100) //for the number of nodes
        {
            int TotalHeight = 0; //counter to calculate average height
            int BalancedTrees = 0; //counter to show that isAVL() works
            for (int i = 1; i <= 1000; i++) //1000 trials
            {
                //creates a new binary tree
                BinaryTree.BinaryTree<int> BT = new BinaryTree.BinaryTree<int>(f);
                TotalHeight += BT.Height(); //will be divided by 1000 later
                if (BT.isAVL())
                {
                    BalancedTrees++; //adds to the balanced trees counter
                }
            }
            Console.WriteLine("Average Height of Binary Trees with {0} nodes is {1}", f, TotalHeight / 1000);
            Console.WriteLine("Log2(N) = {0}", Math.Log2(f));
            Console.WriteLine("There are {0} balanced trees with {1} nodes", BalancedTrees, f);
            Console.WriteLine("-----------------------------------------------------------------------------------------");
        }

        //make 5 Binary Trees and show if they Meet AVL requirements
        Console.WriteLine("\nPart B:");
        for (int i = 1; i <= 5; i++)
        {
            BinaryTree.BinaryTree<int> BT1 = new BinaryTree.BinaryTree<int>(10);
            Console.WriteLine("Binary Search Tree {0}: is AVL?: {1}", i, BT1.isAVL());
            BT1.Print();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
        }


        Console.WriteLine("\nPart C:");
        List<int> L = new List<int>();  //initialize new list
        Random rnd = new Random();  //random object to insert random integers into the list
        for (int k = 0; k <= 30; k++)  //insert 30 random integers
        {
            L.Add(rnd.Next(1, 100));
        }

        L.Sort();  //sort the list
        List<int> unique = L.Distinct().ToList();  //remove duplicate items
        BinarySearchTree.BinarySearchTree<int> BST = new BinarySearchTree.BinarySearchTree<int>(unique, 0, unique.Count() - 1); //-1 as lists start from position 0
        BST.Print(); //print the tree to see
        Console.WriteLine("The Binary Search Tree is {0}balanced", BST.isAVL() ? "" : "not ");  //if isAVL() returns false, the word not is added
    }
}





// Insert (Iterative Version)
// Inserts an item into a BST 
// Duplicate items are not inserted
// Worst case time complexity:  O(n)

/* For this assignment this part will not be used
public void Insert(T item)
{
    Node<T> curr;
    bool inserted = false;

    if (root == null)
        root = new Node<T>(item);                // Add a node to an empty BST
    else
    {
        curr = root;
        while (!inserted)
        {
            if (item.CompareTo(curr.Item) < 0)
            {
                if (curr.Left == null)           // Empty spot
                {
                    curr.Left = new Node<T>(item);
                    inserted = true;
                }
                else
                    curr = curr.Left;            // Move left
            }
            else
                if (item.CompareTo(curr.Item) > 0)
            {
                if (curr.Right == null)      // Empty spot
                {
                    curr.Right = new Node<T>(item);
                    inserted = true;
                }
                else
                    curr = curr.Right;       // Move right
            }
            else
                inserted = true;             // Already inserted
        }
    }
}

// Public Insert (Recursive Version)
// Inserts an item into a BST
// Duplicate items are not inserted
// Worst case time complexity:  O(n)

public void RecursiveInsert(T item)
{
    root = Insert(item, root);
}

// Private Insert
// Time complexity:  O(n)

private Node<T> Insert(T item, Node<T> curr)
{
    if (curr == null)                            // Empty spot
        return new Node<T>(item);
    else
        if (item.CompareTo(curr.Item) < 0)       // Move left
        curr.Left = Insert(item, curr.Left);
    else
            if (item.CompareTo(curr.Item) > 0)   // Move right
        curr.Right = Insert(item, curr.Right);
    // else already inserted

    return curr;
}

// Remove (Iterative Version)
// Remove an item from a BST
// Nothing is performed if the item is not found
// Time complexity:  O(n)


For this assignment, Insert is not to be used*/