using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace GmlParser
{
    class Program
    {

        /// <summary>
        /// GMLからSQLを生成します。
        /// 
        /// 入力ファイルはe-Statからダウンロードします。
        /// http://e-stat.go.jp/SG2/eStatGIS/page/download.html
        /// [平成２２年国勢調査（小地域）2010/10/01] -> [男女別人口総数及び世帯総数] -> 任意の地域選択し[検索] -> 境界データにて[世界測地系緯度経度・GML形式]
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "/?")
            {
                Console.WriteLine("GML形式の入力ファイルをTransact-SQLに変換し、ファイル出力します。");
                Console.WriteLine("");
                Console.WriteLine("gmlparser [入力ファイル名]");
                Console.WriteLine("例）gmlparser h22ka13101.gml");
                Console.WriteLine("");
                Console.WriteLine("入力ファイルはe-Statからダウンロードします。");
                Console.WriteLine("http://e-stat.go.jp/SG2/eStatGIS/page/download.html");
                Console.WriteLine("[平成２２年国勢調査（小地域）2010/10/01] -> [男女別人口総数及び世帯総数] -> 任意の地域選択し[検索] -> 境界データにて[世界測地系緯度経度・GML形式]");
                Console.WriteLine("");
                Console.WriteLine("出力ファイル名は入力ファイル名.sqlです。");
                return;
            }
            Console.WriteLine("");

            // 引数から入力ファイル名取得
            //var inFile = "\\\\roz\\情報システム\\全国タクシー配車\\市区町村ポリゴン\\町域ポリゴン\\GMLサンプル\\h22ka13101.gml";
            var inFile = args[0];

            // ファイル読み込み
            XElement root;
            try
            {
                root = XElement.Load(inFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("入力ファイルを開けません！");
                Console.WriteLine(e.Message);
                return;
            }

            //namespace
            XNamespace nsGml = "http://www.opengis.net/gml";
            XNamespace nsFme = "http://www.safe.com/gml/fme";
            
            // 丁字の要素をコレクションで取得
            IEnumerable<XElement> nodes;
            try
            {
                nodes = from i in root.Descendants(nsGml + "featureMember") select i;
            }
            catch (Exception e)
            {
                Console.WriteLine("このGMLは本ツールで変換できません！");
                Console.WriteLine(e.Message);
                return;
            }

            // 丁字の要素を一件ずつSQLに変換
            var sb = new StringBuilder();
            foreach (var item in nodes)
	        {
                string comment = null;
                string sql = null;
                
                try
                {
                    var properties = (XElement)item.FirstNode;
                    var AREA = properties.Element(nsFme + "AREA").Value;
                    var PERIMETER = properties.Element(nsFme + "PERIMETER").Value;
                    var KEN = properties.Element(nsFme + "KEN").Value;
                    var CITY = properties.Element(nsFme + "CITY").Value;
                    var KEN_NAME = properties.Element(nsFme + "KEN_NAME").Value;
                    var SITYO_NAME = properties.Element(nsFme + "SITYO_NAME").Value;
                    var GST_NAME = properties.Element(nsFme + "GST_NAME").Value;
                    var CSS_NAME = properties.Element(nsFme + "CSS_NAME").Value;
                    var HCODE = properties.Element(nsFme + "HCODE").Value;
                    var KIHON1 = properties.Element(nsFme + "KIHON1").Value;
                    var KIHON2 = properties.Element(nsFme + "KIHON2").Value;
                    var AREA_MAX_F = properties.Element(nsFme + "AREA_MAX_F").Value;
                    var KIGO_D = properties.Element(nsFme + "KIGO_D").Value;
                    var N_KEN = properties.Element(nsFme + "N_KEN").Value;
                    var N_CITY = properties.Element(nsFme + "N_CITY").Value;
                    var N_C1 = properties.Element(nsFme + "N_C1").Value;
                    var KIGO_E = properties.Element(nsFme + "KIGO_E").Value;
                    var KIGO_I = properties.Element(nsFme + "KIGO_I").Value;
                    var MOJI = properties.Element(nsFme + "MOJI").Value;
                    var SEQ_NO2 = properties.Element(nsFme + "SEQ_NO2").Value;
                    var JINKO = properties.Element(nsFme + "JINKO").Value;
                    var SETAI = properties.Element(nsFme + "SETAI").Value;
                    var KEY_CODE = properties.Element(nsFme + "KEY_CODE").Value;

                    // GML入力 スペース区切り/右回り 例) 緯度 経度 緯度 経度 …
                    // WKT出力 スペースおよびカンマ区切り/左回り 例）経度 緯度,経度 緯度,…
                    var coor = (properties.Descendants(nsGml + "posList")).First().Value.Split(' ');
                    Array.Reverse(coor);
                    var poligon = new StringBuilder();
                    for (int i = 0; i < coor.Count(); i+=2)
                    {
                        poligon.AppendFormat("{0} {1},", coor[i], coor[i + 1]);
                    }
                    comment = string.Format("-- {0} {1} {2}\r\n", SEQ_NO2, GST_NAME, MOJI);
                    sql = string.Format(
                            "INSERT INTO town(AREA, PERIMETER, KEN, CITY, KEN_NAME, SITYO_NAME, GST_NAME, CSS_NAME, HCODE, KIHON1, KIHON2, AREA_MAX_F, KIGO_D, N_KEN, N_CITY, N_C1, KIGO_E, KIGO_I, MOJI, SEQ_NO2, JINKO, SETAI, KEY_CODE, Polygon) VALUES({0}, {1}, N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', {8}, N'{9}', N'{10}', N'{11}', N'{12}', N'{13}', N'{14}', N'{15}', N'{16}', N'{17}', N'{18}', {19}, {20}, {21}, N'{22}', geography::STGeomFromText('POLYGON(({23}))', 4326));\r\n",
                            AREA, PERIMETER, KEN, CITY, KEN_NAME, SITYO_NAME, GST_NAME, CSS_NAME, HCODE, KIHON1, KIHON2, AREA_MAX_F, KIGO_D, N_KEN, N_CITY, N_C1, KIGO_E, KIGO_I, MOJI, SEQ_NO2, JINKO, SETAI, KEY_CODE, poligon.ToString().TrimEnd(',')
                            );
                }
                catch (Exception)
                {
                    throw;
                }

                //Debug.Print(comment);
                //Debug.Print(sql);
                sb.AppendFormat(comment);
                sb.AppendFormat(sql);
                Console.Write(comment);
            }

            // ファイル書き込み
            var outFile = Path.ChangeExtension(inFile, ".sql");
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");
            System.IO.File.WriteAllText(outFile, sb.ToString(), enc);

            Console.WriteLine("");
            Console.WriteLine("完了しました！");
            Console.WriteLine(outFile);

        }
    }
}
