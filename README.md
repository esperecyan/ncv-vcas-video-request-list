NCV　VirtualCast動画リクエスト一覧プラグイン
============================================
コメントでリクエストされた、[VirtualCast] のスタジオで再生可能な動画を一覧管理するための、[NiconamaCommentViewer] 用プラグインです。

[VirtualCast] のユーザーコミュニティ [Vキャスカラオケ部] のメタカラ喫茶における利用を想定。

[VirtualCast]: https://virtualcast.jp/ "バーチャルキャストは、時間や場所に囚われず、全国各地のユーザーと共に非日常な日常を体験できるコミュニケーションサービスです"
[NiconamaCommentViewer]: https://www.posite-c.com/application/ncv/ "NiconamaCommentViewer とは？　• ニコニコ生放送のコメント専用ビューアです　• 放送中番組、タイムシフトともに利用可能です"
[Vキャスカラオケ部]: https://twitter.com/masanyu_vr/status/1447145052271099904 "SYNCROOMを利用した凸待ちカラオケ配信などを行うコミュニティ"

開発を行う場合のフォルダ階層
----------------------------
- 親フォルダ
	+ ncv-vcas-video-request-list
		* NCVVCasVideoRequestList.sln
	+ NCV
		* Plugin.dll
		* NicoLibrary.dll
		* plugins ← 出力先
			- HtmlAgilityPack.dll
			- HtmlAgilityPack - LICENSE
			- NCVVCasVideoRequestList.dll
			- NCVVCasVideoRequestList.URL

ライセンス
---------
当リポジトリ内のコードのライセンスは [Mozilla Public License Version 2.0] \(MPL-2.0) です。  
© 2022 100の人

以下のライブラリを参照 (動的リンク) しています。
- NiconamaCommentViewerに含まれる Plugin.dll、NicoLibrary.dll
	+ Copyright (C) 2009-2022 posite-c
- Html Agility Pack
	+ MIT License (MIT)
	+ Copyright © ZZZ Projects Inc.
	+ https://github.com/zzzprojects/html-agility-pack/blob/v1.11.42/LICENSE

[Mozilla Public License Version 2.0]: https://www.mozilla.org/MPL/2.0/
