using System;

namespace CatalogoBiblioteca
{
    // Clase principal del programa
    class Program
    {
        // Árbol B+ utilizado para organizar los libros por código
        static ArbolBPlus arbol =
            new ArbolBPlus(4);

        // Min Heap utilizado para priorizar
        // los libros con menos préstamos
        static MinHeap minHeap =
            new MinHeap(100);

        // Max Heap utilizado para priorizar
        // los libros con más préstamos
        static MaxHeap maxHeap =
            new MaxHeap(100);

        // Archivo CSV utilizado para guardar los datos
        static ArchivoLibros archivo =
            new ArchivoLibros("libros.csv");

        // Punto de entrada del programa
        static void Main(string[] args)
        {
            // Carga los libros guardados anteriormente
            // en las tres estructuras de datos
            archivo.Cargar(
                arbol,
                minHeap,
                maxHeap
            );

            int opcion;

            // Mantiene el menú activo hasta seleccionar salir
            do
            {
                Console.WriteLine();

                Console.WriteLine(
                    "------ CATÁLOGO DE BIBLIOTECA ------"
                );

                Console.WriteLine("1. Registrar libro");
                Console.WriteLine("2. Buscar libro");
                Console.WriteLine("3. Eliminar libro");
                Console.WriteLine("4. Registrar préstamo");
                Console.WriteLine("5. Registrar devolución");
                Console.WriteLine("6. Mostrar catálogo");
                Console.WriteLine("7. Mostrar Min Heap");
                Console.WriteLine("8. Mostrar Max Heap");
                Console.WriteLine("9. Mostrar Árbol B+");
                Console.WriteLine("10. Recorrer Árbol B+");
                Console.WriteLine("11. Buscar en Min Heap");
                Console.WriteLine("12. Buscar en Max Heap");
                Console.WriteLine("13. Recorrer Min Heap");
                Console.WriteLine("14. Recorrer Max Heap");
                Console.WriteLine("15. Salir");

                Console.Write(
                    "\nSelecciona una opción: "
                );

                // Valida que el usuario ingrese un número entero
                if (!int.TryParse(
                    Console.ReadLine() ?? "",
                    out opcion))
                {
                    opcion = -1;
                }

                // Ejecuta la opción seleccionada
                switch (opcion)
                {
                    case 1:
                        RegistrarLibro();
                        break;

                    case 2:
                        BuscarLibro();
                        break;

                    case 3:
                        EliminarLibro();
                        break;

                    case 4:
                        RegistrarPrestamo();
                        break;

                    case 5:
                        RegistrarDevolucion();
                        break;

                    case 6:
                        MostrarCatalogo();
                        break;

                    case 7:
                        MostrarMinHeap();
                        break;

                    case 8:
                        MostrarMaxHeap();
                        break;

                    case 9:
                        MostrarArbol();
                        break;

                    case 10:
                        RecorrerArbol();
                        break;

                    case 11:
                        BuscarMinHeap();
                        break;

                    case 12:
                        BuscarMaxHeap();
                        break;

                    case 13:
                        RecorrerMinHeap();
                        break;

                    case 14:
                        RecorrerMaxHeap();
                        break;

                    case 15:
                        Console.WriteLine(
                            "Programa finalizado."
                        );
                        break;

                    default:
                        Console.WriteLine(
                            "Opción no válida."
                        );
                        break;
                }

            } while (opcion != 15);
        }

        // Registra un nuevo libro
        static void RegistrarLibro()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- REGISTRAR LIBRO ----"
            );

            Console.Write("Código: ");

            string codigo =
                Console.ReadLine() ?? "";

            // Comprueba que el código no esté vacío
            if (codigo == "")
            {
                Console.WriteLine(
                    "El código no puede estar vacío."
                );

                return;
            }

            // Comprueba que el código no esté repetido
            if (arbol.Buscar(codigo) != null)
            {
                Console.WriteLine(
                    "Ese código ya existe."
                );

                return;
            }

            Console.Write("Título: ");

            string titulo =
                Console.ReadLine() ?? "";

            Console.Write("Autor: ");

            string autor =
                Console.ReadLine() ?? "";

            Console.Write("Categoría: ");

            string categoria =
                Console.ReadLine() ?? "";

            Console.Write(
                "Copias disponibles: "
            );

            // Lee una cantidad válida de copias
            int copias =
                LeerEntero();

            // No permite cantidades negativas
            if (copias < 0)
            {
                Console.WriteLine(
                    "Las copias no pueden ser negativas."
                );

                return;
            }

