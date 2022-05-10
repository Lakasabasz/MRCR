using System.Text.Json;

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
        try
        {
            EditorWindow editor = new EditorWindow(worldPath);
            editor.ShowDialog();
        }
        catch (JsonException) {}
    }

    public void DisplayGameWindow(string worldPath)
    {
        throw new System.NotImplementedException();
    }
}