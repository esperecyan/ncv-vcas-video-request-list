<#
.SYNOPSIS
	GitHub Releaseページへのアップロード用に、ビルド後のファイルを、ZIPアーカイブへまとめます。
.DESCRIPTION
	ZIPファイルの出力先は、デスクトップです。
	ZIPファイル名は、ビルドしたファイルの出力先フォルダ名です。
	同一パスのファイルがあった場合は上書きします。
.PARAMETER TargetDir
	ビルドしたファイルの出力先フォルダのパス。
#>
param([string]$TargetDir)

$OUTPUT_FILE_NAMES = @(
	'Csv - LICENSE',
	'Csv.dll',
	'HtmlAgilityPack - LICENSE',
	'HtmlAgilityPack.dll',
	'NCVVCasVideoRequestList.dll',
	'NCVVCasVideoRequestList.URL'
)

$DESTINATION_FOLDER_PATH = [Environment]::GetFolderPath('Desktop')

Compress-Archive `
	-LiteralPath ($OUTPUT_FILE_NAMES | ForEach-Object { Join-Path $TargetDir $_ } ) `
	-DestinationPath (Join-Path $DESTINATION_FOLDER_PATH ((Split-Path $TargetDir -Leaf) + '.zip')) `
	-Force
