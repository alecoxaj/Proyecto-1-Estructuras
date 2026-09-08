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

        // Devuelve la cantidad de elementos almacenados
        public int Cantidad
        {
            get { return cantidad; }
        }

        // Inserta un libro en el Min Heap
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
            // Crea un nuevo arreglo con el doble de capacidad
            Libro?[] nuevo =
                new Libro?[elementos.Length * 2];

            // Copia los elementos del arreglo anterior
            for (int i = 0; i < elementos.Length; i++)
            {
                nuevo[i] = elementos[i];
            }

            // Sustituye el arreglo anterior
            elementos = nuevo;
        }

        // Hace subir un elemento para mantener el orden del Min Heap
        private void Subir(int indice)
        {
            while (indice > 0)
            {
                // Calcula la posición del padre
                int padre =
                    (indice - 1) / 2;

                // Si el padre tiene menos o la misma cantidad
                // de préstamos, ya se cumple la propiedad del Min Heap
                if (elementos[padre]!.VecesPrestado <=
                    elementos[indice]!.VecesPrestado)
                {
                    break;
                }

                // Intercambia el elemento con su padre
                Intercambiar(
                    padre,
                    indice
                );

                // Continúa comprobando desde la posición del padre
                indice =
                    padre;
            }
        }
}   }