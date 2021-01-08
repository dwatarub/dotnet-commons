using System.Linq;
using System.Text.RegularExpressions;

namespace ArceniX.Commons.Extensions
{
    /// <summary>
    /// <see cref="string"/> に対する、さまざまな拡張機能を提供します。
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 指定された文字列が <c>null</c> または <see cref="string.Empty"/> 文字列であるかどうかを示します。
        /// このメソッドは <see cref="string.IsNullOrEmpty(string)"/> の拡張ラッパーです。
        /// </summary>
        /// <param name="value">テストする文字列。</param>
        /// <returns>パラメーターが <c>null</c> または空の文字列 ("") の場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static bool IsNullOrEmpty( this string value ) => string.IsNullOrEmpty( value );

        /// <summary>
        /// 指定された文字列が <c>null</c> または <see cref="string.Empty"/> 文字列であるか、空白文字だけで構成されているかどうかを示します。
        /// このメソッドは <see cref="string.IsNullOrWhiteSpace(string)"/> の拡張ラッパーです。
        /// </summary>
        /// <param name="value">テストする文字列。</param>
        /// <returns>パラメーターが <c>null</c> または <see cref="string.Empty"/> 文字列であるか、空白文字だけで構成されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static bool IsNullOrWhiteSpace( this string value ) => string.IsNullOrWhiteSpace( value );

        /// <summary>
        /// 指定した文字列内に、検索候補が含まれる数を取得します。
        /// </summary>
        /// <param name="value">検索対象となる文字列。</param>
        /// <param name="candidate">検索する候補文字を含む <see cref="string"/> 配列。</param>
        /// <returns>検索対象文字列の中に含まれる、検索候補文字の数。</returns>
        public static int GetIncludedCandidatesCount( this string value, string[] candidate )
        {
            // nullや空文字等の指定で例外が発生しないようクッション
            value = string.IsNullOrEmpty( value ) ? string.Empty : value;
            candidate = candidate ?? new string[] { };

            return value.Distinct().Count( c => candidate.Distinct().Contains( c.ToString() ) );
        }

        /// <summary>
        /// この文字列の先頭に、指定した正規表現に一致する文字が含まれている数を取得します。
        /// 正規表現は文字単位で評価を行うため、行頭文字などを含める必要はありません。
        /// </summary>
        /// <param name="value">対象となる文字列。</param>
        /// <param name="pattern">一致させる正規表現パターン。</param>
        /// <returns>正規表現パターンに一致した文字数。</returns>
        public static int GetHeadingCharactersCount( this string value, string pattern ) => value.TakeWhile( c => Regex.IsMatch( c.ToString(), pattern ) ).Count();

        /// <summary>
        /// 指定した文字列を、コマンドラインパラメータとして使用可能な文字列にエスケープします。
        /// </summary>
        /// <param name="value">対象となる文字列。</param>
        /// <returns>コマンドラインパラメータとして使用可能な文字列。</returns>
        public static string EscapeForCommandLine( this string value )
        {
            /* https://msdn.microsoft.com/ja-jp/library/78f4aasd.aspx#Anchor_0 を元にコマンドラインパラメータ用にエスケープを行う */

            // " 直前の \ リテラルはすべて \\ とし、 " リテラルを \" とする
            value = Regex.Replace( value, @"(\\*)""", @"$1\$&" );

            // 末尾の \ をエスケープ（全体を囲む " がエスケープされてしまうのを防ぐ）
            value = Regex.Replace( value, @"(\\+)$", "$1$1" );

            // 全体を " で囲む
            value = $"\"{ value }\"";

            return value;
        }
    }
}
