using Parcs;

namespace Distance
{
    [Serializable]
    internal class Executor
    {
        public static int factorial(int x)
        {
            int result = 1;

            for (int i = 2; i <= x; i++)
            {
                result *= i;
            }

            return result;
        }

        public void GenerateOrder(int orderId, List<Point> basicOrder, ref List<Point> resultOrder)
        {
            List<Point> basicOrderCopy = basicOrder;

            int fact = factorial(basicOrder.Count() - 1);

            for (int i = basicOrderCopy.Count() - 1; i > 0; i--)
            {
                int pos = orderId / fact;

                resultOrder.Add(basicOrderCopy[pos]);

                basicOrderCopy.RemoveAt(pos);

                orderId %= fact;
                fact /= i;
            }

            resultOrder.Add(basicOrderCopy[0]);
        }

        static double CalculateDistance(List<Point> order)
        {
            double distance = 0;
            for (int i = 0; i < order.Count - 1; i++)
            {
                distance += CalculateEuclideanDistance(order[i], order[i + 1]);
            }
            return distance;
        }

        static double CalculateEuclideanDistance(Point point1, Point point2)
        {
            double deltaX = point2.X - point1.X;
            double deltaY = point2.Y - point1.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static void UpdateAnswer(ref double optimalDistance, ref List<Point> optimalOrder, List<Point> orderResult)
        {
            double distance = CalculateDistance(orderResult);

            if (distance < optimalDistance)
            {
                optimalDistance = distance;
                optimalOrder = orderResult;
            }
        }

        public static void printAnswer(ref List<Point> answer, double optimalDistance)
        {
            Console.WriteLine("Answer: ");
            {
                Console.WriteLine("Найоптимальніший обхід:");

                foreach (Point point in answer)
                {
                    Console.WriteLine(point.X + "; " + point.Y);
                }
                Console.WriteLine("Відстань: " + optimalDistance);
            }
        }

        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            List<Point> dot = info.Parent.ReadObject<List<Point>>();
            int indexStart = info.Parent.ReadObject<int>();
            int indexEnd = info.Parent.ReadObject<int>();

            double optimalDistance = double.MaxValue;
            List<Point> optimalOrder = new List<Point>();

            for (int i = indexStart; i < indexEnd; i++)
            {
                List<Point> order = new List<Point>();
                List<Point> itemsCopy = new List<Point>(dot);
                GenerateOrder(i, itemsCopy, ref order);
                //printOrder(ref order);
                //printOrder(ref items);

                UpdateAnswer(ref optimalDistance, ref optimalOrder, order);
                //printAnswer(ref answer);
            }
            printAnswer(ref optimalOrder, optimalDistance);
            info.Parent.WriteObject(optimalOrder);
            info.Parent.WriteObject(optimalDistance);

            return;
        }
    }
}
