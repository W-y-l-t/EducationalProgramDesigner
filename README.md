# EducationalProgramDesigner

The project is a model that allows you to create and edit educational programs that include users, laboratory work, lecture materials, subjects, 
and educational programs.

> The **tests** are written using the XUnit framework \
> CI/CD is also configured.

## Functionality

### 1. Users
- Have a unique identifier and name.
- Author of educational materials.

### 2. Laboratory work
- ID, name, description, evaluation criteria, number of points.
- The ability to create a new laboratory based on an existing one (the prototype ID is stored).
- The change is available only to the author (except for points).

### 3. Lecture materials
- ID, name, description, content (string).
- The ability to create new materials based on existing ones (the prototype ID is stored).
- The change is available only to the author.

### 4. Items
- ID, title, list of laboratory and lecture materials.
- Reference to the test or exam:
- **Exam**: total number of points (100).
- **Test**: minimum number of points to pass.
- The ability to create based on an existing item (the prototype ID is stored).
- The change is available only to the author (laboratory and scores are unchanged).

### 5. Educational programs
- ID, name, list of items.
- Linking subjects to semesters.
- Indication of the responsible person (supervisor).

## Generative Patterns Used
- **Prototype**: creating objects based on existing ones.
- **Builder**: simplify the assembly of complex objects.
- **Factory**: for abstraction of entity creation.

## Repositories
An in-memory repository is implemented for each entity.:
- Storing objects in memory.
- Search for objects by IDs.
