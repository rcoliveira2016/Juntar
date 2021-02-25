# Juntar
 
##instalação

Rodar o seguinte comando nbo powershell

```powershell
Invoke-Expression (New-Object System.Net.WebClient).DownloadString('https://get.scoop.sh')
iwr -useb get.scoop.sh | iex
scoop bucket add my-bucket https://github.com/rcoliveira2016/bucket
scoop update
scoop install juntar
```
Nota: se você receber um erro, pode ser necessário alterar a política de execução (ou seja, habilitar o Powershell) com
```powershell
Set-ExecutionPolicy RemoteSigned -scope CurrentUser
```
