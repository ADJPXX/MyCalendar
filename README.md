# MyCalendar

Aplicação de console em C# que consulta os compromissos do dia no calendário principal do Google Calendar e os exibe em português.

## Funcionalidades

- Autentica uma conta Google usando OAuth 2.0.
- Consulta os eventos do calendário principal (`primary`) para o dia atual, conforme o fuso horário local da máquina.
- Expande eventos recorrentes e ordena os resultados pelo horário de início.
- Exibe horários para eventos com hora marcada e identifica eventos de dia inteiro.
- Mostra `Sem título` quando um evento não tem resumo.

## Tecnologias

- C# e .NET 10 (`net10.0`)
- Google Calendar API, por meio das bibliotecas oficiais `Google.Apis.Calendar.v3` e `Google.Apis.Auth`
- GitHub Actions para publicar manualmente uma versão Windows x64

## Requisitos

- .NET 10 SDK para compilar e executar pelo código-fonte.
- Uma conta Google com acesso ao Google Calendar.
- Um projeto no Google Cloud com a Google Calendar API ativada e um cliente OAuth para aplicativo de desktop.
- Navegador e acesso à internet para a primeira autorização.

## Configurar o acesso ao Google Calendar

1. No [Google Cloud Console](https://console.cloud.google.com/), crie ou selecione um projeto e ative a **Google Calendar API**.
2. Configure a tela de consentimento OAuth. Se o aplicativo estiver no modo de teste, adicione sua conta Google como usuária de teste.
3. Crie um **OAuth client ID** do tipo **Desktop app** e baixe o arquivo JSON de credenciais.
4. Renomeie o arquivo baixado para `credentials.json` e coloque-o no mesmo diretório do executável que será iniciado. Para execução pelo código-fonte, veja os comandos abaixo.
5. Inicie o programa. Na primeira execução, o navegador será aberto para autorizar o acesso. A biblioteca Google armazena o token localmente na pasta configurada como `Token` e o reutiliza nas próximas execuções.

O arquivo `credentials.json` contém dados privados e não deve ser enviado ao repositório ou compartilhado. Ele já está listado no `.gitignore`. O código solicita atualmente o escopo OAuth `Calendar`, que permite acesso amplo ao calendário; a funcionalidade implementada neste projeto apenas consulta eventos.

## Compilar e executar no Windows

Na pasta do repositório, execute no PowerShell:

```powershell
dotnet restore .\MyCalendar.sln
dotnet build .\MyCalendar.sln --configuration Debug
Copy-Item .\credentials.json .\bin\Debug\net10.0\credentials.json
dotnet run --no-build --project .\MyCalendar.csproj
```

O projeto não copia `credentials.json` automaticamente para a pasta de saída. Após recompilar, copie o arquivo novamente para o diretório de saída usado pela configuração (`bin\Debug\net10.0` ou `bin\Release\net10.0`).

## Publicação Windows x64

O fluxo manual em `.github/workflows/release.yml` publica um executável autocontido para Windows x64. Para reproduzir localmente:

```powershell
dotnet publish .\MyCalendar.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  /p:PublishSingleFile=true `
  /p:PublishTrimmed=false `
  --output publish

Copy-Item .\credentials.json .\publish\credentials.json
.\publish\MyCalendar.exe
```

A release do GitHub contém `MyCalendar.exe`, mas não inclui credenciais. Cada usuário precisa fornecer seu próprio `credentials.json` ao lado do executável publicado.

## Estrutura do projeto

```text
MyCalendar/
├── Program.cs                     # Interface de console e impressão dos eventos
├── CalendarManager.cs             # Coordena autenticação e consulta
├── Services/
│   ├── AuthenticationService.cs   # OAuth e criação do serviço Google Calendar
│   └── GoogleCalendarService.cs   # Consulta e conversão dos eventos
├── Models/
│   ├── CalendarEvent.cs           # Modelo de evento exibido
│   └── CalendarInfo.cs            # Modelo definido, ainda não usado no fluxo atual
├── Resources/
│   └── calendar.ico               # Ícone da aplicação
└── .github/workflows/
    └── release.yml                # Publicação manual de release Windows x64
```

## Comportamento e limites atuais

- A consulta cobre somente o calendário principal da conta autenticada.
- A API retorna os resultados em páginas de até 250 eventos por padrão; como a implementação não percorre páginas seguintes, agendas muito cheias podem aparecer incompletas.
- A definição de “hoje” usa a data e o deslocamento de fuso horário local da máquina.
- O programa é interativo: limpa a tela ao mostrar o resultado e aguarda uma tecla antes de fechar.
- Erros de autenticação, rede ou configuração ainda não são apresentados por mensagens tratadas pela aplicação.
