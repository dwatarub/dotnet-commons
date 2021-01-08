using System;
using System.Linq;

namespace ArceniX.Commons.Extensions
{
    /// <summary>
    /// さまざまな拡張機能を提供します。
    /// </summary>
    public static class GenericExtension
    {
        /// <summary>
        /// この値が、指定した範囲に含まれているかどうかを取得します。
        /// </summary>
        /// <param name="value">確認対象となる値。</param>
        /// <param name="from">範囲の開始値。</param>
        /// <param name="to">範囲の終了値。</param>
        /// <returns>値が範囲に含まれている場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool IsRange<T>( this T value, T from, T to ) where T : IComparable => from.CompareTo( value ) <= 0 && value.CompareTo( to ) <= 0;

        /// <summary>
        /// この値が、<paramref name="from"/> 以上 <paramref name="to"/> 未満かどうかを取得します。
        /// </summary>
        /// <param name="value">確認対象となる値。</param>
        /// <param name="from">範囲の開始値。</param>
        /// <param name="to">範囲の終了値。</param>
        /// <returns>値が <paramref name="from"/> 以上 <paramref name="to"/> 未満である場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool IsRangeBelow<T>( this T value, T from, T to ) where T : IComparable => from.CompareTo( value ) <= 0 && value.CompareTo( to ) < 0;

        /// <summary>
        /// この値が、<paramref name="from"/> より大きく <paramref name="to"/> 以下かどうかを取得します。
        /// </summary>
        /// <param name="value">確認対象となる値。</param>
        /// <param name="from">範囲の開始値。</param>
        /// <param name="to">範囲の終了値。</param>
        /// <returns>値が <paramref name="from"/> より大きく <paramref name="to"/> 以下である場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool IsRangeAbove<T>( this T value, T from, T to ) where T : IComparable => from.CompareTo( value ) < 0 && value.CompareTo( to ) <= 0;

        /// <summary>
        /// この値が、<paramref name="from"/> より大きく <paramref name="to"/> 未満かどうかを取得します。
        /// </summary>
        /// <param name="value">確認対象となる値。</param>
        /// <param name="from">範囲の開始値。</param>
        /// <param name="to">範囲の終了値。</param>
        /// <returns>値が <paramref name="from"/> より大きく <paramref name="to"/> 未満である場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool IsRangeAboveAndBelow<T>( this T value, T from, T to ) where T : IComparable => from.CompareTo( value ) < 0 && value.CompareTo( to ) < 0;

        /// <summary>
        /// この値が、指定された <paramref name="candidates"/> のいずれかに一致するかどうかを判定します。
        /// </summary>
        /// <param name="value">確認対象となる値。</param>
        /// <param name="candidates">候補となる値のコレクション。</param>
        /// <returns>コレクションのいずれかに値が一致する場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool MatchAny<T>( this T value, params T[] candidates ) where T : IComparable => candidates.Any( t => t.CompareTo( value ) == 0 );
    }
}
