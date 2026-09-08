using System;

namespace CatalogoBiblioteca
{
    // Representa cada nodo que forma parte del Árbol B+
    public class NodoBPlus
    {
        // Indica si el nodo es una hoja o un nodo interno
        public bool EsHoja;

        // Cantidad de claves almacenadas actualmente
        public int CantidadClaves;

        // Arreglo que almacena las claves de los libros
        public string?[] Claves;

        // En los nodos hoja se almacenan las referencias a los libros
        public Libro?[] Libros;

        // En los nodos internos se almacenan referencias a los hijos
        public NodoBPlus?[] Hijos;

        // Enlaza una hoja con la siguiente hoja del Árbol B+
        public NodoBPlus? Siguiente;

        // Constructor del nodo
        public NodoBPlus(bool esHoja, int orden)
        {
            EsHoja = esHoja;
            CantidadClaves = 0;

            // Se crean los arreglos utilizando el orden del árbol
            Claves = new string?[orden];
            Libros = new Libro?[orden];
            Hijos = new NodoBPlus?[orden + 1];

            Siguiente = null;
        }
    }

    // Implementación del Árbol B+ utilizado para organizar
    // y buscar los libros mediante su código
    public class ArbolBPlus
    {
        // Nodo raíz del árbol
        private NodoBPlus raiz;

        // Orden máximo utilizado por el Árbol B+
        private int orden;

        // Cantidad máxima de claves permitidas por nodo
        private int maxClaves;

        // Cantidad mínima de claves permitidas en una hoja
        private int minClavesHoja;

        // Cantidad mínima de claves permitidas en un nodo interno
        private int minClavesInterno;

        // Constructor del Árbol B+
        public ArbolBPlus(int orden)
        {
            // Se establece un orden válido por defecto
            if (orden < 3)
            {
                orden = 4;
            }

            this.orden = orden;

            // Un nodo puede almacenar como máximo orden - 1 claves
            maxClaves = orden - 1;

            // Calcula el mínimo de claves permitido en las hojas
            minClavesHoja =
                (int)Math.Ceiling(maxClaves / 2.0);

            // Calcula el mínimo de claves en nodos internos
            minClavesInterno =
                (int)Math.Ceiling(orden / 2.0) - 1;

            // Inicialmente el árbol contiene una raíz vacía
            // que también es una hoja
            raiz = new NodoBPlus(true, orden);
        }

        // Compara dos códigos ignorando diferencias
        // entre mayúsculas y minúsculas
        private int Comparar(string primero, string segundo)
        {
            return string.Compare(
                primero,
                segundo,
                StringComparison.OrdinalIgnoreCase
            );
        }

        // Inserta un libro en el Árbol B+
        public bool Insertar(Libro libro)
        {
            // Evita insertar códigos repetidos
            if (Buscar(libro.Codigo) != null)
            {
                return false;
            }

            // Si la raíz está llena, se debe dividir
            if (raiz.CantidadClaves == maxClaves)
            {
                // Crea una nueva raíz interna
                NodoBPlus nuevaRaiz =
                    new NodoBPlus(false, orden);

                // La raíz anterior pasa a ser el primer hijo
                nuevaRaiz.Hijos[0] = raiz;

                raiz = nuevaRaiz;

                // Divide el hijo que estaba lleno
                DividirHijo(raiz, 0);
            }

            // Inserta el libro en un nodo con espacio
            InsertarNoLleno(raiz, libro);

            // Actualiza las claves de separación
            RecalcularSeparadores(raiz);

            return true;
        }

