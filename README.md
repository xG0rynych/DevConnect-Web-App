# DevConnect

DevConnect to webowa aplikacja zbudowana przy użyciu ASP.NET MVC, która umożliwia deweloperom i entuzjastom technologii komunikację, dzielenie się wiedzą i współpracę. Projekt składa się z dwóch głównych komponentów: aplikacji webowej DevConnect oraz API do wyszukiwania RelevancheSearchAPI.

## Funkcje

### 1. DevConnect (ASP.NET MVC)

- **Rejestracja/Logowanie**: Użytkownicy mogą tworzyć konta i logować się do aplikacji.
- **Autoryzacja i Autentykacja**: Zabezpieczone strony i zasoby dostępne tylko dla zalogowanych użytkowników.
- **Tworzenie Artykułów i Pytań**: Użytkownicy mogą tworzyć nowe artykuły oraz zadawać pytania.
- **Dodawanie Komentarzy**: Możliwość dodawania komentarzy do artykułów i pytań.
- **Usuwanie i Edycja**: Użytkownicy mogą usuwać swoje artykuły/pytania oraz aktualizować profil, w tym dodawać avatar.
- **Realtime Chat**: Użytkownicy mogą komunikować się ze sobą w czasie rzeczywistym przy użyciu SignalR.

### 2. RelevancheSearchAPI (ASP.NET REST API)

- **Wyszukiwanie Relewantnych Treści**: API umożliwia wyszukiwanie artykułów i pytań na podstawie tytułów z użyciem zaawansowanego algorytmu.
- **OpenAI Embedding**: Wykorzystanie API OpenAI do przekształcania tekstu w wektory (embedding).
- **Kosinusowe Similarity**: Wyszukiwanie odbywa się poprzez obliczanie kosinusowego podobieństwa, co pozwala na uzyskanie najbardziej relewantnych wyników.

## Technologie

- **ASP.NET MVC**: Do stworzenia głównej aplikacji webowej.
- **ASP.NET Core Web API**: Do stworzenia API do wyszukiwania treści.
- **Entity Framework**: Do zarządzania bazą danych.
- **SignalR**: Do implementacji komunikacji w czasie rzeczywistym.
- **OpenAI API**: Do generowania embeddingów tekstowych.
- **MSSQL**: Baza danych, w której przechowywane są dane aplikacji.

## Wzorce Projektowe

W projekcie DevConnect zastosowałem kilka wzorców projektowych, aby kod był bardziej modularny, czytelny i łatwiejszy do utrzymania:

- **Fasada (Facade)**: Wzorzec Fasada został użyty do uproszczenia interakcji pomiędzy różnymi modułami aplikacji, szczególnie w przypadku złożonych operacji, które wymagają współpracy wielu komponentów. Dzięki Fasadzie mogłem stworzyć prostszy interfejs dla warstwy biznesowej, co ułatwia zarządzanie złożonością kodu.

- **Repozytorium (Repository)**: Zastosowanie wzorca Repozytorium umożliwiło oddzielenie logiki dostępu do danych od logiki biznesowej. Repozytoria zostały użyte do abstrakcji nad warstwą dostępu do danych, co ułatwia testowanie oraz zmianę źródeł danych bez konieczności modyfikowania logiki aplikacji.

- **Wstrzykiwanie zależności (Dependency Injection, DI)**: Dzięki DI, komponenty aplikacji są bardziej modularne i łatwe do testowania. Wstrzykiwanie zależności pozwala na dynamiczne dostarczanie zależności do obiektów, co zmniejsza ich współzależność i pozwala na łatwą podmianę komponentów (np. w kontekście testów jednostkowych).

---

# DevConnect

DevConnect is a web application built using ASP.NET MVC, designed to facilitate communication, knowledge sharing, and collaboration among developers and technology enthusiasts. The project consists of two main components: the DevConnect web application and the RelevancheSearchAPI.

## Features

### 1. DevConnect (ASP.NET MVC)

- **Registration/Login**: Users can create accounts and log in to the application.
- **Authorization and Authentication**: Secure pages and resources accessible only to logged-in users.
- **Article and Question Creation**: Users can create new articles and ask questions.
- **Commenting**: Users can add comments to articles and questions.
- **Deleting and Editing**: Users can delete their articles/questions and update their profiles, including adding an avatar.
- **Realtime Chat**: Users can communicate with each other in real time using SignalR.

### 2. RelevancheSearchAPI (ASP.NET REST API)

- **Relevant Content Search**: The API enables searching for articles and questions based on titles using an advanced algorithm.
- **OpenAI Embedding**: Utilizes the OpenAI API to transform text into vectors (embedding).
- **Cosine Similarity**: Searches are performed by calculating cosine similarity, which helps to return the most relevant results.

## Technologies

- **ASP.NET MVC**: Used to build the main web application.
- **ASP.NET Core Web API**: Used to build the search API.
- **Entity Framework**: For database management.
- **SignalR**: For implementing real-time communication.
- **OpenAI API**: For generating text embeddings.
- **MSSQL**: The database where the application's data is stored.

## Design Patterns

In the DevConnect project, I applied several design patterns to make the code more modular, readable, and easier to maintain:

- **Facade**: The Facade pattern was used to simplify the interaction between different modules of the application, especially in complex operations requiring cooperation among multiple components. The Facade allows for a simpler interface for the business layer, making it easier to manage code complexity.

- **Repository**: The Repository pattern was implemented to separate data access logic from business logic. Repositories abstract the data access layer, making testing easier and allowing data sources to be changed without modifying the application logic.

- **Dependency Injection (DI)**: DI makes application components more modular and easier to test. Dependency Injection allows dependencies to be dynamically provided to objects, reducing their interdependencies and enabling easy swapping of components (e.g., in the context of unit testing).
