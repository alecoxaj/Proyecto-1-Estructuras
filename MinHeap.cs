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

        // Busca un libro por su código
        public Libro? Buscar(string codigo)
        {
            // La búsqueda es lineal porque el Heap está
            // organizado por préstamos y no por código
            for (int i = 0; i < cantidad; i++)
            {
                if (elementos[i]!.Codigo == codigo)
                {
                    return elementos[i];
                }
            }

            // Retorna null si el libro no existe
            return null;
        }

        // Elimina un libro por su código
        public bool Eliminar(string codigo)
        {
            int indice = -1;

            // Busca la posición del libro
            for (int i = 0; i < cantidad; i++)
            {
                if (elementos[i]!.Codigo == codigo)
                {
                    indice = i;
                    break;
                }
            }

            // Si no se encontró el libro, no se elimina nada
            if (indice == -1)
            {
                return false;
            }

            // Coloca el último elemento en la posición
            // del elemento que se quiere eliminar
            elementos[indice] =
                elementos[cantidad - 1];

            // Limpia la última posición
            elementos[cantidad - 1] =
                null;

            cantidad--;

            // Si la posición continúa dentro del Heap,
            // se debe recuperar el orden
            if (indice < cantidad)
            {
                // Calcula la posición del padre
                int padre =
                    (indice - 1) / 2;

                // Si el elemento es menor que su padre,
                // debe subir
                if (
                    indice > 0 &&
                    elementos[indice]!.VecesPrestado <
                    elementos[padre]!.VecesPrestado
                )
                {
                    Subir(indice);
                }
                else
                {
                    // De lo contrario, comprueba si debe bajar
                    Bajar(indice);
                }
            }

            return true;
        }

        // Extrae el libro con menor cantidad de préstamos
        public Libro? ExtraerMinimo()
        {
            // Comprueba si el Heap está vacío
            if (cantidad == 0)
            {
                return null;
            }

            // En un Min Heap el elemento mínimo está en la raíz
            Libro minimo =
                elementos[0]!;

            // Mueve el último elemento a la raíz
            elementos[0] =
                elementos[cantidad - 1];

            elementos[cantidad - 1] =
                null;

            cantidad--;

            // Reorganiza el Heap desde la raíz
            if (cantidad > 0)
            {
                Bajar(0);
            }

            return minimo;
        }

        // Hace bajar un elemento para mantener el orden del Min Heap
        private void Bajar(int indice)
        {
            while (true)
            {
                // Calcula las posiciones de los hijos
                int izquierdo =
                    indice * 2 + 1;

                int derecho =
                    indice * 2 + 2;

                // Inicialmente se considera al actual como el menor
                int menor =
                    indice;

                // Comprueba si el hijo izquierdo es menor
                if (
                    izquierdo < cantidad &&
                    elementos[izquierdo]!.VecesPrestado <
                    elementos[menor]!.VecesPrestado
                )
                {
                    menor =
                        izquierdo;
                }

                // Comprueba si el hijo derecho es menor
                if (
                    derecho < cantidad &&
                    elementos[derecho]!.VecesPrestado <
                    elementos[menor]!.VecesPrestado
                )
                {
                    menor =
                        derecho;
                }

                // Si no existe un hijo menor, termina
                if (menor == indice)
                {
                    break;
                }

                // Intercambia con el hijo de menor prioridad
                Intercambiar(
                    indice,
                    menor
                );

                indice =
                    menor;
            }
        }

        // Intercambia dos posiciones dentro del arreglo
        private void Intercambiar(
            int primero,
            int segundo)
        {
            Libro? temporal =
                elementos[primero];

            elementos[primero] =
                elementos[segundo];

            elementos[segundo] =
                temporal;
        }

        // Imprime todos los libros del Min Heap
        public void Imprimir()
        {
            // Comprueba si existen elementos
            if (cantidad == 0)
            {
                Console.WriteLine(
                    "El Min Heap está vacío."
                );

                return;
            }

            // Muestra los elementos en el orden interno del Heap
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine(
                    "Código: " +
                    elementos[i]!.Codigo +
                    " | Título: " +
                    elementos[i]!.Titulo +
                    " | Préstamos: " +
                    elementos[i]!.VecesPrestado
                );
            }
        }

        // Recorre el Min Heap
        public void Recorrer()
        {
            if (cantidad == 0)
            {
                Console.WriteLine(
                    "El Min Heap está vacío."
                );

                return;
            }

            // Recorre todas las posiciones ocupadas del arreglo
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine(
                    elementos[i]!.Codigo +
                    " -> " +
                    elementos[i]!.VecesPrestado +
                    " préstamos"
                );
            }
        }
    }
}   