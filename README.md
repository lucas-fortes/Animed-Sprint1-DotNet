# Animed API - .NET

API REST desenvolvida em **ASP.NET Core Web API** para o projeto Animed, dentro do contexto do Challenge CLYVO VET.

O objetivo da API é fornecer uma camada backend para gerenciamento de tutores, pets, consultas e vacinas, utilizando arquitetura em camadas, Entity Framework Core e persistência em banco de dados Oracle.

---

## Integrantes

- Erick Bernardes Bradaschia - RM 565733
- Gabriel Santos Claudino - RM 564054
- Kaiky de Oliveira Silva - RM 566067
- Lucas Fortes de Lima - RM 559523
- Jonathan Moreira Gomes - RM 565060

---

## Tecnologias Utilizadas

- C#
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- Oracle Database
- Oracle.EntityFrameworkCore
- Swagger / OpenAPI
- Visual Studio Community
- VS Code

---

## Arquitetura do Projeto

O projeto segue uma estrutura em camadas:

```text
Animed-Sprint1-DotNet/
├── Animed.slnx
├── dotnet-tools.json
├── README.md
└── AnimedApi/
    ├── Controllers/
    ├── Data/
    ├── Dtos/
    ├── Models/
    ├── Repositories/
    ├── Services/
    ├── Program.cs
    ├── appsettings.json
    └── AnimedApi.csproj
```

### Camadas

- `Controllers`: recebem as requisições HTTP e retornam as respostas da API.
- `Services`: concentram regras de negócio, validações e chamadas aos repositórios.
- `Repositories`: realizam o acesso aos dados utilizando Entity Framework Core.
- `Models`: representam as entidades persistidas no banco de dados.
- `Dtos`: representam os objetos de entrada e saída da API.
- `Data`: contém o `AnimedDbContext`, responsável pela configuração do banco.

---

## Entidades Principais

A API possui quatro entidades principais:

- Tutor
- Pet
- Consulta
- Vacina

### Tutor

Representa o responsável pelo pet.

Campos principais:

- Id
- Nome
- CPF
- Email
- Telefone
- CriadoEm

### Pet

Representa o animal vinculado a um tutor.

Campos principais:

- Id
- Nome
- Espécie
- Raça
- Idade
- Peso
- TutorId
- CriadoEm

### Consulta

Representa uma consulta clínica realizada para um pet.

Campos principais:

- Id
- DataConsulta
- Motivo
- Diagnóstico
- Tratamento
- Observações
- NivelUrgencia
- TutorId
- PetId
- CriadoEm

### Vacina

Representa uma vacina aplicada em um pet.

Campos principais:

- Id
- Nome
- DataAplicacao
- DataProximaDose
- Observações
- PetId
- CriadoEm

---

## Banco de Dados

A API utiliza **Oracle Database** com **Entity Framework Core**.

A configuração do banco está no `Program.cs`:

```csharp
builder.Services.AddDbContext<AnimedDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("AnimedDatabase"));
});
```

A connection string fica configurada em `appsettings.json`, porém sem senha real:

```json
{
  "ConnectionStrings": {
    "AnimedDatabase": "User Id=SEU_USUARIO_ORACLE;Password=SENHA_CONFIGURADA_NO_USER_SECRETS;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

A senha real deve ser configurada localmente usando **User Secrets**, para evitar expor credenciais no GitHub.

---

## Configuração do User Secrets

Dentro da pasta `AnimedApi`, execute:

```powershell
dotnet user-secrets init
```

Depois configure a connection string real:

```powershell
dotnet user-secrets set "ConnectionStrings:AnimedDatabase" "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
```

Observação: a senha real do banco não deve ser commitada no repositório.

---

## Como Executar o Projeto

### 1. Clonar o repositório

```powershell
git clone https://github.com/lucas-fortes/Animed-Sprint1-DotNet.git
```

### 2. Entrar na pasta do projeto

```powershell
cd Animed-Sprint1-DotNet
```

### 3. Restaurar dependências

```powershell
dotnet restore
```

### 4. Compilar o projeto

```powershell
dotnet build
```

### 5. Entrar na pasta da API

```powershell
cd AnimedApi
```

### 6. Executar a API

```powershell
dotnet run
```

### 7. Acessar o Swagger

Abra no navegador:

```text
http://localhost:5073/swagger
```

A porta pode variar conforme o ambiente local. Caso seja diferente, utilize a porta exibida no terminal após executar `dotnet run`.

---

## Migrations

Para criar uma migration:

```powershell
dotnet tool run dotnet-ef migrations add CriacaoInicialOracle
```

Para aplicar as alterações no banco Oracle:

```powershell
dotnet tool run dotnet-ef database update
```

---

## Endpoints da API

### Tutores

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tutores` | Lista todos os tutores |
| GET | `/api/tutores/{id}` | Busca um tutor por ID |
| POST | `/api/tutores` | Cria um novo tutor |
| PUT | `/api/tutores/{id}` | Atualiza um tutor existente |
| DELETE | `/api/tutores/{id}` | Remove um tutor |

