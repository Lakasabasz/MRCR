namespace MRCR.services;

public interface IFactoryWindow
{
    void DisplayEditorWindow(string worldPath);
    void DisplayGameWindow(string worldPath);
}

internal class FactoryWindow : IFactoryWindow
{
    public void DisplayEditorWindow(string worldPath)
    {
        Editor editor = new Editor(worldPath);
        editor.ShowDialog();
    }

    public void DisplayGameWindow(string worldPath)
    {
        throw new System.NotImplementedException();
    }
}