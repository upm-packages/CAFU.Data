# Changelog

## [1.0.0-preview.10] - 2021-01-21

Reference UniRx on GitHub instead of Self-hosted registry

### Changes

- Reference UniRx on GitHub instead of Self-hosted registry
- Upgrade UniTask v2

## [1.0.0-preview.9] - 2020-05-25

Add support to WebGL

### Features

- Use `UnityEngine.PlayerPrefs` (IndexedDB) instead of `System.IO.File` #10 

## [1.0.0-preview.8] - 2020-05-20

Inverse terms between serialize, and deserialize

* Inverse terms between serialize, and deserialize #9 

## [1.0.0-preview.7] - 2020-05-18

Add overload to omit arguments

### Features

- 引数を省略するためのオーバーロードを追加

## [1.0.0-preview.6] - 2020-05-18

Add classes for synchronize

### Features

- 同期処理としてファイルの読み書きを行うためのクラスを追加

## [1.0.0-preview.5] - 2020-05-17

Remove unnecessary dependencies

### Fixes

- Remove unnecessary dependencies #5 

## [1.0.0-preview.4] - 2020-05-17

Fix error on inject multiple times

### Fixes

- #2 利用する Container を正しく SubContainer にする

## [1.0.0-preview.3] - 2020-05-17

Fix error on inject multiple times

### Fixes

-  #2 DataAdapterInstaller を複数箇所で使用すると Inject のエラー

## [1.0.0-preview.2] - 2020-05-16

Add Serialize/Deserialize

### Features

- #1  Support to Serialize/Deserialize between `byte[]` and `T`

## [1.0.0-preview.1] - 2020-05-16

Initial release - Preview.1

### Features

- Implement `IAsyncDataAdapter` to load/save/delete/exists data