### Pets

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/pets` | Lista todos os pets |
| GET | `/api/pets/{id}` | Busca um pet por ID |
| GET | `/api/pets/tutor/{tutorId}` | Lista pets vinculados a um tutor |
| POST | `/api/pets` | Cria um novo pet |
| PUT | `/api/pets/{id}` | Atualiza um pet existente |
| DELETE | `/api/pets/{id}` | Remove um pet |

### Consultas

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/consultas` | Lista consultas |
| GET | `/api/consultas/{id}` | Busca uma consulta por ID |
| GET | `/api/consultas?tutorId=&petId=&data=` | Filtra consultas por tutor, pet ou data |
| POST | `/api/consultas` | Cria uma nova consulta |
| PUT | `/api/consultas/{id}` | Atualiza uma consulta existente |
| DELETE | `/api/consultas/{id}` | Remove uma consulta |

### Vacinas

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/vacinas` | Lista vacinas |
| GET | `/api/vacinas/{id}` | Busca uma vacina por ID |
| GET | `/api/vacinas?petId=` | Filtra vacinas por pet |
| POST | `/api/vacinas` | Cria uma nova vacina |
| PUT | `/api/vacinas/{id}` | Atualiza uma vacina existente |
| DELETE | `/api/vacinas/{id}` | Remove uma vacina |

---

## Exemplos de Requisição

### Criar Tutor

```json
{
  "nome": "Ana Silva",
  "cpf": "12345678995",
  "email": "ana.silva@email.com",
  "telefone": "11999991000"
}
```

### Criar Pet

```json
{
  "nome": "Cookie",
  "especie": "Canina",
  "raca": "Golden Retriever",
  "idade": 4,
  "peso": 21.5,
  "tutorId": 1
}
```

### Criar Consulta

```json
{
  "dataConsulta": "2026-05-24T20:30:00",
  "motivo": "Check-up geral",
  "diagnostico": "Animal em bom estado geral",
  "tratamento": "Manter rotina de acompanhamento",
  "observacoes": "Retorno recomendado em 30 dias",
  "nivelUrgencia": "Baixa",
  "tutorId": 1,
  "petId": 1
}
```

### Criar Vacina

```json
{
  "nome": "Vacina V10",
  "dataAplicacao": "2026-05-24T20:40:00",
  "dataProximaDose": "2027-05-24T20:40:00",
  "observacoes": "Aplicação anual preventiva",
  "petId": 1
}
```

---

## Códigos HTTP Utilizados

A API utiliza retornos HTTP adequados para operações REST:

- `200 OK`: consulta realizada com sucesso;
- `201 Created`: recurso criado com sucesso;
- `204 No Content`: recurso removido com sucesso;
- `400 Bad Request`: erro de validação;
- `404 Not Found`: recurso não encontrado.

---

## Validações Implementadas

A API possui validações nas camadas de serviço, incluindo:

- obrigatoriedade de nome do tutor;
- CPF do tutor com 11 números;
- obrigatoriedade de e-mail e telefone do tutor;
- obrigatoriedade de nome e espécie do pet;
- idade do pet não negativa;
- peso do pet maior que zero;
- validação de existência do tutor antes de cadastrar pet;
- validação de existência do tutor e pet antes de cadastrar consulta;
- validação de vínculo entre pet e tutor na consulta;
- validação de existência do pet antes de cadastrar vacina.

---

## Organização REST

A API foi organizada utilizando padrão REST, com separação dos métodos HTTP:

- `GET` para consultas;
- `POST` para criação;
- `PUT` para atualização;
- `DELETE` para remoção.

As rotas seguem o padrão:

```text
/api/tutores
/api/pets
/api/consultas
/api/vacinas
```

---

## Observações de Segurança

- A senha do banco Oracle não deve ser enviada para o GitHub.
- A connection string real deve ser configurada localmente usando User Secrets.
- Arquivos locais como `.vs/`, `bin/`, `obj/`, `appsettings.Development.json` e arquivos de usuário são ignorados pelo `.gitignore`.

---

## Status da Entrega

```text
API REST criada com ASP.NET Core.
CRUD implementado para Tutores, Pets, Consultas e Vacinas.
Entity Framework Core configurado.
Persistência configurada com Oracle Database.
Swagger/OpenAPI habilitado.
Arquitetura organizada em Controllers, Services, Repositories, DTOs, Models e Data.
Retornos HTTP adequados implementados.
Validações principais implementadas nas Services.
```