using System;
using System.Linq;

namespace ArceniX.Commons.Extensions
{
    /// <summary>
    /// 列挙体の各メンバに設定した属性を取得するための拡張機能を提供します。
    /// </summary>
    public static class EnumExtension
    {
        #region methods
        /// <summary>
        /// <c>Code</c> 属性を取得します。
        /// </summary>
        public static int GetCode( this Enum value ) => value.GetAttribute<CodeAttribute>()?.Code ?? -1;

        /// <summary>
        /// <c>Extension</c> 属性を取得します。
        /// </summary>
        public static string GetExtension( this Enum value ) => value.GetAttribute<ExtensionAttribute>()?.Extension ?? string.Empty;

        /// <summary>
        /// <c>AlternateName</c> 属性を取得します。
        /// </summary>
        public static string GetAlterName( this Enum value ) => value.GetAttribute<AlternateNameAttribute>()?.AlternateName ?? string.Empty;

        /// <summary>
        /// 特定の属性を取得します。
        /// </summary>
        /// <typeparam name="TAttribute">取得する属性型</typeparam>
        private static TAttribute GetAttribute<TAttribute>( this Enum value ) where TAttribute : Attribute
        {
            //リフレクションを用いて列挙体の型から情報を取得
            var fieldInfo = value.GetType().GetField( value.ToString() );
            //指定した属性のリスト
            var attributes = fieldInfo?.GetCustomAttributes( typeof( TAttribute ), false ).Cast<TAttribute>();

            //属性がなかった場合、空を返す
            if ( ( attributes?.Count() ?? 0 ) <= 0 )
            {
                return null;
            }

            //同じ属性が複数含まれていても、最初のみ返す
            return attributes.First();
        }
        #endregion

        #region classes
        /// <summary>
        /// <see cref="Enum"/> の付加情報として設定可能な <c>Code</c> 属性を指定します。このクラスは継承できません。
        /// </summary>
        [AttributeUsage( AttributeTargets.Field, Inherited = false, AllowMultiple = false )]
        public sealed class CodeAttribute : Attribute
        {
            #region constructors
            /// <summary>
            /// 指定されたコード値で <see cref="CodeAttribute"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="code">設定する <see cref="Code"/> 値。</param>
            public CodeAttribute( int code ) => Code = code;
            #endregion

            #region properties
            /// <summary>
            /// コード値を取得します。
            /// </summary>
            public int Code { get; }
            #endregion
        }

        /// <summary>
        /// <see cref="Enum"/> の付加情報として設定可能な <c>Extension</c> 属性を指定します。このクラスは継承できません。
        /// </summary>
        public sealed class ExtensionAttribute : Attribute
        {
            #region constructors
            /// <summary>
            /// 指定された文字列で <see cref="ExtensionAttribute" /> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="extension">設定する <see cref="Extension"/> 値。</param>
            public ExtensionAttribute( string extension ) => Extension = extension;
            #endregion

            #region properties
            /// <summary>
            /// 拡張子を取得します。
            /// </summary>
            public string Extension { get; }
            #endregion
        }

        /// <summary>
        /// <see cref="Enum"/> の付加情報として設定可能な <c>AlternateName</c> 属性を指定します。このクラスは継承できません。
        /// </summary>
        public sealed class AlternateNameAttribute : Attribute
        {
            #region constructors
            /// <summary>
            /// 指定された文字列で <see cref="AlternateNameAttribute" /> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="alternateName">設定する <see cref="AlternateName"/> 値。</param>
            public AlternateNameAttribute( string alternateName ) => AlternateName = alternateName;
            #endregion

            #region properties
            /// <summary>
            /// 表示名を取得します。
            /// </summary>
            public string AlternateName { get; }
            #endregion
        }
        #endregion
    }
}
