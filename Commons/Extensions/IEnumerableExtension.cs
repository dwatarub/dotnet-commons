using System;
using System.Collections.Generic;
using System.Linq;

namespace ArceniX.Commons.Extensions
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> インターフェースへの拡張機能を提供します。
    /// </summary>
    public static class IEnumerableExtension
    {
        #region methods
        /// <summary>
        /// <see cref="double"/> 値のシーケンスの中央値を取得します。
        /// </summary>
        /// <param name="source">中央値取得の対象となる <see cref="double"/> 値のシーケンス。</param>
        /// <returns>値のシーケンスの中央値。</returns>
        public static double Median( this IEnumerable<double> source )
        {
            if ( source.Count() == 0 )
            {
                throw new InvalidOperationException( "空の IEnumerable に対して、中央値をとることは出来ません。" );
            }

            var sortedList = source.OrderBy( k => k );

            var index = sortedList.Count() / 2;

            var median = default( double );
            if ( sortedList.Count() % 2 == 0 )
            {
                // IEnumerableが偶数個
                median = ( sortedList.ElementAt( index ) + sortedList.ElementAt( index - 1 ) ) / 2;
            }
            else
            {
                // IEnumerableが奇数個
                median = sortedList.ElementAt( index );
            }

            return median;
        }

        /// <summary>
        /// 入力シーケンスの各要素に対して変換関数を呼び出して取得する <see cref="double"/> 値のシーケンスの中央値を取得します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> の要素の型。</typeparam>
        /// <param name="source">中央値の取得の対象となる値のシーケンス。</param>
        /// <param name="selector">各要素に適用する変換関数。</param>
        /// <returns>値のシーケンスの中央値。</returns>
        public static double Median<TSource>( this IEnumerable<TSource> source, Func<TSource, double> selector ) => source.Select( element => selector( element ) ).Median();
        #endregion
    }
}
