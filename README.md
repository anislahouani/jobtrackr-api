# JobTrackr API

JobTrackr est une API REST développée en ASP.NET Core permettant de gérer le suivi de candidatures.

L'objectif de ce projet était de mettre en pratique les bases du développement backend avec .NET : création d'une API, authentification avec JWT, persistance des données avec Entity Framework Core et PostgreSQL, ainsi que la conteneurisation avec Docker.

## Fonctionnalités

- Création d'un compte utilisateur
- Connexion avec authentification JWT
- Gestion des candidatures (CRUD)
- Validation des données envoyées à l'API
- Documentation interactive avec Scalar/OpenAPI

## Stack technique

- ASP.NET Core 10
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Docker & Docker Compose
- Scalar (OpenAPI)

## Architecture

Le projet est organisé de manière simple afin de séparer les responsabilités :

```
Controllers/
Data/
DTOs/
Enums/
Models/
Services/
Configurations/
```

- **Controllers** : exposent les endpoints de l'API.
- **Services** : contiennent la logique métier.
- **Models** : représentent les entités enregistrées en base.
- **DTOs** : objets utilisés pour les requêtes et les réponses.
- **Data** : configuration d'Entity Framework Core et du DbContext.

## Lancer le projet

### 1. Cloner le dépôt

```bash
git clone https://github.com/VOTRE_PSEUDO/JobTrackr.Api.git
cd JobTrackr.Api
```

### 2. Démarrer PostgreSQL

```bash
docker compose up -d
```

### 3. Appliquer les migrations

```bash
dotnet ef database update
```

### 4. Lancer l'API

```bash
dotnet run
```

L'API est ensuite disponible sur :

```
http://localhost:5159
```

La documentation interactive est accessible à l'adresse :

```
http://localhost:5159/scalar/v1
```

## Authentification

L'API utilise des JSON Web Tokens (JWT).

Le fonctionnement est le suivant :

1. créer un compte (`POST /api/Auth/register`)
2. se connecter (`POST /api/Auth/login`)
3. récupérer le token JWT
4. utiliser ce token avec le schéma Bearer pour accéder aux routes protégées

## Endpoints principaux

### Authentification

| Méthode | Route |
|---------|-------|
| POST | /api/Auth/register |
| POST | /api/Auth/login |

### Candidatures

| Méthode | Route |
|---------|-------|
| GET | /api/job-applications |
| GET | /api/job-applications/{id} |
| POST | /api/job-applications |
| PUT | /api/job-applications/{id} |
| DELETE | /api/job-applications/{id} |

## Ce que j'ai appris

Ce projet m'a permis de découvrir et de mettre en pratique :

- la création d'une API REST avec ASP.NET Core ;
- Entity Framework Core et les migrations ;
- PostgreSQL ;
- l'authentification avec JWT ;
- l'injection de dépendances ;
- Docker pour exécuter la base de données ;
- la documentation d'une API avec OpenAPI/Scalar.

## Améliorations possibles

Quelques pistes d'évolution :

- pagination des candidatures ;
- recherche et filtres ;
- rôles utilisateur (Admin/User) ;
- rafraîchissement des tokens (Refresh Token) ;
- tests unitaires et d'intégration.

---

Développé par **Anis Lahouani**.