        // Inserta un libro en un nodo que todavía tiene espacio
        private void InsertarNoLleno(
            NodoBPlus nodo,
            Libro libro)
        {
            // Si se llegó a una hoja, el libro se inserta aquí
            if (nodo.EsHoja)
            {
                int posicion =
                    nodo.CantidadClaves - 1;

                // Desplaza hacia la derecha las claves mayores
                // para conservar el orden por código
                while (
                    posicion >= 0 &&
                    Comparar(
                        nodo.Claves[posicion]!,
                        libro.Codigo
                    ) > 0)
                {
                    nodo.Claves[posicion + 1] =
                        nodo.Claves[posicion];

                    nodo.Libros[posicion + 1] =
                        nodo.Libros[posicion];

                    posicion--;
                }

                // Inserta la nueva clave en su posición correcta
                nodo.Claves[posicion + 1] =
                    libro.Codigo;

                // Guarda la referencia al libro
                nodo.Libros[posicion + 1] =
                    libro;

                nodo.CantidadClaves++;

                return;
            }

            // Determina el hijo por el cual debe continuar
            int indice = 0;

            while (
                indice < nodo.CantidadClaves &&
                Comparar(
                    libro.Codigo,
                    nodo.Claves[indice]!
                ) >= 0)
            {
                indice++;
            }

            NodoBPlus hijo =
                nodo.Hijos[indice]!;

            // Si el hijo está lleno, se divide antes de continuar
            if (hijo.CantidadClaves == maxClaves)
            {
                DividirHijo(nodo, indice);

                // Después de dividir se comprueba
                // cuál de los dos hijos corresponde
                if (
                    Comparar(
                        libro.Codigo,
                        nodo.Claves[indice]!
                    ) >= 0)
                {
                    indice++;
                }
            }

            // Continúa recursivamente hasta llegar a una hoja
            InsertarNoLleno(
                nodo.Hijos[indice]!,
                libro
            );
        }

        // Divide un nodo cuando llega al máximo de claves
        private void DividirHijo(
            NodoBPlus padre,
            int indice)
        {
            // Obtiene el nodo que será dividido
            NodoBPlus hijo =
                padre.Hijos[indice]!;

            // Crea un nuevo nodo del mismo tipo
            NodoBPlus nuevo =
                new NodoBPlus(
                    hijo.EsHoja,
                    orden
                );

            // Si el nodo que se divide es una hoja
            if (hijo.EsHoja)
            {
                // Calcula el punto donde se dividirán las claves
                int punto =
                    (hijo.CantidadClaves + 1) / 2;

                // Calcula cuántas claves pasarán al nuevo nodo
                nuevo.CantidadClaves =
                    hijo.CantidadClaves - punto;

                // Copia las claves y libros hacia la nueva hoja
                for (
                    int i = 0;
                    i < nuevo.CantidadClaves;
                    i++)
                {
                    nuevo.Claves[i] =
                        hijo.Claves[punto + i];

                    nuevo.Libros[i] =
                        hijo.Libros[punto + i];

                    // Limpia las posiciones trasladadas
                    hijo.Claves[punto + i] = null;
                    hijo.Libros[punto + i] = null;
                }

                // Actualiza la cantidad de claves del nodo original
                hijo.CantidadClaves = punto;

                // La nueva hoja apunta a la hoja
                // que anteriormente seguía al hijo
                nuevo.Siguiente =
                    hijo.Siguiente;

                // El hijo ahora apunta hacia la nueva hoja
                hijo.Siguiente =
                    nuevo;

                // Hace espacio para insertar el nuevo hijo
                for (
                    int j = padre.CantidadClaves;
                    j >= indice + 1;
                    j--)
                {
                    padre.Hijos[j + 1] =
                        padre.Hijos[j];
                }

                // Inserta la nueva hoja como hijo del padre
                padre.Hijos[indice + 1] =
                    nuevo;

                // Desplaza las claves separadoras del padre
                for (
                    int j = padre.CantidadClaves - 1;
                    j >= indice;
                    j--)
                {
                    padre.Claves[j + 1] =
                        padre.Claves[j];
                }

                // La primera clave de la nueva hoja
                // se utiliza como separador
                padre.Claves[indice] =
                    nuevo.Claves[0];

                padre.CantidadClaves++;
            }
            else
            {
                // Si es un nodo interno, calcula el punto medio
                int medio =
                    hijo.CantidadClaves / 2;

                // Cantidad de hijos que pasarán al nuevo nodo
                int hijosNuevo =
                    hijo.CantidadClaves - medio;

                // Traslada las referencias de hijos
                for (
                    int i = 0;
                    i < hijosNuevo;
                    i++)
                {
                    nuevo.Hijos[i] =
                        hijo.Hijos[medio + 1 + i];

                    hijo.Hijos[medio + 1 + i] =
                        null;
                }

                // Define la cantidad de claves del nuevo nodo
                nuevo.CantidadClaves =
                    hijosNuevo - 1;

                // Limpia las claves que ya no pertenecen al hijo original
                for (
                    int i = medio;
                    i < hijo.Claves.Length;
                    i++)
                {
                    hijo.Claves[i] = null;
                }

                hijo.CantidadClaves =
                    medio;

                // Desplaza los hijos del padre
                // para crear espacio para el nuevo nodo
                for (
                    int j = padre.CantidadClaves;
                    j >= indice + 1;
                    j--)
                {
                    padre.Hijos[j + 1] =
                        padre.Hijos[j];
                }

                // Agrega el nuevo nodo como hijo
                padre.Hijos[indice + 1] =
                    nuevo;

                padre.CantidadClaves++;

                // Actualiza los separadores del padre
                RecalcularSeparadores(padre);
            }
        }

