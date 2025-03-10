#  ![411356948-a393ccbc-41cb-423b-8b25-cc4464ff366c](https://github.com/user-attachments/assets/4afec596-909c-41c6-8f9b-b4118b2fe1c8)

`Lichado` : A License Checker for Azure DevOps

![411356947-a2fc42ca-f263-42d6-90a6-5f6594e9b862](https://github.com/user-attachments/assets/8a90e15b-1061-4588-b8bc-ec9a75255998)


![Static Badge](https://img.shields.io/badge/written_in-C%23-blue)
![Static Badge](https://img.shields.io/badge/release-v.1.0.0-purple)
![Static Badge](https://img.shields.io/badge/github_contributors-2-green)

**`Lichado`** helps to check and update licenses for Azure DevOps:

## Collaborate with us 👋

* Feel free to give us your **feedback**, make **suggestions** or let us discuss in our 📢 [**discussions tab**](../../discussions/).
* To report **issues** or search for existing issues go to the 🔎 [**issues tab**](../../issues/).

_And please star this repo ⭐_

## Use Cases

### List Azure DevOps Licenses

```
lichado list --org ORG_NAME --pat PERSONAL_ACCESS_TOKEN
```

<img width="815" alt="Lichado List" src="https://github.com/user-attachments/assets/acf1b630-1d67-41ac-a325-d6b7e0094d97" />



### Update Azure DevOps Account License Types from stakeholder to express

```
lichado update --org ORG_NAME --pat PERSONAL_ACCESS_TOKEN --license LIC_FROM --target LIC_TO
```

<img width="824" alt="Lichado Update" src="https://github.com/user-attachments/assets/90012b81-91f5-4742-a75d-947d7349ba05" />


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


## Troubleshooting
