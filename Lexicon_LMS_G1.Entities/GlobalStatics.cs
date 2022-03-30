namespace Lexicon_LMS_G1.Entities;
public static class GlobalStatics
{
    private static string DocumentBaseName = "root";
    private static string DocumentCourseName = $"{SaveDocumentBase}{Path.DirectorySeparatorChar}Course";
    private static string DocumentModuleName = $"{SaveDocumentBase}{Path.DirectorySeparatorChar}Module";
    private static string DocumentActivityName = $"{SaveDocumentBase}{Path.DirectorySeparatorChar}Activity";
    private static string DocumentStudentName = $"{SaveDocumentBase}{Path.DirectorySeparatorChar}Student";
    public static string SaveDocumentBase 
    { 
        get 
        {
            if (!Directory.Exists(DocumentBaseName))
            {
                Directory.CreateDirectory(DocumentBaseName);
            }
            return DocumentBaseName;
        } 
    }
    public static string SaveDocumentCourse 
    { 
        get 
        {
            if (!Directory.Exists(DocumentCourseName))
            {
                Directory.CreateDirectory(DocumentCourseName);
            }
            return DocumentCourseName;
        } 
    }
    public static string SaveDocumentModule
    {
        get
        {
            if (!Directory.Exists(DocumentModuleName))
            {
                Directory.CreateDirectory(DocumentModuleName);
            }
            return DocumentModuleName;
        }
    }
    public static string SaveDocumentActivity
    {
        get
        {
            if (!Directory.Exists(DocumentActivityName))
            {
                Directory.CreateDirectory(DocumentActivityName);
            }
            return DocumentActivityName;
        }
    }
    public static string SaveDocumentStudent
    {
        get
        {
            if (!Directory.Exists(DocumentStudentName))
            {
                Directory.CreateDirectory(DocumentStudentName);
            }
            return DocumentStudentName;
        }
    }
}