        // Busca un libro por su código
        public Libro? Buscar(string codigo)
        {
            // Comienza la búsqueda desde la raíz
            return BuscarRecursivo(
                raiz,
                codigo
            );
        }

        // Realiza la búsqueda recorriendo el Árbol B+
        private Libro? BuscarRecursivo(
            NodoBPlus nodo,
            string codigo)
        {
            // Si se llegó a una hoja se busca directamente
            if (nodo.EsHoja)
            {
                for (
                    int i = 0;
                    i < nodo.CantidadClaves;
                    i++)
                {
                    // Comprueba si el código coincide
                    if (
                        Comparar(
                            nodo.Claves[i]!,
                            codigo
                        ) == 0)
                    {
                        return nodo.Libros[i];
                    }
                }

                // No se encontró el libro
                return null;
            }

            // Determina por cuál hijo debe continuar la búsqueda
            int indice = 0;

            while (
                indice < nodo.CantidadClaves &&
                Comparar(
                    codigo,
                    nodo.Claves[indice]!
                ) >= 0)
            {
                indice++;
            }

            // Continúa recursivamente
            return BuscarRecursivo(
                nodo.Hijos[indice]!,
                codigo
            );
        }

        // Elimina un libro del Árbol B+
        public bool Eliminar(string codigo)
        {
            // Comprueba primero que el libro exista
            if (Buscar(codigo) == null)
            {
                return false;
            }

            // Inicia la eliminación desde la raíz
            EliminarRecursivo(
                raiz,
                codigo
            );

            // Si la raíz interna queda sin claves,
            // su primer hijo se convierte en la nueva raíz
            if (
                !raiz.EsHoja &&
                raiz.CantidadClaves == 0 &&
                raiz.Hijos[0] != null)
            {
                raiz =
                    raiz.Hijos[0]!;
            }

            // Actualiza las claves separadoras
            RecalcularSeparadores(raiz);

            return true;
        }

