using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DrawingModule.Control
{
    public class TreeNode : FrameworkElement
    {
        public TreeNode(TreeNode parent)
        {
            Items = new TreeView().Items;
            ParentNode = parent;
        }

        public TreeNode(TreeNode parent, string text) : this(parent)
        {
            Text = text;
        }

        public string Text { get; set; }

        public TreeNode ParentNode { get; set; }

        public ItemCollection Items { get; set; }

        public int GetLevel()
        {
            if (ParentNode != null)
            {
                return ParentNode.GetLevel() + 1;
            }
            return 0;
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(TreeNode), new PropertyMetadata(default(bool)));

        public TreeNode GetChildNode(string name)
        {
            foreach (TreeNode node in Items)
            {
                if (node.Text.Equals(name))
                    return node;
            }

            return null;
        }

        public bool ContainsChildNode(string name)
        {
            foreach (TreeNode node in Items)
            {
                if (node.Text.Equals(name))
                    return true;
            }

            return false;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(TreeNode), new PropertyMetadata(default(bool)));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public override string ToString()
        {
            return Text;
        }

        public void Remove()
        {
            while (Items.Count > 0)
            {
                ((TreeNode)Items[0]).Remove();
            }

            if (ParentNode != null)
                ParentNode.Items.Remove(this);
        }
    }
}
