using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using DysonSphereBlueprints.Gamelibs.Code.Patchwork;

namespace DysonSphereBlueprints.Gamelibs.Code;

public class BlueprintData
{
    public DateTime time;
    public string gameVersion;
    public string shortDesc;
    public string desc;
    public EIconLayout layout;
    public int icon0;
    public int icon1;
    public int icon2;
    public int icon3;
    public int icon4;
    public int cursorOffset_x;
    public int cursorOffset_y;
    public int cursorTargetArea;
    public int dragBoxSize_x;
    public int dragBoxSize_y;
    public int primaryAreaIdx;
    public BlueprintArea[] areas;
    public BlueprintBuilding[] buildings;
    private int patch;
    public const int PATCH_POLE_AREA_BUG = 1;
    public const int CURRENT_PATCH = 1;
    private List<BlueprintBuilding> _tmp_buidings = new();
    private List<BlueprintBuilding> _del_buidings = new();

    public bool isValid => primaryAreaIdx >= 0 && buildings != null && areas != null && areas.Length != 0;

    public bool isEmpty => buildings == null || buildings.Length == 0;

    public static bool IsNullOrEmpty(BlueprintData blueprint)
    {
        return blueprint == null || !blueprint.isValid || blueprint.buildings.Length == 0;
    }

    public string headerStr =>
        "BLUEPRINT:0," + (int)layout + "," + icon0 + "," +
        icon1 + "," + icon2 + "," + icon3 + "," +
        icon4 + ",0," + time.Ticks + "," + gameVersion + "," +
        shortDesc.Escape() + "," + desc.Escape() + "\"";

    public static BlueprintData CreateNew()
    {
        BlueprintData blueprintData = new BlueprintData();
        blueprintData.ResetAsEmpty();
        return blueprintData;
    }

    public static BlueprintData CreateNew(string _str64Data)
    {
        BlueprintData blueprintData = new BlueprintData(_str64Data);
        return blueprintData.isValid ? blueprintData : null;
    }

    public static BlueprintData CreateFromFile(string _fileName)
    {
        BlueprintData blueprintData = new BlueprintData();
        return blueprintData.LoadBlueprintData(_fileName) == BlueprintDataIOError.OK && blueprintData.isValid
            ? blueprintData
            : null;
    }

    public static string GenerateNewFileName(string path)
    {
        if (string.IsNullOrEmpty(path))
            path = GameConfig.blueprintFolder;
        return CommonUtils.NewFileName01(path, "新的蓝图".Translate(), ".txt");
    }

    public static string GenerateNewFolderName(string path)
    {
        if (string.IsNullOrEmpty(path))
            path = GameConfig.blueprintFolder;
        path += "新的蓝图集".Translate();
        return CommonUtils.NewFilePath(path);
    }

    public BlueprintData() => Reset();

    private BlueprintData(string _str64Data)
    {
        if (ContentFromBase64String(_str64Data) == BlueprintDataIOError.OK)
            return;
        Reset();
    }

    public void Reset()
    {
        time = new DateTime();
        gameVersion = "";
        shortDesc = "";
        desc = "";
        layout = EIconLayout.None;
        icon0 = 0;
        icon1 = 0;
        icon2 = 0;
        icon3 = 0;
        icon4 = 0;
        cursorOffset_x = 0;
        cursorOffset_y = 0;
        dragBoxSize_x = 1;
        dragBoxSize_y = 1;
        cursorTargetArea = 0;
        primaryAreaIdx = -1;
        areas = null;
        buildings = null;
    }

    public void ResetAsEmpty()
    {
        time = DateTime.Now;
        gameVersion = GameConfig.gameVersion.ToFullString();
        shortDesc = "新的蓝图".Translate();
        desc = "";
        layout = EIconLayout.OneIcon;
        icon0 = 0;
        icon1 = 0;
        icon2 = 0;
        icon3 = 0;
        icon4 = 0;
        cursorOffset_x = 0;
        cursorOffset_y = 0;
        dragBoxSize_x = 1;
        dragBoxSize_y = 1;
        cursorTargetArea = 0;
        primaryAreaIdx = 0;
        areas = new BlueprintArea[1];
        areas[0] = new BlueprintArea();
        areas[0].index = 0;
        areas[0].parentIndex = -1;
        areas[0].tropicAnchor = 0;
        areas[0].areaSegments = 200;
        areas[0].anchorLocalOffsetX = 0;
        areas[0].anchorLocalOffsetY = 0;
        areas[0].width = 1;
        areas[0].height = 1;
        buildings = new BlueprintBuilding[0];
    }

