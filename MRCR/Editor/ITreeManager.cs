using System.Windows.Controls;

namespace MRCR.Editor;

public interface ITreeManager
{
    void UpdateTree();
    void UpdateSelection(TreeViewItem old, TreeViewItem newer);
}