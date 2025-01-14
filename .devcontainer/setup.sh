#!/usr/bin/env bash
if [[ -n $(curl -sf 'http://host.docker.internal:11434' > /dev/null) ]]; then
  curl -fsSL https://ollama.com/install.sh | sh
  ollama serve &
else
  echo "Ollama is running on the host machine"
fi
