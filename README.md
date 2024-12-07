# API Animes - Teste Técnico ProTech

Este projeto é uma API para gerenciar animes, desenvolvida como parte de um teste técnico para a ProTech.

## 🚀 Começando

Estas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento.

### 📋 Pré-requisitos

Certifique-se de ter os seguintes itens instalados:

- [Docker](https://www.docker.com/get-started)
- [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)

### 🚀 Clonando o Repositório

Para começar, clone este repositório usando o seguinte comando:

```bash
git clone https://github.com/SeuUsuario/SeuRepositorio.git
cd SeuRepositorio
```

## ⚙️ Execução

Para executar o projeto é necessario executar o seguinte comando no diretorio raiz

```
Docker compose up -d --build
```

### ⚙️ Configurações Opcionais

No arquivo `docker-compose.yml`, você pode ajustar variáveis como senhas e chaves de API para personalizar a configuração:

```yaml
db:
  ...
  environment:
    - SA_PASSWORD=YourStrongPassword123! # Altere para uma senha segura
  ...
api:
  ...
  environment:
    - DB_PASSWORD=YourStrongPassword123! # Certifique-se de que coincide com o banco de dados
    - API_KEY=YourApiKey # Substitua por sua chave de API personalizada
  ...

```

### 📋 Documentação

A documentação da API pode ser acessada após iniciar o projeto. Abra o navegador e vá para:

```bash
http://localhost:5000/swagger/index.html
```

## 🧪 Testes

Este projeto inclui testes unitários utilizando XUnit e Moq. Para executar os testes, use o comando:

```bash
dotnet test
```

## 🛠️ Construído com

- [.NET 9](https://dotnet.microsoft.com/pt-br/)
- [Docker](https://hub.docker.com/)
- [SqlServer](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [XUnit](https://xunit.net/)
- [Moq](https://www.nuget.org/packages/moq/)
- [Swagger](https://swagger.io/)
- [EF Core](https://learn.microsoft.com/pt-br/ef/core/)


## ✒️ Autores

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/YuriGarciaRibeiro" title="GitHub">
        <img src="https://avatars.githubusercontent.com/u/81641949?v=4" width="100px;" alt="Foto do Iuri Silva no GitHub"/><br>
        <sub>
          <b>Yuri Garcia</b>
        </sub>
      </a>
    </td>
  </tr>
</table>

## 📝 Licença

Este projeto está sob a licença [MIT](LICENSE). Veja o arquivo `LICENSE` para mais detalhes.

