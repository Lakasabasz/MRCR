using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using MRCR;
using MRCR.datastructures;
using NUnit.Framework;

namespace MRCR_tests;

public class EditorWindowTests
{
    private static int _lastFileID = 0;
    [ExcludeFromCodeCoverage]
    private string SetupWorldFile()
    {
        string filename = $"{Config.WorldDirectoryPath}WorldFile {_lastFileID++}{Config.WorldFileExtension}";
        if(!Directory.Exists(Config.WorldDirectoryPath)){
            Directory.CreateDirectory(Config.WorldDirectoryPath);
        }
        World world = new World{Name = $"WorldFile {_lastFileID}"};
        FileStream fs = File.Create(filename);
        UnicodeEncoding unicode = new UnicodeEncoding();
        fs.Write(unicode.GetBytes(JsonSerializer.Serialize(world)), 0, unicode.GetByteCount(JsonSerializer.Serialize(world)));
        fs.Close();
        return filename;
    }
    
    [Test, Apartment(ApartmentState.STA), NonParallelizable, Order(1)]
    public void EditorWindowTest()
    {
        string filename = SetupWorldFile();
        Editor editor = new Editor(filename); 
        Assert.Pass();
    }
}