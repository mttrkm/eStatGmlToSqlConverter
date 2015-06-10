# eStatGmlToSqlConverter
e-Statで公開されている国税調査（小地域）GMLをMSSQL向けのINSERT文に変換します。境界を示すposListはWKT形式に変換します。

本ツールは、Visual Studio 2013で作成したコンソールアプリケーションです。 GML形式の入力ファイルをTransact-SQLに変換し、ファイル出力します。


$> eStatGmlToSqlConverter 入力ファイル名

例）$> eStatGmlToSqlConverter h22ka13101.gml

入力ファイルはe-StatのWebサイトからダウンロードできます。
http://e-stat.go.jp/SG2/eStatGIS/page/download.html

[平成２２年国勢調査（小地域）2010/10/01] -> [男女別人口総数及び世帯総数] -> 任意の地域選択し[検索] -> 境界データにて[世界測地系緯度経度・GML形式]

入力データの利用にあたっては、e-StatのWebサイトの[ご利用にあたって]をよくお読みください。 http://www.e-stat.go.jp/
