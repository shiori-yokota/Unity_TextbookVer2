【IronPythonのインストール】
・IronPythonのホームページ (http://ironpython.net/) からIronPython2.7をダウンロードする
　　# Download IronPythonをクリックしていけばmsi版がある
　　# zip版もあるがPythonモジュールが含まれていないためmsi版をダウンロードする
　　# msiは32bitなのでx64環境のWindowsでは(x86)がついたディレクトリに展開する

・WheelDuck/Assets/Pluginsに移動する
　　UnityのアセットにIronPythonを組み込む
　　IronPythonのPlatforms/Net35フォルダにある以下の6つのdllファイルを
　　Unityのアセットに登録（コピー）する
　　　　- IronPython.dll
　　　　- IronPython.Modules.dll
　　　　- Microsoft.Dynamic.dll
　　　　- Microsoft.Scripting.Core.dll
　　　　- Microsoft.Scripting.dll
　　　　- Microsoft.Scripting.Metadata.dll

・WheelDuck/Python/Libに移動する
　　Pythonの標準モジュールを使用するため、
　　IronPythonのLibフォルダ内の全てをコピーする

・Unityのプロジェクト設定を変更する
　　組み込んだIronPythonが動作するようにプロジェクトの設定を変更する
　　　　# Edit -> Project Settings -> Player メニューから
　　　　# PlayerSettings インスペクターを開きOptimization セクションにある
　　　　# Api Compatibility Level を .NET 2.0 に設定する

----------

【Directory & File configuration】
WheelDuck/
   |
   |
   |---- Assets/
   |          |
   |          + Models		IronPythonを組み込む
   |                 |
   |                 + Effects/			迷路の状態を表示するテキスト
   |                 + Environment/
   |                        |
   |                        + Common/	各チャプター共通の環境に関するprefab
   |                        + Wall/		迷路の壁prefab
   |
   |                 + Menu/			お宝の保存先
   |                        |
   |                        + Images/		ボタンとして使用する画像
   |                        + Scripts/		メニュー画面の操作に必要なスクリプト(cs)
   |                        + TextPrefabs/	ゲーム画面に表示する各マスの確率を表示させるprefab
   |
   |                 + Robot/		ロボットのprefab
   |                 + Treasures/	チャプター10に使用するお宝のprefab
   |
   |          + Plugins		IronPythonを組み込む場所
   |          + Scenes		シーンの保存先
   |                 |
   |                 + chapter**/			各チャプターのシーン
   |                        |
   |                        + Scripts/		各チャプターに必要なロボットのコントローラー，モデレーターのスクリプト(cs)
   |
   |                 + Menu/				【このシーンを最初に再生する】
   |
   |---- ProjectSettings/
   |          |
   |          |---- ~~.asset
   |
   |---- Python/
               |
               + Lib/			Pythonの標準モジュールを使用
               + ScreenShots/	チャプター10で撮影した画像の保存先
               + Sources/
                     |
                     + **.py	各章のpythonコード保存先



----------

Assets/Models/Menu/Imagesに使用している画像元
	http://www.defaulticon.com/