            // Crea el objeto Libro
            Libro libro =
                new Libro(
                    codigo,
                    titulo,
                    autor,
                    categoria,
                    copias
                );

            // Inserta el mismo libro en las tres estructuras
            arbol.Insertar(libro);
            minHeap.Insertar(libro);
            maxHeap.Insertar(libro);

            // Guarda los cambios en el archivo CSV
            archivo.Guardar(arbol);

            Console.WriteLine(
                "Libro registrado correctamente."
            );
        }

        // Busca un libro mediante su código
        static void BuscarLibro()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- BUSCAR LIBRO ----"
            );

            Console.Write(
                "Ingresa el código del libro: "
            );

            string codigo =
                Console.ReadLine() ?? "";

            // Realiza la búsqueda en el Árbol B+
            Libro? libro =
                arbol.Buscar(codigo);

            if (libro == null)
            {
                Console.WriteLine(
                    "Libro no encontrado."
                );
            }
            else
            {
                MostrarDatosLibro(libro);
            }
        }

        // Elimina un libro del sistema
        static void EliminarLibro()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- ELIMINAR LIBRO ----"
            );

            Console.Write(
                "Ingresa el código para eliminar: "
            );

            string codigo =
                Console.ReadLine() ?? "";

            // Comprueba primero si existe
            Libro? libro =
                arbol.Buscar(codigo);

            if (libro == null)
            {
                Console.WriteLine(
                    "Libro no encontrado."
                );

                return;
            }

            // Elimina el libro de las tres estructuras
            arbol.Eliminar(codigo);
            minHeap.Eliminar(codigo);
            maxHeap.Eliminar(codigo);

            // Actualiza el archivo CSV
            archivo.Guardar(arbol);

            Console.WriteLine(
                "Libro eliminado correctamente."
            );
        }

        // Registra el préstamo de un libro
        static void RegistrarPrestamo()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- REGISTRAR PRÉSTAMO ----"
            );

            Console.Write(
                "Ingrese el código del libro: "
            );

            string codigo =
                Console.ReadLine() ?? "";

            // Busca el libro en el Árbol B+
            Libro? libro =
                arbol.Buscar(codigo);

            if (libro == null)
            {
                Console.WriteLine(
                    "Libro no encontrado."
                );

                return;
            }

            // Comprueba si existen copias disponibles
            if (libro.CopiasDisponibles <= 0)
            {
                Console.WriteLine(
                    "No hay copias disponibles."
                );

                return;
            }

            // Se elimina temporalmente de los Heaps
            // porque cambiará su cantidad de préstamos
            minHeap.Eliminar(codigo);
            maxHeap.Eliminar(codigo);

            // Reduce una copia disponible
            libro.CopiasDisponibles--;

            // Aumenta la cantidad de veces prestado
            libro.VecesPrestado++;

            // Se inserta nuevamente para recuperar
            // el orden de los Heaps
            minHeap.Insertar(libro);
            maxHeap.Insertar(libro);

            // Guarda los cambios
            archivo.Guardar(arbol);

            Console.WriteLine(
                "Préstamo registrado correctamente."
            );

            Console.WriteLine(
                "Copias disponibles: " +
                libro.CopiasDisponibles
            );

            Console.WriteLine(
                "Veces prestado: " +
                libro.VecesPrestado
            );
        }

        // Registra la devolución de un libro
        static void RegistrarDevolucion()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- REGISTRAR DEVOLUCIÓN ----"
            );

            Console.Write(
                "Ingrese el código del libro: "
            );

            string codigo =
                Console.ReadLine() ?? "";

            // Busca el libro
            Libro? libro =
                arbol.Buscar(codigo);

            if (libro == null)
            {
                Console.WriteLine(
                    "Libro no encontrado."
                );

                return;
            }

            // Aumenta la cantidad de copias disponibles
            libro.CopiasDisponibles++;

            // No es necesario reorganizar los Heaps
            // porque VecesPrestado no cambia
            archivo.Guardar(arbol);

            Console.WriteLine(
                "Devolución registrada correctamente."
            );

            Console.WriteLine(
                "Copias disponibles: " +
                libro.CopiasDisponibles
            );
        }

        // Muestra todos los libros del catálogo
        static void MostrarCatalogo()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- CATÁLOGO DE LIBROS ----"
            );

            // Utiliza el recorrido de las hojas del Árbol B+
            arbol.Recorrer();
        }

        // Muestra el contenido del Min Heap
        static void MostrarMinHeap()
        {
            Console.WriteLine();

            Console.WriteLine(
                "---- MIN HEAP ----"
            );

            minHeap.Imprimir();
        }

