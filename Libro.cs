using System;

namespace CatalogoBiblioteca
{
    public class Libro
    {
        // Datos principales de cada libro
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }

        // Cantidad de ejemplares disponibles para préstamo
        public int CopiasDisponibles { get; set; }

        // Cantidad de veces que el libro ha sido prestado
        public int VecesPrestado { get; set; }

        // Constructor utilizado para crear un nuevo libro
        public Libro(
            string codigo,
            string titulo,
            string autor,
            string categoria,
            int copiasDisponibles)
        {
            Codigo = codigo;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
            CopiasDisponibles = copiasDisponibles;

            // Un libro nuevo inicia sin préstamos
            VecesPrestado = 0;
        }

        // Muestra los datos de un libro
        public void Mostrar()
        {
            Console.WriteLine("Código: " + Codigo);
            Console.WriteLine("Título: " + Titulo);
            Console.WriteLine("Autor: " + Autor);
            Console.WriteLine("Categoría: " + Categoria);
            Console.WriteLine("Copias disponibles: " + CopiasDisponibles);
            Console.WriteLine("Veces prestado: " + VecesPrestado);
       }
    }
}