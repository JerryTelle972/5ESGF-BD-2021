{
 "cells": [
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "# Projet Sudoku sous Spark avec la  méthode de Norvig\n",
    "\n",
    "## Présentation de Norvig"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Peter Norvig est un scientifique américain, réputé pour ses travaux chez Google\n",
    "- Il y a une dizaine d’années, Peter Norvig a écrit un essai sur la résolution de sudoku en Python afin de prouver à ses proches que le sudoku était chronophage\n",
    "- Ses buts étaient de créer une interface facile d’accès, qui couvrait les performances de plusieurs niveaux de difficulté et qui affichait le temps de résolution\n",
    "- Depuis, son code a été traduit dans différents langages informatiques"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Pour réaliser notre projet nous avons utilisé le code [NorvigSolver](https://github.com/PKRoma/LinqSudokuSolver/blob/master/Solver.cs) déjà disponilbe en lignesur github et nous avons adapté ce code à notre projet afin de l'intégrer à Spark."
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "### Notre solution :"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Elle est formée de 3 classes et d'une interface : \n",
    "\n",
    "- La class program.cs \n",
    "- La class Sudoku.cs\n",
    "- La class SudokuSpark.cs \n",
    "- Une interface Isudoku.cs"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "## Sudoku.cs\n",
    "\n",
    "Nous avons apporté une modification au code original. Nous avons remarqué que la grille de sudoku était de type dictionnaire. Notre objectif était de récupérer le type dictionnaire afin de le modifier en type sudoku. Ci-joint la méthode sudoku intégrée dans la classe Sudoku:"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "![méthode Sudoku](assets/images/sudoku.jpg)"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "### L'intégration des  métohdes runSpark et  Sudokusolution "
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Dans la classe Sudoku spark nous avons intégré deux métodes : \n",
    "\n",
    "- La méthode runSpark : \n",
    "![RunSpark](assets/images/runSpark.jpg)\n",
    "\n",
    "- La méthode SudokuSolver \n",
    "![image](assets/images/sudokuSolution.jpg)"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "La première méthode permet d'initialiser SparkSession avec les paramètres pour le nombre de coeurs et le nombre d'instances. Puis de créer un Dataframe et afin de transférer les données du csv dessus.\n",
    "Il contient également la création de l'UDF Spark et l'appel de cette UDF au travers d'une requete SQL.\n"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "La deuxième méthode (SudokuSolver) est directement appelée dans la première méthode runSpark pour résoude le sudoku, en prenant en paramètre un type de caractère et retourne un type sudoku."
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "## Class SudokuBenchmarks"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Nous avons intégré la classe sudoku benchmarks celle-ci nous permet d'effectuer des tests de performance de la méthode runSpark avec les paramètres que nous allons intégrer. Je vous présente deux tests effectués: un avec 500 sudokus à résoudre et l'autre avec 1000."
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "Les résultats obtenus sont les suivants :"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "**500 sudokus**"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "1 core et 1 instance : 3.293 s \n",
    "\n"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "1 core 5 instance : 3.239 s"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "![image](assets/images/500.jpg)"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "**1000 sudokus**"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "1 core et 1 instance : 4.929 s"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "1 core 5 instance : 4.645 s"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "![image](assets/images/800.jpg)"
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "**Conclusion** "
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "La méthode de Novig résolue très rapidement les sudokus avec une différence entre (1 core et 1 instance) et (1 core et 5 instances) peu notable sur 500 sudokus, qui est à peine de l'orde de 54 ms."
   ]
  },
  {
   "cell_type": "markdown",
   "metadata": {},
   "source": [
    "La méthode de Novig résolue très rapidement les sudokus avec une différence entre (1 core et 1 instance) et (1 core et 5 instances) peu notable sur 800 sudokus, qui est à peine de l'orde de 0.284 s."
   ]
  }
 ],
 "metadata": {
  "kernelspec": {
   "display_name": "Python 3",
   "language": "python",
   "name": "python3"
  },
  "language_info": {
   "codemirror_mode": {
    "name": "ipython",
    "version": 3
   },
   "file_extension": ".py",
   "mimetype": "text/x-python",
   "name": "python",
   "nbconvert_exporter": "python",
   "pygments_lexer": "ipython3",
   "version": "3.8.5"
  }
 },
 "nbformat": 4,
 "nbformat_minor": 4
}
