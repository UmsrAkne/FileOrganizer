# FileOrganizer

ファイル整理のための自作ツール。

## 使い方

アプリを立ち上げたら、エクスプローラーからファイルをドラッグアンドドロップ。

再度ドラッグアンドドロップを行うと、現在の情報は完全に消去され、新しく DD したファイルリストに切り替わる。

## 操作方法

- キー操作
    - j : 下キー相当
    - k : 上キー相当
    - u : ページアップ相当
    - d : ページダウン相当
    - h : ページアップ相当
    - l : ページダウン相当
    - i : ファイルをカウントするかどうかをトグル。
    - m : ファイルにマークを付ける / 外す。
    - n : 次のマークにジャンプ
    - m : 前のマークにジャンプ
    - Enter : 選択中のファイルがサウンドファイルの場合は再生する。

- マウス操作
    - ファイルをダブルクリックした際、サウンドファイルなら再生する。

## 機能一覧

- メニューバー
    - 表示
        - i でカウントから除外したファイルを表示
        - i でカウントから除外したファイルを非表示

    - 編集
        - i でカウントから除外したファイルにプレフィックスをつける。  
        問答無用でファイルをリネームするので実行時は注意。  
        プレフィックスは "ignore_"