        // Busca recursivamente la hoja que contiene
        // el libro y lo elimina
        private bool EliminarRecursivo(
            NodoBPlus nodo,
            string codigo)
        {
            // Si se llegó a una hoja, se busca el código
            if (nodo.EsHoja)
            {
                for (
                    int i = 0;
                    i < nodo.CantidadClaves;
                    i++)
                {
                    if (
                        Comparar(
                            nodo.Claves[i]!,
                            codigo
                        ) == 0)
                    {
                        // Desplaza los elementos siguientes
                        // para ocupar la posición eliminada
                        for (
                            int j = i;
                            j < nodo.CantidadClaves - 1;
                            j++)
                        {
                            nodo.Claves[j] =
                                nodo.Claves[j + 1];

                            nodo.Libros[j] =
                                nodo.Libros[j + 1];
                        }

                        // Limpia la última posición
                        nodo.Claves[
                            nodo.CantidadClaves - 1
                        ] = null;

                        nodo.Libros[
                            nodo.CantidadClaves - 1
                        ] = null;

                        nodo.CantidadClaves--;

                        return true;
                    }
                }

                return false;
            }

            // Determina el hijo donde se encuentra el código
            int indice = 0;

            while (
                indice < nodo.CantidadClaves &&
                Comparar(
                    codigo,
                    nodo.Claves[indice]!
                ) >= 0)
            {
                indice++;
            }

            NodoBPlus hijo =
                nodo.Hijos[indice]!;

            // Continúa la eliminación en el hijo
            bool eliminado =
                EliminarRecursivo(
                    hijo,
                    codigo
                );

            if (!eliminado)
            {
                return false;
            }

            // Comprueba si el hijo quedó con menos claves
            // de las permitidas
            if (TieneUnderflow(hijo))
            {
                BalancearHijo(
                    nodo,
                    indice
                );
            }

            // Actualiza los separadores del nodo
            RecalcularSeparadores(nodo);

            return true;
        }

        // Verifica si un nodo tiene menos claves de las permitidas
        private bool TieneUnderflow(
            NodoBPlus nodo)
        {
            if (nodo.EsHoja)
            {
                return nodo.CantidadClaves <
                       minClavesHoja;
            }

            return nodo.CantidadClaves <
                   minClavesInterno;
        }

        // Balancea un nodo utilizando sus hermanos
        private void BalancearHijo(
            NodoBPlus padre,
            int indice)
        {
            NodoBPlus hijo =
                padre.Hijos[indice]!;

            // Referencias a los posibles hermanos
            NodoBPlus? izquierdo = null;
            NodoBPlus? derecho = null;

            // Obtiene el hermano izquierdo si existe
            if (indice > 0)
            {
                izquierdo =
                    padre.Hijos[indice - 1];
            }

            // Obtiene el hermano derecho si existe
            if (indice < padre.CantidadClaves)
            {
                derecho =
                    padre.Hijos[indice + 1];
            }

            // Intenta pedir prestado al hermano izquierdo
            if (
                izquierdo != null &&
                PuedePrestar(izquierdo))
            {
                PrestarDesdeIzquierda(
                    izquierdo,
                    hijo
                );

                RecalcularSeparadores(padre);

                return;
            }

            // Si no se pudo, intenta con el hermano derecho
            if (
                derecho != null &&
                PuedePrestar(derecho))
            {
                PrestarDesdeDerecha(
                    hijo,
                    derecho
                );

                RecalcularSeparadores(padre);

                return;
            }

            // Si ningún hermano puede prestar,
            // se realiza una fusión
            if (izquierdo != null)
            {
                Fusionar(
                    izquierdo,
                    hijo
                );

                // Elimina la referencia al hijo fusionado
                QuitarHijo(
                    padre,
                    indice
                );
            }
            else if (derecho != null)
            {
                Fusionar(
                    hijo,
                    derecho
                );

                QuitarHijo(
                    padre,
                    indice + 1
                );
            }

            // Actualiza las claves separadoras
            RecalcularSeparadores(padre);
        }

        // Verifica si un hermano puede prestar una clave
        private bool PuedePrestar(
            NodoBPlus nodo)
        {
            if (nodo.EsHoja)
            {
                return nodo.CantidadClaves >
                       minClavesHoja;
            }

            return nodo.CantidadClaves >
                   minClavesInterno;
        }

