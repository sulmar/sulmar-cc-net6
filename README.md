# .NET6
Przykłady ze szkolenia .NET6

## Podstawy

### Komendy CLI
#### Środowisko
- ``` dotnet --version ``` - wyświetlenie aktualnie używanej wersji SDK
- ``` dotnet --list-sdks ``` - wyświetlenie listy zainstalowanych SDK
- ``` dotnet new globaljson ``` - utworzenie pliku _global.json_
- ``` dotnet new globaljson --sdk-version {version} ``` - utworzenie pliku _global.json_ i ustawienie wersji SDK

#### Projekt
- ``` dotnet new --list ``` - wyświetlenie listy dostępnych szablonów
- ``` dotnet new {template} ``` - utworzenie nowego projektu na podstawie wybranego szablonu, np. console, web
- ``` dotnet new {template} -o {output} ``` - utworzenie nowego projektu w podanym katalogu
- ``` dotnet restore ``` - pobranie pakietów nuget powiązanych z projektem
- ``` dotnet build ``` - kompilacja projektu
- ``` dotnet run ``` - uruchomienie projektu
- ``` dotnet watch run ``` - uruchomienie projektu w trybie śledzenia zmian
- ``` dotnet run {app.dll}``` - uruchomienie aplikacji
- ``` dotnet test ``` - uruchomienie testów jednostkowych
- ``` dotnet watch test ``` - uruchomienie testów jednostkowych w trybie śledzenia zmian
- ``` dotnet add {project.csproj} reference {library.csproj} ``` - dodanie odwołania do biblioteki
- ``` dotnet remove {project.csproj} reference {library.csproj} ``` - usunięcie odwołania do biblioteki
- ``` dotnet clean ``` - wyczyszczenie wyniku kompilacji, czyli zawartości folderu pośredniego _obj_ oraz folderu końcowego _bin_

#### Rozwiązanie
- ``` dotnet new sln ``` - utworzenie nowego rozwiązania
- ``` dotnet new sln --name {name} ``` - utworzenie nowego rozwiązania o określonej nazwie
- ``` dotnet sln add {folder}``` - dodanie projektu z folderu do rozwiązania
- ``` dotnet sln remove {folder}``` - usunięcie projektu z folderu z rozwiązania
- ``` dotnet sln add {project.csproj}``` - dodanie projektu do rozwiązania
- ``` dotnet sln remove {project.csproj}``` - usunięcie projektu z rozwiązania


## REST API

| Akcja   | Przeznaczenie  |  
|---|---|
| GET | pobierz |
| POST | utwórz |
| PUT | zamień |
| PATCH | modyfikacja |
| DELETE | usuń |


## Polecane

- React
https://codewithmosh.com/courses/enrolled/357787

- JSON Patch
https://jsonpatch.com

- How to Run C# in VSCode (Compile, Debug, and Create a Project)
https://www.youtube.com/watch?v=DAsyjpqhDp4


## Docker

- _Czemu każdy programista musi znać dockera?_
https://javamaster.it/czemu-kazdy-programista-musi-znac-dockera/

### Polecenia
- ``` docker --version ``` - wyświetlenie wersji 
- ``` docker images ``` - wyświetlenie listy pobranych obrazów
- ``` docker ps ``` - wyświetlenie uruchomionych kontenerów
- ``` docker ps -a ``` - wyświetlenie wszystkich kontenerów

### Seq

- UI
`http://localhost:5341`

#### Docker

`docker run --name cc-seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest`

##### Docker Compose
1. docker-compose.yml

~~~ yaml
version: '3.4'

  seq:
      image: datalust/seq:latest
      ports:
        - "5341:80"
      environment:
        - ACCEPT_EULA=Y
      restart: unless-stopped
      volumes:
        - ./seq-data:/data

~~~

2. `docker-compose up`


## Konfiguracja

### User Secrets
https://www.karltarvas.com/2019/10/28/visual-studio-mac-manage-user-secrets.html

#### Komendy
- ``` dotnet user-secrets init ``` - utworzenie sekretów
- ``` dotnet user-secrets set "{key}" "{value}" ``` - ustawienie wartości klucza
- ``` dotnet user-secrets list ``` - wyświetlenie listy kluczy i wartości
- ``` dotnet user-secrets remove "{key}" ``` - usunięcie wskazanego klucza
- ``` dotnet user-secrets clear ``` - usunięcie wszystkich kluczy

## React.JS

### Utworzenie projektu

1. `npx create-react-app foldername`
2. `cd foldername`
3. `npm start`