    public void ResetContentAsEmpty()
    {
        cursorOffset_x = 0;
        cursorOffset_y = 0;
        dragBoxSize_x = 1;
        dragBoxSize_y = 1;
        cursorTargetArea = 0;
        primaryAreaIdx = 0;
        areas = new BlueprintArea[1];
        areas[0] = new BlueprintArea();
        areas[0].index = 0;
        areas[0].parentIndex = -1;
        areas[0].tropicAnchor = 0;
        areas[0].areaSegments = 200;
        areas[0].anchorLocalOffsetX = 0;
        areas[0].anchorLocalOffsetY = 0;
        areas[0].width = 1;
        areas[0].height = 1;
        buildings = new BlueprintBuilding[0];
    }

    public BlueprintData Clone()
    {
        string base64String = ToBase64String();
        BlueprintData blueprintData = new BlueprintData();
        int num = (int)blueprintData.FromBase64String(base64String);
        return blueprintData.isValid ? blueprintData : null;
    }

    public void Free()
    {
        if (areas != null)
        {
            for (int index = 0; index < areas.Length; ++index)
                areas[index] = null;
        }

        Reset();
    }

    public void RemoveBuildings(int _itemId)
    {
        throw new NotImplementedException();
        // this._tmp_buidings.Clear();
        // this._del_buidings.Clear();
        // this._tmp_buidings.AddRange((IEnumerable<BlueprintBuilding>) this.buildings);
        // for (int index = 0; index < this._tmp_buidings.Count; ++index)
        // {
        //     if ((int) this._tmp_buidings[index].itemId == _itemId)
        //     {
        //         BlueprintBuilding tmpBuiding = this._tmp_buidings[index];
        //         this._tmp_buidings.RemoveAt(index);
        //         this._del_buidings.Add(tmpBuiding);
        //         --index;
        //     }
        // }
        // for (int index = 0; index < this._tmp_buidings.Count; ++index)
        // {
        //     PrefabDesc prefabDesc = LDB.models.Select((int) this._tmp_buidings[index].modelIndex).prefabDesc;
        //     if (prefabDesc.isInserter)
        //     {
        //         if ((this._tmp_buidings[index].inputObj == null || this._del_buidings.Contains(this._tmp_buidings[index].inputObj)) && (this._tmp_buidings[index].outputObj == null || this._del_buidings.Contains(this._tmp_buidings[index].outputObj)))
        //         {
        //             BlueprintBuilding tmpBuiding = this._tmp_buidings[index];
        //             this._tmp_buidings.RemoveAt(index);
        //             this._del_buidings.Add(tmpBuiding);
        //             --index;
        //         }
        //     }
        //     else if (prefabDesc.isBelt)
        //     {
        //         if (this._del_buidings.Contains(this._tmp_buidings[index].inputObj) && !LDB.models.Select((int) this._tmp_buidings[index].inputObj.modelIndex).prefabDesc.isBelt)
        //         {
        //             BlueprintBuilding tmpBuiding = this._tmp_buidings[index];
        //             this._tmp_buidings.RemoveAt(index);
        //             this._del_buidings.Add(tmpBuiding);
        //             --index;
        //         }
        //         else if (this._del_buidings.Contains(this._tmp_buidings[index].outputObj) && !LDB.models.Select((int) this._tmp_buidings[index].outputObj.modelIndex).prefabDesc.isBelt)
        //         {
        //             BlueprintBuilding tmpBuiding = this._tmp_buidings[index];
        //             this._tmp_buidings.RemoveAt(index);
        //             this._del_buidings.Add(tmpBuiding);
        //             --index;
        //         }
        //     }
        // }
        // for (int index = 0; index < this._tmp_buidings.Count; ++index)
        // {
        //     if (this._del_buidings.Contains(this._tmp_buidings[index].inputObj))
        //         this._tmp_buidings[index].inputObj = (BlueprintBuilding) null;
        //     if (this._del_buidings.Contains(this._tmp_buidings[index].outputObj))
        //         this._tmp_buidings[index].outputObj = (BlueprintBuilding) null;
        //     this._tmp_buidings[index].index = index;
        // }
        // this.buildings = this._tmp_buidings.ToArray();
        // this._tmp_buidings.Clear();
        // this._del_buidings.Clear();
    }

