# <img src='assets/Lichado_Logo.png' height=40> &nbsp; `Lichado`: A License Checker for Azure DevOps 


![Lichado Banner](assets/Lichado_Banner.png)


![Static Badge](https://img.shields.io/badge/written_in-C%23-blue)
![Static Badge](https://img.shields.io/badge/release-v.1.0.0-purple)
![Static Badge](https://img.shields.io/badge/github_contributors-2-green)

**`Lichado`** helps to check and update licenses for Azure DevOps:

## Collaborate with us 👋

* To report **issues** or search for existing issues go to the 🔎 [**issues tab**](../../issues/).

_And please star this repo ⭐_

## Use Cases

### List Azure DevOps Licenses

```
lichado list --org ORG_NAME --pat PERSONAL_ACCESS_TOKEN
```

<img width="815" alt="Lichado List" src="assets/Lichado_List.png" />



### Update Azure DevOps Account License Types from stakeholder to express

```
lichado update --org ORG_NAME --pat PERSONAL_ACCESS_TOKEN --license LIC_FROM --target LIC_TO
```

<img width="824" alt="Lichado Update" src="assets/Lichado_Update.png" />


### Update Azure DevOps Account License Types, but exclude users from updates which are defined in exclude.json

```
lichado update --org ORG_NAME --pat PERSONAL_ACCESS_TOKEN --license LIC_FROM --target LIC_TO --file exclude.json
```

exclude.json:

```json
{
  "Users": ["fred@whiteduck.de", "user01@outlook.de"]
}
```

## Installation

### Homebrew (macOS, Linux)
 - Install `lichado` with `brew install whiteducksoftware/tap/lichado`

### Binary (Windows)
1. **Download**  
   - Grab the latest `lichado.exe` from the [releases page](https://github.com/whiteducksoftware/azure-devops-license-checker/releases).
   - Extract the binary from the zip folder

2. **Create a “Lichado” folder**  
   The "Program" folder is a suitable option (but put it anywhere you like)
   ```powershell
   mkdir Lichado

3. **Copy the binary**
   ```powershell
   copy .\path\to\lichado.exe C:\path\to\Lichado

4.	**Add C:\path\to\Lichado to your PATH**
	1.	Press **Win+R**, type sysdm.cpl and hit **Enter**.
	2.	Go to **Advanced** → **Environment Variables…**
	3.	Under **User variables** (or **System variables**), select **Path**, then click **Edit…**
	4.	Click **New** and enter:
    ```
    C:\path\to\Lichado
  5. Click **OK** on all dialogs to save.

5. **Restart your terminal**
   Close and reopen any Command Prompt or PowerShell windows.

6. **Verify installation**
   ```powershell
   where lichado
   lichado --help
