using System;
using System.Collections;
using System.IO;
using System.Text;

namespace DysonSphereBlueprints.Gamelibs.Code;

public static class MD5F
{
    private static uint A;
    private static uint B;
    private static uint C;
    private static uint D;
    private const int S11 = 7;
    private const int S12 = 12;
    private const int S13 = 17;
    private const int S14 = 22;
    private const int S21 = 5;
    private const int S22 = 9;
    private const int S23 = 14;
    private const int S24 = 20;
    private const int S31 = 4;
    private const int S32 = 11;
    private const int S33 = 16;
    private const int S34 = 23;
    private const int S41 = 6;
    private const int S42 = 10;
    private const int S43 = 15;
    private const int S44 = 21;

    private static uint F(uint x, uint y, uint z) => (uint) ((int) x & (int) y | ~(int) x & (int) z);

    private static uint G(uint x, uint y, uint z) => (uint) ((int) x & (int) z | (int) y & ~(int) z);

    private static uint H(uint x, uint y, uint z) => x ^ y ^ z;

    private static uint I(uint x, uint y, uint z) => y ^ (x | ~z);

    private static void FF(ref uint a, uint b, uint c, uint d, uint mj, int s, uint ti)
    {
        a = a + F(b, c, d) + mj + ti;
        a = a << s | a >> 32 - s;
        a += b;
    }

    private static void GG(ref uint a, uint b, uint c, uint d, uint mj, int s, uint ti)
    {
        a = a + G(b, c, d) + mj + ti;
        a = a << s | a >> 32 - s;
        a += b;
    }

    private static void HH(ref uint a, uint b, uint c, uint d, uint mj, int s, uint ti)
    {
        a = a + H(b, c, d) + mj + ti;
        a = a << s | a >> 32 - s;
        a += b;
    }

    private static void II(ref uint a, uint b, uint c, uint d, uint mj, int s, uint ti)
    {
        a = a + I(b, c, d) + mj + ti;
        a = a << s | a >> 32 - s;
        a += b;
    }

    private static void MD5_Init()
    {
        A = 1732584193U;
        B = 4024216457U;
        C = 2562383102U;
        D = 271734598U;
    }

    private static uint[] MD5_Append(byte[] input)
    {
        int num1 = 1;
        int length = input.Length;
        int num2 = length % 64;
        int num3;
        int num4;
        if (num2 < 56)
        {
            num3 = 55 - num2;
            num4 = length - num2 + 64;
        }
        else if (num2 == 56)
        {
            num3 = 63;
            num1 = 1;
            num4 = length + 8 + 64;
        }
        else
        {
            num3 = 63 - num2 + 56;
            num4 = length + 64 - num2 + 64;
        }
        ArrayList arrayList = new ArrayList(input);
        if (num1 == 1)
            arrayList.Add((byte) 128);
        for (int index = 0; index < num3; ++index)
            arrayList.Add((byte) 0);
        long num5 = length * 8L;
        byte num6 = (byte) ((ulong) num5 & byte.MaxValue);
        byte num7 = (byte) ((ulong) (num5 >>> 8) & byte.MaxValue);
        byte num8 = (byte) ((ulong) (num5 >>> 16) & byte.MaxValue);
        byte num9 = (byte) ((ulong) (num5 >>> 24) & byte.MaxValue);
        byte num10 = (byte) ((ulong) (num5 >>> 32) & byte.MaxValue);
        byte num11 = (byte) ((ulong) (num5 >>> 40) & byte.MaxValue);
        byte num12 = (byte) ((ulong) (num5 >>> 48) & byte.MaxValue);
        byte num13 = (byte) (num5 >>> 56);
        arrayList.Add(num6);
        arrayList.Add(num7);
        arrayList.Add(num8);
        arrayList.Add(num9);
        arrayList.Add(num10);
        arrayList.Add(num11);
        arrayList.Add(num12);
        arrayList.Add(num13);
        byte[] array = (byte[]) arrayList.ToArray(typeof (byte));
        uint[] numArray = new uint[num4 / 4];
        long index1 = 0;
        long index2 = 0;
        for (; index1 < num4; index1 += 4L)
        {
            numArray[index2] = (uint) (array[index1] | array[index1 + 1L] << 8 | array[index1 + 2L] << 16 | array[index1 + 3L] << 24);
            ++index2;
        }
        return numArray;
    }

