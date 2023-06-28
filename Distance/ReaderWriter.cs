namespace Distance
{
    internal class ReaderWriter
    {
        public static void ReadPointsFromFile(string filePath, ref List<Point> list)
        {
            List<Point> points = new List<Point>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] coordinates = line.Split(';');

                    if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double x) && double.TryParse(coordinates[1], out double y))
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при зчитуванні файлу: " + ex.Message);
            }

            list = points;
        }

        public static void write(string filePath, ref List<Point> answer, double optimalDistance)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                Console.WriteLine("Найоптимальніший обхід:");

                foreach (Point point in answer)
                {
                    Console.WriteLine(point.X + "; " + point.Y);
                }
                Console.WriteLine("Відстань: " + optimalDistance);
            }
        }
    }
}
