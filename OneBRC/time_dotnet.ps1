dotnet build --no-restore -c Release ./src/OneBRC

$sw = [System.Diagnostics.Stopwatch]::StartNew()

./src/OneBRC/bin/release/net9.0/OneBRC.exe ./billion.txt

$sw.Stop()
$sw.Elapsed