    private static uint[] MD5_Trasform(uint[] x)
    {
        for (int index = 0; index < x.Length; index += 16)
        {
            uint a = A;
            uint b = B;
            uint c = C;
            uint d = D;
            FF(ref a, b, c, d, x[index], 7, 3614090360U);
            FF(ref d, a, b, c, x[index + 1], 12, 3906451286U);
            FF(ref c, d, a, b, x[index + 2], 17, 606105819U);
            FF(ref b, c, d, a, x[index + 3], 22, 3250441966U);
            FF(ref a, b, c, d, x[index + 4], 7, 4118548399U);
            FF(ref d, a, b, c, x[index + 5], 12, 1200080426U);
            FF(ref c, d, a, b, x[index + 6], 17, 2821735971U);
            FF(ref b, c, d, a, x[index + 7], 22, 4249261313U);
            FF(ref a, b, c, d, x[index + 8], 7, 1770035416U);
            FF(ref d, a, b, c, x[index + 9], 12, 2336552879U);
            FF(ref c, d, a, b, x[index + 10], 17, 4294925233U);
            FF(ref b, c, d, a, x[index + 11], 22, 2304563134U);
            FF(ref a, b, c, d, x[index + 12], 7, 1805586722U);
            FF(ref d, a, b, c, x[index + 13], 12, 4254626195U);
            FF(ref c, d, a, b, x[index + 14], 17, 2792965006U);
            FF(ref b, c, d, a, x[index + 15], 22, 968099873U);
            GG(ref a, b, c, d, x[index + 1], 5, 4129170786U);
            GG(ref d, a, b, c, x[index + 6], 9, 3225465664U);
            GG(ref c, d, a, b, x[index + 11], 14, 643717713U);
            GG(ref b, c, d, a, x[index], 20, 3384199082U);
            GG(ref a, b, c, d, x[index + 5], 5, 3593408605U);
            GG(ref d, a, b, c, x[index + 10], 9, 38024275U);
            GG(ref c, d, a, b, x[index + 15], 14, 3634488961U);
            GG(ref b, c, d, a, x[index + 4], 20, 3889429448U);
            GG(ref a, b, c, d, x[index + 9], 5, 569495014U);
            GG(ref d, a, b, c, x[index + 14], 9, 3275163606U);
            GG(ref c, d, a, b, x[index + 3], 14, 4107603335U);
            GG(ref b, c, d, a, x[index + 8], 20, 1197085933U);
            GG(ref a, b, c, d, x[index + 13], 5, 2850285829U);
            GG(ref d, a, b, c, x[index + 2], 9, 4243563512U);
            GG(ref c, d, a, b, x[index + 7], 14, 1735328473U);
            GG(ref b, c, d, a, x[index + 12], 20, 2368359562U);
            HH(ref a, b, c, d, x[index + 5], 4, 4294588738U);
            HH(ref d, a, b, c, x[index + 8], 11, 2272392833U);
            HH(ref c, d, a, b, x[index + 11], 16, 1839030562U);
            HH(ref b, c, d, a, x[index + 14], 23, 4259657740U);
            HH(ref a, b, c, d, x[index + 1], 4, 2763975236U);
            HH(ref d, a, b, c, x[index + 4], 11, 1272893353U);
            HH(ref c, d, a, b, x[index + 7], 16, 4139469664U);
            HH(ref b, c, d, a, x[index + 10], 23, 3200236656U);
            HH(ref a, b, c, d, x[index + 13], 4, 681279174U);
            HH(ref d, a, b, c, x[index], 11, 3936430074U);
            HH(ref c, d, a, b, x[index + 3], 16, 3572445317U);
            HH(ref b, c, d, a, x[index + 6], 23, 76029189U);
            HH(ref a, b, c, d, x[index + 9], 4, 3654602809U);
            HH(ref d, a, b, c, x[index + 12], 11, 3873151461U);
            HH(ref c, d, a, b, x[index + 15], 16, 530742520U);
            HH(ref b, c, d, a, x[index + 2], 23, 3299628645U);
            II(ref a, b, c, d, x[index], 6, 4096336452U);
            II(ref d, a, b, c, x[index + 7], 10, 1126891415U);
            II(ref c, d, a, b, x[index + 14], 15, 2878612391U);
            II(ref b, c, d, a, x[index + 5], 21, 4237533241U);
            II(ref a, b, c, d, x[index + 12], 6, 1700485571U);
            II(ref d, a, b, c, x[index + 3], 10, 2399980690U);
            II(ref c, d, a, b, x[index + 10], 15, 4293915773U);
            II(ref b, c, d, a, x[index + 1], 21, 2240044497U);
            II(ref a, b, c, d, x[index + 8], 6, 1873313359U);
            II(ref d, a, b, c, x[index + 15], 10, 4264355552U);
            II(ref c, d, a, b, x[index + 6], 15, 2734768916U);
            II(ref b, c, d, a, x[index + 13], 21, 1309151649U);
            II(ref a, b, c, d, x[index + 4], 6, 4149444226U);
            II(ref d, a, b, c, x[index + 11], 10, 3174756917U);
            II(ref c, d, a, b, x[index + 2], 15, 718787259U);
            II(ref b, c, d, a, x[index + 9], 21, 3951481745U);
            A += a;
            B += b;
            C += c;
            D += d;
        }
        return new uint[4]{ A, B, C, D };
    }

