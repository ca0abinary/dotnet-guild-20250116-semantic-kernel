#!/usr/bin/env bash
if [[ -z $(curl -sf 'http://host.docker.internal:11434') ]]; then
  if [[ -z $(curl -sf 'http://localhost:11434') ]]; then
    curl -fsSL https://ollama.com/install.sh | sh
    ollama serve &
    return
  fi
fi
echo "Ollama is running on the host machine"
