1. Visão Geral
Sistema completo para gestão de clientes seguindo princípios de DDD e CQRS, composto por:

Frontend: Aplicação Angular com Material Design

Backend: API .NET 8 com Entity Framework Core

Banco de Dados: PostgreSQL com Event Sourcing

Infraestrutura: Docker e Docker Compose

2. Requisitos Técnicos
Componente	Especificações
Node.js	v18+
.NET SDK	v8.0
Docker	v24+
PostgreSQL	v16
RAM	4GB+ (8GB recomendado)
3. Configuração do Ambiente
3.1 Repositórios
bash
Copy
git clone https://github.com/metondaniel/cliente-frontend.git
git clone https://github.com/metondaniel/cliente-backend.git
3.2 Variáveis de Ambiente
Criar .env na raiz de cada projeto:

Backend (.env)

ini
Copy
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=clientes;Username=postgres;Password=senha123
Frontend (.env)

ini
Copy
NG_APP_API_URL=http://localhost:5000/api
4. Estrutura do Projeto
4.1 Backend ( .NET 8 )
Copy
src/
├── Services/           # Entidades e regras de negócio
├── Repository/       # Implementações de repositório
├── WebAPI/           # Controladores e configuração
└── tests/            # Testes unitários e de integração
4.2 Frontend ( Angular )
Copy
src/app/
├── core/             # Serviços globais
├── features/         # Módulos funcionais
├── shared/           # Componentes reutilizáveis
└── assets/           # Recursos estáticos
5. Execução Local
5.1 Banco de Dados
bash
Copy
docker run --name clientes-db -e POSTGRES_PASSWORD=senha123 -p 5432:5432 -d postgres:16
5.2 Backend
bash
Copy
cd cliente-backend
dotnet restore
dotnet watch run
5.3 Frontend
bash
Copy
cd cliente-frontend
npm install
ng serve -o