        // Toma un elemento del hermano izquierdo
        private void PrestarDesdeIzquierda(
            NodoBPlus izquierdo,
            NodoBPlus hijo)
        {
            // Caso de hojas
            if (hijo.EsHoja)
            {
                // Desplaza las claves del hijo hacia la derecha
                for (
                    int i = hijo.CantidadClaves;
                    i > 0;
                    i--)
                {
                    hijo.Claves[i] =
                        hijo.Claves[i - 1];

                    hijo.Libros[i] =
                        hijo.Libros[i - 1];
                }

                // Pasa la última clave del hermano izquierdo
                // al inicio del hijo
                hijo.Claves[0] =
                    izquierdo.Claves[
                        izquierdo.CantidadClaves - 1
                    ];

                hijo.Libros[0] =
                    izquierdo.Libros[
                        izquierdo.CantidadClaves - 1
                    ];

                // Limpia la posición trasladada
                izquierdo.Claves[
                    izquierdo.CantidadClaves - 1
                ] = null;

                izquierdo.Libros[
                    izquierdo.CantidadClaves - 1
                ] = null;

                izquierdo.CantidadClaves--;
                hijo.CantidadClaves++;
            }
            else
            {
                // Cantidad de hijos actuales
                int cantidadHijos =
                    hijo.CantidadClaves + 1;

                // Desplaza los hijos hacia la derecha
                for (
                    int i = cantidadHijos;
                    i > 0;
                    i--)
                {
                    hijo.Hijos[i] =
                        hijo.Hijos[i - 1];
                }

                // Toma el último hijo del hermano izquierdo
                hijo.Hijos[0] =
                    izquierdo.Hijos[
                        izquierdo.CantidadClaves
                    ];

                izquierdo.Hijos[
                    izquierdo.CantidadClaves
                ] = null;

                izquierdo.CantidadClaves--;
                hijo.CantidadClaves++;

                // Actualiza las claves internas
                RecalcularSeparadores(izquierdo);
                RecalcularSeparadores(hijo);
            }
        }

        // Toma un elemento del hermano derecho
        private void PrestarDesdeDerecha(
            NodoBPlus hijo,
            NodoBPlus derecho)
        {
            // Caso de hojas
            if (hijo.EsHoja)
            {
                // Coloca la primera clave del hermano derecho
                // al final del hijo
                hijo.Claves[
                    hijo.CantidadClaves
                ] = derecho.Claves[0];

                hijo.Libros[
                    hijo.CantidadClaves
                ] = derecho.Libros[0];

                hijo.CantidadClaves++;

                // Desplaza las claves del hermano derecho
                // hacia la izquierda
                for (
                    int i = 0;
                    i < derecho.CantidadClaves - 1;
                    i++)
                {
                    derecho.Claves[i] =
                        derecho.Claves[i + 1];

                    derecho.Libros[i] =
                        derecho.Libros[i + 1];
                }

                // Limpia la última posición del hermano derecho
                derecho.Claves[
                    derecho.CantidadClaves - 1
                ] = null;

                derecho.Libros[
                    derecho.CantidadClaves - 1
                ] = null;

                derecho.CantidadClaves--;
            }
            else
            {
                // Pasa el primer hijo del hermano derecho
                // al final de los hijos del nodo
                hijo.Hijos[
                    hijo.CantidadClaves + 1
                ] = derecho.Hijos[0];

                hijo.CantidadClaves++;

                int cantidadHijosDerecho =
                    derecho.CantidadClaves + 1;

                // Desplaza los hijos del hermano derecho
                // hacia la izquierda
                for (
                    int i = 0;
                    i < cantidadHijosDerecho - 1;
                    i++)
                {
                    derecho.Hijos[i] =
                        derecho.Hijos[i + 1];
                }

                derecho.Hijos[
                    cantidadHijosDerecho - 1
                ] = null;

                derecho.CantidadClaves--;

                RecalcularSeparadores(hijo);
                RecalcularSeparadores(derecho);
            }
        }

