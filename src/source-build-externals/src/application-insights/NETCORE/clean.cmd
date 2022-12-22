del /q %TEMP%\VisualStudioTestExplorerExtensions\*
for /d %%x in (%TEMP%\VisualStudioTestExplorerExtensions\*) do @rd /s /q "%%x"