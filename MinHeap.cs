using System;

namespace CatalogoBiblioteca
{
    // Min Heap utilizado para organizar los libros
    // dando prioridad a los que tienen menos préstamos
    public class MinHeap
    {
        // Arreglo que almacena los libros del Heap
        private Libro?[] elementos;

        // Cantidad actual de elementos almacenados
        private int cantidad;

        // Constructor del Min Heap
        public MinHeap(int capacidad)
        {
            // Si la capacidad no es válida, se utiliza 10
            if (capacidad <= 0)
            {
                capacidad = 10;
            }

            elementos = new Libro?[capacidad];
            cantidad = 0;

        }
}   }