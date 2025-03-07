# docfx metadata .\v1\docfx.json

docfx build .\v1\docfx.json

Remove-Item .\_site\v1\public\*.map -Force

Copy-Item .\common-script.js .\_site\

Copy-Item .\common-script.js .\_site\v1\

Remove-Item .\..\src\OpenSettings.Docs\wwwroot\* -Force -Recurse

Copy-Item -Path .\_site\* -Destination .\..\src\OpenSettings.Docs\wwwroot\ -Recurse

docfx serve .\_site