        // Fusiona dos nodos hermanos
        private void Fusionar(
            NodoBPlus izquierdo,
            NodoBPlus derecho)
        {
            // Si ambos nodos son hojas
            if (izquierdo.EsHoja)
            {
                // Posición desde donde comenzarán a copiarse
                // los elementos del nodo derecho
                int posicion =
                    izquierdo.CantidadClaves;

                // Copia las claves y libros del nodo derecho
                for (
                    int i = 0;
                    i < derecho.CantidadClaves;
                    i++)
                {
                    izquierdo.Claves[
                        posicion + i
                    ] = derecho.Claves[i];

                    izquierdo.Libros[
                        posicion + i
                    ] = derecho.Libros[i];
                }

                // Suma la cantidad de claves
                izquierdo.CantidadClaves +=
                    derecho.CantidadClaves;

                // Mantiene el enlace entre las hojas
                izquierdo.Siguiente =
                    derecho.Siguiente;
            }
            else
            {
                // Calcula la posición donde se agregarán
                // los hijos del nodo derecho
                int posicionHijos =
                    izquierdo.CantidadClaves + 1;

                int hijosDerecho =
                    derecho.CantidadClaves + 1;

                // Copia todas las referencias a hijos
                for (
                    int i = 0;
                    i < hijosDerecho;
                    i++)
                {
                    izquierdo.Hijos[
                        posicionHijos + i
                    ] = derecho.Hijos[i];
                }

                // Actualiza la cantidad total de claves
                izquierdo.CantidadClaves =
                    izquierdo.CantidadClaves +
                    derecho.CantidadClaves +
                    1;

                RecalcularSeparadores(
                    izquierdo
                );
            }
        }

        // Quita un hijo del nodo padre después de una fusión
        private void QuitarHijo(
            NodoBPlus padre,
            int indiceHijo)
        {
            // Calcula la cantidad actual de hijos
            int cantidadHijos =
                padre.CantidadClaves + 1;

            // Desplaza hacia la izquierda las referencias
            for (
                int i = indiceHijo;
                i < cantidadHijos - 1;
                i++)
            {
                padre.Hijos[i] =
                    padre.Hijos[i + 1];
            }

            // Limpia la última referencia
            padre.Hijos[
                cantidadHijos - 1
            ] = null;

            padre.CantidadClaves--;

            // Limpia la clave separadora que ya no se necesita
            if (padre.CantidadClaves >= 0)
            {
                padre.Claves[
                    padre.CantidadClaves
                ] = null;
            }
        }

        // Actualiza las claves separadoras de los nodos internos
        private void RecalcularSeparadores(
            NodoBPlus nodo)
        {
            // Las hojas no poseen separadores internos
            if (nodo.EsHoja)
            {
                return;
            }

            // Primero actualiza recursivamente los hijos
            for (
                int i = 0;
                i <= nodo.CantidadClaves;
                i++)
            {
                if (nodo.Hijos[i] != null)
                {
                    RecalcularSeparadores(
                        nodo.Hijos[i]!
                    );
                }
            }

            // Cada separador corresponde a la primera clave
            // del subárbol ubicado a su derecha
            for (
                int i = 0;
                i < nodo.CantidadClaves;
                i++)
            {
                nodo.Claves[i] =
                    ObtenerPrimeraClave(
                        nodo.Hijos[i + 1]!
                    );
            }

            // Limpia las posiciones sobrantes
            for (
                int i = nodo.CantidadClaves;
                i < nodo.Claves.Length;
                i++)
            {
                nodo.Claves[i] = null;
            }
        }

        // Obtiene la primera clave de un subárbol
        private string ObtenerPrimeraClave(
            NodoBPlus nodo)
        {
            NodoBPlus actual =
                nodo;

            // Desciende siempre por el primer hijo
            // hasta llegar a una hoja
            while (!actual.EsHoja)
            {
                actual =
                    actual.Hijos[0]!;
            }

            return actual.Claves[0]!;
        }

