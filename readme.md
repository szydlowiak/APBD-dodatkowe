***APBD - dodatkowe zadanie***
---
---
## Wymagania funkcjonalne

**1. Utworzenie nowego wydarzenia**
- Wprowadzenie danych: tytuł, opis, data, maksymalna liczba uczestników.
- Data wydarzenia nie może być przeszła.

**2. Przypisanie prelegenta do wydarzenia**
- Możliwość przypisania wielu prelegentów do jednego wydarzenia.
- Prelegent nie może być przypisany do dwóch wydarzeń w tym samym czasie.

**3. Rejestracja uczestnika na wydarzenie**
- Sprawdzenie limitu miejsc – jeśli limit osiągnięty, rejestracja niemożliwa.
- Uczestnik może być zarejestrowany tylko raz na dane wydarzenie.

**4. Anulowanie rejestracji uczestnika**
- Uczestnik może anulować swój udział do 24 godzin przed rozpoczęciem wydarzenia.

**5. Pobranie listy wydarzeń z informacją o wolnych miejscach**
- Endpoint powinien zwracać wszystkie nadchodzące wydarzenia wraz z:
    - nazwami prelegentów,
    - liczbą zarejestrowanych uczestników,
    - liczbą wolnych miejsc.

**6. Wygenerowanie raportu udziału uczestników**
- Dla danego uczestnika zwróć wszystkie wydarzenia, w których brał udział, z datami i nazwiskami prelegentów.
---
## Realizacja

*Wykorzystano Swagger do ułatwienia testów*

*W bazie znajdują się już przykładowe dane*

*Przy operacjach POST nie sa zwracane payload'y JSON, lecz zwykłe informacje typu "Cancelled", "Registered"*

*Dodatkowo utworzony został handler dla exception dla dobrej praktyki (uwaga z poprzedniego zadania)*