    public void CheckBuildingData()
    {
        int length = 0;
        BlueprintBuilding[] blueprintBuildingArray = new BlueprintBuilding[buildings.Length];
        for (int index = 0; index < buildings.Length; ++index)
        {
            if (buildings[index].index == index)
            {
                blueprintBuildingArray[length] = buildings[index];
                ++length;
            }
            else
                buildings[index].index = -1;
        }

        if (length != buildings.Length)
        {
            for (int index = 0; index < buildings.Length; ++index)
            {
                if (buildings[index].index == index)
                {
                    if (buildings[index].outputObj != null && buildings[index].outputObj.index < 0)
                        buildings[index].outputObj = null;
                    if (buildings[index].inputObj != null && buildings[index].inputObj.index < 0)
                        buildings[index].inputObj = null;
                }
            }

            buildings = null;
            buildings = new BlueprintBuilding[length];
            for (int index = 0; index < length; ++index)
            {
                buildings[index] = blueprintBuildingArray[index];
                buildings[index].index = index;
            }
        }
    }

    public bool SaveBlueprintData(string _fileName)
    {
        if (!isValid)
            return false;
        try
        {
            string fullName = new FileInfo(_fileName).Directory.FullName;
            if (!Directory.Exists(fullName))
                Directory.CreateDirectory(fullName);
            File.WriteAllText(_fileName, ToBase64String(), Encoding.ASCII);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public BlueprintDataIOError LoadBlueprintData(string _fileName)
    {
        string str64Data;
        try
        {
            str64Data = File.ReadAllText(_fileName, Encoding.ASCII);
        }
        catch
        {
            Reset();
            return BlueprintDataIOError.FileIOError;
        }

        try
        {
            int num = (int)FromBase64String(str64Data);
            if (num != 0)
                Reset();
            return (BlueprintDataIOError)num;
        }
        catch
        {
            Reset();
            return BlueprintDataIOError.DataCorruption;
        }
    }

    public BlueprintDataIOError LoadHeader(string _fileName)
    {
        if (File.Exists(_fileName))
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder(128);
                using (FileStream input = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader(input))
                    {
                        int num = input.Length < 8192L ? (int)input.Length : 8192;
                        char[] chArray = binaryReader.ReadChars(28);
                        stringBuilder.Append(chArray);
                        for (int index = 0; index < num; ++index)
                        {
                            char ch = binaryReader.ReadChar();
                            stringBuilder.Append(ch);
                            if (ch == '"')
                                break;
                        }
                    }
                }

                return HeaderFromBase64String(stringBuilder.ToString());
            }
            catch
            {
                Reset();
                return BlueprintDataIOError.FileIOError;
            }
        }

