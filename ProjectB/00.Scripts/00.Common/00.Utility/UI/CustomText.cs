using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomText : Text
{
    public bool disableWordWrap;

    public override string text
    {
        get => base.text;
        set
        {
            if (disableWordWrap)
            {
                string newString = string.Empty;

                string[] oldTextLines = value.Split(' ');
                for (int i = 0; i < oldTextLines.Length; i++)
                {
                    // 만약 나눈 Line의 첫 글자가 공백이면 제거해줌.
                    oldTextLines[i].TrimStart();

                    newString += oldTextLines[i] + '\u00A0';
                }

                base.text = newString;
                return;
            }
            base.text = value;
        }
    }
}