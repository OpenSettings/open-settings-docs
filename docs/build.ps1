Remove-Item .\_site\* -Force -Recurse

# docfx metadata .\v1\docfx.json

docfx build .\v1\docfx.json

Remove-Item .\_site\v1\public\*.map -Force

Copy-Item .\storage\common-script.js .\_site\

Copy-Item .\storage\common-script.js .\_site\v1\

Copy-Item .\storage\robots.txt .\_site\

Copy-Item .\v1\favicon.ico .\_site\

Remove-Item .\..\src\OpenSettings.Docs\wwwroot\* -Force -Recurse

Copy-Item -Path .\_site\* -Destination .\..\src\OpenSettings.Docs\wwwroot\ -Recurse

Copy-Item .\storage\index.html .\_site\

docfx serve .\_site