        Reset();
        return BlueprintDataIOError.FileIOError;
    }

    public BlueprintDataIOError HeaderFromBase64String(string contentStr)
    {
        Reset();
        if (contentStr.Length < 28)
        {
            Reset();
            return BlueprintDataIOError.HeaderDataError;
        }

        if (contentStr[0] == 'B' && contentStr[1] == 'L' && contentStr[2] == 'U' && contentStr[3] == 'E' &&
            contentStr[4] == 'P' && contentStr[5] == 'R' && contentStr[6] == 'I' && contentStr[7] == 'N' &&
            contentStr[8] == 'T')
        {
            if (contentStr[9] == ':')
            {
                try
                {
                    int num = Math.Min(contentStr.Length, 8192);
                    for (int index = 28; index < num; ++index)
                    {
                        if (contentStr[index] == '"')
                        {
                            string[] strArray = contentStr.Substring(10, index - 10).Split(',');
                            if (strArray.Length < 12)
                                return BlueprintDataIOError.HeaderDataError;
                            int o1;
                            strArray[1].ToInt(out o1);
                            strArray[2].ToInt(out icon0);
                            strArray[3].ToInt(out icon1);
                            strArray[4].ToInt(out icon2);
                            strArray[5].ToInt(out icon3);
                            strArray[6].ToInt(out icon4);
                            long o2;
                            strArray[8].ToLong(out o2);
                            gameVersion = strArray[9];
                            shortDesc = strArray[10].Unescape();
                            desc = strArray[11].Unescape();
                            layout = (EIconLayout)o1;
                            time = new DateTime(o2);
                            return BlueprintDataIOError.OK;
                        }
                    }

                    Reset();
                    return BlueprintDataIOError.HeaderDataError;
                }
                catch
                {
                    Reset();
                    return BlueprintDataIOError.HeaderDataError;
                }
            }
        }

        Reset();
        return BlueprintDataIOError.HeaderDataError;
    }

    public BlueprintDataIOError CheckSignature(string _str64Data)
    {
        int num = Math.Max(_str64Data.Length - 36, 0);
        for (int index = _str64Data.Length - 1; index >= num; --index)
        {
            if (_str64Data[index] == '"')
            {
                if (_str64Data.Length - 1 - index >= 32)
                    return MD5F.Compute(_str64Data.Substring(0, index))
                        .Equals(_str64Data.Substring(index + 1, 32), StringComparison.Ordinal)
                        ? BlueprintDataIOError.OK
                        : BlueprintDataIOError.MD5CannotMatch;
                break;
            }
        }

        return BlueprintDataIOError.DataCorruption;
    }

    public BlueprintDataIOError ContentFromBase64String(string strData)
    {
        try
        {
            int num1 = -1;
            int num2 = -1;
            for (int index = 0; index < strData.Length; ++index)
            {
                if (strData[index] == '"')
                {
                    num1 = index;
                    break;
                }
            }

            if (num1 < 0)
                return BlueprintDataIOError.DataCorruption;
            for (int index = strData.Length - 1; index >= 0; --index)
            {
                if (strData[index] == '"')
                {
                    num2 = index;
                    break;
                }
            }

            if (num2 < num1 + 32)
                return BlueprintDataIOError.DataCorruption;
            using (MemoryStream memoryStream1 =
                   new MemoryStream(Convert.FromBase64String(strData.Substring(num1 + 1, num2 - num1 - 1))))
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(memoryStream1, CompressionMode.Decompress))
                        gzipStream.CopyTo(memoryStream2);
                    memoryStream2.Position = 0L;
                    using (BinaryReader r = new BinaryReader(memoryStream2))
                        Import(r);
                }
            }

            return BlueprintDataIOError.OK;
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Blueprint Error: \r\n" + ex.ToString().Replace("Exception", "Excption"));
            return BlueprintDataIOError.DataCorruption;
        }
    }

    public string ToBase64String()
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            using (MemoryStream output = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(output))
                {
                    Export(w);
                    output.Position = 0L;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (GZipStream destination = new GZipStream(memoryStream, CompressionMode.Compress))
                            output.CopyTo(destination);
                        byte[] array = memoryStream.ToArray();
                        stringBuilder.Append(headerStr);
                        stringBuilder.Append(Convert.ToBase64String(array));
                        string str = MD5F.Compute(stringBuilder.ToString());
                        stringBuilder.Append("\"");
                        stringBuilder.Append(str);
                        return stringBuilder.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString().Replace("Exception", "Excption"));
        }

        return "";
    }

    public StringBuilder ToBase64StringBuilder()
    {
        try
        {
            StringBuilder base64StringBuilder = new StringBuilder(1024);
            using (MemoryStream output = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(output))
                {
                    Export(w);
                    output.Position = 0L;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (GZipStream destination = new GZipStream(memoryStream, CompressionMode.Compress))
                            output.CopyTo(destination);
                        byte[] array = memoryStream.ToArray();
                        base64StringBuilder.Append(headerStr);
                        base64StringBuilder.Append(Convert.ToBase64String(array));
                        string str = MD5F.Compute(base64StringBuilder.ToString());
                        base64StringBuilder.Append("\"");
                        base64StringBuilder.Append(str);
                        return base64StringBuilder;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString().Replace("Exception", "Excption"));
        }

        return null;
    }

    public BlueprintDataIOError FromBase64String(string str64Data)
    {
        if (string.IsNullOrEmpty(str64Data))
            return BlueprintDataIOError.DataCorruption;
        BlueprintDataIOError blueprintDataIoError1 = HeaderFromBase64String(str64Data);
        if (blueprintDataIoError1 != BlueprintDataIOError.OK)
        {
            Reset();
            return blueprintDataIoError1;
        }

        BlueprintDataIOError blueprintDataIoError2 = CheckSignature(str64Data);
        if (blueprintDataIoError2 != BlueprintDataIOError.OK)
        {
            Reset();
            return blueprintDataIoError2;
        }

        BlueprintDataIOError blueprintDataIoError3 = ContentFromBase64String(str64Data);
        if (blueprintDataIoError3 == BlueprintDataIOError.OK)
            return BlueprintDataIOError.OK;
        Reset();
        return blueprintDataIoError3;
    }

    public void Import(BinaryReader r)
    {
        patch = r.ReadInt32();
        cursorOffset_x = r.ReadInt32();
        cursorOffset_y = r.ReadInt32();
        cursorTargetArea = r.ReadInt32();
        dragBoxSize_x = r.ReadInt32();
        dragBoxSize_y = r.ReadInt32();
        primaryAreaIdx = r.ReadInt32();
        int length1 = r.ReadByte();
        areas = length1 >= 0 && length1 <= 64 && primaryAreaIdx >= -1 && primaryAreaIdx <= length1
            ? new BlueprintArea[length1]
            : throw new Exception("Corrupt Data");
        for (int index = 0; index < length1; ++index)
        {
            areas[index] = new BlueprintArea();
            areas[index].Import(r);
        }

        int length2 = r.ReadInt32();
        buildings = length2 >= 0 && length2 <= 1048576
            ? new BlueprintBuilding[length2]
            : throw new Exception("Corrupt Data");
        for (int index = 0; index < length2; ++index)
        {
            buildings[index] = new BlueprintBuilding();
            buildings[index].Import(r);
        }

        for (int index = 0; index < length2; ++index)
        {
            if (buildings[index].outputObj == null && buildings[index].tempOutputObjIdx >= 0)
                buildings[index].outputObj = buildings[buildings[index].tempOutputObjIdx];
            if (buildings[index].inputObj == null && buildings[index].tempInputObjIdx >= 0)
                buildings[index].inputObj = buildings[buildings[index].tempInputObjIdx];
        }

        DataRepair();
    }

    public void Export(BinaryWriter w)
    {
        patch = 1;
        w.Write(patch);
        w.Write(cursorOffset_x);
        w.Write(cursorOffset_y);
        w.Write(cursorTargetArea);
        w.Write(dragBoxSize_x);
        w.Write(dragBoxSize_y);
        w.Write(primaryAreaIdx);
        int length1 = areas != null ? areas.Length : 0;
        w.Write((byte)length1);
        for (int index = 0; index < length1; ++index)
            areas[index].Export(w);
        int length2 = buildings != null ? buildings.Length : 0;
        w.Write(length2);
        for (int index = 0; index < length2; ++index)
            buildings[index].Export(w);
    }

    public void DataRepair()
    {
        int length = areas.Length;
        if (patch >= 1 || length <= 0 || (areas[0].areaSegments != 4 || areas[0].anchorLocalOffsetX != 17
                ? areas[length - 1].areaSegments != 4
                    ? 0
                    : areas[length - 1].anchorLocalOffsetX == 17 ? 1 : 0
                : 1) == 0)
            return;
        for (int index = 0; index < length; ++index)
            areas[index].anchorLocalOffsetX = 0;
    }
}