    private static uint[] MD5_Append_Opt(byte[] input)
    {
        int num1 = 1;
        int length1 = input.Length;
        int num2 = length1 % 64;
        int num3;
        int num4;
        if (num2 < 56)
        {
            num3 = 55 - num2;
            num4 = length1 - num2 + 64;
        }
        else if (num2 == 56)
        {
            num3 = 63;
            num1 = 1;
            num4 = length1 + 8 + 64;
        }
        else
        {
            num3 = 63 - num2 + 56;
            num4 = length1 + 64 - num2 + 64;
        }
        int length2 = input.Length;
        int length3 = num3 + 8 + length2;
        if (num1 == 1)
            ++length3;
        byte[] destinationArray = new byte[length3];
        Array.Copy(input, destinationArray, length2);
        if (num1 == 1)
            destinationArray[length2++] = 128;
        for (int index = 0; index < num3; ++index)
            destinationArray[length2++] = 0;
        long num5 = length1 * 8L;
        byte num6 = (byte) ((ulong) num5 & byte.MaxValue);
        byte num7 = (byte) ((ulong) (num5 >>> 8) & byte.MaxValue);
        byte num8 = (byte) ((ulong) (num5 >>> 16) & byte.MaxValue);
        byte num9 = (byte) ((ulong) (num5 >>> 24) & byte.MaxValue);
        byte num10 = (byte) ((ulong) (num5 >>> 32) & byte.MaxValue);
        byte num11 = (byte) ((ulong) (num5 >>> 40) & byte.MaxValue);
        byte num12 = (byte) ((ulong) (num5 >>> 48) & byte.MaxValue);
        byte num13 = (byte) (num5 >>> 56);
        byte[] numArray1 = destinationArray;
        int index1 = length2;
        int num14 = index1 + 1;
        int num15 = num6;
        numArray1[index1] = (byte) num15;
        byte[] numArray2 = destinationArray;
        int index2 = num14;
        int num16 = index2 + 1;
        int num17 = num7;
        numArray2[index2] = (byte) num17;
        byte[] numArray3 = destinationArray;
        int index3 = num16;
        int num18 = index3 + 1;
        int num19 = num8;
        numArray3[index3] = (byte) num19;
        byte[] numArray4 = destinationArray;
        int index4 = num18;
        int num20 = index4 + 1;
        int num21 = num9;
        numArray4[index4] = (byte) num21;
        byte[] numArray5 = destinationArray;
        int index5 = num20;
        int num22 = index5 + 1;
        int num23 = num10;
        numArray5[index5] = (byte) num23;
        byte[] numArray6 = destinationArray;
        int index6 = num22;
        int num24 = index6 + 1;
        int num25 = num11;
        numArray6[index6] = (byte) num25;
        byte[] numArray7 = destinationArray;
        int index7 = num24;
        int num26 = index7 + 1;
        int num27 = num12;
        numArray7[index7] = (byte) num27;
        byte[] numArray8 = destinationArray;
        int index8 = num26;
        int num28 = index8 + 1;
        int num29 = num13;
        numArray8[index8] = (byte) num29;
        uint[] numArray9 = new uint[num4 / 4];
        long index9 = 0;
        long index10 = 0;
        for (; index9 < num4; index9 += 4L)
        {
            numArray9[index10] = (uint) (destinationArray[index9] | destinationArray[index9 + 1L] << 8 | destinationArray[index9 + 2L] << 16 | destinationArray[index9 + 3L] << 24);
            ++index10;
        }
        return numArray9;
    }

