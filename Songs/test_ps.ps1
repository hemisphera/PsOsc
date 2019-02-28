$PsOscEngine.SendOscMessage("/stop")

Start-Sleep -Seconds 5

$PsOscEngine.SendOscMessage("/play")