using System;

namespace CatalogoBiblioteca
{
    // Max Heap utilizado para organizar los libros
    // dando prioridad a los que tienen más préstamos
    public class MaxHeap
    {
        // Arreglo que almacena los libros del Heap
        private Libro?[] elementos;

        // Cantidad actual de elementos almacenados
        private int cantidad;

        // Constructor del Max Heap
        public MaxHeap(int capacidad)
        {
            // Si la capacidad no es válida, se utiliza 10
            if (capacidad <= 0)
            {
                capacidad = 10;
            }

            elementos = new Libro?[capacidad];
            cantidad = 0;
        }

        // Devuelve la cantidad de elementos almacenados
        public int Cantidad
        {
            get { return cantidad; }
        }

        // Inserta un libro en el Max Heap
        public void Insertar(Libro libro)
        {
            // Si el arreglo está lleno, aumenta su capacidad
            if (cantidad == elementos.Length)
            {
                AumentarCapacidad();
            }

            // Inserta inicialmente el libro al final
            elementos[cantidad] = libro;

            // Reorganiza el Heap hacia arriba
            Subir(cantidad);

            cantidad++;
        }

        // Aumenta el tamaño del arreglo cuando se llena
        private void AumentarCapacidad()
        {
            // Crea un arreglo con el doble de capacidad
            Libro?[] nuevo =
                new Libro?[elementos.Length * 2];

            // Copia los elementos del arreglo anterior
            for (int i = 0; i < elementos.Length; i++)
            {
                nuevo[i] = elementos[i];
            }

            elementos = nuevo;
        }

        // Hace subir un elemento para mantener el orden del Max Heap
        private void Subir(int indice)
        {
            while (indice > 0)
            {
                // Calcula la posición del padre
                int padre =
                    (indice - 1) / 2;

                // Si el padre tiene más o la misma cantidad
                // de préstamos, el Max Heap ya está ordenado
                if (elementos[padre]!.VecesPrestado >=
                    elementos[indice]!.VecesPrestado)
                {
                    break;
                }

                // Intercambia el elemento con su padre
                Intercambiar(
                    padre,
                    indice
                );

                indice =
                    padre;
            }
        }
