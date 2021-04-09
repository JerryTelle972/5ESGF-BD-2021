# Projet Sudoku sous Spark avec la  méthode de Norvig

## Présentation de Norvig

Peter Norvig est un scientifique américain, réputé pour ses travaux chez Google
- Il y a une dizaine d’années, Peter Norvig a écrit un essai sur la résolution de sudoku en Python afin de prouver à ses proches que le sudoku était chronophage
- Ses buts étaient de créer une interface facile d’accès, qui couvrait les performances de plusieurs niveaux de difficulté et qui affichait le temps de résolution
- Depuis, son code a été traduit dans différents langages informatiques

Pour réaliser notre projet nous avons utilisé le code [NorvigSolver](https://github.com/PKRoma/LinqSudokuSolver/blob/master/Solver.cs) déjà disponilbe en lignesur github et nous avons adapté ce code à notre projet afin de l'intégrer à Spark.

### Notre solution :

Elle est formée de 3 classes et d'une interface : 

- La class program.cs 
- La class Sudoku.cs
- La class SudokuSpark.cs 
- Une interface Isudoku.cs

## Sudoku.cs

Nous avons apporté une modification au code original. Nous avons remarqué que la grille de sudoku était de type dictionnaire. Notre objectif était de récupérer le type dictionnaire afin de le modifier en type sudoku. Ci-joint la méthode sudoku intégrée dans la classe Sudoku:

<img src="assets/images/sudoku.jpg">

### L'intégration des  métohdes runSpark et  Sudokusolution 

Dans la classe Sudoku spark nous avons intégré deux métodes : 

- La méthode runSpark : 
![RunSpark](assets/images/runSpark.jpg)

- La méthode SudokuSolver 
![image](assets/images/sudokuSolution.jpg)

La première méthode permet d'initialiser SparkSession avec les paramètres pour le nombre de coeurs et le nombre d'instances. Puis de créer un Dataframe et afin de transférer les données du csv dessus.
Il contient également la création de l'UDF Spark et l'appel de cette UDF au travers d'une requete SQL.


La deuxième méthode (SudokuSolver) est directement appelée dans la première méthode runSpark pour résoude le sudoku, en prenant en paramètre un type de caractère et retourne un type sudoku.

## Class SudokuBenchmarks

Nous avons intégré la classe sudoku benchmarks celle-ci nous permet d'effectuer des tests de performance de la méthode runSpark avec les paramètres que nous allons intégrer. Je vous présente deux tests effectués: un avec 500 sudokus à résoudre et l'autre avec 1000.

Les résultats obtenus sont les suivants :

**500 sudokus**

1 core et 1 instance : 3.293 s 



1 core 5 instance : 3.239 s

![image](assets/images/500.jpg)

**1000 sudokus**

1 core et 1 instance : 4.929 s

1 core 5 instance : 4.645 s

![image](assets/images/800.jpg)

**Conclusion** 

La méthode de Novig résolue très rapidement les sudokus avec une différence entre (1 core et 1 instance) et (1 core et 5 instances) peu notable sur 500 sudokus, qui est à peine de l'orde de 54 ms.

La méthode de Novig résolue très rapidement les sudokus avec une différence entre (1 core et 1 instance) et (1 core et 5 instances) peu notable sur 800 sudokus, qui est à peine de l'orde de 0.284 s.
