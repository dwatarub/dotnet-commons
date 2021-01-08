using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using static ArceniX.Commons.Extensions.GenericExtension;

namespace ArceniX.RegAsmHelper
{
    class Program
    {
        private static readonly string _regasmPath = Path.Combine( RuntimeEnvironment.GetRuntimeDirectory(), "RegAsm.exe" );
        private static readonly FileInfo _asmInfo = new FileInfo( Assembly.GetExecutingAssembly().Location );

        private const string PROCESS_REGISTER = "--register";
        private const string PROCESS_UNREGISTER = "--unregister";

        private static readonly string[] _processTypes = { PROCESS_REGISTER, PROCESS_UNREGISTER };

        static void Main( string[] args )
        {
            try
            {
                if ( args.Length < 2 )
                {
                    throw new ArgumentException();
                }

                // 第一パラメータ：処理区分
                var processType = args[ 0 ];
                if ( !processType.MatchAny( _processTypes ) )
                {
                    throw new ArgumentException();
                }

                // 第二パラメータ：対象DLL
                var targetDll = args[ 1 ];
                if ( !File.Exists( targetDll ) )
                {
                    throw new FileNotFoundException( "処理対象のDLLファイルが見つかりません。" );
                }

                if ( !File.Exists( _regasmPath ) )
                {
                    throw new FileNotFoundException( "RegAsm.exe が見つかりません。 .NET Frameworkが正しくインストールされているか確認してください。" );
                }

                // RegAsm.exe用にパラメータを整形
                var parameters = new string[] { };
                switch ( processType )
                {
                    case PROCESS_REGISTER:
                        parameters = new[] { targetDll, "/nologo", "/tlb", "/codebase" };
                        break;
                    case PROCESS_UNREGISTER:
                        parameters = new[] { targetDll, "/nologo", "/u" };
                        break;
                }

                Execute( _regasmPath, string.Join( " ", parameters ) );
            }
            catch ( ArgumentException )
            {
                Console.WriteLine( GetUsage() );
            }
            catch ( FileNotFoundException e )
            {
                Console.Error.WriteLine( e.Message );
            }
        }

        private static string GetUsage()
        {
            var sb = new StringBuilder();
            sb.AppendLine( $"RegAsm.exeのパスを検索し、処理をラップします。" );
            sb.AppendLine( $"Usage: {_asmInfo.Name} <option> <filepath>" );
            sb.AppendLine();
            sb.AppendLine( $"Options:" );
            sb.AppendLine( $"  {_processTypes[ 0 ].PadRight( _processTypes.Max( s => s.Length ) )}  アセンブリを登録します。" );
            sb.AppendLine( $"  {_processTypes[ 1 ].PadRight( _processTypes.Max( s => s.Length ) )}  アセンブリの登録を解除します。" );
            sb.AppendLine();
            sb.AppendLine( $"RegAsm.exe: {_regasmPath}" );

            return sb.ToString();
        }

        private static int Execute( string p, string arg )
        {
            /********************** 外部プロセス実行 ***********************/
            var proc = new Process()
            {
                // 実行プログラムとパラメータ
                StartInfo = new ProcessStartInfo()
                {
                    FileName = p,

                    Arguments = arg,

                    // ウィンドウ非表示
                    CreateNoWindow = true,
                    UseShellExecute = false,

                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            proc.OutputDataReceived += ( sender, e ) =>
            {
                Console.WriteLine( e.Data );
                Debug.WriteLine( e.Data, "StdOut" );
            };
            // 標準エラーデータ受信イベント（デバッグ用）
            proc.ErrorDataReceived += ( sender, e ) =>
            {
                Console.Error.WriteLine( e.Data );
                Debug.WriteLine( e.Data, "Error" );
            };

            // 実行
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            // OCR部品は同期実行
            proc.WaitForExit();

            var exitCode = proc.ExitCode;

            // プロセスを終了させる
            proc.Close();

            return exitCode;
        }
    }
}
