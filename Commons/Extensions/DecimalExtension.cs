using System;

namespace ArceniX.Commons.Extensions
{
    /// <summary>
    /// <see cref="decimal"/> に対する、さまざまな拡張機能を提供します。
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 小数部の値を、指定した桁まで切り捨てます。
        /// </summary>
        /// <param name="d">切り捨てる <see cref="decimal"/></param>
        /// <param name="decimals">切り捨てた結果の数値の小数点以下の桁数を指定する <c>0</c> から <c>28</c> までの値。</param>
        /// <returns>小数点以下の桁数 <paramref name="decimals"/> に切り捨てられた <paramref name="d"/> と等価の <see cref="decimal"/> 数値。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="decimals"/> が <c>0</c> から <c>28</c> までの範囲内の値ではありません。</exception>
        public static decimal RoundDown( this decimal d, int decimals )
        {
            if ( decimals < 0 || 28 < decimals )
            {
                throw new ArgumentOutOfRangeException( "decimals が 0 から 28 までの範囲内の値ではありません。" );
            }

            // カットしない小数桁数を取得
            var save = ( decimal )Math.Pow( 10, decimals );

            return decimal.Truncate( d * save ) / save;
        }

        /// <summary>
        /// 指定された値が正の数（0よりも大きい）かどうかを返します。
        /// </summary>
        /// <param name="d">確認する <see cref="decimal"/> 値。</param>
        /// <returns>値が0より大きい場合、<c>true</c>。それ以外の場合、<c>false</c>。</returns>
        public static bool IsPositive( this decimal d ) => d > decimal.Zero;

        /// <summary>
        /// 整数部の桁数を取得します。
        /// </summary>
        /// <param name="d">桁数を取得する <see cref="decimal"/> 値。</param>
        /// <returns>整数部の桁数。</returns>
        public static int GetScaleIntegerPart( this decimal d )
        {
            var str = d.ToString().TrimStart( '0' );
            var index = str.IndexOf( '.' );

            if ( index == -1 )
            {
                return str.Length;
            }

            return str.Substring( 0, index ).Length;
        }

        /// <summary>
        /// 小数部の桁数を取得します。
        /// </summary>
        /// <param name="d">桁数を取得する <see cref="decimal"/> 値。</param>
        /// <returns>小数部の桁数。</returns>
        public static int GetScaleDecimalPart( this decimal d )
        {
            var str = d.ToString().TrimEnd( '0' );
            var index = str.IndexOf( '.' );

            if ( index == -1 )
            {
                return 0;
            }

            return str.Substring( index + 1 ).Length;
        }
    }
}