        // Imprime la estructura del Árbol B+
        public void Imprimir()
        {
            // Comprueba si el árbol se encuentra vacío
            if (raiz.CantidadClaves == 0)
            {
                Console.WriteLine(
                    "El árbol B+ está vacío."
                );

                return;
            }

            // Inicia la impresión desde la raíz
            MostrarNodo(
                raiz,
                0
            );
        }

        // Muestra recursivamente cada nodo y su nivel
        private void MostrarNodo(
            NodoBPlus nodo,
            int nivel)
        {
            Console.Write(
                "Nivel " +
                nivel +
                ": "
            );

            // Muestra todas las claves del nodo
            for (
                int i = 0;
                i < nodo.CantidadClaves;
                i++)
            {
                Console.Write(
                    nodo.Claves[i] +
                    " "
                );
            }

            // Identifica si el nodo es una hoja
            if (nodo.EsHoja)
            {
                Console.WriteLine(
                    "[HOJA]"
                );
            }
            else
            {
                // Identifica un nodo interno
                Console.WriteLine(
                    "[INTERNO]"
                );

                // Muestra recursivamente todos sus hijos
                for (
                    int i = 0;
                    i <= nodo.CantidadClaves;
                    i++)
                {
                    if (nodo.Hijos[i] != null)
                    {
                        MostrarNodo(
                            nodo.Hijos[i]!,
                            nivel + 1
                        );
                    }
                }
            }
        }

        // Recorre las hojas enlazadas del Árbol B+
        public void Recorrer()
        {
            // Obtiene la hoja ubicada más a la izquierda
            NodoBPlus? hoja =
                ObtenerPrimeraHoja();

            // Comprueba si el árbol está vacío
            if (
                hoja == null ||
                raiz.CantidadClaves == 0)
            {
                Console.WriteLine(
                    "El árbol B+ está vacío."
                );

                return;
            }

            // Recorre todas las hojas utilizando Siguiente
            while (hoja != null)
            {
                // Muestra los libros almacenados en cada hoja
                for (
                    int i = 0;
                    i < hoja.CantidadClaves;
                    i++)
                {
                    Console.WriteLine(
                        hoja.Claves[i] +
                        " - " +
                        hoja.Libros[i]!.Titulo
                    );
                }

                // Avanza hacia la siguiente hoja
                hoja =
                    hoja.Siguiente;
            }
        }

        // Obtiene la primera hoja del Árbol B+
        private NodoBPlus? ObtenerPrimeraHoja()
        {
            NodoBPlus actual =
                raiz;

            // Desde la raíz siempre toma el primer hijo
            // hasta encontrar una hoja
            while (!actual.EsHoja)
            {
                actual =
                    actual.Hijos[0]!;
            }

            return actual;
        }

        // Devuelve todos los libros almacenados
        public Libro[] ObtenerTodos()
        {
            // Primero cuenta cuántos libros existen
            int cantidad =
                ContarLibros();

            // Crea un arreglo con el tamaño exacto
            Libro[] libros =
                new Libro[cantidad];

            // Comienza desde la primera hoja
            NodoBPlus? hoja =
                ObtenerPrimeraHoja();

            int posicion = 0;

            // Recorre todas las hojas enlazadas
            while (hoja != null)
            {
                for (
                    int i = 0;
                    i < hoja.CantidadClaves;
                    i++)
                {
                    // Agrega cada libro al arreglo
                    libros[posicion] =
                        hoja.Libros[i]!;

                    posicion++;
                }

                hoja =
                    hoja.Siguiente;
            }

            return libros;
        }

        // Cuenta los libros almacenados en las hojas
        private int ContarLibros()
        {
            int contador = 0;

            // Obtiene la primera hoja
            NodoBPlus? hoja =
                ObtenerPrimeraHoja();

            // Recorre todas las hojas
            while (hoja != null)
            {
                // Suma la cantidad de claves de cada hoja
                contador +=
                    hoja.CantidadClaves;

                hoja =
                    hoja.Siguiente;
            }

            return contador;
        }
    }
}