    private static byte[] MD5_Array(byte[] input)
    {
        MD5_Init();
        uint[] numArray1 = MD5_Trasform(MD5_Append(input));
        byte[] numArray2 = new byte[numArray1.Length * 4];
        int index1 = 0;
        int index2 = 0;
        while (index1 < numArray1.Length)
        {
            numArray2[index2] = (byte) (numArray1[index1] & byte.MaxValue);
            numArray2[index2 + 1] = (byte) (numArray1[index1] >> 8 & byte.MaxValue);
            numArray2[index2 + 2] = (byte) (numArray1[index1] >> 16 & byte.MaxValue);
            numArray2[index2 + 3] = (byte) (numArray1[index1] >> 24 & byte.MaxValue);
            ++index1;
            index2 += 4;
        }
        return numArray2;
    }

    private static string ArrayToHexString(byte[] array, bool uppercase)
    {
        string hexString = "";
        string format = "x2";
        if (uppercase)
            format = "X2";
        foreach (byte num in array)
            hexString += num.ToString(format);
        return hexString;
    }

    public static string Compute(byte[] message)
    {
        MD5_Init();
        uint[] numArray = MD5_Trasform(MD5_Append(message));
        byte[] array = new byte[numArray.Length * 4];
        int index1 = 0;
        int index2 = 0;
        while (index1 < numArray.Length)
        {
            array[index2] = (byte) (numArray[index1] & byte.MaxValue);
            array[index2 + 1] = (byte) (numArray[index1] >> 8 & byte.MaxValue);
            array[index2 + 2] = (byte) (numArray[index1] >> 16 & byte.MaxValue);
            array[index2 + 3] = (byte) (numArray[index1] >> 24 & byte.MaxValue);
            ++index1;
            index2 += 4;
        }
        return ArrayToHexString(array, true);
    }

    public static string Compute(Stream stream)
    {
        byte[] input = new BinaryReader(stream).ReadBytes((int) stream.Length);
        MD5_Init();
        uint[] numArray = MD5_Trasform(MD5_Append(input));
        byte[] array = new byte[numArray.Length * 4];
        int index1 = 0;
        int index2 = 0;
        while (index1 < numArray.Length)
        {
            array[index2] = (byte) (numArray[index1] & byte.MaxValue);
            array[index2 + 1] = (byte) (numArray[index1] >> 8 & byte.MaxValue);
            array[index2 + 2] = (byte) (numArray[index1] >> 16 & byte.MaxValue);
            array[index2 + 3] = (byte) (numArray[index1] >> 24 & byte.MaxValue);
            ++index1;
            index2 += 4;
        }
        return ArrayToHexString(array, true);
    }

    public static string Compute_Opt(Stream stream)
    {
        byte[] input = new BinaryReader(stream).ReadBytes((int) stream.Length);
        MD5_Init();
        uint[] numArray = MD5_Trasform(MD5_Append_Opt(input));
        byte[] array = new byte[numArray.Length * 4];
        int index1 = 0;
        int index2 = 0;
        while (index1 < numArray.Length)
        {
            array[index2] = (byte) (numArray[index1] & byte.MaxValue);
            array[index2 + 1] = (byte) (numArray[index1] >> 8 & byte.MaxValue);
            array[index2 + 2] = (byte) (numArray[index1] >> 16 & byte.MaxValue);
            array[index2 + 3] = (byte) (numArray[index1] >> 24 & byte.MaxValue);
            ++index1;
            index2 += 4;
        }
        return ArrayToHexString(array, true);
    }

    public static string Compute(string message)
    {
        return ArrayToHexString(MD5_Array(Encoding.UTF8.GetBytes(message)), true);
    }
}