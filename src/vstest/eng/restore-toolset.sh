if [[ "$warn_not_as_error" == *';'* ]]; then
  for property in ${properties[@]+"${properties[@]}"}; do
    if [[ "$property" == "/p:DotNetBuildOrchestrator=true" ]]; then
      # Arcade 9 unescapes the property before its inner Exec; keep that shell command escaped.
      properties+=("/warnnotaserror:${warn_not_as_error//;/,}")
      warn_not_as_error="${warn_not_as_error//;/%253B}"
      break
    fi
  done
fi
