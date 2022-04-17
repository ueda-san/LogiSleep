# LogiSleep

ディスプレイの消灯に合わせてロジクールのゲーミングキーボードのバックライトを消灯します。
おまけでディスプレイの消灯防止機能/即座に消灯機能もあります。

- Logitech Gaming Software か GHUB が必要です。
- ゲームなど他のLEDをコントロールするアプリとは同時に利用できません。


## 使い方

x64版を使う場合はReleaseページからダウンロードしてください。
Windows起動時に実行したい場合はショートカットをスタートアップに登録してください。(最初からタスクバーに入れたい場合はショートカットのプロパティの「実行時の大きさ」を「最小化」に)

![LogiSleep3_ja](https://user-images.githubusercontent.com/70194698/163714716-62e6179b-a0bd-4040-b871-cb97ca205c7c.png)


- ディスプレイの電源が切れたときキーボードのLEDを消す にチェックを入れるとディスプレイの消灯に連動してキーボードのバックライトも消えます。
- ディスプレイの電源が切れるのを防止する にチェックを入れると定期的にマウス移動イベントを発生させてディスプレイが消灯するのを防ぎます。
- ディスプレイを消す を押すと即座にディスプレイの電源を切ります。
 
<img width="611" alt="LogiSleep1_ja" src="https://user-images.githubusercontent.com/70194698/163714773-990fbf8d-7f23-44cd-9383-dfc5ce80d187.png">


最小化ボタンを押すとタスクトレイに入ります。
タスクトレイアイコンをダブルクリックすればウィンドウを表示します。

<img width="309" alt="LogiSleep2_ja" src="https://user-images.githubusercontent.com/70194698/163714788-ba30c267-c624-4689-a8b8-1678ac60f899.png">



## ビルド方法

x86版を使いたい場合、または自分でビルドしたい場合はソースコードをビルドして SDK から LogitechLedEnginesWrapper.dll を実行ファイルと同じディレクトリにコピーしてください。


## 動作状況

Windows10 (21H2) / LED_SDK 9.00

|        | LGS 9.04.28 | GHUB 2022.4.250563    |
|--------|-------------|-----------------------|
| G610   | ok          | ok                    |
| G710+  | ok(*1)      | キーボードが認識されない   |
| G15 v2 | NG          | キーボードが認識されない   |

*1 WASD ゾーンの個別ライティングが復元されない.


## ライセンス

MIT ライセンス

Logicool の LEDイルミネーションSDK のライセンスはSDK付属ドキュメントに従います。

https://gaming.logicool.co.jp/ja-jp/innovation/developer-lab.html


