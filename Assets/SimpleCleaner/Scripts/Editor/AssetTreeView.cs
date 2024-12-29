using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;

namespace SimpleCleaner.Editor
{
    /// <summary>
    /// Class to reveal assets in tree view with toggle box
    /// </summary>
    class AssetTreeView : TreeView
    {
        private List<string> assets = new List<string>();
        private HashSet<int> toggledItemIds = new HashSet<int>();

        public AssetTreeView(TreeViewState state) : base(state) { }

        public void SetAssets(List<string> assets)
        {
            this.assets = assets;
            Reload();
            CheckAllItems(rootItem, true);
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            int id = 1;

            // Setup parent-child structure
            //   for Tree view structure
            foreach (var asset in assets)
            {
                var pathParts = asset.Split('/');
                TreeViewItem currentParent = root;

                for (int i = 0; i < pathParts.Length; i++)
                {
                    string pathPart = pathParts[i];
                    TreeViewItem existingChild = currentParent.children?.Find(child => child.displayName == pathPart);

                    if (existingChild == null)
                    {
                        var newChild = new TreeViewItem { id = id++, displayName = pathPart, parent = currentParent };
                        if (currentParent.children == null)
                            currentParent.children = new List<TreeViewItem>();
                        currentParent.children.Add(newChild);
                        currentParent = newChild;
                    }
                    else
                    {
                        currentParent = existingChild;
                    }
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect toggleButtonRect = new Rect(args.rowRect.x, args.rowRect.y, GetContentIndent(args.item), args.rowRect.height);
            base.RowGUI(new RowGUIArgs
            {
                rowRect = toggleButtonRect,
                item = args.item,
                label = "",
                selected = args.selected,
                focused = args.focused
            });

            Rect checkBoxRect = new Rect(toggleButtonRect.xMax + 2, args.rowRect.y, 20, args.rowRect.height);
            bool isToggled = toggledItemIds.Contains(args.item.id);
            bool newToggled = EditorGUI.Toggle(checkBoxRect, isToggled);

            if (newToggled != isToggled)
            {
                if (newToggled)
                {
                    CheckAllItems(args.item, true);
                    UpdateParentItems(args.item, true);
                }
                else
                {
                    CheckAllItems(args.item, false);
                    UpdateParentItems(args.item, false);
                }
            }

            Rect nameRect = new Rect(checkBoxRect.xMax + 5, args.rowRect.y, args.rowRect.width - checkBoxRect.xMax - 5, args.rowRect.height);
            EditorGUI.LabelField(nameRect, args.item.displayName);
        }

        private void CheckAllItems(TreeViewItem item, bool isChecked)
        {
            if (isChecked)
                toggledItemIds.Add(item.id);
            else
                toggledItemIds.Remove(item.id);

            if (item.children != null)
            {
                foreach (var child in item.children)
                {
                    CheckAllItems(child, isChecked);
                }
            }
        }

        /// <summary>
        /// Update parents' state using recursion
        /// </summary>
        private void UpdateParentItems(TreeViewItem item, bool isChecked)
        {
            if (item.parent == null)
                return;

            if (isChecked)
            {
                bool allChildrenChecked = item.parent.children.TrueForAll(child => toggledItemIds.Contains(child.id));
                if (allChildrenChecked)
                    toggledItemIds.Add(item.parent.id);
            }
            else
            {
                toggledItemIds.Remove(item.parent.id);
            }

            UpdateParentItems(item.parent, isChecked);
        }

        /// <summary>
        /// Return selected assets (to remove unused assets)
        /// </summary>
        public List<string> GetSelectedAssets()
        {
            var selected = new List<string>();

            foreach (var id in toggledItemIds)
            {
                var item = FindItem(id, rootItem);
                if (item != null && item.hasChildren == false)
                {
                    selected.Add(FindAssetPath(item));
                }
            }

            return selected;
        }

        private string FindAssetPath(TreeViewItem item)
        {
            List<string> pathParts = new List<string>();
            TreeViewItem currentItem = item;

            while (currentItem != null && currentItem.parent != null)
            {
                pathParts.Insert(0, currentItem.displayName);
                currentItem = currentItem.parent;
            }

            return string.Join("/", pathParts);
        }
    }
}
