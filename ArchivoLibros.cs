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