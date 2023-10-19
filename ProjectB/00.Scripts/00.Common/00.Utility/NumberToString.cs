using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class NumberToString
{
    static string[] unitSymbol = new string[] { "", "만", "억", "조", "경", "해" };
    //static string[] unitSymbolRevers = new string[] { "해", "경", "조", "억", "만", "" };
    static string[] unitSymbolRevers = new string[] { "정", "간", "구", "양", "자", "해", "경", "조", "억", "만", "" };
    const string Zero = "0";

    static readonly string[] CurrencyUnits = new string[]
    {
         "", "만", "억", "조", "경", "해", "자",// "자", "양", "구", "간", "정", "재", "극",  "항하사", "아승기", "나유타", "불가사의", "무량대수",
         "양","대","검", "소", "녀", "키","우","기","A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
         "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "CE",
         "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU",
         "CV", "CW", "CY", "CZ", "DA", "DB", "DC", "DD", "DE",
         "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU",
         "DV", "DW", "DY", "DZ"
    };

    public static string ToCurrencyString(this double number)
    {
        if (-1d < number && number < 1d)
        {
            return Zero;
        }
        if (true == double.IsInfinity(number))
        {
            return "Infinity";
        }
        //string significant = (number < 0) ? "-" : string.Empty;
        string showNumber = string.Empty;
        string unitString = string.Empty;
        string[] partsSplit = number.ToString("E").Split('+');
        if (partsSplit.Length < 2)
        {
            UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0})", number)); return Zero;
        }

        if (false == int.TryParse(partsSplit[1], out int exponent))
        {
            UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0}) : partsSplit[1] = {1}", number, partsSplit[1]));
            return Zero;
        }

        int quotient = exponent / 4;
        int remainder = exponent % 4;
        if (exponent < 4)
        {
            showNumber = Math.Truncate(number).ToString();
        }
        else
        {
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * Math.Pow(10, remainder);
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unitString = CurrencyUnits[quotient];

        return string.Format("{0}{1}", showNumber, unitString);// significant,
    }

    // long 보다 double이 최대 값이 커서 double  사용
    //public static string ToString(long value)
    //{
    //    if (value <= 0) { return "0"; }

    //    int unitID = 0;

    //    string number = string.Format("{0:# #### #### #### #### #### #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = splits.Length; i > 0; i--)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i - 1], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Insert(0, $"{ digits}{ unitSymbol[unitID] }");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Insert(0, $"{ splits[i - 1] }");
    //        }
    //        unitID++;

    //    }

    //    return sb.ToString();
    //}

    //public static string ToStringCount(long value)
    //{
    //    if (value <= 0) { return "0"; }

    //    int unitID = 10;

    //    string number = string.Format("{0:# #### #### #### #### #### #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < splits.Length && i < 2; i++)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //                //sb.Insert(sb.Length - 1, $"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            //sb.Insert(sb.Length - 1, $"{ splits[i] }");
    //        }

    //    }

    //    return sb.ToString();
    //}

    //public static string ToStringCount(int value)
    //{
    //    if (value <= 0) { return "0"; }

    //    int unitID = 10;

    //    string number = string.Format("{0:# #### #### #### #### #### #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < splits.Length && i < 2; i++)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //                //sb.Insert(sb.Length - 1, $"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            //sb.Insert(sb.Length - 1, $"{ splits[i] }");
    //        }

    //    }

    //    return sb.ToString();
    //}


    //public static string ToStringCount(float value)
    //{
    //    if (value <= 0) { return "0"; }

    //    int unitID = 10;

    //    string number = string.Format("{0:# #### #### #### #### #### #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < splits.Length && i < 2; i++)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //                //sb.Insert(sb.Length - 1, $"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            //sb.Insert(sb.Length - 1, $"{ splits[i] }");
    //        }

    //    }

    //    return sb.ToString();
    //}

    //public static string ToStringCount(double value)
    //{
    //    if (value <= 0) { return "0"; }

    //    int unitID = 5;

    //    string number = string.Format("{0:# #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < splits.Length && i < 2; i++)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //                //sb.Insert(sb.Length - 1, $"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            //sb.Insert(sb.Length - 1, $"{ splits[i] }");
    //        }

    //    }

    //    return sb.ToString();
    //}

    //public static string ToStringCount(double value)
    //{
    //    if (value <= 0) { return "0"; }

    //    if (-1d < value && value < 1d)
    //    {
    //        return Zero;
    //    }

    //    int unitID = 10;

    //    string number = string.Format("{0:# #### #### #### #### #### #### #### #### #### ####}", value).TrimStart();
    //    string[] splits = number.Split(' ');

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < splits.Length && i < 2; i++)
    //    {
    //        int digits = 0;
    //        if (int.TryParse(splits[i], out digits))
    //        {
    //            // 앞자리가 0이 아닐때
    //            if (digits != 0)
    //            {
    //                sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //                //sb.Insert(sb.Length - 1, $"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            }
    //        }
    //        else
    //        {
    //            // 마이너스나 숫자외 문자
    //            sb.Append($"{ digits}{ unitSymbolRevers[unitID - (splits.Length - 1) + i]}");
    //            //sb.Insert(sb.Length - 1, $"{ splits[i] }");
    //        }

    //    }

    //    return sb.ToString();
    //}
}
