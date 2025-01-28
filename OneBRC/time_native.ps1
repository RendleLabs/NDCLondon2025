$sw = [System.Diagnostics.Stopwatch]::StartNew()

./native/OneBRC.exe ./billion.txt

Write-Output "--------"

$sw.Stop()
$sw.Elapsed
