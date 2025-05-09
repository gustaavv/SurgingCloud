---
title: Source Installation
---

Clone the repository:

```powershell
git clone https://github.com/gustaavv/SurgingCloud.git
cd SurgingCloud
```

Publish the project:

```powershell
dotnet publish ./SurgingCloud.Cli/SurgingCloud.Cli.csproj -c Release -o ./publish/
```

After publishing, you are ready to use `.\publish\SurgingCloud.Cli.exe`