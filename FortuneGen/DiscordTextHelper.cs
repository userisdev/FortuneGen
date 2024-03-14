using System.Globalization;
using System.Text.RegularExpressions;
/// <summary> DiscordTextHelper class. </summary>
internal static partial class DiscordTextHelper
{
    /// <summary> Divides the specified text. </summary>
    /// <param name="text"> The text. </param>
    /// <returns> </returns>
    public static IEnumerable<string> Divide(this string text)
    {
        int lastIndex = 0;
        IEnumerable<Match> emojiMatches = EmojiRegex().Matches(text).Cast<Match>();
        foreach (Match match in emojiMatches)
        {
            // 直前の絵文字の後から現在の絵文字の前までの部分をクラスタとして処理
            string cluster = text[lastIndex..match.Index];
            foreach (string s in SplitTextElement(cluster))
            {
                yield return s;
            }

            yield return match.Value;

            // 最後にマッチしたインデックスを更新
            lastIndex = match.Index + match.Length;
        }

        // 最後の絵文字の後から文字列の最後までの部分をクラスタとして処理
        string lastCluster = text[lastIndex..];
        foreach (string s in SplitTextElement(lastCluster))
        {
            yield return s;
        }
    }

    /// <summary> Randoms the specified text. </summary>
    /// <param name="text"> The text. </param>
    /// <returns> </returns>
    public static string Random(this string text)
    {
        return string.Join(string.Empty, text.Divide().Select(e => (text: e, guid: Guid.NewGuid())).OrderBy(e => e.guid).Select(e => e.text));
    }

    /// <summary> Reverses the specified text. </summary>
    /// <param name="text"> The text. </param>
    /// <returns> </returns>
    public static string Reverse(this string text)
    {
        return string.Join(string.Empty, text.Divide().Reverse());
    }

    /// <summary> Emojis the regex. </summary>
    /// <returns> </returns>
    [GeneratedRegex(@"<(a)?:.*?:(\d+)>")]
    private static partial Regex EmojiRegex();

    /// <summary> Splits the text element. </summary>
    /// <param name="input"> The input. </param>
    /// <returns> </returns>
    private static IEnumerable<string> SplitTextElement(string input)
    {
        TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(input);
        while (enumerator.MoveNext())
        {
            yield return enumerator.GetTextElement();
        }
    }
}
