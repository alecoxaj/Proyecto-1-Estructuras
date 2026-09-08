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

            // Si el código no existe, retorna false
            if (indice == -1)
            {
                return false;
            }

            // Sustituye el elemento eliminado
            // con el último elemento del Heap
            elementos[indice] =
                elementos[cantidad - 1];

            elementos[cantidad - 1] =
                null;

            cantidad--;

            // Recupera la propiedad del Max Heap
            if (indice < cantidad)
            {
                int padre =
                    (indice - 1) / 2;

                // Si el elemento tiene más préstamos
                // que su padre, debe subir
                if (
                    indice > 0 &&
                    elementos[indice]!.VecesPrestado >
                    elementos[padre]!.VecesPrestado
                )
                {
                    Subir(indice);
                }
                else
                {
                    // De lo contrario comprueba si debe bajar
                    Bajar(indice);
                }
            }

            return true;
        }

        // Extrae el libro con mayor cantidad de préstamos
        public Libro? ExtraerMaximo()
        {
            // Comprueba si el Heap está vacío
            if (cantidad == 0)
            {
                return null;
            }

            // En un Max Heap el elemento máximo está en la raíz
            Libro maximo =
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

            return maximo;
        }

        // Hace bajar un elemento para mantener el orden del Max Heap
        private void Bajar(int indice)
        {
            while (true)
            {
                // Calcula las posiciones de los hijos
                int izquierdo =
                    indice * 2 + 1;

                int derecho =
                    indice * 2 + 2;

                // Inicialmente se considera al actual como el mayor
                int mayor =
                    indice;

                // Comprueba si el hijo izquierdo es mayor
                if (
                    izquierdo < cantidad &&
                    elementos[izquierdo]!.VecesPrestado >
                    elementos[mayor]!.VecesPrestado
                )
                {
                    mayor =
                        izquierdo;
                }

                // Comprueba si el hijo derecho es mayor
                if (
                    derecho < cantidad &&
                    elementos[derecho]!.VecesPrestado >
                    elementos[mayor]!.VecesPrestado
                )
                {
                    mayor =
                        derecho;
                }

                // Si el elemento ya es mayor que sus hijos, termina
                if (mayor == indice)
                {
                    break;
                }

                // Intercambia con el hijo de mayor prioridad
                Intercambiar(
                    indice,
                    mayor
                );

                indice =
                    mayor;
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

        // Imprime todos los libros del Max Heap
        public void Imprimir()
        {
            // Comprueba si existen elementos
            if (cantidad == 0)
            {
                Console.WriteLine(
                    "El Max Heap está vacío."
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

        // Recorre el Max Heap
        public void Recorrer()
        {
            if (cantidad == 0)
            {
                Console.WriteLine(
                    "El Max Heap está vacío."
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
