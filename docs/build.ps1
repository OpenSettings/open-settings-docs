Param([string]$version = 'v2')

Remove-Item .\_site\* -Force -Recurse

docfx metadata ".\$version\docfx.json"
docfx build    ".\$version\docfx.json"
docfx pdf      ".\$version\docfx.json"

Remove-Item ".\_site\$version\public\*.map" -Force

Copy-Item .\storage\common-script.js .\_site\

Copy-Item .\storage\common-script.js ".\_site\$version\"

Copy-Item .\storage\robots.txt .\_site\

Copy-Item .\v2\favicon.ico .\_site\

Remove-Item ".\..\src\OpenSettings.Docs\wwwroot\$version" -Force -Recurse

Copy-Item -Path .\_site\* -Destination .\..\src\OpenSettings.Docs\wwwroot\ -Recurse

Copy-Item .\storage\index.html .\_site\

docfx serve .\_site