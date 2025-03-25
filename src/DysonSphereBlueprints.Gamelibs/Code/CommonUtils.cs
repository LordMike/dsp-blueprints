using System.IO;
using System.Text;

namespace DysonSphereBlueprints.Gamelibs.Code;

public static class CommonUtils
{
    public static string ValidFileName(this string name)
    {
        StringBuilder stringBuilder = new StringBuilder(name.Length + 5);
        for (int index = 0; index < name.Length; ++index)
        {
            if (name[index] == ':' || name[index] == '*' || name[index] == '?' || name[index] == '"' ||
                name[index] == '<' || name[index] == '>' || name[index] == '|' || name[index] == '\\' ||
                name[index] == '/' || name[index] == '\r' || name[index] == '\n' || name[index] == '\t')
                stringBuilder.Append(" ");
            else
                stringBuilder.Append(name[index]);
        }

        return stringBuilder.ToString();
    }

    public static string SlashDirectory(this string dir)
    {
        switch (dir)
        {
            case null:
                return "";
            case "":
                return "";
            default:
                dir = dir.Replace("\\", "/").Replace("//", "/").Replace("//", "/");
                if (dir[dir.Length - 1] != '/')
                    dir += "/";
                return dir;
        }
    }

    public static string NewFilePath(string path)
    {
        if (path.Trim().Length < 1)
            path = "New Folder";
        path = path.SlashDirectory();
        path = path.Substring(0, path.Length - 1);
        string path1 = path;
        if (Directory.Exists(path1))
        {
            int num = 2;
            do
            {
                path1 = path + " (" + num + ")";
                ++num;
            } while (Directory.Exists(path1));
        }

        return path1 + "/";
    }

    public static string NewFileName01(string path, string filename, string ext)
    {
        if (filename.Trim().Length < 1)
            filename = "Untitled";
        filename = filename.ValidFileName();
        path = path.SlashDirectory();
        int num = 0;
        string path1;
        do
        {
            ++num;
            path1 = path + filename + " " + num.ToString("000") + ext;
        } while (File.Exists(path1));

        return path1;
    }
}