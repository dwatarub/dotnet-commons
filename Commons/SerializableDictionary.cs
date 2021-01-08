using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ArceniX.Commons
{
    /// <summary>
    /// XML シリアル化および逆シリアル化が可能な <see cref="Dictionary{TKey, TValue}"/> を提供します。
    /// </summary>
    /// <typeparam name="TKey">ディクショナリ内のキーの型。</typeparam>
    /// <typeparam name="TValue">ディクショナリ内の値の型。</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region constructors
        /// <summary>
        /// 空で、既定の初期量を備え、キーの型の既定の等値比較演算子を使用する、<see cref="SerializableDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SerializableDictionary() : base() { }

        /// <summary>
        /// シリアル化したデータを使用して、<see cref="SerializableDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">
        /// <see cref="SerializableDictionary{TKey, TValue}"/> をシリアル化するために必要な情報を格納している <see cref="SerializationInfo"/> オブジェクト。
        /// </param>
        /// <param name="context">
        /// <see cref="SerializableDictionary{TKey, TValue}"/> に関連付けられているシリアル化ストリームのソースおよびデスティネーションを格納している <see cref="StreamingContext"/> 構造体。
        /// </param>
        protected SerializableDictionary( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
        #endregion

        #region methods
        /// <summary>
        /// XML スキーマを取得します。
        /// </summary>
        /// <returns>
        /// <see cref="WriteXml(XmlWriter)"/> メソッドによって生成され
        /// <see cref="ReadXml(XmlReader)"/> メソッドによって処理されるオブジェクトの
        /// XML 表現を記述する <see cref="XmlSchema"/>。
        /// </returns>
        /// <remarks>このメソッドの戻り値は、常に <c>null</c> です。</remarks>
        public XmlSchema GetSchema() => null;

        /// <summary>
        /// オブジェクトの XML 表現からオブジェクトを生成します。
        /// </summary>
        /// <param name="reader">オブジェクトの逆シリアル化元である <see cref="XmlReader"/> ストリーム。</param>
        public void ReadXml( XmlReader reader )
        {
            var serializer = new XmlSerializer( typeof( KeyValue ) );

            reader.Read();
            while ( reader.NodeType != XmlNodeType.EndElement )
            {
                if ( serializer.Deserialize( reader ) is KeyValue kv )
                {
                    Add( kv.Key, kv.Value );
                }
            }
            reader.Read();
        }

        /// <summary>
        /// オブジェクトを XML 表現に変換します。
        /// </summary>
        /// <param name="writer">オブジェクトのシリアル化先の <see cref="XmlWriter"/> ストリーム。</param>
        public void WriteXml( XmlWriter writer )
        {
            var serializer = new XmlSerializer( typeof( KeyValue ) );
            foreach ( var key in Keys )
            {
                serializer.Serialize( writer, new KeyValue( key, this[ key ] ) );
            }
        }
        #endregion

        #region classes
        /// <summary>
        /// 設定または取得できる、キー/値ペアを提供します。
        /// </summary>
        public class KeyValue
        {
            #region constructors
            /// <summary>
            /// <see cref="KeyValue"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public KeyValue()
            { }

            /// <summary>
            /// 指定したキーと値を使用して、<see cref="KeyValue"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public KeyValue( TKey key, TValue value )
            {
                Key = key;
                Value = value;
            }
            #endregion

            #region properties
            /// <summary>
            /// キー/値ペア内のキーを取得または設定します。
            /// </summary>
            public TKey Key { get; set; }

            /// <summary>
            /// キー/値ペア内の値を取得または設定します。
            /// </summary>
            public TValue Value { get; set; }
            #endregion
        }
        #endregion
    }
}