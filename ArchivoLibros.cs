using System;
using System.IO;

namespace CatalogoBiblioteca
{
    // Se encarga de cargar y guardar los libros
    // utilizando un archivo CSV
    public class ArchivoLibros
    {
        // Guarda el nombre o ruta del archivo
        private string nombreArchivo;

        // Constructor de la clase
        public ArchivoLibros(string nombreArchivo)
        {
            this.nombreArchivo = nombreArchivo;
        }

        // Carga los libros guardados en el archivo CSV
        public void Cargar(
            ArbolBPlus arbol,
            MinHeap minHeap,
            MaxHeap maxHeap)
        {
            // Si el archivo no existe, lo crea
            // únicamente con los encabezados
            if (!File.Exists(nombreArchivo))
            {
                using (StreamWriter archivo =
                    new StreamWriter(nombreArchivo))
                {
                    archivo.WriteLine(
                        "Codigo,Titulo,Autor,Categoria,CopiasDisponibles,VecesPrestado"
                    );
                }

                return;
            }

            // Lee todas las líneas almacenadas en el archivo
            string[] lineas =
                File.ReadAllLines(nombreArchivo);

            // Empieza en 1 para ignorar los encabezados
            for (int i = 1; i < lineas.Length; i++)
            {
                // Ignora líneas vacías
                if (lineas[i] == "")
                {
                    continue;
                }

                // Separa los datos utilizando la coma
                string[] datos =
                    lineas[i].Split(',');

                // Cada registro debe contener seis campos
                if (datos.Length != 6)
                {
                    continue;
                }

                // Recupera los datos de texto
                string codigo = datos[0];
                string titulo = datos[1];
                string autor = datos[2];
                string categoria = datos[3];

                int copias;
                int vecesPrestado;

                // Convierte las copias disponibles a número entero
                if (!int.TryParse(
                    datos[4],
                    out copias))
                {
                    continue;
                }

                // Convierte la cantidad de préstamos a entero
                if (!int.TryParse(
                    datos[5],
                    out vecesPrestado))
                {
                    continue;
                }

                // Crea el objeto Libro con los datos recuperados
                Libro libro =
                    new Libro(
                        codigo,
                        titulo,
                        autor,
                        categoria,
                        copias
                    );

                // Recupera la cantidad de veces
                // que el libro había sido prestado
                libro.VecesPrestado =
                    vecesPrestado;

                // Inserta el mismo libro en las tres estructuras
                arbol.Insertar(libro);
                minHeap.Insertar(libro);
                maxHeap.Insertar(libro);
            }
        }

        // Guarda todos los libros en el archivo CSV
        public void Guardar(
            ArbolBPlus arbol)
        {
            // Obtiene todos los libros almacenados
            // en las hojas del Árbol B+
            Libro[] libros =
                arbol.ObtenerTodos();

            // Abre el archivo sobrescribiendo
            // su contenido anterior
            using (StreamWriter archivo =
                new StreamWriter(
                    nombreArchivo,
                    false))
            {
                // Escribe los encabezados
                archivo.WriteLine(
                    "Codigo,Titulo,Autor,Categoria,CopiasDisponibles,VecesPrestado"
                );

                // Escribe cada libro en una línea del archivo
                for (int i = 0; i < libros.Length; i++)
                {
                    archivo.WriteLine(
                        libros[i].Codigo + "," +
                        libros[i].Titulo + "," +
                        libros[i].Autor + "," +
                        libros[i].Categoria + "," +
                        libros[i].CopiasDisponibles + "," +
                        libros[i].VecesPrestado
                    );
                }
            }
        }
    }
}